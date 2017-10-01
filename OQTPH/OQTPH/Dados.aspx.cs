using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OQTPH.Utils;
using OQTPH.Models;
using System.Text;

namespace OQTPH
{
    public partial class Dados : System.Web.UI.Page
    {
        UsuarioModelo _usuario;
        public string user;

        protected void Page_Load(object sender, EventArgs e)
        {
            _usuario = UsuarioModelo.Validar();
            if (_usuario != null)
            {
                PHin.Visible = false;
                PHout.Visible = true;
            }
            else
            {
                PHin.Visible = true;
                PHout.Visible = false;
            }

            if (_usuario != null)
            {
                using (SqlConnection conn = Sql.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("Select nome, email, username from usuario where id_usuario = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _usuario.Id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblNome.Text = reader.GetString(0);
                                lblEmail.Text = reader.GetString(1);
                                lblUserN.Text = reader.GetString(2);
                                user = reader.GetString(2);

                                btn.Visible = false;
                                ph1.Visible = ph2.Visible = ph3.Visible = btnUp.Visible = true;
                                Page.Title = "Atualizar Perfil - OQTPH";
                            }
                        }
                    }
                }
            }
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(txtUserN.Text.Trim()) == false || txtUserN.Text.Trim().Length < 15) && (string.IsNullOrWhiteSpace(txtEmail.Text.Trim()) == false))
            {
                bool temUn = false;
                bool temEm = false;
                try
                {
                    using (SqlConnection conn = Sql.OpenConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand("select username from usuario where username = @u", conn))
                        {
                            cmd.Parameters.AddWithValue("@u", txtUserN.Text.Trim());
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    lblMsg.Text = "Username já utilizado!";
                                    temUn = true;
                                    throw new Exception();
                                }
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand("select email from usuario where email = @e", conn))
                        {
                            cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    lblMsg.Text = "E-mail já utilizado!";
                                    temEm = true;
                                    throw new Exception();
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    if (temEm || temUn)
                    {
                        return;
                    }
                    else
                    {
                        lblMsg.Text = "Erro de conexão inesperado!";
                        return;
                    }

                }
            }


            try
            {
                _usuario = UsuarioModelo.Criar(txtNome.Text.Trim(), txtEmail.Text.Trim(), txtUserN.Text.Trim(), txtSenha.Text.Trim());
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                return;
            }

            if (_usuario == null)
            {
                lblMsg.Text = "Não foi possível criar usuário!";
                txtSenha.Text = "";
                return;
            }

            lblMsg.Text = "Conta criada com sucesso!";

            //***********************************envia e-mail para pessoa q acabou de se cadastrar***********************************
            //se não conseguir enviar deve fazer alguma coisa pra enviar mais tarde, como guardar um valor em uma tabela para indicar isso, mas não há tanta necessidade disso
            // Especifica o servidor SMTP e a porta
            try
            {
                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                {

                    // EnableSsl ativa a comunicação segura com o servidor
                    client.EnableSsl = true;

                    // Especifica a credencial utilizada para envio da mensagem
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("ep3ada@gmail.com", "bandtec20172");

                    // Especifia o remetente e o destinatário da mensagem
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(
                        new System.Net.Mail.MailAddress("ep3ada@gmail.com", "Grupo Ask4Projekt", Encoding.UTF8),
                        new System.Net.Mail.MailAddress(txtEmail.Text.Trim()));

                    // Preenche o corpo e o assunto da mensagem
                    message.BodyEncoding = Encoding.UTF8;
                    message.Body = "Obrigado por se cadastrar no nosso site!\n Nome de Usuário: " + txtUserN.Text.Trim() + "\n Email: " + txtEmail.Text.Trim() + "\n\n\n Grupo Ask4Projekt.";
                    message.SubjectEncoding = Encoding.UTF8;
                    message.Subject = "Cadastro feito com sucesso!";

                    // Anexos devem ser adicionados através do método
                    // message.Attachments.Add()

                    // Envia a mensagem
                    client.Send(message);
                }
            }
            catch (Exception)
            {
                // Exceções devem ser tratadas aqui!
            }

            //zera caixas de texto
            txtNome.Text = "";
            txtEmail.Text = "";
            txtUserN.Text = "";
            txtSenha.Text = "";

            Response.Redirect("~/Login.aspx");
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (_usuario != null)
            {
                Response.Redirect("~/Perfil.aspx");
            }
            else
            {
                //ao clicar no botão voltar volta para a pagina default
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {
            bool mudouEmail = false;
            bool atualizou = false;
            string msg = "";
            SqlTransaction trans = null;

            try
            {

                if (string.IsNullOrWhiteSpace(txtNome.Text.Trim()) &&
                    string.IsNullOrWhiteSpace(txtEmail.Text.Trim()) &&
                    string.IsNullOrWhiteSpace(txtSenha.Text.Trim()) &&
                    string.IsNullOrWhiteSpace(txtUserN.Text.Trim()))
                {
                    throw new Exception();
                }

                using (SqlConnection conn = Sql.OpenConnection())
                {
                    trans = conn.BeginTransaction();

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(txtNome.Text.Trim()))
                        {
                            if (txtNome.Text.Trim().Length < 30)
                            {
                                using (SqlCommand cmd = new SqlCommand("Update usuario set nome = @nome where id_usuario = @id", conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@nome", txtNome.Text.Trim());
                                    cmd.Parameters.AddWithValue("@id", _usuario.Id);

                                    cmd.ExecuteNonQuery();
                                }

                            }
                            else
                            {
                                msg = "Campo Nome deve ter até 30 caracteres!";
                                throw new Exception();
                            }
                        }


                        if (!string.IsNullOrWhiteSpace(txtUserN.Text.Trim()))
                        {
                            if (txtUserN.Text.Trim().Length < 15)
                            {
                                bool temUn = false;
                                using (SqlCommand cmd = new SqlCommand("select username from usuario where username = @u", conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@u", txtUserN.Text.Trim());
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        if (reader.HasRows)
                                        {
                                            msg = "Username já utilizado!";
                                            temUn = true;
                                            throw new Exception();
                                        }
                                    }
                                }

                                if (temUn == false)
                                {
                                    using (SqlCommand cmd = new SqlCommand("Update usuario set username = @usern where id_usuario = @id", conn, trans))
                                    {
                                        cmd.Parameters.AddWithValue("@usern", txtUserN.Text.Trim());
                                        cmd.Parameters.AddWithValue("@id", _usuario.Id);
                                        cmd.ExecuteNonQuery();
                                        user = txtUserN.Text.Trim();
                                    }
                                }

                            }
                            else
                            {
                                msg = "Campo Username deve ter até 15 caracteres!";
                                throw new Exception();
                            }

                        }

                        if (!string.IsNullOrWhiteSpace(txtEmail.Text.Trim()))
                        {
                            bool temEm = false;
                            using (SqlCommand cmd = new SqlCommand("select email from usuario where email = @e", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        msg = "E-mail já utilizado!";
                                        temEm = true;
                                        throw new Exception();
                                    }
                                }
                            }

                            if (temEm == false)
                            {
                                using (SqlCommand cmd = new SqlCommand("Update usuario set email = @email where id_usuario = @id", conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                                    cmd.Parameters.AddWithValue("@id", _usuario.Id);
                                    cmd.ExecuteNonQuery();

                                    mudouEmail = true;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(txtSenha.Text.Trim()))
                        {
                            using (SqlCommand cmd = new SqlCommand("Update usuario set senha = @senha where id_usuario = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@senha", PasswordHash.CreateHash(txtSenha.Text.Trim()));
                                cmd.Parameters.AddWithValue("@id", _usuario.Id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        trans.Commit();
                        atualizou = true;
                    }
                    catch (Exception)
                    {
                        if (trans != null)
                            trans.Rollback();
                    }
                    finally
                    {
                        if (trans != null)
                            trans.Dispose();
                    }


                    if (atualizou)
                    {
                        //lblMsg.Text = "Atualizado com sucesso";
                        Response.Redirect("~/Perfil.aspx");
                    }
                    else if (msg != "")
                    {
                        //lblMsg.Text = "Não foi possível completar a operação!";
                        lblMsg.Text = msg;
                    }
                    else
                    {
                        lblMsg.Text = "Não foi possível completar a operação!";
                    }

                }
            }
            catch (Exception)
            {
                lblMsg.Text = "Erro de conexão inesperado!";
            }


            //*********************************************NAO SEI SE É O MLHOR LUGAR**********************************************
            //***********************************envia e-mail para pessoa q acabou de se cadastrar***********************************
            //se não conseguir enviar deve fazer alguma coisa pra enviar mais tarde, como guardar um valor em uma tabela para indicar isso, mas não há tanta necessidade disso

            // Especifica o servidor SMTP e a porta
            if (mudouEmail)
            {
                try
                {
                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                    {
                        // EnableSsl ativa a comunicação segura com o servidor
                        client.EnableSsl = true;

                        // Especifica a credencial utilizada para envio da mensagem
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential("ep3ada@gmail.com", "bandtec20172");

                        // Especifia o remetente e o destinatário da mensagem
                        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(
                            new System.Net.Mail.MailAddress("ep3ada@gmail.com", "Grupo Ask4Projekt", Encoding.UTF8),
                            new System.Net.Mail.MailAddress(txtEmail.Text.Trim()));

                        // Preenche o corpo e o assunto da mensagem
                        message.BodyEncoding = Encoding.UTF8;
                        message.Body = "Obrigado por se cadastrar no nosso site!\n Nome de Usuário: " + user + "\n Email: " + txtEmail.Text.Trim() + "\n\n\n Grupo Ask4Projekt.";
                        message.SubjectEncoding = Encoding.UTF8;
                        message.Subject = "Cadastro feito com sucesso!";

                        // Anexos devem ser adicionados através do método
                        // message.Attachments.Add()

                        // Envia a mensagem
                        client.Send(message);
                    }
                }
                catch (Exception)
                {
                    // Exceções devem ser tratadas aqui!
                }
            }


        }

        protected void aOUT_Click(object sender, EventArgs e)
        {
            //ao apertar logout chama metdo fazerlogout q apaga cookie e token do banco de dados
            //recarrega a pagina paracompletar a operação
            //Usuario u = Usuario.Validar();
            _usuario.FazerLogout();
            Response.Redirect("~/Default.aspx");
        }

    }
}