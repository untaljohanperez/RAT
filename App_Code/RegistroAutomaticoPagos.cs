using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de RegistroAutomaticoPagos
/// </summary>
public class RegistroAutomaticoPagos
{
    MPersistentecia mPer = new MPersistentecia();
    public RegistroAutomaticoPagos()
    {

    }

    public void facturarServicios(Recibo recibo, int idTerceroUsuario, SqlConnection cn, SqlTransaction tr)
    {
        try
        {

            foreach (Servicio servicio in recibo.servicios)
            {
                switch (servicio.idServicio)
                {
                    case 91:
                        mPer.facturarInscripcion(recibo.idReferencia, idTerceroUsuario, cn, tr);
                        break;
                    case 138:
                        mPer.facturarMatricula(recibo.idRelacion, idTerceroUsuario, cn, tr);
                        break;
                    case 888:
                        mPer.facturarSLIES(recibo.idReferencia, idTerceroUsuario, cn,  tr);
                        break;
                    case 850:
                        //mPer.facturarIdiomas(recibo.idReferencia, idTerceroUsuario, "ListB_Connection");
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    public int insertarEncabezadoComprobante(int idReferencia, int idTerceroUsuario, int idTipoPago, SqlConnection cn, SqlTransaction tr)
    {
        try
        {

            int idComprobante = 0;
            DataTable dt = mPer.insertarEncabezadoComprobante(idReferencia, idTerceroUsuario, idTipoPago, cn, tr);
            if (dt.Rows.Count > 0)
            {
                DataRow row = (DataRow)dt.Rows[0];
                if (!String.IsNullOrEmpty(row["IdsComprobId"].ToString()))
                {
                    idComprobante = Convert.ToInt32(row["IdsComprobId"].ToString());
                }

            }
            return idComprobante;
        }
        catch (Exception e)
        {
            throw;
        }
    }
    public int insertarFormaIngreso(int idComprobante, int idTipoPago,  int valorConsignado,SqlConnection cn, SqlTransaction tr)
    {
        try
        {

            int idFormaIngreso = 0;
            DataTable dt = mPer.insertarFormaIngreso(idComprobante, idTipoPago, valorConsignado, cn, tr);
            if (dt.Rows.Count > 0)
            {
                DataRow row = (DataRow)dt.Rows[0];
                if (!String.IsNullOrEmpty(row["IdsFormaIngId"].ToString()))
                {
                    idFormaIngreso = Convert.ToInt32(row["IdsFormaIngId"].ToString());
                }

            }
            return idFormaIngreso;
        }
        catch (Exception e)
        {

            throw(e);
        }
    }
    public int insertarTipoPagoConsignacion(int idTercero, int idFormaIngreso, int idCuentaBancaria, string fechaTransaccion, int valorConsignado, SqlConnection cn, SqlTransaction tr)
    {
        try
        {

            int idTipoPagoConsignacion = 0;
            DataTable dt = mPer.insertarTipoPagoConsignacion(idTercero, idFormaIngreso, idCuentaBancaria, fechaTransaccion, valorConsignado, cn, tr);
            if (dt.Rows.Count > 0)
            {
                DataRow row = (DataRow)dt.Rows[0];
                if (!String.IsNullOrEmpty(row["IdsConsigId"].ToString()))
                {
                    idTipoPagoConsignacion = Convert.ToInt32(row["IdsConsigId"].ToString());
                }

            }
            return idTipoPagoConsignacion;
        }
        catch (Exception e)
        {

            throw(e);
        }
    }
    public int insertarPago(int idReferencia, int idComprobante, int valorConsignado, int idTipoPago,SqlConnection cn, SqlTransaction tr)
    {
        try
        {

            int idPago = 0;
            DataTable dt = mPer.insertarPago(idReferencia, idComprobante, valorConsignado, idTipoPago,cn, tr);
            if (dt.Rows.Count > 0)
            {
                DataRow row = (DataRow)dt.Rows[0];
                if (!String.IsNullOrEmpty(row["IdsPagoId"].ToString()))
                {
                    idPago = Convert.ToInt32(row["IdsPagoId"].ToString());
                }

            }
            return idPago;
        }
        catch (Exception e)
        {

            throw(e);
        }
    }
    public int integrarComprobante(int idPago, int idUsuario, SqlConnection cn, SqlTransaction tr)
    {
        try
        {

            int idComprobante = 0;
            DataTable dt = mPer.integrarComprobante(idPago, idUsuario, cn, tr);
            if (dt.Rows.Count > 0)
            {
                DataRow row = (DataRow)dt.Rows[0];
                if (!String.IsNullOrEmpty(row["IdsComprobId"].ToString()))
                {
                    idComprobante = Convert.ToInt32(row["IdsComprobId"].ToString());
                }

            }
            return idComprobante;
        }
        catch (Exception e)
        {

            throw(e);
        }
    }

    public int integrarComprobanteConsignacion(int idPago, int idUsuario, SqlConnection cn, SqlTransaction tr)
    {
        try
        {

            int idComprobante = 0;
            DataTable dt = mPer.integrarComprobanteConsignacion(idPago, idUsuario, cn, tr);
            if (dt.Rows.Count > 0)
            {
                DataRow row = (DataRow)dt.Rows[0];
                if (!String.IsNullOrEmpty(row["IdsComprobId"].ToString()))
                {
                    idComprobante = Convert.ToInt32(row["IdsComprobId"].ToString());
                }

            }
            return idComprobante;
        }
        catch (Exception e)
        {

            throw(e);
        }
    }

    //integrarComprobante


    public void aprobarRelacion(Recibo recibo, int idComprobante)
    {
        try
        {

            foreach (Servicio servicio in recibo.servicios)
            {
                switch (servicio.idServicio)
                {
                    case 91:
                        mPer.aprobarInscripcion(recibo.idRelacion, 7, 1,"ListB_Connection");
                        break;
                    case 138:
                        //mPer.aprobarMatricula(recibo.idRelacion, idTerceroUsuario);
                        break;
                    case 888:
                        mPer.aprobarSLIES(recibo.idReferencia, idComprobante, "ListB_Connection");
                        break;
                    case 850:
                        //mPer.aprobarIdiomas(recibo.idReferencia, idTerceroUsuario, "ListB_Connection");
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            throw(ex);
        }
    }


    public DataTable consultarResultadoRegistroTransacciones(int idReferencia)
    {
        try
        {
            DataTable dt = mPer.consultarResultadoRegistroTransacciones(idReferencia, "ListB_connection");
            return dt;
        }
        catch (Exception ex)
        {
            throw(ex);
        }
    }

    public int consultarCajaUsuarioTesoreriaAbierta(int terceroUsuarioId)
    {
        try
        {
            int idCajaUsuario = 0;
            DataTable dt = mPer.consultarCajaUsuarioTesoreriaAbierta(terceroUsuarioId, "ListB_connection");
            if (dt.Rows.Count > 0)
            {
                DataRow row = (DataRow)dt.Rows[0];
                if (!String.IsNullOrEmpty(row["IdCaja"].ToString()))
                {
                    idCajaUsuario = Convert.ToInt32(row["IdCaja"].ToString());
                }

            }
            return idCajaUsuario;
        }
        catch (Exception e)
        {

            throw(e);
        }
    }

    public int castTipoPagoRapToContabilidad(int idFormaPagoRap)
    {
        int idFormaPagoContabilidad = 0;

        switch (idFormaPagoRap)
        {   
            case 1:
                idFormaPagoContabilidad = 4;
                break;
            case 2:
                idFormaPagoContabilidad = 3;
                break;
            case 3:
                idFormaPagoContabilidad = 6;
                break;
            case 4:
                idFormaPagoContabilidad = 7;
                 break;
        }
        return idFormaPagoContabilidad;
    }



}




