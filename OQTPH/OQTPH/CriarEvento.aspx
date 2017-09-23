<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CriarEvento.aspx.cs" Inherits="OQTPH.CriarEvento" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="pt-br">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Criar Evento - OQTPH</title>
    
    <script src="/assets/javascript/script.js" type="text/javascript"></script>

    <!-- Bootstrap -->
    <link href="/assets/css/style.css" rel="stylesheet" runat="server" />
    <link rel="icon" type="image/x-icon" href="~/logo.ico" />
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
                            <a href="CriaEvento.aspx">Criar Evento</a>&emsp;&emsp;
                            <a href="Perfil.aspx">Perfil</a>&emsp;&emsp;
                            
                            <asp:LinkButton ID="aOUT" OnClick="aOUT_Click" runat="server">Sair</asp:LinkButton>

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
        <h2 class="text-center">Criar Evento</h2>
        <div class="container">
            <div class="row text-center">
                <hr />

                <div>
                    <div>
                        <asp:PlaceHolder runat="server" Visible="false" ID="placeholderEvento">
                            <div>
                                <div>
                                    <asp:Label Text="Imagem atual:" runat="server" />
                                </div>
                                <img runat="server" id="img" src="#" alt="Imagem do Evento" width="332" height="232" />
                            </div>
                        </asp:PlaceHolder>
                    </div>

                    <div>

                        <div>
                            <div>
                                <label for="up">Imagem do evento:</label>
                            </div>
                            <div>
                                <asp:FileUpload ID="up" runat="server" class="btn btn-primary" role="button" accept="image/*" BorderStyle="Ridge" ValidateRequestMode="Disabled" />
                                <br />
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtNomeEv">Nome do evento:</label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="txtNomeEv" placeholder="Evento Legal" MaxLength="60" BorderStyle="Ridge" />
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtHR">Hora do evento:</label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="txtHR" TextMode="Time" BorderStyle="Ridge" />
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtValEv">Valor:</label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="txtValEv" placeholder="40" onkeyup="moeda(this)" MaxLength="6" BorderStyle="Ridge" />
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtIngEv">Número de Ingressos:</label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="txtIngEv" placeholder="300" MaxLength="6" onkeyup="formataInteiro(this, event)" BorderStyle="Ridge" />
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="dropCatg">Categoria:</label>
                            </div>
                            <div>
                                <asp:DropDownList ID="dropCatg" runat="server">
                                    <asp:ListItem Text="Selecione..." Value="0" Selected="True" />
                                    <asp:ListItem Text="Ciência/Tecnologia" Value="Ciência/Tecnologia" />
                                    <asp:ListItem Text="Show" Value="Show" />
                                    <asp:ListItem Text="Infantil" Value="Infantil" />
                                    <asp:ListItem Text="Festa" Value="Festa" />
                                    <asp:ListItem Text="Teatro" Value="Teatro" />
                                    <asp:ListItem Text="Concerto" Value="Concerto" />
                                    <asp:ListItem Text="Stand-up" Value="Stand-up" />
                                    <asp:ListItem Text="Moda/Beleza" Value="Moda/Beleza" />
                                    <asp:ListItem Text="Artes" Value="Artes" />
                                    <asp:ListItem Text="Business" Value="Business" />
                                    <asp:ListItem Text="Dança" Value="Dança" />
                                    <asp:ListItem Text="Outras" Value="Outras" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtDescEv">Descrição do evento:</label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="txtDescEv" Rows="6" TextMode="MultiLine" onkeypress="return this.value.length<=700" Width="50%" BorderStyle="Ridge" />
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtfone">Telefone:</label>
                            </div>
                            <div>
                                <asp:TextBox ID="txtfone" runat="server" placeholder="(11) 7777-7777" MaxLength="15" onkeyup="formataTelefone(this, event)" BorderStyle="Ridge"></asp:TextBox>
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtEnd">Logradouro:</label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="txtEnd" placeholder="Rua das Maldivas" BorderStyle="Ridge" />
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtNroEnd">Número:</label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="txtNroEnd" placeholder="209" MaxLength="4" onkeyup="formataInteiro(this, event)" BorderStyle="Ridge" />
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtBairroEnd">Bairro:</label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="txtBairroEnd" placeholder="Jabaquara" BorderStyle="Ridge" />
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtCid">Cidade:</label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="txtCid" placeholder="São Paulo" BorderStyle="Ridge" />
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtEst">Estado:</label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="txtEst" placeholder="SP" MaxLength="2" BorderStyle="Ridge" />
                            </div>
                        </div>

                        <div>
                            <div>
                                <label for="txtDT">Data do evento:</label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="txtDT" TextMode="DateTime" onkeyup="formataData(this, event)" MaxLength="10" placeholder="01/01/2017" BorderStyle="Ridge" />
                                <br />
                            </div>
                        </div>

                    </div>

                </div>

                <div>
                    <asp:Button runat="server" class="btn btn-primary" role="button" Text="Criar" ID="btnCriar" OnClick="btnCriar_Click" Visible="true" />
                    <asp:Button runat="server" class="btn btn-primary" role="button" Text="Atualizar" ID="btnAtualiza" OnClick="btnAtualiza_Click" Visible="false" />
                    <asp:Button Text="Voltar" class="btn btn-primary" role="button" runat="server" ID="btnVoltar" OnClick="btnVoltar_Click" formnovalidate="true" />
                    <div>
                        <asp:Label Text="" runat="server" ID="lblMsg" />
                        <br />
                        <br />
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
        </div>
    </form>
</body>
</html>
