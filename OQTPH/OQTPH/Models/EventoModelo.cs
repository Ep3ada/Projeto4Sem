using System;
using System.Collections.Generic;
using OQTPH.Models;
using System.Web;
using OQTPH.Utils;

namespace OQTPH.Models
{
    public class EventoModelo
    {
        public int Id { get; set; }
        public int NumeroIngressos { get; set; }
        public int CriadorId { get; set; }
        public string Nome { get; set; }
        public string Criador { get; set; }
        public string Descricao { get; set;}
        //public bool ehMeu { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataCompra { get; set; }
        public EnderecoModelo Endereco { get; set; }
        public ICarregar StrategyCarregar { get; set; }
        public ISalvar StrategySalvar { get; set; }

        public void Salvar()
        {
            StrategySalvar.Salvar();
        }

        public void Carregar()
        {
            StrategyCarregar.Carregar();
        }

        /*
        public abstract bool Carregar();
        public abstract bool Remover();
        */
    }
}