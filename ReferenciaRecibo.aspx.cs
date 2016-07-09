using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.Hosting;
using Microsoft.Reporting.WebForms;

public partial class ReferenciaRecibo : System.Web.UI.Page
{
    MPersistentecia mPersistentecia = new MPersistentecia();
    RegistroAutomaticoPagos rap = new RegistroAutomaticoPagos();
    string rutaReporteComprobIngreso = "Services/ServiceComprobante.svc/GetComprobante/";

    protected void Page_Load(object sender, EventArgs e)
    {
        Form.DefaultButton = btnConsultarRefencia.UniqueID;
        if (string.IsNullOrEmpty((string)Session["user"]) && string.IsNullOrEmpty((string)Session["password"]))
        {
            Response.Redirect("Login.aspx", true);
        }
        cargarFormasPagoUsuario();
    }

    protected void btnConsultarRefencia_Click(object sender, EventArgs e)
    {
        ConsultarReferencia();
    }

    protected void ConsultarReferencia()
    {
        try
        {

            MPersistentecia mPer = new MPersistentecia();
            Recibo recibo = new Recibo();
            recibo.referencia = TxtReferenciaRecibo.Text.PadLeft(10, '0');
            DataTable dtRecibo = recibo.listarRecibo();
            DataTable dtServiciosRecibo = recibo.listarSerciviosRecibo();
            int idTercero = mPer.listarTerceroRelacionReferenciaRecibo(recibo.referencia, "ListB_Connection");
            Session["recibo"] = recibo;
            gvReferencia.DataSource = dtRecibo;
            gvReferencia.DataBind();
            gvServiciosReferencia.DataSource = dtServiciosRecibo;
            gvServiciosReferencia.DataBind();

            if (recibo.idReferencia > 0)
            {
                DataTable dtInfoCliente = mPer.listarInformacionClienteReferenciaRecibo(recibo.referencia, "ListB_Connection");
                gvInfoCliente.DataSource = dtInfoCliente;
                gvInfoCliente.DataBind();

                DataTable dtInfoPago = rap.consultarResultadoRegistroTransacciones(recibo.idReferencia);

                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarInfo", "mostrarWellInfo(); $('.wellAcciones').fadeIn();", true);

                if (dtInfoPago.Rows.Count > 0)
                {
                    gvInfoPago.DataSource = dtInfoPago;
                    gvInfoPago.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarInfoPago", " mostrarWellInfoPago(); $('.wellAcciones').fadeOut();", true);
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarInfo", "mostrarWellInfo(); $('.wellAcciones').fadeIn(); ", true);
                 soloFacturacion.Checked = false; //Desactivar solo facturacion
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "noValida", "sweetAlert(\"Referencia no valida\");  $('.txtReferenciaRecibo').val(\"\");  ", true);
            }
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error", "console.log(\""+ex.Message+"\")", true);
        }


        

    }

    protected void btnRegistrarTransacciones_Click(object sender, EventArgs e)
    {
        registrarTransaccionesRecibo((Recibo)Session["recibo"]);
    }

    protected void registrarTransaccionesRecibo(Recibo recibo)
    {
        DataTable dtResultadoProceso = configurarDataTableResultado();
        string scriptCliente = "";

        try
        {
            int idComprobante;
            int idFormaIngreso;
            int idTipoPagoConsignacion;
            int idPago;
            int valorTotalRecibo;
            SqlConnection cn;
            int idTerceroUsuario = Convert.ToInt32(Session["idTerceroUsuario"].ToString());
            DateTime hoy = DateTime.Today;
            string fechaActual = hoy.Year.ToString() + hoy.Month.ToString().PadLeft(2, '0') + hoy.Day.ToString().PadLeft(2, '0');
            string connetionString = WebConfigurationManager.ConnectionStrings["ListB_Connection"].ToString();
            int idCajaUsuario = rap.consultarCajaUsuarioTesoreriaAbierta(idTerceroUsuario);
            int idTipoPagoRap = Convert.ToInt32(inputFormaPago.Value);
            int idTipoPagoContabilidad = rap.castTipoPagoRapToContabilidad(idTipoPagoRap);
            
            try
            {
                valorTotalRecibo = recibo.valorTotalRecibo();
                int idTercero = mPersistentecia.listarTerceroRelacionReferenciaRecibo(recibo.referencia, "ListB_Connection");

                if (idTercero > 0 && valorTotalRecibo > 0 && recibo.estadoRecibo == true && recibo.idFactura == 0 && idCajaUsuario > 0 && (idTipoPagoContabilidad > 0 || soloFacturacion.Checked == true) )
                {

                    cn = new SqlConnection(connetionString);
                    cn.Open();//abrir conexion

                    SqlTransaction tr;
                    tr = cn.BeginTransaction();

                    try
                    {
                        //Crear factura para los servicios
                        rap.facturarServicios(recibo, idTerceroUsuario, cn, tr);

                        if (!soloFacturacion.Checked)
                        {
                           idComprobante = rap.insertarEncabezadoComprobante(recibo.idReferencia, idTerceroUsuario, idTipoPagoContabilidad, cn, tr);

                           //Asignar Valor idComprobante a input para link a reporte
                           txtIdComprobante.Value = idComprobante.ToString(); 

                           //Insetar TblForma ingreso para Efectivo y consignacion
                           if(idTipoPagoContabilidad == 3 || idTipoPagoContabilidad == 4)
                           {
                               idFormaIngreso = rap.insertarFormaIngreso(idComprobante, idTipoPagoContabilidad, valorTotalRecibo, cn, tr);

                               //Insetar tblConsignacion si es esta forma de ingreso
                               if (idTipoPagoContabilidad == 3)//Consignacion
                               {
                                   idTipoPagoConsignacion = rap.insertarTipoPagoConsignacion(idTercero, idFormaIngreso, 5, fechaActual, valorTotalRecibo, cn, tr);
                               }

                           } 
                           idPago = rap.insertarPago(recibo.idReferencia, idComprobante, valorTotalRecibo, idTipoPagoContabilidad, cn, tr);
                           rap.integrarComprobanteConsignacion(idPago, Convert.ToInt32(Session["idUsuario"].ToString()), cn, tr);
                           rap.aprobarRelacion(recibo, idComprobante);

                     scriptCliente += " setLinkToReportComprob(\"" + rutaReporteComprobIngreso + "\",\"" + idComprobante + " \");";

                       }//if (!soloFacturacion.Checked)

                         //Commit la transacción
                         tr.Commit();

                        
                        scriptCliente += "MostrarResultadoProcesoReferencia();" ;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        tr.Rollback();
                        scriptCliente = "sweetAlert(\"Ha ocurrido un error en el proceso, por favor inténtalo de nuevo.\"); console.log(\" " + ex.Message.Replace("'", "").Replace("\"", "").Replace(Environment.NewLine, "") + " \"); mostrarWellInfo();";
                    }
                } else if (recibo.idFactura > 0) {
                    scriptCliente = "sweetAlert(\"Esta referencia ya tiene una factura y un pago asociado\"); mostrarWellInfo(); mostrarWellInfoPago();";
                }
                else if (recibo.estadoRecibo == false)
                {
                    scriptCliente = "sweetAlert(\"Esta referencia no está vigente para este concepto de pago\");";
                } else if (idCajaUsuario == 0)
                {
                    scriptCliente = "sweetAlert(\"No hay una caja abierta para este usuario para la fecha " + DateTime.Today.ToLongDateString() + "\"); mostrarWellInfo();";
                }else if (idTipoPagoContabilidad == 0)
                {
                    scriptCliente = "sweetAlert(\"Seleccione la forma de pago para esta transacción \"); mostrarWellInfo(); $('.wellAcciones').fadeIn();";
                }
               else if (valorTotalRecibo == 0)
               {
                  scriptCliente = "sweetAlert(\"Este recibo no tiene conceptos de pago relacionados\"); mostrarWellInfo();";
               }

            //Consultar Resultado del proceso
            DataTable rowResultadoProceso = rap.consultarResultadoRegistroTransacciones(recibo.idReferencia);
                if (rowResultadoProceso.Rows.Count > 0)
                {
                    dtResultadoProceso.ImportRow(rowResultadoProceso.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        catch (Exception e)
        {
            throw (e);
        }

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script2", scriptCliente, true);
        gvResultadoProceso.DataSource = dtResultadoProceso;
        gvResultadoProceso.DataBind();
      
    }

    public DataTable configurarDataTableResultado()
    {
        DataTable dtResultadoProceso = new DataTable();
        dtResultadoProceso.Columns.Add("Referencia");
        dtResultadoProceso.Columns.Add("Factura");
        dtResultadoProceso.Columns.Add("Total facturado");
        dtResultadoProceso.Columns.Add("Comprobante Ingreso");
        dtResultadoProceso.Columns.Add("Valor Pagado");
        dtResultadoProceso.Columns.Add("Documento");
        dtResultadoProceso.Columns.Add("Tercero");
        dtResultadoProceso.Columns.Add("Fecha_Comprobante");

        return dtResultadoProceso;
    }

    public void cargarFormasPagoUsuario()
    {
        DataTable dtFormasPago = mPersistentecia.listarFormasPagoUsuario(Convert.ToInt32(Session["idUsuario"].ToString()), "RAP_connection");
        rptFormasPagoUsuario.DataSource = dtFormasPago;
        rptFormasPagoUsuario.DataBind();
    }



}
















