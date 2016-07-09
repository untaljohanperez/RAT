<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" MasterPageFile="~/Site.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css">
    <link href='http://fonts.googleapis.com/css?family=Varela+Round' rel='stylesheet' type='text/css'>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.13.1/jquery.validate.min.js"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <script src="Scripts/login.js"></script>
    <link href="Content/login.css" rel="stylesheet" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.2.0.min.js" type="text/javascript"></script>
    <link href="Content/animate.css" rel="stylesheet" />
    <script src="Scripts/sweetalert.min.js"></script>
    <link href="Content/sweetalert.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.min.js"></script>

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">


    <!-- LOGIN FORM -->
    <div class="text-center" style="padding: 50px 0">
        <div class="logo">Inicio de Sesión</div>
        <!-- Main Form -->
        <div class="login-form-1">
            <div id="login-form" class="text-left">
                <div class="login-form-main-message"></div>
                <div class="main-login-form">
                    <div class="login-group">
                        <input runat="server" type="text" class="form-control" id="lg_username" name="lg_username" placeholder="Usuario" />
                        <input runat="server" type="password" class="form-control" id="lg_password" name="lg_password" placeholder="Contraseña" />
                        <asp:Button ID="login_button" runat="server" OnClick="login_button_Click"
                                    CssClass="login-button fa fa-chevron-right" UseSubmitBehavior="false" Text="&#61524;">
                        </asp:Button>
                    </div>
                </div>
            </div>
        </div>
        <!-- end:Main Form -->
        <div class="text-muted " style="font-family: monospace; font-size: 18px;">
            <div class="login-form-alert" style="display: none;"><i class="fa fa-exclamation-circle"></i> <span runat="server" id="loginMensaje"> </span> </div>
        </div>
    </div>

</asp:Content>



