using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Teste2.Models
{
    public class Banda
    {
        public int BandaId { get; set; }
        [Remote("BandaExistente", "Banda",ErrorMessage = "Banda já existente.")]
        [Required(ErrorMessage = "Nome da Banda Obrigatório")]
        public string NomeBanda { get; set; }
        [ForeignKey("Musico")]
        public int ?MusicoId { get; set; }
        public Musico Musico { get; set; }
        public string Dono { get; set; }
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Imagem Obrigatória")]
        public string LinkImagem { get; set; }
        public int ?Quantidade { get; set; }
        public virtual ICollection<MusicoBanda> musicobanda { get; set; }
        public virtual ICollection<Solicitacao> solicitacoes { get; set; }

        public Banda()
        {
            Quantidade = 0;
        }
    }
}