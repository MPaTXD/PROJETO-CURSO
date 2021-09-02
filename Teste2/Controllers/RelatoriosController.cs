using PagedList;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teste2.Models;

namespace Teste2.Controllers
{
    public class RelatoriosController : Controller
    {
        private Teste2Context db = new Teste2Context();

        // GET: Relatorios
        public ActionResult ListagemBandasGenero()
        {
            var Banda = db.Bandas;
            var Bandas1 = Banda.OrderBy(pr => pr.Quantidade).ToList();
            
            var pdf = new ViewAsPdf
            {
                PageSize = Size.A4,
                ViewName = "ListagemBandasGenero",
                IsGrayScale = false,
                Model = Bandas1.ToPagedList(1, Bandas1.Count())
            };
            return pdf;
        }

        public ActionResult ListagemMusicos()
        {
            var Musicos = db.Musicos;
            var MusicosIdade = Musicos.OrderBy(y => y.Idade).ToList();
            var x = MusicosIdade.Where(y => y.Categoria == Musico.LicenseTypes.Vocalista);
            var pdf = new ViewAsPdf
            {
                PageSize = Size.A4,
                ViewName = "ListagemMusicos",
                IsGrayScale = false,
                Model = x.ToPagedList(1, x.Count())
            };
            return pdf;
        }

        public ActionResult ListagemConta(int? id)
        {
            var m = db.Musicos;
            var m2 = m.Where(m1 => m1.MusicoId == id).ToList();
            var x = m2;
            var pdf = new ViewAsPdf
            {
                PageSize = Size.A4,
                ViewName = "ListagemConta",
                IsGrayScale = false,
                Model = x.ToPagedList(1, x.Count())
            };
            return pdf;
        }
    }
}
