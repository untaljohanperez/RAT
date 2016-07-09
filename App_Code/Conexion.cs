using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Descripción breve de Conexion
/// </summary>
public class Conexion
{
	public Conexion()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}


    /// <summary>
    /// Cierra la conexión abierta ©JPardo 2014FEB14
    /// </summary>
    /// <param name="cmd">objeto del tipo SqlCommand</param>
    private static void Desconectar(SqlCommand cmd)
    {
        //Si existe una conexión y está abierta, ciérrels
        if (cmd != null && cmd.Connection.State == ConnectionState.Open)
        {
            cmd.Connection.Close();
        }
    }//public static void Desconectar(SqlCommand cmd)


    /// <summary>
    /// Ejecuta un procedimiento almacenado ©JPardo 2014FEB14
    /// </summary>
    /// <param name="NombreSP">Nombre del procedimiento almacenado</param>
    /// <param name="Parametros">Lista de parámetros del tipo List SqlParamter </param>
    /// <param name="ConexionString">Cadena de conexión a la base de datos</param>
    /// <returns></returns>
    public DataTable EjecutarProcedimientoAlmacenado(string NombreSP, List<SqlParameter> Parametros, string ConnetionString)
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(ConnetionString))
            {
                cn.Open();//abrir conexion

                SqlDataAdapter da = null;

                //Ejecutar el procedimiento como una transacción
                SqlTransaction tr;
                tr = cn.BeginTransaction();

                SqlCommand cmd = cn.CreateCommand();
                cmd.Connection = cn;
                cmd.Transaction = tr;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = NombreSP;
                //cmd.Parameters.Clear();


                DataSet ds = new DataSet();

                try
                {
                    //Determinar si hay parámetros en el procedimiento
                    if (Parametros != null)
                    {
                        //Añadir cada parámetro al procedimiento
                        foreach (SqlParameter item in Parametros)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }//if (Parametros != null)

                    //SqlAdapter utiliza el SqlCommand para llenar el Dataset
                    da = new SqlDataAdapter(cmd);

                    //ejecuta procedimiento y llena el dataset (En lugar de cmd.ExecuteNonQuery(), Este a su vez retorna la cantidad de filas afectadas.)
                    da.SelectCommand.CommandTimeout = 180;//Máximo 3 minutos
                    da.Fill(ds);

                    //Commit la transacción
                    tr.Commit();

                }
                catch (Exception ex)
                {
                     tr.Rollback();//Devolver la ejecución de la transacción
                    throw (ex);
                }
                finally
                {
                    //Cerrar conexión con o sin error
                    cmd.Parameters.Clear();
                    Desconectar(cmd);
                }
                try
                {
                    //Intenta devolver la primera tabla
                    int UltimaTabla = ds.Tables.Count - 1;
                    return ds.Tables[UltimaTabla];
                }
                catch (IndexOutOfRangeException ex)
                {
                    //Si no encuentra una tabla de datos retorna nulo
                    return null;
                }
                //Devolver el DataTable

            }//using (SqlConnection cn = new SqlConnection(ConnetionString))

        }
        catch (Exception ex)
        {
            throw ex;
        }//try

    }//EjecutarProcedimientoAlmacenado
    public DataTable EjecutarProcedimientoAlmacenado2(string NombreSP, List<SqlParameter> Parametros, string ConnetionString)
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(ConnetionString))
            {
                cn.Open();//abrir conexion

                SqlDataAdapter da = null;


                SqlCommand cmd = cn.CreateCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = NombreSP;
                //cmd.Parameters.Clear();


                DataSet ds = new DataSet();

                try
                {
                    //Determinar si hay parámetros en el procedimiento
                    if (Parametros != null)
                    {
                        //Añadir cada parámetro al procedimiento
                        foreach (SqlParameter item in Parametros)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }//if (Parametros != null)

                    //SqlAdapter utiliza el SqlCommand para llenar el Dataset
                    da = new SqlDataAdapter(cmd);

                    //ejecuta procedimiento y llena el dataset (En lugar de cmd.ExecuteNonQuery(), Este a su vez retorna la cantidad de filas afectadas.)
                    da.SelectCommand.CommandTimeout = 180;//Máximo 3 minutos
                    da.Fill(ds);

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    //Cerrar conexión con o sin error
                    cmd.Parameters.Clear();
                    Desconectar(cmd);
                }

                try
                {
                    //Intenta devolver la primera tabla
                    int UltimaTabla = ds.Tables.Count - 1;
                    return ds.Tables[UltimaTabla];
                }
                catch (IndexOutOfRangeException ex)
                {
                    //Si no encuentra una tabla de datos retorna nulo
                    return null;
                }
                //Devolver el DataTable

            }//using (SqlConnection cn = new SqlConnection(ConnetionString))

        }
        catch (Exception ex)
        {
            throw ex;
        }//try

    }//EjecutarProcedimientoAlmacenado

    public DataTable EjecutarProcedimientoAlmacenado3(string NombreSP, List<SqlParameter> Parametros, SqlConnection cn, SqlTransaction tr)
    {
        try
        {
            

                SqlDataAdapter da = null;
                SqlCommand cmd = cn.CreateCommand();
                cmd.Transaction = tr;
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = NombreSP;
                //cmd.Parameters.Clear();


                DataSet ds = new DataSet();

                try
                {
                    //Determinar si hay parámetros en el procedimiento
                    if (Parametros != null)
                    {
                        //Añadir cada parámetro al procedimiento
                        foreach (SqlParameter item in Parametros)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }//if (Parametros != null)

                    //SqlAdapter utiliza el SqlCommand para llenar el Dataset
                    da = new SqlDataAdapter(cmd);

                    //ejecuta procedimiento y llena el dataset (En lugar de cmd.ExecuteNonQuery(), Este a su vez retorna la cantidad de filas afectadas.)
                    da.SelectCommand.CommandTimeout = 180;//Máximo 3 minutos
                    da.Fill(ds);

                    //tr.Commit();

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    //Cerrar conexión con o sin error
                    cmd.Parameters.Clear();
                    //Desconectar(cmd);
                }

                try
                {
                    //Intenta devolver la primera tabla
                    int UltimaTabla = ds.Tables.Count - 1;
                    return ds.Tables[UltimaTabla];
                }
                catch (IndexOutOfRangeException ex)
                {
                    //Si no encuentra una tabla de datos retorna nulo
                    return null;
                }
                //Devolver el DataTable

        }
        catch (Exception ex)
        {
            throw ex;
        }//try

    }

}