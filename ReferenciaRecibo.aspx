<%@ Page Title="Referencia Recibo" Language="C#" AutoEventWireup="true" CodeFile="ReferenciaRecibo.aspx.cs" Inherits="ReferenciaRecibo"
    MasterPageFile="~/Site.master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div class="row">
        <h4 class="text-muted">Registro de transacciones por referencia recibo</h4>

        <div class="form-group" style="margin-top: 40px;">

            <asp:Label Text="Refencia Recibo:" CssClass="control-label" runat="server" AssociatedControlID="TxtReferenciaRecibo" />
            <asp:TextBox ID="TxtReferenciaRecibo" TabIndex="1" CssClass="form-control txtReferenciaRecibo" runat="server" placeholder="Referencia Recibo" />
        </div>
        <%--boton consultar Referencia--%>
        <div class="form-group text-center">
            <div class="btn btn-disabled btn-lg" tabindex="2" style="text-transform: initial;" onclick="consultarReferencia();">Consultar Referencia</div>
            <asp:Button ID="btnConsultarRefencia" OnClick="btnConsultarRefencia_Click"
                CssClass="btnConsultarRefencia hide" Text="Consultar Información" runat="server" UseSubmitBehavior="false" />
        </div>
    </div>

    <div class="row">

        <div class="well wellInfo" style="display: none;">
            <div class="table-responsive">
                <h3 class="text-muted">Información del recibo</h3>
                <asp:GridView ID="gvReferencia" TabIndex="3" AutoGenerateColumns="false" CssClass="table table-hover table-condensed table-bordered gvReferencia" runat="server" DataKeyNames="IdReferencia">
                    <Columns>
                        <asp:BoundField DataField="IdReferencia" HeaderText="IdReferencia" Visible="false" />
                        <asp:BoundField DataField="Referencia" HeaderText="Referencia" />
                        <asp:BoundField DataField="Factura" HeaderText="Factura" />
                        <asp:BoundField DataField="Vigente" HeaderText="Recibo vigente?" />
                        <asp:BoundField DataField="FechaCreacion" HeaderText="Fecha de creación referencia" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="table-responsive">
                <h3 class="text-muted">Información de servicios</h3>
                <asp:GridView ID="gvServiciosReferencia" TabIndex="4" AutoGenerateColumns="false" CssClass="table table-hover table-condensed table-bordered gvServiciosReferencia" runat="server" DataKeyNames="IdsServId">
                    <Columns>
                        <asp:BoundField DataField="IdsServId" HeaderText="IdServicio" Visible="false" />
                        <asp:BoundField DataField="CodigoServicio" HeaderText="CodigoServicio" />
                        <asp:BoundField DataField="ChrServDesc" HeaderText="Servicio" />
                        <asp:BoundField DataField="ValorServicio" HeaderText="Valor de Servicio" />
                    </Columns>

                </asp:GridView>
                <div class="table-responsive">
                    <h3 class="text-muted">Información del cliente</h3>
                    <asp:GridView ID="gvInfoCliente" TabIndex="4" AutoGenerateColumns="true" CssClass="table table-hover table-condensed table-bordered gvInfoCliente" runat="server">
                    </asp:GridView>
                </div>
                <div class="table-responsive wellInfoPago" style="display: none;">
                    <h3 class="text-muted">Información del pago</h3>
                    <asp:GridView ID="gvInfoPago" TabIndex="5" AutoGenerateColumns="true" CssClass="table table-hover table-condensed table-bordered gvInfo" runat="server">
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!--Acciones-->
        <div class="wellAcciones" style="display: none;">

            <div class="well">
                <h3 class="text-muted">Proceso</h3>
                <div class="row">
                    <div class="col-md-offset-2 col-md-7 text-center">
                        <asp:CheckBox id="soloFacturacion" runat="server" CssClass="soloFacturacion" onclick="displayFormasPago(event);" name="soloFacturacion" Text="Solo Facturación" />
                    </div>
                </div>
            </div>

            <div class="well wellFormasPago">    
            <h3 class="text-muted">Formas de Pago</h3>
            <div class="row text-center ">
                <asp:Repeater ID="rptFormasPagoUsuario" runat="server">
                    <ItemTemplate>
                        <div class="col-3" style="display:inline;" >    
                            <div class="radio-inline">
                                <label style="font-size:17px" tabindex="<%# 5 + Convert.ToInt32(Eval("IdFormaPago").ToString())  %>" onclick="formapago(<%# Eval("IdFormaPago") %>)">
                                    <input class="text-muted" style="font-size:60px;" type="radio" name="optionFormasPago" 
                                        id="formaPago<%# Eval("IdFormaPago") %>" value="<%# Eval("IdFormaPago") %>" 
                                         />
                                    <%# Eval("NombreFormaPago") %>
                                </label>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
                <input id="inputFormaPago" runat="server" type="text" value="0" class="inputFormaPago hide"/>
            </div>
            
            <div class=" well text-center" style="margin-top:20px;">
                <asp:Button ID="btnRegistrarTransacciones" OnClick="btnRegistrarTransacciones_Click"
                    Text="Registrar Transacciones" runat="server" Style="text-transform: initial; background-color: #FFFFFF;"
                    CssClass="btnRegistrarTransacciones btn btn-lg text-center"
                    UseSubmitBehavior="false" />
            </div>
        </div>

        <!--Acciones-->

        <!--Resultado-->
        <div class="well wellResultadoProceso" style="display: none; margin-top: 50px">
            <div class="table-responsive">
                <h3 class="text-muted">Resultado Transacciones</h3>
                <asp:GridView ID="gvResultadoProceso" runat="server" CellPadding="4"
                    Style="margin-top: 20px;"
                    GridLines="None" CssClass="table table-hover table-striped table-condensed gvResultadoProceso">
                </asp:GridView>
                <a id="linkReporteComprobante" href="http://www.google.com" target="_blank" class="btn btn-block">Comprobante</a>
                        <input id="txtIdComprobante" style="display:none;" runat="server" type="text" class="idComprobante" value="0" />
            </div>
        </div>  
        <!--Resultado-->

    </div>



</asp:Content>
