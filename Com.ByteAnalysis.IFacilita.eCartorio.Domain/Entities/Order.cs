using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities
{
    public class Order : EntityBase
    {
        public long NumeroPedido { get; set; }

        public DateTime DataPedido { get; set; }

        public int QuantidadeAtos { get; set; }

        public decimal ValorPedido { get; set; }

        public Requerente Requerente { get; set; }

        public IEnumerable<Ato> Atos { get; set; }

        public string AtosCompactados { get; set; }

        public string Billet { get; set; }

        public string UrlCallback { get; set; }

        public string UrlCallbackResponse { get; set; }

        public bool OrderCompleted { get; set; }
    }

    public class Requerente
    {
        public string Cpf { get; set; }

        public string Cnpj { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }
    }

    public class Ato
    {
        public long NumeroAto { get; set; }

        public string Status { get; set; }

        public string Certidao { get; set; }

        public string Cartorio { get; set; }

        public string Finalidade { get; set; }

        public DateTime? DataPagamento { get; set; }

        public string Cerp { get; set; }

        public string TipoAto { get; set; }

        public string UrlAct { get; set; }
    }
}
