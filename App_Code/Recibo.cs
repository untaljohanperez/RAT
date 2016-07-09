using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Recibo
/// </summary>
public class Recibo
{
    MPersistentecia mPersistentecia; 
    public int idReferencia { get; set; }
    public String referencia { get; set; }

    public List<Servicio> servicios;
    public String fechaCreacion { get; set; }
    public Boolean estadoRecibo { get; set; } 
    public int idRelacion { get; set; }
    public int idFactura{ get; set; }

    public Recibo()
    {
        servicios = new List<Servicio>();
        mPersistentecia = new MPersistentecia();
    }

    public Recibo(int idReferencia, String referencia, List<Servicio> servicios, int idRelacion, int idFactura,String fechaCreacion)
    {
        this.idReferencia = idReferencia;
        this.referencia = referencia;
        this.servicios = servicios;
        this.idRelacion = idRelacion;
        this.idFactura = idFactura;
        this.fechaCreacion = fechaCreacion;
     }

    public DataTable listarRecibo()
    {
         DataTable dtRecibo =  mPersistentecia.listarReferencia(this.referencia, "ListB_Connection");
        if (dtRecibo.Rows.Count > 0)
        {
            DataRow row = dtRecibo.Rows[0];
            this.idReferencia = Convert.ToInt32(row["IdReferencia"].ToString());
            this.idRelacion = Convert.ToInt32(row["IdRelacion"].ToString());
            this.estadoRecibo = Convert.ToBoolean(row["Ok"].ToString());
            this.fechaCreacion = row["FechaCreacion"].ToString();
            this.idFactura = Convert.ToInt32(row["IdFactura"].ToString());
        }

        return dtRecibo;
    }

    public DataTable listarSerciviosRecibo()
    {
            DataTable dtServiciosRecibo = mPersistentecia.listarServiciosReferencia(this.idReferencia.ToString(), "ListB_Connection");


            if (dtServiciosRecibo.Rows.Count > 0)
            {
                foreach (DataRow rowServicios in dtServiciosRecibo.Rows)
                {
                    Servicio servicioRecibo = new Servicio();
                    servicioRecibo.idServicio = Convert.ToInt32(rowServicios["IdsServId"].ToString());
                    servicioRecibo.nombreServicio = rowServicios["chrServDesc"].ToString();
                    servicioRecibo.valorServicio = (int)Convert.ToDecimal(rowServicios["ValorServicio"].ToString());
                    this.servicios.Add(servicioRecibo);
                }

            }
        return dtServiciosRecibo;
    }

    public int valorTotalRecibo()
    {
        int valorTotalRecibo = 0;
        foreach (Servicio servicio  in this.servicios)
        {
            valorTotalRecibo += servicio.valorServicio;
        }

        return valorTotalRecibo;
    }


}