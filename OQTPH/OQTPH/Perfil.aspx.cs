using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OQTPH.Models;
using OQTPH.Utils;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

namespace OQTPH
{
    public partial class Perfil : System.Web.UI.Page
    {
        UsuarioModelo _usuario;
        public List<EventoModelo> _eventosCriados = new List<EventoModelo>();
        public List<EventoModelo> _eventosAdquiridos = new List<EventoModelo>();
        public List<EventoModelo> _eventosAlerta = new List<EventoModelo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            _usuario = UsuarioModelo.Validar();
            if (_usuario == null)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            //try
            //{
            using (SqlConnection conn = Sql.OpenConnection())
            {
                bool temDados = false;
                using (SqlCommand cmd = new SqlCommand("SELECT nome, email, username FROM Usuario where id_usuario = @id", conn))
                {
                    //substitui os parametros com arroba na query
                    cmd.Parameters.AddWithValue("@id", _usuario.Id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && reader.HasRows)
                        {
                            lblNome.Text = reader.GetString(0);
                            lblEmail.Text = reader.GetString(1);
                            lblUserN.Text = reader.GetString(2);
                            temDados = true;
                        }
                    }
                }

                if (temDados)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT id_evento, nome_evento, dt_evento FROM Evento where cod_criador = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _usuario.Id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dropEC.Items.Clear();
                                while (reader.Read() == true)
                                {

                                    ListItem item = new ListItem(reader.GetString(1), reader.GetInt32(0).ToString());
                                    dropEC.Items.Add(item);

                                    EventoModelo evento = new EventoModelo { Id = reader.GetInt32(0), Nome = reader.GetString(1), Data = reader.GetDateTime(2) };

                                    _eventosCriados.Add(evento);

                                }
                                lblEC.Visible = false;
                            }
                            else
                            {
                                //se não retornar valor escreve isso na div
                                //EvCriou.Controls.Add(new Label() { Text = "Não há nenhum." });
                                lblEC.Visible = true;
                                EvCriou.Visible = false;
                            }

                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT e.id_evento, e.nome_evento, e.dt_evento, c.dt_compra FROM EVENTO AS e join COMPRA AS c on (e.id_evento = c.cod_evento) where c.cod_pessoa = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _usuario.Id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read() == true)
                                {

                                    EventoModelo evento = new EventoModelo { Id = reader.GetInt32(0), Nome = reader.GetString(1), Data = reader.GetDateTime(2), DataCompra = reader.GetDateTime(3) };

                                    _eventosAdquiridos.Add(evento);
                                }
                                lblIA.Visible = false;
                            }
                            else
                            {
                                //se não retornar valor escreve isso na div
                                //EvComprou.Controls.Add(new Label() { Text = "Não há nenhum." });
                                lblIA.Visible = true;
                            }
                        }
                    }

                    string select = "";
                    bool tem;
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT filt from filtro where id_user = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", _usuario.Id);

                            select = cmd.ExecuteScalar().ToString();
                            tem = true;
                        }
                    }
                    catch (Exception)
                    {
                        //EvCat.Controls.Add(new Label() { Text = "Não há nenhum." });
                        divEP.Visible = false;
                        lblEP.Visible = true;
                        tem = false;
                    }


                    if (tem)
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT top 5 id_evento, nome_evento, dt_evento FROM evento where catg = @cat and dt_evento >= @now order by dt_criou desc", conn))
                        {
                            //antes de mudar pro retorno do scalar tava assim
                            //using (SqlCommand cmd = new SqlCommand("SELECT top 5 id_evento, nome_evento, dt_evento FROM evento where catg = @cat and dt_evento >= @now order by dt_criou desc", conn))
                            //{
                            //antiga query   SELECT top 5 id_evento, nome_evento, dt_evento FROM evento where catg = @cat order by dt_evento desc
                            cmd.Parameters.AddWithValue("@cat", select);
                            cmd.Parameters.AddWithValue("@now", DateTime.Now);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                //EvCat.Controls.Clear();
                                if (reader.HasRows)
                                {
                                    _eventosAlerta.Clear();
                                    while (reader.Read())
                                    {


                                        EventoModelo evento = new EventoModelo() { Id = reader.GetInt32(0), Nome = reader.GetString(1), Data = reader.GetDateTime(2) };
                                        _eventosAlerta.Add(evento);
                                    }

                                    lblEP.Visible = false;
                                    divEP.Visible = true;

                                }
                                else
                                {
                                    //EvCat.Controls.Add(new Label() { Text = "Não há nenhum." });
                                    divEP.Visible = false;
                                    lblEP.Visible = true;
                                }
                            }
                        }
                    }


                }
            }
            //}
            //catch (Exception)
            //{
            //faz alguma coisa, escreve na tela ou algo  do tipo
            //ouredireciona pra login por alguma razão
            //}
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            //vai pra pagina de edição que pode ser tanto a dados quanto edilçao mesmo
            Response.Redirect("~/Dados.aspx");
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            string cat = drop.SelectedItem.Value;

            if (cat != "Selecione" || taNoDrop(cat))
            {
                string select = "";

                // try
                //{
                using (SqlConnection conn = Sql.OpenConnection())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("update filtro set filt = @filt output inserted.filt where id_user = @id ", conn))
                        {
                            cmd.Parameters.AddWithValue("@filt", cat);
                            cmd.Parameters.AddWithValue("@id", _usuario.Id);

                            select = cmd.ExecuteScalar().ToString();
                            //tem = Convert.ToBoolean((int)cmd.ExecuteNonQuery());

                        }
                    }
                    catch (Exception)
                    {
                        using (SqlCommand cmd = new SqlCommand("insert into filtro output inserted.filt values(@filt, @id)", conn))
                        {
                            cmd.Parameters.AddWithValue("@filt", cat);
                            cmd.Parameters.AddWithValue("@id", _usuario.Id);

                            select = (string)cmd.ExecuteScalar();

                        }


                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT top 5 id_evento, nome_evento, dt_evento FROM evento where catg = @cat and dt_evento >= @now order by dt_criou desc", conn))
                    {
                        //antes de mudar pro retorno do scalar tava assim
                        //using (SqlCommand cmd = new SqlCommand("SELECT top 5 id_evento, nome_evento, dt_evento FROM evento where catg = @cat and dt_evento >= @now order by dt_criou desc", conn))
                        //{
                        //antiga query   SELECT top 5 id_evento, nome_evento, dt_evento FROM evento where catg = @cat order by dt_evento desc
                        cmd.Parameters.AddWithValue("@cat", select);
                        cmd.Parameters.AddWithValue("@now", DateTime.Now);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            //EvCat.Controls.Clear();
                            if (reader.HasRows)
                            {
                                _eventosAlerta.Clear();
                                while (reader.Read())
                                {
                                    EventoModelo evento = new EventoModelo() { Id = reader.GetInt32(0), Nome = reader.GetString(1), Data = reader.GetDateTime(2) };
                                    _eventosAlerta.Add(evento);

                                }

                                lblEP.Visible = false;
                                divEP.Visible = true;

                            }
                            else
                            {
                                //EvCat.Controls.Add(new Label() { Text = "Não há nenhum." });
                                divEP.Visible = false;
                                lblEP.Visible = true;
                            }
                        }
                    }

                }
                // }
                //catch (Exception)
                //{
                //}
            }
        }

        public bool taNoDrop(string s)
        {
            string[] vet = { "Ciência/Tecnologia", "Show", "Infantil", "Festa", "Teatro", "Concerto", "Stand-up", "Moda/Beleza", "Artes", "Business", "Dança", "Outras" };

            for (int i = 0; i < vet.Length; i++)
            {
                if (s == vet[i])
                {
                    return true;
                }
            }
            return false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool apagou = false;
                List<int> end = new List<int>();
                bool tem = false;
                SqlTransaction trans = null;
                using (SqlConnection conn = Sql.OpenConnection())
                {
                    trans = conn.BeginTransaction();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("delete from filtro where id_user = @id", conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@id", _usuario.Id);

                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand("delete from compra where cod_pessoa = @id", conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@id", _usuario.Id);

                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand("select cod_endereco from evento where cod_criador = @id", conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@id", _usuario.Id);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        end.Add(reader.GetInt32(0));
                                    }
                                    tem = true;
                                }
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand("delete from evento where cod_criador = @id", conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@id", _usuario.Id);
                            cmd.ExecuteNonQuery();
                        }

                        if (tem)
                        {
                            for (int i = 0; i < end.Count; i++)
                            {
                                using (SqlCommand cmd2 = new SqlCommand("delete from endereco where cod_endereco = @cod", conn, trans))
                                {
                                    cmd2.Parameters.AddWithValue("@cod", end[i]);
                                    cmd2.ExecuteNonQuery();
                                }
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand("delete from usuario where id_usuario = @id", conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@id", _usuario.Id);

                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                        apagou = true;
                    }
                    catch (Exception)
                    {
                        if (trans != null)
                        {
                            trans.Rollback();
                        }
                    }
                    finally
                    {
                        if (trans != null)
                        {
                            trans.Dispose();
                        }
                    }

                    if (apagou)
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Não foi possível concluir a operação!";
                    }

                }
            }
            catch (Exception)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Erro de conexão inesperado!";
            }
        }

        protected void btnDEC_Click(object sender, EventArgs e)
        {
            if (dropEC.SelectedItem != null)
            {
                bool apagou = false;
                int cod = int.Parse(dropEC.SelectedItem.Value);
                int end = 0;

                SqlTransaction trans = null;
                try
                {
                    using (SqlConnection conn = Sql.OpenConnection())
                    {
                        trans = conn.BeginTransaction();
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand("delete from compra where cod_evento = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@id", cod);
                                cmd.ExecuteNonQuery();
                            }


                            using (SqlCommand cmd = new SqlCommand("delete from evento where id_evento = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@id", cod);
                                cmd.ExecuteNonQuery();
                            }

                            using (SqlCommand cmd = new SqlCommand("select cod_endereco from evento where id_evento = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@id", cod);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        end = reader.GetInt32(0);
                                    }
                                }
                            }

                            if (end > 0)
                            {
                                using (SqlCommand cmd = new SqlCommand("delete from endereco where cod_endereco = @end", conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@end", end);
                                    cmd.ExecuteNonQuery();
                                }
                            }


                            trans.Commit();
                            apagou = true;
                        }
                        catch (Exception)
                        {
                            if (trans != null)
                            {
                                trans.Rollback();
                            }
                        }
                        finally
                        {
                            if (trans != null)
                            {
                                trans.Dispose();
                            }

                        }

                        if (apagou)
                        {
                            Response.Redirect("~/Perfil.aspx");
                        }

                    }
                }
                catch (Exception)
                {

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