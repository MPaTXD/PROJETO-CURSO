using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Teste2.Models;

namespace Teste2.Controllers
{
    public class ConvitesController : Controller
    {
        private Teste2Context db = new Teste2Context();

        // GET: Convites
        public ActionResult Index(int? id)
        {
            var convites = db.Convites.Include(m => m.Musico);
            return View(convites.ToList());
        }
        public ActionResult SeusConvites(int? id)
        {
            ViewBag.Musico = db.Musicos.Where(m => m.MusicoId == id).ToList();
            ViewBag.MusicoBanda = db.MusicoBandas.Where(mb => mb.MusicoId == id);
            ViewBag.convites = db.Convites.Where(m => m.MusicoId == id).Include(m => m.Musico);
            return View();
        }

        [HttpPost]
        public ActionResult SeusConvites(int? id, int? id2, int? id3)
        {
            var verificar = db.MusicoBandas.Where(mb => mb.Fk_Banda == id3 && mb.MusicoId == id).FirstOrDefault();
            if (verificar == null)
            {
                Musico m = new Musico();
                m = db.Musicos.Find(id);
                Banda b = new Banda();
                b = db.Bandas.Find(id3);
                MusicoBanda musicoBanda = new MusicoBanda();
                musicoBanda.Fk_Banda = b.BandaId;
                musicoBanda.MusicoId = m.MusicoId;
                b.Quantidade = b.Quantidade + 1;
                db.MusicoBandas.Add(musicoBanda);
                Convite c = new Convite();
                c = db.Convites.Find(id2);
                db.Convites.Remove(c);
                db.SaveChanges();
                var mensagem = "Sucesso!, Voce aceitou o convite!";
                TempData["Mensagem"] = mensagem;
                return RedirectToAction("TelaMusico", "Musicos");
            }
            else
            {
                Convite c = new Convite();
                c = db.Convites.Find(id2);
                db.Convites.Remove(c);
                db.SaveChanges();
                var mensagem = "Ops, Voce ja faz parte dessa banda, O convite sera excluido";
                TempData["Mensagem"] = mensagem;
                return RedirectToAction("TelaMusico", "Musicos");
            }
        }
        public ActionResult ExcluirConvite(int? id2)
        {
            Convite c = new Convite();
            c = db.Convites.Find(id2);
            db.Convites.Remove(c);
            db.SaveChanges();
            var mensagem = "Convite excluido com sucesso!";
            TempData["Mensagem"] = mensagem;
            return RedirectToAction("TelaMusico","Musicos");
        }

        // GET: Convites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convite convite = db.Convites.Find(id);
            if (convite == null)
            {
                return HttpNotFound();
            }
            return View(convite);
        }

        // GET: Convites/Create
        public ActionResult Create(int? id)
        {
            ViewBag.MusicoId = new SelectList(db.Musicos.Where(m=>m.MusicoId != id), "MusicoId", "Nome");
            return View();
        }

        // POST: Convites/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConviteId,Texto,BandaConvite")] Convite convite, List<int>MusicoId, int? id)
        {
            if (ModelState.IsValid)
            {
                Banda b = new Banda();
                b = db.Bandas.Where(a => a.MusicoId == id).FirstOrDefault();
                foreach (var idMusico in MusicoId)
                {
                    Convite c = new Convite();
                    c.Texto = convite.Texto;
                    c.BandaConvite = b.BandaId;
                    c.MusicoId = idMusico;
                    c.NomeBanda = b.NomeBanda;
                    db.Convites.Add(c);
                }
                db.SaveChanges();
                var mensagem = "Convite criado com sucesso!";
                TempData["Mensagem"] = mensagem;
                return RedirectToAction("TelaMusico","Musicos");
            }

            ViewBag.MusicoId = new SelectList(db.Musicos, "MusicoId", "Nome", convite.MusicoId);
            return View(convite);
        }

        // GET: Convites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convite convite = db.Convites.Find(id);
            if (convite == null)
            {
                return HttpNotFound();
            }
            ViewBag.MusicoId = new SelectList(db.Musicos, "MusicoId", "Nome", convite.MusicoId);
            return View(convite);
        }

        // POST: Convites/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConviteId,Texto,BandaConvite,MusicoId")] Convite convite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(convite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MusicoId = new SelectList(db.Musicos, "MusicoId", "Nome", convite.MusicoId);
            return View(convite);
        }

        // GET: Convites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convite convite = db.Convites.Find(id);
            if (convite == null)
            {
                return HttpNotFound();
            }
            return View(convite);
        }

        // POST: Convites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Convite convite = db.Convites.Find(id);
            db.Convites.Remove(convite);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
