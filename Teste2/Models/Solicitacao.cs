using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Teste2.Models
{
    public class Solicitacao
    {
        public int SolicitacaoId { get; set; }
        public string Texto { get; set; }
        public int? SolicitacaoMusico { get; set; }
        public string NomeMusico { get; set; }
        [ForeignKey("Banda")]
        public int BandaId { get; set; }
        public Banda Banda { get; set; }
    }
}