using System;
using System.Web.UI.WebControls;
using OQTPH.Models;
using System.Data.SqlClient;
using OQTPH.Utils;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;


namespace OQTPH
{
    public partial class Default : System.Web.UI.Page
    {
        UsuarioModelo u;

        public List<EventoModelo> lista = new List<EventoModelo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //guarda numero q será usado para acessar as paginas anterior e proxima
            int ant, prox;
            //guarda o valor do filtro q tem os valores vindos do dropdown
            string filtro;
            //cria uma constante q define o numero max de itens por pagina
            const int itemPPage = 6;
            //variavel q receberá o numero de itens presentes no banco
            int numDeEvs;
            //guarda numero da pagina atual
            int numpage;
            //guarda numero gerado por conta realizada com numero de eventos no banco
            int maxpages;

            //verifica se há um cookie com as infos do usuário | se tiver mostra botão de logout| se não botão sign in e sign up
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

            //verifica se há algum valor no parametro 'page' e se esse valor é inteiro
            //caso não haja valor inteiro, atribui 1 à 'page'
            if ((int.TryParse(Request.QueryString["page"], out numpage) == false) || numpage < 1)
            {
                numpage = 1;
            }

            if (Request.QueryString["filter"] == null || taNoDrop(Request.QueryString["filter"]) == false)
            {
                filtro = null;
            }
            else
            {
                filtro = Request.QueryString["filter"].ToString();
            }

            if (!IsPostBack)
            {
                for (int i = 0; i < drop.Items.Count; i++)
                {
                    if (filtro == null)
                    {
                        i = drop.Items.Count - 1;
                    }
                    if (drop.Items[i].Value == filtro)
                    {
                        drop.SelectedIndex = i;
                        i = drop.Items.Count - 1;
                    }
                }
            }

            if (filtro == "Todas" || filtro == null)
            {

                //dá select count, peg ao resultado e divide pelo numero de intenns por pagina, dai pega esse outro result e cria os links
                //offset numero de itenpopag * pagenum-1 fetch itensporpag rows only
                //  try
                //{
                using (SqlConnection conn = Sql.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Count(id_evento) FROM Evento", conn))
                    {
                        numDeEvs = (int)cmd.ExecuteScalar();
                    }

                    // maxpages recebe calculo pra definir numero máximo de páginas
                    //verifica se o numero passado na query string é maior q o numero maximo de paginas
                    //se for maior, atribui o maxpages q representa a ultima pagina, para não fazer busca por pagina inexistente
                    maxpages = (int)Math.Ceiling((double)numDeEvs / (double)itemPPage);

                    if (maxpages == 0)
                    {
                        numpage = 1;
                    }
                    else if (maxpages < numpage)
                    {
                        numpage = maxpages;
                    }

                    // verifica se há itens para se mostrar, caso não haja deve mostrar em um label q não tem ou uma imagem
                    if (numDeEvs > 0)
                    {
                        //faz busca por nome e id do evento na tabela evento por eventos q ainda tenham ingresso
                        using (SqlCommand cmd = new SqlCommand("SELECT Nome_evento, id_evento FROM Evento where nro_ingressos > 0 order by nome_evento offset @nItens * (@page - 1) rows fetch next @nItens rows only", conn))
                        {
                            //substitui os parametros com arroba na query pelo numero da pagina e pelo numero de itens por pagina
                            cmd.Parameters.AddWithValue("@page", numpage);
                            cmd.Parameters.AddWithValue("@nItens", itemPPage);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                // Tenta obter o registro
                                while (reader.Read() == true)
                                {

                                    string nomeEv = reader.GetString(0);
                                    int idEv = reader.GetInt32(1);

                                    EventoModelo evento = new EventoModelo() { Id = idEv, Nome = nomeEv };
                                    lista.Add(evento);
                                }
                            }
                        }
                    }
                    //else{ coloca uma mensagem dizendo q não tem eventos ou uma imagem

                }
                //  }
                //catch (Exception)
                //{
                //lblMsg.Text = "Erro na conexão!";
                //}

                //pega numero total e divide pelo tanto de itens por pagina
                //faz assim int maxpages = (int)Math.ceiling((double)num, (double)nitems);
                //*****int maxpages = (int)Math.Ceiling((double)numDeEvs / (double)itemPPage);
                ant = numpage - 1;
                prox = numpage + 1;

                if (numpage < 2)
                {
                    lbCur.Text = "1";
                }
                else
                {
                    lbCur.Text = numpage.ToString();
                }

                //se o numero da pagina é maior q 1 e menor q maxpages
                //atribui as tags <a> da pagina anterior e da proxima com o valor da pagina atual menos 1 para a anterior e da atual mais 1 para a proxima
                if (numpage > 1 && numpage < maxpages)
                {
                    Aant.Attributes.Add("href", string.Format("~/Default.aspx?filter={0}&page={1}", filtro, ant));
                    Aprox.Attributes.Add("href", string.Format("~/Default.aspx?filter={0}&page={1}", filtro, prox));

                }
                else
                {
                    //caso seja igual a 1 link anterior é desativada e prox é pagina atual mais 1
                    //caso seja igual a maxpages pro´xima é desativada e ant é atual menos 1
                    if (numpage == 1)
                    {
                        Aant.Disabled = true;
                        Aprox.Attributes.Add("href", string.Format("~/Default.aspx?filter={0}&page={1}", filtro, prox));
                    }
                    else if (numpage == maxpages)
                    {
                        Aprox.Disabled = true;
                        Aant.Attributes.Add("href", string.Format("~/Default.aspx?filter={0}&page={1}", filtro, ant));
                    }
                }

            }
            else
            {

                //dá select count, peg ao resultado e divide pelo numero de intenns por pagina, dai pega esse outro result e cria os links
                //offset numero de itenpopag * pagenum-1 fetch itensporpag rows only
                //  try
                //{
                using (SqlConnection conn = Sql.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Count(id_evento) FROM Evento where catg = @cat", conn))
                    {
                        cmd.Parameters.AddWithValue("@cat", filtro);

                        numDeEvs = (int)cmd.ExecuteScalar();
                    }

                    // maxpages recebe calculo pra definir numero máximo de páginas
                    //verifica se o numero passado na query string é maior q o numero maximo de paginas
                    //se for maior, atribui o maxpages q representa a ultima pagina, para não fazer busca por pagina inexistente
                    maxpages = (int)Math.Ceiling((double)numDeEvs / (double)itemPPage);

                    if (maxpages < numpage)
                    {
                        numpage = maxpages;
                    }

                    // verifica se há itens para se mostrar, caso não haja deve mostrar em um label q não tem ou uma imagem
                    if (numDeEvs > 0 && maxpages > 0)
                    {
                        //faz busca por nome e id do evento na tabela evento por eventos q ainda tenham ingresso
                        using (SqlCommand cmd = new SqlCommand("SELECT Nome_evento, id_evento FROM Evento where nro_ingressos > 0 and catg = @cat order by nome_evento offset @nItens * (@page - 1) rows fetch next @nItens rows only", conn))
                        {
                            //substitui os parametros com arroba na query pelo numero da pagina e pelo numero de itens por pagina
                            cmd.Parameters.AddWithValue("@cat", filtro);
                            cmd.Parameters.AddWithValue("@page", numpage);
                            cmd.Parameters.AddWithValue("@nItens", itemPPage);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                // Tenta obter o registro
                                while (reader.Read() == true)
                                {
                                    string nomeEv = reader.GetString(0);
                                    int idEv = reader.GetInt32(1);

                                    EventoModelo evento = new EventoModelo() { Id = idEv, Nome = nomeEv };
                                    lista.Add(evento);
                                }
                            }
                        }
                    }
                    //else{ coloca uma mensagem dizendo q não tem eventos ou uma imagem

                }
                //  }
                //catch (Exception)
                //{
                //lblMsg.Text = "Erro na conexão!";
                //}

                //pega numero total e divide pelo tanto de itens por pagina
                //faz assim int maxpages = (int)Math.ceiling((double)num, (double)nitems);
                //*****int maxpages = (int)Math.Ceiling((double)numDeEvs / (double)itemPPage);
                ant = numpage - 1;
                prox = numpage + 1;

                if (numpage < 2)
                {
                    lbCur.Text = "1";
                }
                else
                {
                    lbCur.Text = numpage.ToString();
                }

                //se for igual a um disabilita prox e ant
                //se o numero da pagina é maior q 1 e menor q maxpages
                //atribui as tags <a> da pagina anterior e da proxima com o valor da pagina atual menos 1 para a anterior e da atual mais 1 para a proxima

                if (maxpages == 0)
                {
                    Aprox.Disabled = true;
                    Aant.Disabled = true;
                }
                else if (numpage > 1 && numpage < maxpages)
                {
                    Aant.Attributes.Add("href", string.Format("~/Default.aspx?filter={0}&page={1}", filtro, ant));
                    Aprox.Attributes.Add("href", string.Format("~/Default.aspx?filter={0}&page={1}", filtro, prox));
                }
                else
                {
                    //caso seja igual a 1 link anterior é desativada e prox é pagina atual mais 1
                    //caso seja igual a maxpages pro´xima é desativada e ant é atual menos 1
                    if (numpage == 1)
                    {
                        if (numpage == maxpages)
                        {
                            Aprox.Disabled = true;
                            Aant.Disabled = true;
                        }
                        else
                        {
                            Aant.Disabled = true;
                            Aprox.Attributes.Add("href", string.Format("~/Default.aspx?filter={0}&page={1}", filtro, prox));
                        }
                    }
                    else if (numpage == maxpages)
                    {
                        Aprox.Disabled = true;
                        Aant.Attributes.Add("href", string.Format("~/Default.aspx?filter={0}&page={1}", filtro, ant));
                    }
                }
            }
        }

        protected void aOUT_Click(object sender, EventArgs e)
        {
            //ao apertar logout chama metdo fazerlogout q apaga cookie e token do banco de dados
            //recarrega a pagina paracompletar a operação
            //Usuario u = Usuario.Validar();
            u.FazerLogout();
            Response.Redirect("~/Default.aspx");
        }

        protected void drop_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CarregaEv(drop.SelectedItem.Value);
            if (taNoDrop(drop.SelectedItem.Value))
            {
                Response.Redirect(string.Format("~/Default.aspx?filter={0}", drop.SelectedItem.Value));
            }

        }

        public bool taNoDrop(string s)
        {
            string[] vet = { "Todas", "Ciência/Tecnologia", "Show", "Infantil", "Festa", "Teatro", "Concerto", "Stand-up", "Moda/Beleza", "Artes", "Business", "Dança", "Outras" };

            for (int i = 0; i < vet.Length; i++)
            {
                if (s == vet[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}