using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Teste2.Models
{
    
    public class Musico
    {
        public int MusicoId { get; set; }
        [Required(ErrorMessage ="Nome obrigatório")]
        public string Nome { get; set; }
        public int Idade { get; set; }
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Informe um Email Valido!")]
        [Remote("EmailExistente", "Musicos", ErrorMessage = "Email já Cadastrado")]
        [Required(ErrorMessage = "Email obrigatório")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Senha obrigatório")]
        public string Senha { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirmar Senha obrigatório")]
        [System.ComponentModel.DataAnnotations.Compare("Senha", ErrorMessage = "A senha Digitada não é Semelhante!")]
        public string ConfirmarSenha { get; set; }
        public enum LicenseTypes
        {
            Baixista = 1,
            Vocalista = 2,
            Baterista = 3,
            Guitarrista = 4,
            Tecladista = 5,
        }
        [Range(1, int.MaxValue, ErrorMessage = "Categoria obrigatória")]
        public LicenseTypes Categoria { get; set; }
        [Required(ErrorMessage = "Imagem Obrigatória")]
        public string LinkPerfil { get; set; }
        public virtual ICollection<MusicoBanda>musicobanda { get; set; }
        public virtual ICollection<Convite> convites { get; set; }
        public virtual ICollection<Expulsao> expulsoes { get; set; }
    }
}