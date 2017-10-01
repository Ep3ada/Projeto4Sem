using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OQTPH
{
    public class EventoModelo
    {
        public int eventoID { get; set; }
        public string eventoNome { get; set; }
        public DateTime eventoData { get; set; }
        public DateTime eventoDataCompra { get; set; }
    }
}