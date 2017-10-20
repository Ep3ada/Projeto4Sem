<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="OQTPH.Perfil" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="pt-br">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Meu perfil - OQTPH</title>

    <!-- Bootstrap -->
    <link href="/assets/css/style.css" rel="stylesheet" runat="server" />
    <link rel="icon" type="image/x-icon" href="/assets/images/logoAba.ico" />
    <style type="text/css"></style>

</head>
<body>
    <form id="form1" runat="server">

        <nav>
            <div class="container">


                <div class="collapse navbar-collapse">
                    <ul class="nav navbar-nav">
                    </ul>
                    <ul class="nav navbar-nav">
                        <li class="active"></li>
                        <a href="Default.aspx">
                            <img src="/assets/images/logoSite.png" width="62" height="62" alt="" /></a>
                    </ul>

                    <ul class="nav navbar-nav navbar-right hidden-sm">

                        <div class="thumbnail">
                            <a href="CriaEvento.aspx">Criar Evento</a>&emsp;&emsp;
                            <a href="Perfil.aspx">Perfil</a>&emsp;&emsp;
                            
                            <asp:LinkButton ID="LinkButton1" OnClick="aOUT_Click" runat="server" meta:resourcekey="LinkButton1Resource1">Sair</asp:LinkButton>

                        </div>
                    </ul>
                    &emsp;&emsp;&emsp;

                </div>
            </div>
        </nav>
        <div class="container">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="carousel-inner">
                        <div class="item active">
                            <img class="img-responsive" src="/assets/images/bannerSite.jpg" alt="thumb" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <h2 class="text-center">Perfil</h2>
        <div class="container">
            <div class="row text-center">
                <hr />
                <div>
                    <div>
                        <asp:Label ID="lblNome" runat="server" meta:resourcekey="lblNomeResource1"></asp:Label>

                    </div>
                    <div>
                        <asp:Label ID="lblUserN" runat="server" meta:resourcekey="lblUserNResource1"></asp:Label>

                    </div>
                    <div>
                        <asp:Label ID="lblEmail" runat="server" meta:resourcekey="lblEmailResource1"></asp:Label>

                        <br />

                    </div>

                    <div>
                        <!-- esse botão é pra editar a conta-->
                        <asp:Button ID="btnEditar" runat="server" class="btn btn-primary" role="button" Text="Editar" OnClick="btnEditar_Click" meta:resourcekey="btnEditarResource1" />
                        <br />
                    </div>
                    <div>
                        <!-- esse botão é pra editar a conta-->
                        <asp:Button ID="btnDelete" runat="server" class="btn btn-primary" role="button" Text="Apagar Perfil" OnClick="btnDelete_Click" meta:resourcekey="btnDeleteResource1" />
                        <br />
                        <asp:Label ID="lblMsg" runat="server" Visible="False" meta:resourcekey="lblMsgResource1"></asp:Label>
                    </div>

                    <div style="margin: 5px">
                        <label for="EvCriou">Eventos criados:</label>
                        <div id="EvCriou" runat="server">
                            <!-- tem  q carrgar do banco e adicionar um link pra ir pro evento ou pra editar o seu evento no caso  -->

                            <table>
                                <tbody>
                                    <%for (int i = 0; i < _eventosCriados.Count; i++)
                                        {%>
                                    <tr>
                                        <td><%=_eventosCriados[i].Nome %></td>
                                        <td><%=_eventosCriados[i].Data.Date.ToString("d")%></td>

                                        <td><%=_eventosCriados[i].Data.TimeOfDay%></td>

                                        <td><a href="Evento.aspx?evento=<%=_eventosCriados[i].Id %>">Acessar</a></td>
                                    </tr>
                                    <% } %>
                                </tbody>
                            </table>
                            <div>
                                <asp:Button ID="btnDEC" runat="server" class="btn btn-primary" role="button" OnClick="btnDEC_Click" Text="Apagar Evento" meta:resourcekey="btnDECResource1" />
                                <asp:DropDownList runat="server" ID="dropEC" meta:resourcekey="dropECResource1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <asp:Label Text="Não há nenhum!" ID="lblEC" runat="server" Visible="False" meta:resourcekey="lblECResource1" />
                    </div>


                    <!--<div style="margin: 5px"> -->

                    <label for="EvComprou">Ingressos Adquiridos:</label>

                    <div id="EvComprou" runat="server">
                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-6">
                            <div class="thumbnail">

                                <!-- mesma coisa do de cima mas o link é só pra ir pro evento
                traz nome data e hora e preco e lugar tbm talvez
                no c# cria uma div mae uma div filha q gurda o texto e uma div filha q tem o botão-->

                                <table>
                                    <tbody>

                                        <%for (int i = 0; i < _eventosAdquiridos.Count; i++)
                                            {%>
                                        <tr>
                                            <td><%=_eventosAdquiridos[i].Nome %>&nbsp;</td>
                                            <td><%=_eventosAdquiridos[i].Data.Date.ToString("d")%> <%=_eventosAdquiridos[i].Data.TimeOfDay%>&nbsp;</td>
                                            <br />
                                            <td>Comprou em:<%=_eventosAdquiridos[i].DataCompra.Date.ToString("d") %> </td>
                                            <br />
                                            <td>&nbsp;<a href="Evento.aspx?evento=<%=_eventosAdquiridos[i].Id %>">Acessar</a></td>
                                        </tr>
                                        <% } %>
                                    </tbody>
                                </table>
                                <asp:Label Text="Não há nenhum!" ID="lblIA" runat="server" Visible="False" meta:resourcekey="lblIAResource1" />
                            </div>
                        </div>

                    </div>

                    <div>
                        <div>
                            <br />
                            <br />
                            <br />
                            <asp:DropDownList ID="drop" runat="server" meta:resourcekey="dropResource1">
                                <asp:ListItem Text="Selecione" Value="Selecione" Selected="True" meta:resourcekey="ListItemResource1" />
                                <asp:ListItem Text="Ciência/Tecnologia" Value="Ciência/Tecnologia" meta:resourcekey="ListItemResource2" />
                                <asp:ListItem Text="Show" Value="Show" meta:resourcekey="ListItemResource3" />
                                <asp:ListItem Text="Infantil" Value="Infantil" meta:resourcekey="ListItemResource4" />
                                <asp:ListItem Text="Festa" Value="Festa" meta:resourcekey="ListItemResource5" />
                                <asp:ListItem Text="Teatro" Value="Teatro" meta:resourcekey="ListItemResource6" />
                                <asp:ListItem Text="Concerto" Value="Concerto" meta:resourcekey="ListItemResource7" />
                                <asp:ListItem Text="Stand-up" Value="Stand-up" meta:resourcekey="ListItemResource8" />
                                <asp:ListItem Text="Moda/Beleza" Value="Moda/Beleza" meta:resourcekey="ListItemResource9" />
                                <asp:ListItem Text="Artes" Value="Artes" meta:resourcekey="ListItemResource10" />
                                <asp:ListItem Text="Business" Value="Business" meta:resourcekey="ListItemResource11" />
                                <asp:ListItem Text="Dança" Value="Dança" meta:resourcekey="ListItemResource12" />
                                <asp:ListItem Text="Outras" Value="Outras" meta:resourcekey="ListItemResource13" />
                            </asp:DropDownList>

                            <asp:Button ID="btn" runat="server" class="btn btn-primary" role="button" OnClick="btn_Click" Text="Criar filtro" meta:resourcekey="btnResource1" />
                        </div>

                        <div>
                            <label for="EvCat">Eventos procurados</label>
                            <div id="EvCat" runat="server">
                                <div id="divEP" runat="server">
                                    <table>
                                        <tbody>
                                            <%for (int i = 0; i < _eventosAlerta.Count; i++)
                                                {%>
                                            <tr>
                                                <td><%=_eventosAlerta[i].Nome %></td>
                                                <br />
                                                <td><%=_eventosAlerta[i].Data.Date.ToString("d")%></td>
                                                <br />
                                                <td><a href="Evento.aspx?evento=<%=_eventosAlerta[i].Id %>">Acessar</a></td>
                                            </tr>
                                            <% } %>
                                        </tbody>
                                    </table>
                                </div>

                                <asp:Label Text="Não há Nenhum!" ID="lblEP" runat="server" Visible="False" meta:resourcekey="lblEPResource1" />
                            </div>
                        </div>
                    </div>

                </div>
                <hr />
            </div>
        </div>
        <div class="container well">
            <div class="row">
                <div class="col-xs-6 col-sm-6 col-md-6 col-lg-5">
                    <address>
                        <strong>O que tem pra hoje?, Inc.</strong><br />
                        Digital School<br />
                        R. Estela, 268, SP, 04011-001<br />
                    </address>
                    <address>
                        <strong>Contato</strong><br />
                        <a href="mailto:#">ep3ada@gmail.com</a>
                    </address>
                </div>
            </div>
        </div>

        <footer class="text-center">
            <div class="container">
                <div class="row">
                    <div class="col-xs-12">
                        <p>Copyright © O que tem pra hoje?. All rights reserved.</p>
                    </div>
                </div>
            </div>
        </footer>
        </div>
    </form>
</body>
</html>
