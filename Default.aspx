<%@ Page Title="REGISTRO AUTOMATICO DE PAGOS" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

    <script type="text/javascript">

    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <h4 class="text-muted" style=" margin-left: 15px;">
        Importación de información a SION desde archivos planos
    </h4>
    <div class="container" style="margin-top:30px"> 
        <div class="row">
            <div class="col-lg-12">
                <div class="form-group">
                    <asp:Label ID="Label1" runat="server" Text="Formato Archivo Plano" AssociatedControlID="ddFormatoArchivo" CssClass="control-label"></asp:Label>
                    <asp:DropDownList ID="ddFormatoArchivo" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
<%--        <div class="row">
            <div class="col-lg-12">
                <div class="form-group">
                    <asp:Label ID="Label2" runat="server" Text="Periodo Academico" AssociatedControlID="ddPeriodoAcademico"
                        CssClass="control-label"></asp:Label>
                    <asp:DropDownList ID="ddPeriodoAcademico" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-lg-12">
                <div class="form-group">
                </div>
                <div class="input-group">
                    <span class="input-group-addon btn-default" onclick="abrirFileUpload();">Seleccionar <span class="glyphicon glyphicon-folder-open"></span></span>
                    <div class="form-control1" id="txtRutaArchivo" aria-label="Amount (to the nearest dollar)"></div>
                    <span class="input-group-addon btn-default" onclick="cargarArchivo();">Cargar <span class="glyphicon glyphicon glyphicon-upload"></span></span>
                </div>
                <asp:Button ID="btnCargarArchivo" OnClick="btnCargarArchivo_Click" CssClass="hide bt" runat="server" Text="Cargar Archivo" ClientIDMode="Static" UseSubmitBehavior="false" />
            </div>
        </div>

    </div>


    <div class="row hide pnlInformacion">
        <div class="col-lg-12">
            <div class="well row" style="margin-top: 20px;">
                <div class="text-center">
                    <asp:Label ID="lblNombreArchivo" runat="server" Text="Nombre Archivo" CssClass="control-label h5 hide lblNombreArchivo text-center"></asp:Label>
                </div>
                <p class="text-center" style="margin-top:15px;">El archivo ha sido cargado exitosamente, haga click en este boton para importar la información al sistema</p>
                <div class="col-lg-7 col-lg-offset-5">
                    <span class="btn btn-default" onclick="ImportarArchivo();">Importar <span class="glyphicon glyphicon glyphicon-cloud"></span></span>
                    <asp:Button ID="btnImportarArchivo" class="btnImportarArchivo" OnClick="btnImportarArchivo_Click" CssClass="hide" runat="server" Text="Importar Archivo" ClientIDMode="Static" />
                </div>
            </div>
        </div>
    </div>




    <div class="table-responsive"> 
          
        <asp:GridView ID="GridView1" runat="server" CellPadding="4"
            style="margin-top:20px;"
            GridLines="None" CssClass="table table-hover table-striped table-condensed hide gvArchivoPlano ">
        </asp:GridView>

    </div>

    <div class="well wellResultadoProceso" style="display:none; margin-top: 50px">
        <div class="table-responsive">
            <h3 class="text-muted">Resultado Transacciones</h3>
            <asp:GridView ID="gvResultadoProceso" runat="server" CellPadding="4"
                Style="margin-top: 20px;"
                GridLines="None" CssClass="table table-hover table-striped table-condensed gvResultadoProceso">
            </asp:GridView>
        </div>
    </div>
    <asp:FileUpload ID="FileUpload1" runat="server" accept=".txt, .inf" CssClass="hide" ViewStateMode="Inherit" onchange="ponerRutaArchivoEnCliente(event.target.value);" />
    <textarea class="hide" id="TextArea1" cols="20" rows="2" runat="server"></textarea>



</asp:Content>
