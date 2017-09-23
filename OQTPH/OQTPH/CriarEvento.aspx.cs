using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OQTPH.Models;
using OQTPH.Utils;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace OQTPH
{
    public partial class CriarEvento : System.Web.UI.Page
    {
        Usuario _usuario;

        //será true se o usuário que estiver tentando acessar
        //a página com id do evento for o criador do mesmo
        //e será disponibilizada a edição do mesmo
        public bool _ehMeu;

        public int _idEvento;
       // public DateTime DataDB;
        public int _idEndereco;

        protected void Page_Load(object sender, EventArgs e)
        {
            //verifica se usuario está logado, se não estiver redireciona para a página de Login
            _usuario = Usuario.Validar();

            if (_usuario == null)
            {
                Response.Redirect("~/Login.aspx?p=Ev");
                return;
            }
            else
            {
                //mudar para modif ao inves de p
                //if (int.TryParse(Request.QueryString["modif"], out _idEvento) == false)
                if (int.TryParse(Request.QueryString["p"], out _idEvento) == false)
                {
                    return;
                }

                using (SqlConnection conn = Sql.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("select ev.nome_evento, ev.dt_evento, ev.desc_evento, ev.nro_ingressos, " +
                        "ev.telefone, ev.valor, ev.catg, en.logradouro, en.nro_log, en.bairro, en.cidade, en.estado, en.cod_endereco from evento ev join " +
                        "endereco en on(ev.cod_endereco = en.cod_endereco) where cod_criador = @id and id_evento = @ev", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _usuario.Id);
                        cmd.Parameters.AddWithValue("@ev", _idEvento);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //DataDB = reader.GetDateTime(1);
                                _idEndereco = reader.GetInt32(12);

                                _ehMeu = true;

                                placeholderEvento.Visible = true;
                                btnCriar.Visible = false;
                                btnAtualiza.Visible = true;

                                if (!IsPostBack)
                                {
                                    img.Src = string.Format("~/Img.ashx?idimgev={0}", _idEvento);
                                    txtfone.Text = reader.GetString(4);
                                    txtNomeEv.Text = reader.GetString(0);
                                    txtDescEv.Text = reader.GetString(2);
                                    txtIngEv.Text = reader.GetInt32(3).ToString();
                                    txtDT.Text = reader.GetDateTime(1).Date.ToString("d");
                                    txtHR.Text = reader.GetDateTime(1).ToString("HH:mm");
                                    txtEnd.Text = reader.GetString(7);
                                    txtNroEnd.Text = reader.GetInt32(8).ToString();
                                    txtBairroEnd.Text = reader.GetString(9);
                                    txtCid.Text = reader.GetString(10);
                                    txtEst.Text = reader.GetString(11);
                                    string valor = reader.GetDouble(5).ToString();
                                    txtValEv.Text = valor.Replace(".", ",");

                                    for (int i = 0; i < dropCatg.Items.Count; i++)
                                    {
                                        if (reader.GetString(6) == null)
                                        {
                                            i = dropCatg.Items.Count - 1;
                                        }
                                        if (dropCatg.Items[i].Value == reader.GetString(6))
                                        {
                                            dropCatg.SelectedIndex = i;
                                            i = dropCatg.Items.Count - 1;
                                        }
                                    }
                                }

                                Page.Title = "Atualizar Evento - OQTPH";
                            }
                        }
                    }
                }
            }
        }

        protected void btnCriar_Click(object sender, EventArgs e)
        {
            bool criou = false;

            string nome, descricao, categoria, telefone, bairro, cidade, estado, endereco;
            int nroIngressos, nroEndereco;
            float valorDoIngresso;
            DateTime dtaEv, hra, data;

            //*****************Parte do evento
            if (string.IsNullOrWhiteSpace(txtNomeEv.Text.Trim()) || txtNomeEv.Text.Length > 80)
            { lblMsg.Text = "Nome inválido!"; return; }
            nome = txtNomeEv.Text;

            if (string.IsNullOrWhiteSpace(txtDescEv.Text.Trim()) || txtDescEv.Text.Length > 700)
            { lblMsg.Text = "Descrição inválida!"; return; }
            descricao = txtDescEv.Text;

            if (float.TryParse(txtValEv.Text.Trim().Replace(",", "."), System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out valorDoIngresso) == false || valorDoIngresso < 0)
            { lblMsg.Text = "Digite um valor válido!"; return; }

            if (dropCatg.SelectedItem.Value == "0")
            { lblMsg.Text = "Selecione Categoria!"; return; }
            categoria = dropCatg.SelectedItem.Value;

            if (int.TryParse(txtIngEv.Text.Trim(), out nroIngressos) == false || nroIngressos <= 0)
            { lblMsg.Text = "Digite número de ingressos válidos!"; return; }

            Regex reg1 = new Regex(@"^\(\d{2}\) \d{4}-\d{5}$", RegexOptions.None);
            Regex reg2 = new Regex(@"^\(\d{2}\) \d{4}-\d{4}$", RegexOptions.None);


            if (string.IsNullOrWhiteSpace(txtfone.Text.Trim()) == true || (reg1.IsMatch(txtfone.Text.Trim()) == false && reg2.IsMatch(txtfone.Text.Trim()) == false))
            {
                lblMsg.Text = "Telefone inválido!";
                return;
            }
            telefone = txtfone.Text.Trim();

            //******************Parte do endereco
            if (string.IsNullOrWhiteSpace(txtEnd.Text.Trim()))
            { lblMsg.Text = "Logradouro inválido!"; return; }
            endereco = txtEnd.Text;

            if (int.TryParse(txtNroEnd.Text.Trim(), out nroEndereco) == false && nroEndereco <= 0)
            { lblMsg.Text = "Número de endereço inválido!"; return; }

            if (string.IsNullOrWhiteSpace(txtBairroEnd.Text.Trim()))
            { lblMsg.Text = "Bairro inválido!"; return; }
            bairro = txtBairroEnd.Text;

            if (string.IsNullOrWhiteSpace(txtCid.Text.Trim()))
            { lblMsg.Text = "Cidade inválida!"; return; }
            cidade = txtCid.Text;

            if (string.IsNullOrWhiteSpace(txtEst.Text.Trim()))
            { lblMsg.Text = "Estado inválido!"; return; }
            estado = txtEst.Text;

            if (DateTime.TryParse(txtDT.Text.Trim(), System.Globalization.CultureInfo.GetCultureInfo("pt-br"), System.Globalization.DateTimeStyles.None, out dtaEv) == false || (DateTime.Compare(dtaEv.Date, DateTime.Now.Date) < 0))
            { lblMsg.Text = "Data inválida"; return; }

            if (DateTime.TryParse(txtHR.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture /* change if appropriate */, System.Globalization.DateTimeStyles.None, out hra) == false)
            { lblMsg.Text = "Hora inválida!"; return; }

            data = new DateTime(dtaEv.Year, dtaEv.Month, dtaEv.Day, hra.Hour, hra.Minute, hra.Second);

            if (DateTime.Compare(data.Date, DateTime.Now.Date) == 0)
            {
                if (TimeSpan.Compare(data.TimeOfDay, DateTime.Now.TimeOfDay) < 0)
                {
                    lblMsg.Text = "Hora inválida!";
                    return;
                }
            }

            //********************Parte da imagem
            if (!up.HasFile)
            { lblMsg.Text = "Selecione uma foto!"; return; }

            if (up.PostedFile.ContentType != "image/png" && up.PostedFile.ContentType != "image/jpg" && up.PostedFile.ContentType != "image/jpeg")
            { lblMsg.Text = "Selecione um arquivo do tipo imagem!"; return; }

            SqlTransaction trans = null;
            try
            {
                using (SqlConnection conn = Sql.OpenConnection())
                {
                    trans = conn.BeginTransaction();

                    try
                    {
                        //variavel que guardará o id do endereco
                        int idEndereco;

                        //envia os dados para a tabela endereco
                        using (SqlCommand command = new SqlCommand("INSERT INTO Endereco (logradouro, nro_log, bairro, cidade, estado)" +
                            "OUTPUT INSERTED.cod_endereco VALUES (@log, @nlog, @bair, @cid, @est)", conn, trans))
                        {
                            //substitui os parametros com arroba(@) pelos valores digitados pelo usuario
                            command.Parameters.AddWithValue("@log", endereco);
                            command.Parameters.AddWithValue("@nlog", nroEndereco);
                            command.Parameters.AddWithValue("@bair", bairro);
                            command.Parameters.AddWithValue("@cid", cidade);
                            command.Parameters.AddWithValue("@est", estado);

                            idEndereco = (int)command.ExecuteScalar();
                            //command.ExecuteScalar();
                        }

                        //envia esses dados para a tabela evento
                        using (SqlCommand command = new SqlCommand("INSERT INTO EVENTO (Nome_Evento, dt_evento, desc_evento, nro_ingressos," +
                            "cod_criador, cod_endereco, telefone, valor, catg, dt_criou) OUTPUT Inserted.id_evento VALUES (@nome, @dt, @desc, @nro," +
                            "@criador, @ender, @fone, @val, @cat, @dtcria)", conn, trans))
                        {
                            //substitui os parametros com arroba(@) pelos valores digitados pelo usuario
                            command.Parameters.AddWithValue("@nome", nome);
                            command.Parameters.AddWithValue("@dt", data);
                            command.Parameters.AddWithValue("@desc", descricao);
                            command.Parameters.AddWithValue("@nro", nroIngressos);

                            command.Parameters.AddWithValue("@criador", _usuario.Id);
                            command.Parameters.AddWithValue("@ender", idEndereco);

                            //tem q validar se é um telefone pq nçao tá validando
                            command.Parameters.AddWithValue("@fone", telefone);

                            command.Parameters.AddWithValue("@val", valorDoIngresso);
                            command.Parameters.AddWithValue("@cat", categoria);

                            //salva a data da criação do evento no banco de dados
                            command.Parameters.AddWithValue("@dtcria", DateTime.Now);

                            int idEvento = (int)command.ExecuteScalar();

                            up.SaveAs(Server.MapPath($"~/App_Data/Images/{idEvento}-imagem.jpg"));
                        }

                        trans.Commit();
                        criou = true;
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

                    if (criou)
                    {
                        lblMsg.Text = "Evento criado com sucesso!";

                        txtNomeEv.Text = "";
                        txtDescEv.Text = "";
                        txtValEv.Text = "";
                        dropCatg.SelectedIndex = 0;
                        txtfone.Text = "";
                        txtIngEv.Text = "";
                        txtEnd.Text = "";
                        txtNroEnd.Text = "";
                        txtBairroEnd.Text = "";
                        txtCid.Text = "";
                        txtEst.Text = "";

                    }
                    else
                    {
                        lblMsg.Text = "Não foi possível concluir a operação!";
                    }

                }
            }
            catch (Exception)
            {
                lblMsg.Text = "Erro de conexão inesperado!";
            }

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (_ehMeu == true)
            {
                Response.Redirect("~/Perfil.aspx");
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnAtualiza_Click(object sender, EventArgs e)
        {
            bool atualizou = false;
            string msg = "";

            SqlTransaction trans = null;
            try
            {
                using (SqlConnection conn = Sql.OpenConnection())
                {
                    trans = conn.BeginTransaction();
                    try
                    {
                        if (string.IsNullOrWhiteSpace(txtNomeEv.Text.Trim()))
                        {
                            msg = "Preencha o campo Nome!";
                            throw new Exception();
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand("update evento set nome_evento = @p where id_evento = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@p", txtNomeEv.Text.Trim());
                                cmd.Parameters.AddWithValue("@id", _idEvento);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (string.IsNullOrWhiteSpace(txtDescEv.Text.Trim()))
                        {
                            msg = "Preencha o campo Descrição!";
                            throw new Exception();
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand("update evento set desc_evento = @p where id_evento = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@p", txtDescEv.Text.Trim());
                                cmd.Parameters.AddWithValue("@id", _idEvento);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (string.IsNullOrWhiteSpace(txtValEv.Text.Trim()))
                        {
                            msg = "Preencha o campo Valor!";
                            throw new Exception();
                        }
                        else
                        {
                            float v;
                            if (float.TryParse(txtValEv.Text.Trim(), out v) == false || v < 0.0)
                            {
                                msg = "Valor do evento inválido!";
                                throw new Exception();
                            }
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("update evento set valor = @p where id_evento = @id", conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@p", v);
                                    cmd.Parameters.AddWithValue("@id", _idEvento);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        if (dropCatg.SelectedIndex != 0)
                        {
                            using (SqlCommand cmd = new SqlCommand("update evento set catg = @p where id_evento = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@p", dropCatg.SelectedItem.Value);
                                cmd.Parameters.AddWithValue("@id", _idEvento);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        Regex reg1 = new Regex(@"^\(\d{2}\) \d{4}-\d{5}$", RegexOptions.None);
                        Regex reg2 = new Regex(@"^\(\d{2}\) \d{4}-\d{4}$", RegexOptions.None);

                        if (string.IsNullOrWhiteSpace(txtfone.Text.Trim()))
                        {
                            msg = "Preencha o campo Telefone!";
                            throw new Exception();
                        }
                        else if (reg1.IsMatch(txtfone.Text.Trim()) == false && reg2.IsMatch(txtfone.Text.Trim()) == false)
                        {
                            msg = "Telefone inválido!";
                            throw new Exception();
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand("update evento set telefone = @p where id_evento = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@p", txtfone.Text.Trim());
                                cmd.Parameters.AddWithValue("@id", _idEvento);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (string.IsNullOrWhiteSpace(txtIngEv.Text.Trim()))
                        {
                            msg = "Preencha o campo Nro de Ingressos!";
                            throw new Exception();
                        }
                        else
                        {
                            int v;
                            if (int.TryParse(txtIngEv.Text.Trim(), out v) == false || v < 0)
                            {
                                msg = "Nro de ingressos inválido!";
                                throw new Exception();
                            }
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("update evento set nro_ingressos = @p where id_evento = @id", conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@p", v);
                                    cmd.Parameters.AddWithValue("@id", _idEvento);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        DateTime dtEv, timeEv, dataEv;
                        if (string.IsNullOrWhiteSpace(txtDT.Text.Trim()))
                        {
                            msg = "Preencha o campo Data!";
                            throw new Exception();
                        }
                        else if (DateTime.TryParse(txtDT.Text.Trim(), System.Globalization.CultureInfo.GetCultureInfo("pt-br"), System.Globalization.DateTimeStyles.None, out dataEv) == false || (DateTime.Compare(dataEv.Date, DateTime.Now.Date) < 0))
                        {
                            msg = "Data inválida!";
                            throw new Exception();
                        }

                        if (string.IsNullOrWhiteSpace(txtHR.Text.Trim()))
                        {
                            msg = "Preencha o campo Hora!";
                            throw new Exception();
                        }
                        else if (DateTime.TryParse(txtHR.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture /* change if appropriate */, System.Globalization.DateTimeStyles.None, out timeEv) == false)
                        {
                            msg = "Hora inválida!";
                            throw new Exception();
                        }

                        dtEv = new DateTime(dataEv.Year, dataEv.Month, dataEv.Day, timeEv.Hour, timeEv.Minute, timeEv.Second);

                        if (DateTime.Compare(dtEv, DateTime.Now) < 0)
                        {
                            msg = "Hora inválida!";
                            throw new Exception();
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand("update evento set dt_evento = @p where id_evento = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@p", dtEv);
                                cmd.Parameters.AddWithValue("@id", _idEvento);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (string.IsNullOrWhiteSpace(txtEnd.Text.Trim()))
                        {
                            msg = "Preencha o campo Logradouro!";
                            throw new Exception();
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand("update endereco set logradouro = @p where cod_endereco = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@p", txtEnd.Text.Trim());
                                cmd.Parameters.AddWithValue("@id", _idEndereco);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (string.IsNullOrWhiteSpace(txtNroEnd.Text.Trim()))
                        {
                            msg = "Preencha o campo Número!";
                            throw new Exception();
                        }
                        else
                        {
                            int n;
                            if (int.TryParse(txtNroEnd.Text.Trim(), out n) == false || n < 0)
                            {
                                msg = "Digite um número de endereco válido!";
                                throw new Exception();
                            }
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("update endereco set nro_log = @p where cod_endereco = @id", conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@p", n);
                                    cmd.Parameters.AddWithValue("@id", _idEndereco);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        if (string.IsNullOrWhiteSpace(txtBairroEnd.Text.Trim()))
                        {
                            msg = "Preencha o campo Bairro!";
                            throw new Exception();
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand("update endereco set bairro = @p where cod_endereco = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@p", txtBairroEnd.Text.Trim());
                                cmd.Parameters.AddWithValue("@id", _idEndereco);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (string.IsNullOrWhiteSpace(txtCid.Text.Trim()))
                        {
                            msg = "Preecnha o campo Cidade!";
                            throw new Exception();
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand("update endereco set cidade = @p where cod_endereco = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@p", txtCid.Text.Trim());
                                cmd.Parameters.AddWithValue("@id", _idEndereco);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (string.IsNullOrWhiteSpace(txtEst.Text.Trim()))
                        {
                            msg = "Preecnha o campo Estado!";
                            throw new Exception();
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand("update endereco set estado = @p where cod_endereco = @id", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@p", txtEst.Text.Trim());
                                cmd.Parameters.AddWithValue("@id", _idEndereco);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (up.HasFile)
                        {
                            if (up.PostedFile.ContentType == "image/png" || up.PostedFile.ContentType == "image/jpg" || up.PostedFile.ContentType == "image/jpeg")
                            {
                                up.SaveAs(Server.MapPath($"~/App_Data/Images/{_idEvento}-imagem.jpg"));
                            }
                            else
                            {
                                msg = "Selecione um arquivo do tipo imagem!";
                                throw new Exception();
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
                        Response.Redirect($"~/Evento.aspx?evento={_idEvento}");
                    }
                    else if (msg != "")
                    {
                        lblMsg.Text = msg;
                    }
                    else
                    {
                        lblMsg.Text = "Não foi possível concluir a operação!";
                    }
                }
            }
            catch (Exception)
            {
                lblMsg.Text = "Erro de conexão inesperado!";
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