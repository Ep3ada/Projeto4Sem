<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Evento.aspx.cs" Inherits="OQTPH.Evento" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="pt-br">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Evento - OQTPH</title>

    <!-- Bootstrap -->
    <link href="/assets/css/style.css" rel="stylesheet" runat="server" />
    <link rel="icon" type="image/x-icon" href="~/logo.ico" />
    <style type="text/css">
</style>

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
                            <img src="/image/logosite.png" width="62" height="62" alt="" /></a>
                    </ul>

                    <ul class="nav navbar-nav navbar-right hidden-sm">

                        <div class="thumbnail">

                            <asp:PlaceHolder ID="PHin" Visible="true" runat="server">
                                <a href="Login.aspx">Entrar</a>&emsp;&emsp;
                                <a href="Dados.aspx">Registrar</a>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="PHout" runat="server" Visible="false">
                                <a href="CriaEvento.aspx">Criar Evento</a>&emsp;&emsp;
                                <a href="Perfil.aspx">Perfil</a>&emsp;&emsp;
                               
                                <asp:LinkButton ID="aOUT" OnClick="aOUT_Click" runat="server">Sair</asp:LinkButton>

                            </asp:PlaceHolder>


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
                            <img class="img-responsive" src="/image/banner.jpg" alt="thumb" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <h2 class="text-center">EVENTO</h2>
        <div class="container">
            <div class="row text-center">
                <hr />
                <div>
                    <div id="div1" runat="server">
                        <div>
                            <img runat="server" id="img" src="#" width="332" height="232" />
                        </div>

                        <div>
                            <label for="lblNome">Nome:</label>
                            <div>
                                <asp:Label runat="server" ID="lblNome"></asp:Label>
                            </div>
                        </div>

                        <div>
                            <label for="lblDesc">Descrição:</label>
                            <div>
                                <asp:Label runat="server" ID="lblDesc"></asp:Label>
                            </div>
                        </div>

                        <div>
                            <label for="lblVal">Valor:</label>
                            <div>
                                <asp:Label runat="server" ID="lblVal"></asp:Label>
                            </div>
                        </div>

                        <div>
                            <label for="lblCat">Categoria:</label>
                            <div>
                                <asp:Label runat="server" ID="lblCat"></asp:Label>
                            </div>
                        </div>

                        <div>
                            <label for="lblIngres">Ingressos Disponíveis:</label>
                            <div>
                                <asp:Label runat="server" ID="lblIngres"></asp:Label>
                            </div>
                        </div>

                        <div>
                            <label for="lblDT">Data:</label>
                            <div>
                                <asp:Label runat="server" ID="lblDT"></asp:Label>
                            </div>
                        </div>

                        <div>
                            <label for="lblHora">Hora:</label>
                            <div>
                                <asp:Label runat="server" ID="lblHora"></asp:Label>
                            </div>
                        </div>

                        <div>
                            <label for="lblLocal">Local:</label>
                            <div>
                                <asp:Label runat="server" ID="lblLocal"></asp:Label>
                            </div>
                        </div>

                        <div>
                            <label for="lblFone">Telefone:</label>
                            <div>
                                <asp:Label runat="server" ID="lblFone"></asp:Label>
                            </div>
                        </div>

                        <div>
                            <label for="lblCriador">Criador:</label>
                            <div>
                                <asp:Label runat="server" ID="lblCriador"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div id="div2" runat="server">
                       
                            <asp:Button ID="btn" runat="server" class="btn btn-primary" role="button" Text="Adquirir" OnClick="btn_Click" Visible="true" />&emsp;
                       
                            <asp:Button ID="btnEdit" runat="server" class="btn btn-primary" role="button" Text="Editar" OnClick="btnEdit_Click" Visible="false" />&emsp;
                        
                        
                            <asp:Button ID="btnD" runat="server" class="btn btn-primary" role="button" Text="Devolver" OnClick="btnD_Click" Visible="false" />&emsp;
                        
                        
                            <asp:Button ID="btnVoltar" runat="server" class="btn btn-primary" role="button" Text="Voltar" OnClick="btnVoltar_Click" />
                        
                    </div>

                    <div>
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <hr />
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

