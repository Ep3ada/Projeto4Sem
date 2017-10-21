using System;
using System.Collections.Generic;
using System.Linq;
using OQTPH.Utils;

namespace OQTPH.Models
{
    public class EnderecoModelo
    {
        int Id { get; set; }
        int Numero { get; set; }
        string Logradouro { get; set; }
        string Bairro { get; set; }
        string Cidade { get; set; }
        string Estado { get; set; }
        
        

    }
}