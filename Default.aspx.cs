using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Activities.Statements;
using System.Web.Configuration;

public partial class _Default : System.Web.UI.Page
{
    MPersistentecia mPersistentecia = new MPersistentecia();
    RegistroAutomaticoPagos rap = new RegistroAutomaticoPagos();
    DataTable dtArchivo;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            if (string.IsNullOrEmpty((string)Session["user"]) && string.IsNullOrEmpty((string)Session["password"]))
            {
                Response.Redirect("Login.aspx", true);
            }
            //CargarComboFormatos();
        }
    }

    public void CargarArchivoDesdeRuta()
    {
        try
        {
            //String rutaArchivo = @"D:\Desktop\Tesorería Archivo Plano\Banco\CON_OTR_REC_PLANO40006566013548220160202163927.inf";
            String rutaArchivo = Server.MapPath(@"~/UploadedFiles/");
            if (FileUpload1.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                rutaArchivo = rutaArchivo + FileUpload1.FileName;

                FileUpload1.PostedFile.SaveAs(rutaArchivo);

                List<String> listArchivoPorLineas = CargarArchivoPlanoEnLista(rutaArchivo);

                DataTable dtFormato = mPersistentecia.ListarFormatoArchivo(1);

                if (validarFormatoCorrectoArchivo(listArchivoPorLineas, dtFormato)) {

                    dtArchivo = LlenarDtDesdeArchivoPlano(listArchivoPorLineas, dtFormato);

                    Session["dtArchivo"] = dtArchivo;

                    TextArea1.Cols = 110 + 20;
                    TextArea1.Rows = listArchivoPorLineas.Count;

                    GridView1.DataSource = dtArchivo;
                    GridView1.DataBind();
                    GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

                    lblNombreArchivo.Text = FileUpload1.FileName;

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "mostrarGrid();", true);
                }
                else
                {   
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(),"script", "mostrarAlerta('El archivo seleccionado no corresponde al formato requerido');", true);
                    Session["dtArchivo"] = null;
                    return;
                }
                
            }
        }
        catch (Exception ex)
        {

            throw(ex);
        }

    }

    protected List<String> CargarArchivoPlanoEnLista(String rutaArchivo)
    {
        try
        {
            List<String> lineas = System.IO.File.ReadLines(@rutaArchivo).ToList<String>();
            return lineas;
        }
        catch (Exception)
        {

            throw;
        }

    }

    protected Boolean validarFormatoCorrectoArchivo(List<String> lineas, DataTable dtFormato)
    {
        try
        {
            
            object longitudLineaFormato;
            longitudLineaFormato = dtFormato.Compute("Sum(Longitud)", "");
            if (lineas.Last<String>().Length == Convert.ToInt32(longitudLineaFormato))
            {
                return true;
            } else {
                return false;
            }  
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected DataTable LlenarDtDesdeArchivoPlano(List<String> lineas, DataTable dtFormato)
    {
        try
        {
            DataTable dtArchivoPlano = new DataTable();
            List<String> fila = new List<String>();


            //Agregar columnas a la tabla
            foreach (DataRow item in dtFormato.AsEnumerable())
            {
                dtArchivoPlano.Columns.Add(item["Nombre"].ToString(), Type.GetType(item["C#"].ToString()));
            }

            String a = String.Empty;
            foreach (String line in lineas)
            {
                a = line;
                fila.Clear();

                foreach (DataRow item in dtFormato.AsEnumerable())
                {

                    Int32 i = (Int32)item["Longitud"];
                    var valor = a.Substring(0, i);
                    valor = !String.IsNullOrEmpty(item["Decimales"].ToString()) ? Convert.ToDecimal(Double.Parse(valor) / Math.Pow(10, Convert.ToDouble(item["Decimales"].ToString()))).ToString() : valor;
                    fila.Add(valor);
                    a = a.Substring(i);
                    TextArea1.Value = TextArea1.Value + "  " + fila.Last<String>();
                }
                TextArea1.Value = TextArea1.Value + "\n";
                dtArchivoPlano.Rows.Add(fila.ToArray());
            }

            return dtArchivoPlano;
        }
        catch (Exception)
        {

            throw;
        }

    }

    protected void registrarTransaccionesdtArchivo(DataTable dtArchivo)
    {
        DataTable dtResultadoProceso = new DataTable();
        dtResultadoProceso.Columns.Add("REFERENCIA");
        dtResultadoProceso.Columns.Add("FACTURA");
        dtResultadoProceso.Columns.Add("TOTAL_FACTURADO");
        dtResultadoProceso.Columns.Add("COMPROBANTE_INGRESO");
        dtResultadoProceso.Columns.Add("VALOR_PAGADO");
        dtResultadoProceso.Columns.Add("DOCUMENTO");
        dtResultadoProceso.Columns.Add("TERCERO");
        dtResultadoProceso.Columns.Add("FECHA_COMPROBANTE");

        try
        {
            int idComprobante;
            int idFormaIngreso;
            int idTipoPagoConsignacion;
            int idPago;
            int valorConsignado;
            int idTerceroUsuario = Convert.ToInt32(Session["idTerceroUsuario"].ToString());
            SqlConnection cn;
            string connetionString = WebConfigurationManager.ConnectionStrings["ListB_Connection"].ToString();

            Recibo recibo;
            foreach (DataRow row in dtArchivo.Rows)
            {
                try
                {
                    recibo = new Recibo();
                    recibo.referencia =  row["Referencia 1"].ToString().Trim().Substring(0,10);
                    recibo.listarRecibo();
                    recibo.listarSerciviosRecibo();

                    valorConsignado = Convert.ToInt32(row["Valor Total"].ToString().Trim());
                    int idTercero = mPersistentecia.listarTerceroRelacionReferenciaRecibo(recibo.referencia, "ListB_Connection");
                    int idTipoPago = 3;//Consignacion

                    if (idTercero > 0 && recibo.idFactura == 0)
                    {

                        cn = new SqlConnection(connetionString);
                        cn.Open();//abrir conexion

                        SqlTransaction tr;
                        tr = cn.BeginTransaction();

                        try
                        {
                            rap.facturarServicios(recibo, idTerceroUsuario, cn, tr);
                            idComprobante = rap.insertarEncabezadoComprobante(recibo.idReferencia, idTerceroUsuario, idTipoPago, cn, tr);
                            idFormaIngreso = rap.insertarFormaIngreso(idComprobante, idTipoPago, valorConsignado, cn, tr);
                            idTipoPagoConsignacion = rap.insertarTipoPagoConsignacion(idTercero, idFormaIngreso, 5, row["Fecha Recaudo"].ToString().Trim(), valorConsignado, cn, tr);
                            idPago = rap.insertarPago(recibo.idReferencia, idComprobante, valorConsignado, idTipoPago,cn, tr);
                            rap.integrarComprobanteConsignacion(idPago, Convert.ToInt32(Session["idUsuario"].ToString()), cn, tr);

                            //Commit la transacción
                            tr.Commit();

                            rap.aprobarRelacion(recibo, idComprobante);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            tr.Rollback();
                        }
                    } //if (idTercero > 0 && recibo.idFactura > 0)

                    //Consultar Resultado del proceso
                    DataTable rowResultadoProceso = rap.consultarResultadoRegistroTransacciones(recibo.idReferencia);
                    dtResultadoProceso.ImportRow(rowResultadoProceso.Rows[0]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            } // for each
            

            
        }
        catch (Exception e)
        {
            throw(e);
        }

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script2", "MostrarResultadoProceso();", true);
        gvResultadoProceso.DataSource = dtResultadoProceso;
        gvResultadoProceso.DataBind();

    }
    
    protected void CargarComboFormatos()
    {
        DataTable dtFormatos = mPersistentecia.ListarFormatos();
        ddFormatoArchivo.DataSource = dtFormatos;
        ddFormatoArchivo.DataTextField = dtFormatos.Columns[1].ColumnName.ToString();
        ddFormatoArchivo.DataValueField = dtFormatos.Columns[0].ColumnName.ToString();
        ddFormatoArchivo.DataBind();
    }

    //protected void CargarComboPeriodosAcademicos()
    //{
    //    DataTable dtPeriodosAcademicos = mPersistentecia.ListarPeriodosAcademicos();
    //    ddPeriodoAcademico.DataSource = dtPeriodosAcademicos;
    //    ddPeriodoAcademico.DataTextField = dtPeriodosAcademicos.Columns[2].ColumnName.ToString();
    //    ddPeriodoAcademico.DataValueField = dtPeriodosAcademicos.Columns[0].ColumnName.ToString();
    //    ddPeriodoAcademico.DataBind();
    //}

    protected void btnCargarArchivo_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        CargarArchivoDesdeRuta();   
    }

    protected void btnImportarArchivo_Click(object sender, EventArgs e)
    {
         registrarTransaccionesdtArchivo((DataTable)Session["dtArchivo"]);
    }


}
