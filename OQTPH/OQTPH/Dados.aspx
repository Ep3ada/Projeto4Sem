<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dados.aspx.cs" Inherits="OQTPH.Dados" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="pt-br">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Registrar - OQTPH</title>

    <!-- Bootstrap -->
    <link href="/assets/css/style.css" rel="stylesheet" runat="server" />
    <link rel="icon" type="image/x-icon" href="/assets/images/logoAba.ico" />
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
                            <img src="/assets/images/logoSite.png" width="62" height="62" alt="" /></a>
                    </ul>

                    <ul class="nav navbar-nav navbar-right hidden-sm">

                        <div class="thumbnail">


                            <asp:PlaceHolder ID="PHin" Visible="true" runat="server">
                                <a href="Login.aspx">Entrar</a>&emsp;&emsp;
                                <a href="Dados.aspx">Registrar</a>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="PHout" runat="server" Visible="false">
                                <a href="CriarEvento.aspx">Criar Evento</a>&emsp;&emsp;
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
                            <img class="img-responsive" src="/assets/images/bannerSite.jpg" alt="thumb" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <h2 class="text-center">Dados</h2>
        <div class="container">
            <div class="row text-center">
                <hr />

                <div>

                    <asp:PlaceHolder runat="server" ID="ph1" Visible="false">
                        <asp:Label Text="Nome atual: " runat="server" />
                        <asp:Label ID="lblNome" runat="server" />
                    </asp:PlaceHolder>

                    <div>
                        <label for="txtNome">Nome:</label>
                        <div>
                            <asp:TextBox runat="server" ID="txtNome" MaxLength="30" />
                        </div>
                    </div>

                </div>

                <div>

                    <asp:PlaceHolder runat="server" ID="ph2" Visible="false">
                        <asp:Label Text="Email Atual: " runat="server" />
                        <asp:Label ID="lblEmail" runat="server" />
                    </asp:PlaceHolder>

                    <div>
                        <label for="txtEmail">Email:</label>
                        <div>
                            <asp:TextBox runat="server" ID="txtEmail" />
                        </div>
                    </div>

                </div>

                <div>

                    <asp:PlaceHolder runat="server" ID="ph3" Visible="false">
                        <asp:Label Text="Username Atual: " runat="server" />
                        <asp:Label ID="lblUserN" runat="server" />
                    </asp:PlaceHolder>

                    <div>
                        <label for="txtUserN">Username:</label>

                        <div>
                            <asp:TextBox runat="server" ID="txtUserN" MaxLength="15" />
                        </div>
                    </div>

                </div>

                <div>
                    <div>
                        <label for="txtSenha">Senha:</label>
                    </div>
                    <asp:TextBox runat="server" ID="txtSenha" TextMode="Password" MaxLength="100" />
                    <br />
                    <br />
                </div>

                <div>
                    <asp:Button runat="server" class="btn btn-primary" role="button" Text="Criar" ID="btn" OnClick="btn_Click" Visible="true" />&emsp;
                    <asp:Button runat="server" class="btn btn-primary" role="button" Text="Atualizar" ID="btnUp" OnClick="btnUp_Click" Visible="false" />&emsp;
                    <asp:Button Text="Voltar" class="btn btn-primary" role="button" runat="server" ID="btnVoltar" OnClick="btnVoltar_Click" formnovalidate="true" />
                </div>
                <div>
                    <asp:Label Text="" runat="server" ID="lblMsg" />
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


