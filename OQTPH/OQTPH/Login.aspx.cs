using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OQTPH.Models;

namespace OQTPH
{
    public partial class Login : System.Web.UI.Page
    {
        Usuario _usuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            _usuario = Usuario.Validar();
            if (_usuario != null)
            {
                Response.Redirect("~/Perfil.aspx");
            }

        }

        protected void btnLogar_Click(object sender, EventArgs e)
        {
            if (Usuario.FazerLogin(txtUserN.Text.Trim(), txtSenha.Text.Trim()) != null)
            {
                if (Request.QueryString["p"] == "Ev")
                {
                    Response.Redirect("CriaEvento.aspx");
                    return;
                }
                else
                {
                    Response.Redirect("Default.aspx");
                    return;
                }
            }
            else { lblMsg.Text = "Não foi possível fazer login!"; }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}