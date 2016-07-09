using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.Reporting.WebForms;


// NOTA: puede usar el comando "Cambiar nombre" del menú "Refactorizar" para cambiar el nombre de clase "ServiceComprobante" en el código, en svc y en el archivo de configuración a la vez.

[ServiceContract(Namespace = "")]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class ServiceComprobante 
{

    [OperationContract]
    //
    [WebGet(UriTemplate = "/GetComprobante/{IdComprobante}")]
    public Stream GetComprobante(string IdComprobante)
    {
        try
        {
            DataSetReciboCaja dataSetComprob = new DataSetReciboCaja();

            DataSetReciboCajaTableAdapters.REPORTE_EncabezadoComprobanteIngresoTableAdapter taEncabezado = new DataSetReciboCajaTableAdapters.REPORTE_EncabezadoComprobanteIngresoTableAdapter();
            taEncabezado.Fill(dataSetComprob.REPORTE_EncabezadoComprobanteIngreso, Convert.ToInt32(IdComprobante));

            DataSetReciboCajaTableAdapters.REPORTE_MovimientosComprobanteIngresoTableAdapter taMovimientos = new DataSetReciboCajaTableAdapters.REPORTE_MovimientosComprobanteIngresoTableAdapter();
            taMovimientos.Fill(dataSetComprob.REPORTE_MovimientosComprobanteIngreso, Convert.ToInt32(IdComprobante));

            ReportDataSource RD1 = new ReportDataSource();
            RD1.Value = dataSetComprob.REPORTE_EncabezadoComprobanteIngreso;
            RD1.Name = "DataSet1";

            ReportDataSource RD2 = new ReportDataSource();
            RD2.Value = dataSetComprob.REPORTE_MovimientosComprobanteIngreso;
            RD2.Name = "DataSet2";

            ReportViewer ReportViewerComprob = new ReportViewer();

            ReportViewerComprob.ProcessingMode = ProcessingMode.Local;
            ReportViewerComprob.LocalReport.DataSources.Clear();
            ReportViewerComprob.LocalReport.DataSources.Add(RD1);
            ReportViewerComprob.LocalReport.ReportEmbeddedResource = @"Reports/ReportComprob.rdlc";
            ReportViewerComprob.LocalReport.ReportPath = @"Reports/ReportComprob.rdlc";

            ReportViewerComprob.LocalReport.DataSources.Add(RD2);
            ReportViewerComprob.LocalReport.ReportEmbeddedResource = @"Reports/ReportComprob.rdlc";
            ReportViewerComprob.LocalReport.ReportPath = @"Reports/ReportComprob.rdlc";

            ReportViewerComprob.LocalReport.Refresh();

            byte[] bytesReporteComprob = ReportViewerComprob.LocalReport.Render("PDF");

            MemoryStream streamReporteComprob = new MemoryStream(bytesReporteComprob);

            streamReporteComprob.Position = 0;

            WebOperationContext.Current.OutgoingResponse.ContentType = "application/pdf";

            return streamReporteComprob;
        }
        catch(Exception ex)
        {
            throw (ex);
        }
       
    }


    public static byte[] ReadFully(Stream input)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }

}
