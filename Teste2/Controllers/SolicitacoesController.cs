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
    public class SolicitacoesController : Controller
    {
        private Teste2Context db = new Teste2Context();

        // GET: Solicitacoes
        public ActionResult Index()
        {
            var solicitacaos = db.Solicitacaos.Include(s => s.Banda);
            return View(solicitacaos.ToList());
        }

        // GET: Solicitacoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitacao solicitacao = db.Solicitacaos.Find(id);
            if (solicitacao == null)
            {
                return HttpNotFound();
            }
            return View(solicitacao);
        }

        // GET: Solicitacoes/Create
        public ActionResult Create(int? id)
        {
            ViewBag.BandaId = new SelectList(db.Bandas.Where(b => b.MusicoId != id), "BandaId", "NomeBanda");
            return View();
        }

        // POST: Solicitacoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SolicitacaoId,Texto,SolicitacaoMusico")] Solicitacao solicitacao, List<int> BandaId, int? id)
        {
            var mensagem = "";
            Musico m1 = new Musico();
            m1 = db.Musicos.Find(id);
            if (ModelState.IsValid)
            {
                foreach (var idBanda in BandaId)
                {
                    var mb = db.MusicoBandas.Where(m => m.MusicoId == id && m.Fk_Banda == idBanda).Count();

                    if (mb < 1)
                    {
                        Solicitacao s = new Solicitacao();
                        s.Texto = solicitacao.Texto;
                        s.SolicitacaoMusico = m1.MusicoId;
                        s.NomeMusico = m1.Nome;
                        s.BandaId = idBanda;
                        db.Solicitacaos.Add(s);
                    }
                    else
                    {
                        mensagem = "Ops, Verifique se voce ja esta em uma das Banda Selecionada!";
                        TempData["Mensagem"] = mensagem;
                        return RedirectToAction("Create");
                    }

                }
                db.SaveChanges();
                mensagem = "Solicitacao enviada com sucesso!";
                TempData["Mensagem"] = mensagem;
                return RedirectToAction("TelaMusico","Musicos");
            }

            ViewBag.BandaId = new SelectList(db.Bandas, "BandaId", "NomeBanda", solicitacao.BandaId);
            return View(solicitacao);
        }

        // GET: Solicitacoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitacao solicitacao = db.Solicitacaos.Find(id);
            if (solicitacao == null)
            {
                return HttpNotFound();
            }
            ViewBag.BandaId = new SelectList(db.Bandas, "BandaId", "NomeBanda", solicitacao.BandaId);
            return View(solicitacao);
        }

        // POST: Solicitacoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SolicitacaoId,Texto,SolicitacaoMusico,BandaId")] Solicitacao solicitacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solicitacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BandaId = new SelectList(db.Bandas, "BandaId", "NomeBanda", solicitacao.BandaId);
            return View(solicitacao);
        }

        // GET: Solicitacoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitacao solicitacao = db.Solicitacaos.Find(id);
            if (solicitacao == null)
            {
                return HttpNotFound();
            }
            return View(solicitacao);
        }

        // POST: Solicitacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Solicitacao solicitacao = db.Solicitacaos.Find(id);
            db.Solicitacaos.Remove(solicitacao);
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
