using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Teste2.Models;
using System.Data.Entity;


namespace Teste2.Controllers
{
    public class BandaController : Controller
    {
        private Teste2Context db = new Teste2Context();

        // GET: Banda
        public ActionResult Registrar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(Banda Banda, int? id)
        {
            var mensagem = "";
            Musico musico;
            if (ModelState.IsValid)
            {
                musico = db.Musicos.Find(id);
                Banda.MusicoId = musico.MusicoId;
                Banda.Dono = musico.Nome;
                var existeNomeBanda = (from u in db.Bandas where u.Dono == Banda.Dono select u).FirstOrDefault();
                if (existeNomeBanda != null)
                {
                    mensagem = "Você já é dono de uma Banda!";
                    TempData["Mensagem2"] = mensagem;
                    return RedirectToAction("Registrar");
                }
                mensagem = "Cadastro da banda efetuado com Sucesso!";
                TempData["Mensagem"] = mensagem;
                db.Bandas.Add(Banda);
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("TelaMusico", "Musicos");
            }
            return RedirectToAction("Registrar");
        }
        public ActionResult BandaExistente(string nomebanda)
        {
            return Json(!db.Bandas.Any(x => x.NomeBanda == nomebanda), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Solicitacao(int? id, int? id2)
        {
            ViewBag.Solicitacao = db.Solicitacaos.Where(s => s.BandaId == id);
            ViewBag.Musicos = db.Musicos.Where(m => m.MusicoId == id2);
            return View();
        }

        [HttpPost]
        public ActionResult Solicitacao(int id, int? id3)
        {
            Solicitacao s = db.Solicitacaos.Find(id3);
            Banda banda = db.Bandas.Find(id);
            Musico musico = db.Musicos.Find(s.SolicitacaoMusico);
            MusicoBanda mbm = new MusicoBanda();
            mbm.MusicoId = musico.MusicoId;
            mbm.Fk_Banda = banda.BandaId;
            db.MusicoBandas.Add(mbm);
            banda.Quantidade = banda.Quantidade + 1;
            db.Entry(banda).State = EntityState.Modified;
            db.Solicitacaos.Remove(s);
            db.SaveChanges();
            var mensagem = "Sucesso, Um novo membro entrou na Banda!";
            TempData["Mensagem"] = mensagem;
            return RedirectToAction("TelaMusico", "Musicos");
        }

        public ActionResult ExcluirSolicitacao(int? id3)
        {
            Solicitacao s = db.Solicitacaos.Find(id3);
            db.Solicitacaos.Remove(s);
            db.SaveChanges();
            var mensagem = "Solicitação excluida com sucesso!";
            TempData["Mensagem"] = mensagem;
            return RedirectToAction("TelaMusico", "Musicos");
        }

        public ActionResult Cadastradas()
        {
            ViewBag.Bandas = db.Bandas.ToList();
            ViewBag.Musicos = db.MusicoBandas.Include(m => m.Musico);
            return View();
        }
        // GET: Musico/Delete
        public ActionResult Deletar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banda banda = db.Bandas.Find(id);
            if (banda == null)
            {
                return HttpNotFound();
            }
            return View(banda);
        }

        // POST: Musico/Delete
        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletarConfirmed(int id)
        {
            var mensagem = "Banda excluida com Sucesso!";
            TempData["Mensagem"] = mensagem;
            Banda banda = db.Bandas.Find(id);
            db.Bandas.Remove(banda);
            db.SaveChanges();
            return RedirectToAction("TelaMusico", "Musicos");
        }
        //Editar Musico
        public ActionResult Editar(int id)
        {
            Banda banda = db.Bandas.Find(id);
            if (banda == null)
            {
                return HttpNotFound();
            }
            return View(banda);
        }

        [HttpPost]
        public ActionResult Editar(Banda banda)
        {
            if (ModelState.IsValid)
            {
                var mensagem = "";
                var a = 0;
                db.Entry(banda).State = EntityState.Modified;
                var bandas = db.Bandas.ToList();
                foreach (var item in bandas)
                {
                    if (item.NomeBanda == banda.NomeBanda)
                    {
                        a++;
                    }
                }
                if (a > 1)
                {
                    mensagem = "Essa Banda ja existe!";
                    TempData["Mensagem2"] = mensagem;
                    return RedirectToAction("Editar");
                }
                db.SaveChanges();
                ModelState.Clear();
                mensagem = "Banda editada com sucesso!";
                TempData["Mensagem2"] = mensagem;
                return RedirectToAction("TelaMusico", "Musicos");
            }
            return RedirectToAction("Editar");
        }
    }
}
       

        

    
