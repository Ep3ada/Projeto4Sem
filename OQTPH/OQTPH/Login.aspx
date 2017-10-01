<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OQTPH.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="pt-br">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>O que tem pra hoje?</title>

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

                            <a href="Login.aspx">Entrar</a>&emsp;&emsp;
                            <a href="Dados.aspx">Registrar</a>

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
        <h2 class="text-center">Login</h2>
        <div class="container">
            <div class="row text-center">
                <hr />
                <div>
                    <div>
                        <label for="txtUserN">Username</label>
                        <div>
                            <asp:TextBox runat="server" ID="txtUserN" required="true" MaxLength="15" />
                        </div>
                    </div>

                    <div>
                        <label for="txtSenha">Senha</label>

                        <div>
                            <asp:TextBox runat="server" ID="txtSenha" TextMode="Password" required="true" />
                            <br />
                        </div>
                    </div>

                    <div>
                        <asp:Button Text="Logar" runat="server" class="btn btn-primary" role="button" ID="btnLogar" OnClick="btnLogar_Click" />
                        <asp:Button Text="Voltar" runat="server" class="btn btn-primary" role="button" ID="btnVoltar" OnClick="btnVoltar_Click" formnovalidate="true" />
                    </div>
                    <div>
                        <asp:Label ID="lblMsg" runat="server" />
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
