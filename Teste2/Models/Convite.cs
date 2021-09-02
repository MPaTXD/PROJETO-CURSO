using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Teste2.Models
{
    public class Convite
    {
        public int ConviteId { get; set; }
        public string Texto { get; set; }
        public int? BandaConvite { get; set; }
        public string NomeBanda { get; set; }
        [ForeignKey("Musico")]
        public int MusicoId { get; set; }
        public Musico Musico { get; set; }
    }
}