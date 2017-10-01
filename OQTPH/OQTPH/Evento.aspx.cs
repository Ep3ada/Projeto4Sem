using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using OQTPH.Utils;
using OQTPH.Models;

namespace OQTPH
{
    public partial class Evento : System.Web.UI.Page
    {
        UsuarioModelo u;
        public int ing;
        public int idEv;
        public bool jaComprei = false;
        public bool ehMeu = false;
        public bool temIng = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            u = UsuarioModelo.Validar();
            if (u != null)
            {
                PHin.Visible = false;
                PHout.Visible = true;
            }
            else
            {
                PHin.Visible = true;
                PHout.Visible = false;
            }



            //if (!IsPostBack)
            //{
            if (int.TryParse(Request.QueryString["evento"], out idEv) == false || idEv <= 0)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            try
            {
                using (SqlConnection conn = Sql.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT ev.nome_evento, ev.desc_evento, ev.nro_ingressos, ev.dt_evento," +
                        "ev.telefone, ev.catg, ev.valor, en.logradouro, en.nro_log, en.bairro, en.cidade," +
                        "en.estado, u.nome, ev.cod_criador FROM Evento ev join endereco en on(ev.cod_endereco = en.cod_endereco) join usuario u on" +
                        "(ev.cod_criador = u.id_usuario) where ev.id_evento = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idEv);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                img.Src = string.Format("~/Img.ashx?idimgev={0}", idEv);
                                lblNome.Text = reader.GetString(0);

                                string desc = reader.GetString(1);
                                desc = desc.Replace("\n", "<br />");
                                lblDesc.Text = desc;

                                lblIngres.Text = reader.GetInt32(2).ToString();

                                ing = reader.GetInt32(2);
                                temIng = ing > 0;

                                lblDT.Text = reader.GetDateTime(3).Date.ToString("d");
                                lblHora.Text = reader.GetDateTime(3).ToString("HH:mm");
                                lblFone.Text = reader.GetString(4);

                                //idEnd = reader.GetInt32(6);
                                lblCat.Text = reader.GetString(5);
                                lblVal.Text = string.Format("{0:C}", reader.GetDouble(6));

                                lblLocal.Text = reader.GetString(7);
                                lblLocal.Text += ", " + reader.GetInt32(8);
                                lblLocal.Text += ", " + reader.GetString(9);
                                lblLocal.Text += ", " + reader.GetString(10);
                                lblLocal.Text += " - " + reader.GetString(11);

                                lblCriador.Text = reader.GetString(12);

                                if (u != null)
                                {
                                    ehMeu = reader.GetInt32(13) == u.Id;
                                }
                            }
                            else
                            {
                                div1.Visible = false;
                                div2.Visible = false;
                                lblMsg.Text = "Este evento não existe! Vá para home.";
                                return;
                            }
                        }
                    }

                    if (!temIng)
                    {
                        btn.Visible = false;
                        lblMsg.Text = "Este evento não tem mais ingressos!";
                    }
                    else
                    {
                        if (u != null && ehMeu == true)
                        {
                            //btn.Text = "Editar";
                            btn.Visible = false;
                            btnEdit.Visible = true;
                        }
                        else if (u != null && ehMeu == false)
                        {
                            try
                            {
                                using (SqlCommand cmd = new SqlCommand("SELECT cod_compra from compra where cod_evento = @ev and cod_pessoa = @pes", conn))
                                {
                                    cmd.Parameters.AddWithValue("@ev", idEv);
                                    cmd.Parameters.AddWithValue("@pes", u.Id);
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            //btnCompra.Text = "Não quero mais!";
                                            //btn.Text = "Devolver Ingresso!";
                                            btn.Visible = false;
                                            btnD.Visible = true;
                                            jaComprei = true;
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                btn.Visible = true;
                                //btn.Text = "Adquirir Ingresso";
                                //jaComprei = false;
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                div1.Visible = div2.Visible = false;
                lblMsg.Text = "Não foi possível concluir a operação!";
                return;
            }

            //}

        }

        protected void btn_Click(object sender, EventArgs e)
        {
            if (u == null)
            {
                //pode colocar na query toEV=66 e aí faz na login pra redirecionar pra esse evento de novo
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                try
                {
                    using (SqlConnection conn = Sql.OpenConnection())
                    {
                        int i = 0;
                        using (SqlCommand cmd = new SqlCommand("select nro_ingressos from evento where id_evento = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", idEv);
                            i = (int)cmd.ExecuteScalar();
                        }
                        if (i != 0)
                        {
                            //adiciona linha em compra
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO COMPRA (cod_evento, cod_pessoa, dt_compra) values(@ev, @us, @dt)", conn))
                            {
                                cmd.Parameters.AddWithValue("@ev", idEv);
                                cmd.Parameters.AddWithValue("@us", u.Id);
                                cmd.Parameters.AddWithValue("@dt", DateTime.Now);
                                cmd.ExecuteNonQuery();
                            }
                            //decremneta o valor de ingressos no banco
                            using (SqlCommand cmd = new SqlCommand("UPDATE evento set nro_ingressos = @n where id_evento = @id", conn))
                            {
                                cmd.Parameters.AddWithValue("@n", i - 1);
                                cmd.Parameters.AddWithValue("@id", idEv);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    lblMsg.Text = "Não foi possível concluir a operação!";
                    return;
                }
                lblMsg.Text = "Adquirido!";

                try
                {
                    using (SqlConnection conn = Sql.OpenConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT ev.nome_evento, ev.desc_evento, ev.nro_ingressos, ev.dt_evento," +
                            "ev.telefone, ev.catg, ev.valor, en.logradouro, en.nro_log, en.bairro, en.cidade," +
                            "en.estado, u.nome, ev.cod_criador FROM Evento ev join endereco en on(ev.cod_endereco = en.cod_endereco) join usuario u on" +
                            "(ev.cod_criador = u.id_usuario) where ev.id_evento = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", idEv);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    img.Src = string.Format("~/Img.ashx?idimgev={0}", idEv);
                                    lblNome.Text = reader.GetString(0);

                                    string desc = reader.GetString(1);
                                    desc = desc.Replace("\n", "<br />");
                                    lblDesc.Text = desc;

                                    lblIngres.Text = reader.GetInt32(2).ToString();
                                    //ing = reader.GetInt32(2);
                                    //semIng = ing > 0;
                                    temIng = reader.GetInt32(2) > 0;

                                    lblDT.Text = reader.GetDateTime(3).Date.ToString("d");
                                    lblHora.Text = reader.GetDateTime(3).ToString("HH:mm");
                                    lblFone.Text = reader.GetString(4);

                                    lblCat.Text = reader.GetString(5);
                                    lblVal.Text = string.Format("{0:C}", reader.GetDouble(6));

                                    lblLocal.Text = reader.GetString(7);
                                    lblLocal.Text += ", " + reader.GetInt32(8);
                                    lblLocal.Text += ", " + reader.GetString(9);
                                    lblLocal.Text += ", " + reader.GetString(10);
                                    lblLocal.Text += " - " + reader.GetString(11);

                                    lblCriador.Text = reader.GetString(12);
                                }
                                else
                                {
                                    div1.Visible = false;
                                    div2.Visible = false;
                                    lblMsg.Text = "Este evento não existe! Vá para home.";
                                    return;
                                }
                            }
                        }

                        if (!temIng)
                        {
                            btn.Visible = false;
                            lblMsg.Text = "Este evento não tem mais ingressos!";
                        }
                        else
                        {
                            try
                            {
                                using (SqlCommand cmd = new SqlCommand("SELECT cod_compra from compra where cod_evento = @ev and cod_pessoa = @pes", conn))
                                {
                                    cmd.Parameters.AddWithValue("@ev", idEv);
                                    cmd.Parameters.AddWithValue("@pes", u.Id);
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            //btnCompra.Text = "Não quero mais!";
                                            //btn.Text = "Devolver Ingresso!";
                                            btn.Visible = false;
                                            btnD.Visible = true;
                                            jaComprei = true;
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                btn.Visible = true;
                                //btn.Text = "Adquirir Ingresso";
                                //jaComprei = false;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }

            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/CriaEvento.aspx?p={idEv}");
        }

        protected void btnD_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = Sql.OpenConnection())
                {
                    int i = 0;
                    using (SqlCommand cmd = new SqlCommand("select nro_ingressos from evento where id_evento = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idEv);
                        i = (int)cmd.ExecuteScalar();
                    }
                    if (i != 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("delete from compra where cod_evento = @ev and cod_pessoa = @pes", conn))
                        {
                            cmd.Parameters.AddWithValue("@ev", idEv);
                            cmd.Parameters.AddWithValue("@pes", u.Id);
                            cmd.ExecuteNonQuery();
                        }
                        using (SqlCommand cmd = new SqlCommand("UPDATE evento set nro_ingressos = @n where id_evento = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@n", ing + 1);
                            cmd.Parameters.AddWithValue("@id", idEv);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblMsg.Text = "Não foi possível concluir a operação!";
                return;
            }
            lblMsg.Text = "Devolvido!";
            btnD.Visible = false;
            btn.Visible = true;

            try
            {
                using (SqlConnection conn = Sql.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT ev.nome_evento, ev.desc_evento, ev.nro_ingressos, ev.dt_evento," +
                        "ev.telefone, ev.catg, ev.valor, en.logradouro, en.nro_log, en.bairro, en.cidade," +
                        "en.estado, u.nome, ev.cod_criador FROM Evento ev join endereco en on(ev.cod_endereco = en.cod_endereco) join usuario u on" +
                        "(ev.cod_criador = u.id_usuario) where ev.id_evento = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idEv);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                img.Src = string.Format("~/Img.ashx?idimgev={0}", idEv);
                                lblNome.Text = reader.GetString(0);

                                string desc = reader.GetString(1);
                                desc = desc.Replace("\n", "<br />");
                                lblDesc.Text = desc;

                                lblIngres.Text = reader.GetInt32(2).ToString();
                                temIng = reader.GetInt32(2) > 0;

                                lblDT.Text = reader.GetDateTime(3).Date.ToString("d");
                                lblHora.Text = reader.GetDateTime(3).ToString("HH:mm");
                                lblFone.Text = reader.GetString(4);

                                lblCat.Text = reader.GetString(5);
                                lblVal.Text = string.Format("{0:C}", reader.GetDouble(6));

                                lblLocal.Text = reader.GetString(7);
                                lblLocal.Text += ", " + reader.GetInt32(8);
                                lblLocal.Text += ", " + reader.GetString(9);
                                lblLocal.Text += ", " + reader.GetString(10);
                                lblLocal.Text += " - " + reader.GetString(11);

                                lblCriador.Text = reader.GetString(12);
                            }
                            else
                            {
                                div1.Visible = false;
                                div2.Visible = false;
                                lblMsg.Text = "Este evento não existe! Vá para home.";
                                return;
                            }
                        }
                    }

                    if (!temIng)
                    {
                        btn.Visible = false;
                        lblMsg.Text = "Este evento não tem mais ingressos!";
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void aOUT_Click(object sender, EventArgs e)
        {
            //ao apertar logout chama metdo fazerlogout q apaga cookie e token do banco de dados
            //recarrega a pagina paracompletar a operação
            //Usuario u = Usuario.Validar();
            u.FazerLogout();
            Response.Redirect("~/Default.aspx");
        }
    }
}