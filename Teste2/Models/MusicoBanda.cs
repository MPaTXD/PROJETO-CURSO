using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Teste2.Models
{
    public class MusicoBanda
    {
        public int MusicoBandaId { get; set; }
        [ForeignKey("Banda")]
        public int Fk_Banda { get; set; }
        public Banda Banda { get; set; }
        [ForeignKey("Musico")]
        public int MusicoId { get; set; }
        public Musico Musico { get; set; }

        public static bool VerificaNomeBanda(string nomedono)
        {
            using (Teste2Context db = new Teste2Context())
            {
                var existeNomeBanda = (from u in db.Bandas where u.Dono == nomedono select u).FirstOrDefault();
                if (existeNomeBanda != null)
                    return true;
                else
                    return false;
            }
        }
    }
}