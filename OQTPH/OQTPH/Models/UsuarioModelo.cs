using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Web;
using OQTPH.Utils;


namespace OQTPH.Models
{
    public class UsuarioModelo
    {
        public const int TamanhoMaximoDoUserN = 15;
        public const int TamanhoMaximoDoNome = 30;
        public const int TamanhoMaximoDaSenha = 100;

        public int Id;
        public string UserN;
        public string Nome;
        public string Email;
        public string Token;

        public UsuarioModelo(int id, string usern, string nome, string email, string token)
        {
            Id = id;
            UserN = usern;
            Nome = nome;
            Email = email;
            Token = token;
        }
        
        public UsuarioModelo(int id, string token)
        {
            Id = id;
            Token = token;
        }

        private static UsuarioModelo PegarDoCliente()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["user"];
            if (cookie == null)
            {
                return null;
            }

            string[] values = cookie.Value.Split('|');
            if (values.Length != 2)
            {
                return null;
            }

            // O objeto Usuario que vem do cliente tem apenas o Id e o Token
            return new UsuarioModelo(int.Parse(values[0]), values[1]);
        }

        private string EnviarParaCliente()
        {
            string value = Id + "|" + Token;

            HttpCookie cookie = new HttpCookie("user", value);

            cookie.Expires = DateTime.UtcNow.AddYears(1);

            HttpContext.Current.Response.SetCookie(cookie);

            return "user=" + value + ";";
        }

        private void RemoverDoCliente()
        {
            HttpCookie cookie = new HttpCookie("user", "");

            cookie.Expires = DateTime.UtcNow.AddYears(-1);

            HttpContext.Current.Response.SetCookie(cookie);
        }

        public static UsuarioModelo Criar(string nome, string email, string usern, string senha)
        {
            if (string.IsNullOrWhiteSpace(usern) || usern.Length > TamanhoMaximoDoUserN)
            { throw new Exception("Username inválido!"); }

            if (string.IsNullOrWhiteSpace(nome) || nome.Length > TamanhoMaximoDoNome)
            { throw new Exception("Nome inválido!"); }

            if (string.IsNullOrEmpty(senha) || senha.Length > TamanhoMaximoDaSenha)
            { throw new Exception("Senha inválida!"); }

            int arroba, arroba2, ponto;
            arroba = email.IndexOf('@');
            arroba2 = email.LastIndexOf('@');
            ponto = email.LastIndexOf('.');
            if (arroba <= 0 || ponto <= (arroba + 1) || ponto == (email.Length - 1) || arroba2 != arroba)
            { throw new Exception("E-mail inválido!"); }

            using (SqlConnection conn = Sql.OpenConnection())
            {
                // Para outros SQL Server
                // using (SqlCommand command = new SqlCommand("INSERT INTO tbUsuario (Login, Nome, Password, Token) OUTPUT tbUser.Id VALUES (@login, @nome, @password, '')", conn)) {

                // Para o Azure
                using (SqlCommand command = new SqlCommand("INSERT INTO USUARIO (Nome, Email, Username, Senha, Token) OUTPUT INSERTED.ID_USUARIO VALUES (@nome, @email, @login, @senha, '')", conn))
                {
                    command.Parameters.AddWithValue("@nome", nome);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@login", usern);
                    command.Parameters.AddWithValue("@senha", PasswordHash.CreateHash(senha));

                    int id = (int)command.ExecuteScalar();

                    // Como esse usuário acabou de ser criado, ele não está logado, por isso não tem token
                    return new UsuarioModelo(id, usern, nome, email, "");
                }
            }
        }

        public static UsuarioModelo Validar()
        {
            UsuarioModelo usuario = PegarDoCliente();
            if (usuario == null)
            { return null; }

            try
            {
                using (SqlConnection conn = Sql.OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand("SELECT username, Nome, Email, Token FROM Usuario WHERE Id_usuario = @id", conn))
                    {
                        command.Parameters.AddWithValue("@id", usuario.Id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read() == false ||
                                string.IsNullOrWhiteSpace(usuario.Token) ||
                                string.Equals(usuario.Token, reader.GetString(3), StringComparison.Ordinal) == false)
                            {

                                return null;
                            }

                            usuario.UserN = reader.GetString(0); usuario.Nome = reader.GetString(1); usuario.Email = reader.GetString(2);
                        }
                    }
                }
            }
            catch (Exception) { return null; }

            return usuario;
        }

        public static UsuarioModelo ValidarException()
        {
            UsuarioModelo usuario = Validar();
            if (usuario == null)
            { throw new Exception("Usuário inválido ou não conectado"); }

            return usuario;
        }

        public static string FazerLogin(string login, string password)
        {
            int id;
            string hash;

            using (SqlConnection conn = Sql.OpenConnection())
            {
                using (SqlCommand command = new SqlCommand("SELECT Id_usuario, senha FROM Usuario WHERE username = @login", conn))
                {
                    command.Parameters.AddWithValue("@login", login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() == false)
                        { return null; }

                        id = reader.GetInt32(0); hash = reader.GetString(1);
                    }
                }

                if (PasswordHash.ValidatePassword(password, hash))
                {
                    using (SqlCommand command = new SqlCommand("UPDATE Usuario SET Token = @token WHERE Id_usuario = @id", conn))
                    {
                        string token = Guid.NewGuid().ToString("N");
                        command.Parameters.AddWithValue("@token", token);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                        // Tanto o nome quanto o login não interessam para enviar para o cliente
                        UsuarioModelo usuario = new UsuarioModelo(id, "", "", "", token);
                        return usuario.EnviarParaCliente();
                    }
                }
            }

            return null;
        }

        public void FazerLogout()
        {
            using (SqlConnection conn = Sql.OpenConnection())
            {
                using (SqlCommand command = new SqlCommand("UPDATE Usuario SET Token = '' WHERE Id_usuario = @id", conn))
                {
                    command.Parameters.AddWithValue("@id", Id);

                    command.ExecuteNonQuery();

                    RemoverDoCliente();

                }
            }
        }
    }
}