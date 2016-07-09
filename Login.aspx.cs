using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Form.DefaultButton = login_button.UniqueID;
        iniciarSesion();
    }


    protected void login_button_Click(object sender, EventArgs e)
    {
        //loginMensaje
        if (lg_username.Value.Trim().Length == 0)
        {
            loginMensaje.InnerText = "Usuario vacío";
            ScriptManager.RegisterStartupScript(this, GetType(), "inconrrect", "$(\".login-form-alert\").fadeIn();", true);
            return;
        }
        else if (lg_password.Value.Trim().Length == 0)
        {
            loginMensaje.InnerText = "Contraseña vacía";
            ScriptManager.RegisterStartupScript(this, GetType(), "inconrrect", "$(\".login-form-alert\").fadeIn();", true);
            return;
        }
        else
        {
            validarUsuario(lg_username.Value.Trim(), lg_password.Value);
        }
        
    }

    protected void iniciarSesion() {
        if (!string.IsNullOrEmpty((string)Session["user"]) && !string.IsNullOrEmpty((string)Session["password"]))
        {
            Response.Redirect("ReferenciaRecibo.aspx", true);
        }
    }

    protected void validarUsuario(string user, string password) {
        MPersistentecia mPersistencia = new MPersistentecia();
        DataTable dtlogin = mPersistencia.verificarUsuarioContraseñaRAP(user, GetSHA1(password), "RAP_connection");
        if (dtlogin.Rows.Count > 0)
        {
            DataRow row = dtlogin.Rows[0];
            Session["user"] = user;
            Session["password"] = password;
            Session["idUsuario"] = row["IdsUsuarioId"].ToString();
            Session["nombreUsuario"] = row["ChrTerNomCompleto"].ToString();
            Session["idTerceroUsuario"] = row["IntTercId"].ToString();
            Response.Redirect("ReferenciaRecibo.aspx");
        }
        else
        {
            loginMensaje.InnerText = "Contraseña incorrecta";
            ScriptManager.RegisterStartupScript(this, GetType(), "inconrrect", "$(\".login-form-alert\").fadeIn();", true);
        }
    }

    public static string GetSHA1(string str)
    {
        SHA1 sha1 = SHA1Managed.Create();
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] stream = null;
        StringBuilder sb = new StringBuilder();
        stream = sha1.ComputeHash(encoding.GetBytes(str));
        for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
        string jhoan = sb.ToString();
        return sb.ToString();
    }
    public string md5(string str)
    {

        MD5 md5 = new MD5CryptoServiceProvider();

        //compute hash from the bytes of text
        //System.Text.UnicodeEncoding
        md5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(str));

        //get hash result after compute it
        byte[] result = md5.Hash;

        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < result.Length; i++)
        {
            //change it into 2 hexadecimal digits
            //for each byte
            strBuilder.Append(result[i].ToString("x2"));
        }

        return strBuilder.ToString();
    }
}