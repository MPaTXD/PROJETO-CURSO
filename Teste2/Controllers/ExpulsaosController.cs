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
    public class ExpulsaosController : Controller
    {
        private Teste2Context db = new Teste2Context();

        // GET: Expulsaos
        public ActionResult Index()
        {
            var expulsaos = db.Expulsaos.Include(e => e.Musico);
            return View(expulsaos.ToList());
        }

        // GET: Expulsaos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expulsao expulsao = db.Expulsaos.Find(id);
            if (expulsao == null)
            {
                return HttpNotFound();
            }
            return View(expulsao);
        }

        public ActionResult Listar(int? id, int? id2)
        {
            ViewBag.Bandas = db.Bandas.Where(b=>b.BandaId == id2).ToList();
            ViewBag.Musicos = db.MusicoBandas.Include(m => m.Musico);
            return View();
        }
        public ActionResult Expulsoes(int? id)
        {
            ViewBag.Musico = db.Musicos.Where(m=>m.MusicoId == id).ToList();
            ViewBag.Expulsao = db.Expulsaos.Where(e => e.MusicoId == id).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Expulsoes(int id)
        {
            Expulsao ep = db.Expulsaos.Find(id);
            db.Expulsaos.Remove(ep);
            db.SaveChanges();
            var mensagem = "Expulsao excluida da correio";
            TempData["Mensagem"] = mensagem;
            return RedirectToAction("TelaMusico","Musicos");
        }
        // GET: Expulsaos/Create
        public ActionResult Create(int? id)
        {
            ViewBag.MusicoId = new SelectList(db.Musicos.Where(m=>m.MusicoId != id), "MusicoId", "Nome");
            return View();
        }

        // POST: Expulsaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpulsaoId,Motivo,Texto")] Expulsao expulsao, int? id, int? id2)
        {
            if (ModelState.IsValid)
            {
                Musico musico = db.Musicos.Where(m => m.MusicoId == id).FirstOrDefault();
                Banda b = db.Bandas.Where(b1 => b1.BandaId == id2).FirstOrDefault();
                expulsao.MusicoId = musico.MusicoId;
                expulsao.NomeBanda = b.NomeBanda;
                db.Expulsaos.Add(expulsao);
                MusicoBanda mb = db.MusicoBandas.Where(mb1 => mb1.MusicoId == id && mb1.Fk_Banda == id2).FirstOrDefault();
                db.MusicoBandas.Remove(mb);
                b.Quantidade = b.Quantidade - 1;
                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
                var mensagem = "Expulsao enviada!";
                TempData["Mensagem"] = mensagem;
                return RedirectToAction("TelaMusico","Musicos");
            }

            ViewBag.MusicoId = new SelectList(db.Musicos, "MusicoId", "Nome", expulsao.MusicoId);
            return View(expulsao);
        }

        // GET: Expulsaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expulsao expulsao = db.Expulsaos.Find(id);
            if (expulsao == null)
            {
                return HttpNotFound();
            }
            ViewBag.MusicoId = new SelectList(db.Musicos, "MusicoId", "Nome", expulsao.MusicoId);
            return View(expulsao);
        }

        // POST: Expulsaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExpulsaoId,Motivo,Texto,MusicoId")] Expulsao expulsao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expulsao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MusicoId = new SelectList(db.Musicos, "MusicoId", "Nome", expulsao.MusicoId);
            return View(expulsao);
        }

        // GET: Expulsaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expulsao expulsao = db.Expulsaos.Find(id);
            if (expulsao == null)
            {
                return HttpNotFound();
            }
            return View(expulsao);
        }

        // POST: Expulsaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expulsao expulsao = db.Expulsaos.Find(id);
            db.Expulsaos.Remove(expulsao);
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
