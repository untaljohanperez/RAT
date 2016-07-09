using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Descripción breve de MPersistentecia
/// </summary>
public class MPersistentecia
{
    Conexion conexion;
    List<SqlParameter> parameters;

    public MPersistentecia()
    {
        conexion = new Conexion();
        parameters = new List<SqlParameter>();
    }

    public DataTable RegistrarTransaccionesPorReferencia(Recibo recibo)
    { 
       DataTable dt = new DataTable();
        parameters.Clear();
        parameters.Add(new SqlParameter("@Referencia", recibo.referencia));
        //parameters.Add(new SqlParameter("@IdPerAcad", recibo.periodoAcademico));
        //parameters.Add(new SqlParameter("@curValorConsignado", recibo.valorTotal));
        //parameters.Add(new SqlParameter("@FechaTransaccion", recibo.fechaTransaccion));
        // dt = conexion.EjecutarProcedimientoAlmacenado2("Contabilidad.dbo.PROCESO_RegistroAutomaticoPagosRecibo", parameters, WebConfigurationManager.ConnectionStrings["RAP_connection"].ToString());
        return dt;
    }

    public DataTable ListarFormatoArchivo(int Idformato)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdFormato", Idformato));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("LISTAR_CamposFormato", parameters, WebConfigurationManager.ConnectionStrings["RAP_connection"].ToString());
        return dt;
    }

    public DataTable ListarFormatos()
    {
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("LISTAR_Formatos", null, WebConfigurationManager.ConnectionStrings["RAP_connection"].ToString());
        return dt;
    }

    public DataTable ListarPeriodosAcademicos()
    {
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("ViceAcad.dbo.LISTAR_PeriodosAcademicos", null, WebConfigurationManager.ConnectionStrings["RAP_connection"].ToString());
        return dt;
    }

    public DataTable listarReferencia(string referencia, string connetion)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@Referencia", referencia));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("LISTAR_ReferenciaRecibo", parameters, WebConfigurationManager.ConnectionStrings[connetion].ToString());
        return dt;
    }

    public DataTable listarServiciosReferencia(string idReferencia, string connetion)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@idReferencia", idReferencia));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("LISTAR_ServiciosReferencia", parameters, WebConfigurationManager.ConnectionStrings[connetion].ToString());
        return dt;
    }
    public DataTable verificarUsuarioContraseñaRAP(string user, string password, string connection)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@User", user));
        parameters.Add(new SqlParameter("@Password", password));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("VERIFICAR_UsuarioContraseñaRAP", parameters, WebConfigurationManager.ConnectionStrings[connection].ToString());
        return dt;
    }
    public DataTable listarInformacionClienteReferenciaRecibo(string referencia, string connection)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@Referencia", referencia));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("LISTAR_InformacionClienteReferenciaRecibo", parameters, WebConfigurationManager.ConnectionStrings[connection].ToString());
        return dt;
    }
    public int listarTerceroRelacionReferenciaRecibo(string referencia, string connection)
    {
        int idTercero = 0;
        parameters.Clear();
        parameters.Add(new SqlParameter("@Referencia", referencia));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("PROCESO_TerceroRelacionReferenciaRecibo", parameters, WebConfigurationManager.ConnectionStrings[connection].ToString());
        if(dt.Rows.Count > 0)
        {
            DataRow row = (DataRow)dt.Rows[0];
            if (!String.IsNullOrEmpty(row["IdTercero"].ToString()))
            {
                idTercero = Convert.ToInt32(row["IdTercero"].ToString());
            }
        }
        return idTercero;
    }
    /****************************************************************************/
    public DataTable facturarSLIES(int idReferencia, int idTerceroUsuario, SqlConnection cn, SqlTransaction tr)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdReferencia", idReferencia));
        parameters.Add(new SqlParameter("@IdTerceroUsuario", idTerceroUsuario));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado3("SLIES2.dbo.PROCESO_SLIES_Facturar", parameters, cn, tr);
        return dt;
    }
    public DataTable facturarInscripcion(int idReferencia, int idTerceroUsuario, SqlConnection cn, SqlTransaction tr)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdReferencia", idReferencia));
        parameters.Add(new SqlParameter("@IdTerceroUsuario", idTerceroUsuario));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado3("Inscripciones.dbo.PROCESO_INSCRIPCION_Facturar", parameters, cn, tr);
        return dt;
    }
    public DataTable facturarMatricula(int idMatricula, int idTerceroUsuario, SqlConnection cn, SqlTransaction tr)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdMatricula", idMatricula));
        parameters.Add(new SqlParameter("@IdTerceroUsuario", idTerceroUsuario));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado3("ViceAcad.dbo.PROCESO_Matricula_Facturar", parameters, cn, tr);
        return dt;
    }
    //public DataTable facturarIdiomas(int idMatricula, int idTerceroUsuario, string connection)
    //{
    //    List<SqlParameter> parameters = new List<SqlParameter>();
    //    parameters.Add(new SqlParameter("@IdMatricula", idMatricula));
    //    parameters.Add(new SqlParameter("@IdTerceroUsuario", idTerceroUsuario));
    //    DataTable dt = conexion.EjecutarProcedimientoAlmacenado("ViceAcad.dbo.PROCESO_Matricula_Facturar", parameters, WebConfigurationManager.ConnectionStrings[connection].ToString());
    //    return dt;
    //}
    public DataTable insertarEncabezadoComprobante(int idReferencia, int idTerceroUsuario, int idTipoPago, SqlConnection cn, SqlTransaction tr)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdReferencia", idReferencia));
        parameters.Add(new SqlParameter("@IdTerceroUsuario", idTerceroUsuario));
        parameters.Add(new SqlParameter("@IdTipoPago", idTipoPago));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado3("Contabilidad.dbo.INSERTAR_EncabezadoComprobanteReferenciaRecibo", parameters, cn, tr);

        return dt;
    }
    public DataTable insertarFormaIngreso(int idComprobante, int idTipoPago, int valorConsignado,SqlConnection cn, SqlTransaction tr)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdsComprobId", idComprobante));
        parameters.Add(new SqlParameter("@IdTipoPago", idTipoPago));
        parameters.Add(new SqlParameter("@ValorConsignado", valorConsignado));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado3("Contabilidad.dbo.INSERTAR_FormaIngresoReferenciaRecibo", parameters, cn, tr);
        return dt;
    }
    public DataTable insertarTipoPagoConsignacion(int idTercero,int idFormaIngreso, int idCuentaBancaria, string fechaTransaccion, int valorConsignado, SqlConnection cn, SqlTransaction tr)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdsFormaIngId", idFormaIngreso));
        parameters.Add(new SqlParameter("@IdCtaBancaria", idCuentaBancaria));
        parameters.Add(new SqlParameter("@FechaTransaccion", fechaTransaccion));
        parameters.Add(new SqlParameter("@IdTercero", idTercero));
        parameters.Add(new SqlParameter("@ValorConsignado", valorConsignado));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado3("Contabilidad.dbo.INSERTAR_TipoPagoConsignacionReferenciaRecibo", parameters, cn, tr);
        return dt;
    }
    public DataTable insertarPago(int idReferencia,int idComprobante,int valorConsignado, int idTipoPago, SqlConnection cn, SqlTransaction tr)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdReferencia", idReferencia));
        parameters.Add(new SqlParameter("@CurValorConsignado", valorConsignado));
        parameters.Add(new SqlParameter("@Idcomprobante", idComprobante));
        parameters.Add(new SqlParameter("@IdTipoPago", idTipoPago));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado3("BDAdmin.dbo.INSERTAR_PagoReferenciaRecibo", parameters, cn, tr);
        return dt;
    }
    public DataTable integrarComprobante(int idPago,int idUsuario, SqlConnection cn, SqlTransaction tr)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IntPagoId", idPago));
        parameters.Add(new SqlParameter("@IntSucursalId", 251));
        parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
        SqlParameter IntRta = new SqlParameter("@IntRta", 0);
        IntRta.Direction = ParameterDirection.Output;
        parameters.Add(IntRta);
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado3("BDAdmin.dbo.PROCESO_IntegrarRC", parameters, cn, tr);
        return dt;
    }
    public DataTable integrarComprobanteConsignacion(int idPago,int idUsuario, SqlConnection cn, SqlTransaction tr)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IntPagoId", idPago));
        parameters.Add(new SqlParameter("@IntSucursalId", 251));
        parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
        SqlParameter IntRta = new SqlParameter("@IntRta", 0);
        IntRta.Direction = ParameterDirection.Output;
        parameters.Add(IntRta);
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado3("PROCESO_IntegrarRC_ConsignacionReferenciaRecibo", parameters, cn, tr);
        return dt;
    }
    public DataTable aprobarSLIES(int idReferencia, int idComprobante, string connection)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdReferencia", idReferencia));
        parameters.Add(new SqlParameter("@IdComprobantePago", idComprobante));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("SLIES2.dbo.APROBAR_InscripcionEvento", parameters, WebConfigurationManager.ConnectionStrings[connection].ToString());
        return dt;
    }
    public DataTable aprobarInscripcion(int idRelacion, int idForma, int estado, string connection)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdRegistro", idRelacion));
        parameters.Add(new SqlParameter("@IdForma", idForma));
        parameters.Add(new SqlParameter("@Estado", estado));
        parameters.Add(new SqlParameter("@IPUsuarioActualizador", " "));
        parameters.Add(new SqlParameter("@IdModuloActualizador ", 4));
        parameters.Add(new SqlParameter("@HostNameActualizador", "RAP"));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("Inscripciones.dbo.ACTUALIZARFormaRegistros", parameters, WebConfigurationManager.ConnectionStrings[connection].ToString());
        return dt;
    }
    //public DataTable aprobarMatricula(int idMatricula, int idTerceroUsuario, string connection)
    //{
    //    List<SqlParameter> parameters = new List<SqlParameter>();
    //    parameters.Add(new SqlParameter("@IdMatricula", idMatricula));
    //    parameters.Add(new SqlParameter("@IdTerceroUsuario", idTerceroUsuario));
    //    DataTable dt = conexion.EjecutarProcedimientoAlmacenado("ViceAcad.dbo.PROCESO_Matricula_Facturar", parameters, WebConfigurationManager.ConnectionStrings[connection].ToString());
    //    return dt;
    //} 
    public DataTable consultarResultadoRegistroTransacciones(int idReferencia, string connection)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdReferencia", idReferencia));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("BDAdmin.dbo.CONSULTAR_ResultadoProcesoReferenciaRecibo", parameters, WebConfigurationManager.ConnectionStrings[connection].ToString());
        return dt;
    }
    public DataTable consultarCajaUsuarioTesoreriaAbierta(int terceroUsuarioId, string connection)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdSucursal", 251));
        parameters.Add(new SqlParameter("@IdTerceroUsuario", terceroUsuarioId));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("Contabilidad.dbo.CONSULTAR_CajaAbiertaTesoreria", parameters, WebConfigurationManager.ConnectionStrings[connection].ToString());
        return dt;
    }
    public DataTable listarFormasPagoUsuario(int UsuarioID, string connection)
    {
        parameters.Clear();
        parameters.Add(new SqlParameter("@IdUsuario", UsuarioID));
        DataTable dt = conexion.EjecutarProcedimientoAlmacenado("RAP.dbo.LISTAR_FormasPagoUsuario", parameters, WebConfigurationManager.ConnectionStrings[connection].ToString());
        return dt;
    }
}