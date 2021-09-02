using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Teste2.Models;

namespace Teste2.Controllers
{
    
    public class MusicoBandasController : Controller
    {
        private Teste2Context db = new Teste2Context();
       
   
        // GET: MusicoBandas
        public ActionResult Index(int? id)
        {
            var musicoBandas = db.MusicoBandas.Where(m => m.MusicoId == id).Include(m => m.Banda).Include(m => m.Musico);
            return View(musicoBandas.ToList());
        }

        // GET: MusicoBandas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicoBanda musicoBanda = db.MusicoBandas.Find(id);
            if (musicoBanda == null)
            {
                return HttpNotFound();
            }
            return View(musicoBanda);
        }

        // GET: MusicoBandas/Create
        public ActionResult Create(int? id)
        {
            //ViewBag.Fk_Banda = new SelectList(db.Bandas, "BandaId", "NomeBanda");
            ViewBag.MusicoId = new SelectList(db.Musicos, "MusicoId", "Nome");
            return View();
        }
       

        // POST: MusicoBandas/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NomeBanda,Dono,NomeDono,Tipo,LinkImagem")] Banda banda, List<int> MusicoId, int? id)
        {
            
            MusicoBanda musicoBanda;
            Musico musico;
            if (ModelState.IsValid)
            {
                musico = db.Musicos.Find(id);
                banda.MusicoId = musico.MusicoId;
                banda.Dono = musico.Nome;
                var existeNomeBanda = (from u in db.Bandas where u.Dono == banda.Dono select u).FirstOrDefault();
                if (existeNomeBanda != null)
                {
                    TempData["Msg3"] = "Ops, Voce ja e dono de uma Banda!";
                    return RedirectToAction("create");
                }
                else
                {
                    db.Bandas.Add(banda);
                    db.SaveChanges();

                    foreach (var idMusico in MusicoId)
                    {
                        musicoBanda = new MusicoBanda();
                        musicoBanda.Fk_Banda = banda.BandaId;
                        musicoBanda.MusicoId = idMusico;
                        banda.Quantidade = banda.Quantidade + 1;
                        db.MusicoBandas.Add(musicoBanda);
                    }
                    db.SaveChanges();
                    TempData["Msg3"] = "Banda Cadastrada com Sucesso!";
                    return RedirectToAction("TelaMusico", "Musicos");
                }
            }
            //ViewBag.Fk_Banda = new SelectList(db.Bandas, "BandaId", "NomeBanda", musicoBanda.Fk_Banda);
            //ViewBag.Fk_Musico = new SelectList(db.Musicos, "MusicoID", "Nome", musicoBanda.Fk_Musico);
            return View();
        }
        

        // GET: MusicoBandas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicoBanda musicoBanda = db.MusicoBandas.Find(id);
            if (musicoBanda == null)
            {
                return HttpNotFound();
            }
            ViewBag.Fk_Banda = new SelectList(db.Bandas, "BandaId", "NomeBanda", musicoBanda.Fk_Banda);
            ViewBag.Fk_Musico = new SelectList(db.Musicos, "MusicoID", "Nome", musicoBanda.MusicoId);
            return View(musicoBanda);
        }

        // POST: MusicoBandas/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MusicoBandaId,Fk_Banda,Fk_Musico")] MusicoBanda musicoBanda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(musicoBanda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Fk_Banda = new SelectList(db.Bandas, "BandaId", "NomeBanda", musicoBanda.Fk_Banda);
            ViewBag.Fk_Musico = new SelectList(db.Musicos, "MusicoID", "Nome", musicoBanda.MusicoId);
            return View(musicoBanda);
        }

        // GET: MusicoBandas/Delete/5
        public ActionResult Delete(int? id , int? id1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicoBanda mb = db.MusicoBandas.Where(m => m.MusicoId == id1 && m.Fk_Banda == id).Include(m => m.Musico).Include(b=>b.Banda).SingleOrDefault();
            ViewBag.Fk_Banda = mb.Banda.NomeBanda.ToString();
            ViewBag.MusicoId = mb.Musico.Nome.ToString();
            ViewBag.Imagem = mb.Banda.LinkImagem;
            if (mb == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        // POST: MusicoBandas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id, int? id1)
        {
            Musico m1 = db.Musicos.Find(id1);
            Banda banda2 = db.Bandas.Where(b => b.MusicoId == id1 && b.Dono == m1.Nome).FirstOrDefault();
            MusicoBanda mb = db.MusicoBandas.Where(m => m.MusicoId == id1 &&  m.Fk_Banda == id).SingleOrDefault();
            if (mb != null)
            {
                Banda banda = new Banda();
                banda = db.Bandas.Find(id);
                banda.Quantidade = banda.Quantidade - 1;
                db.MusicoBandas.Remove(mb);
                db.Entry(banda).State = EntityState.Modified;
                TempData["Msg7"] = "Voce saiu da Banda!";
                db.SaveChanges();
            }
            return RedirectToAction("TelaMusico","Musicos");
        }

        //Get: AdicionaMembro
        public ActionResult AdicionaMembro(int? id)
        {
            ViewBag.Musicos = db.MusicoBandas.Include(m => m.Musico);
            ViewBag.bandas = db.Bandas.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AdicionaMembro(int? id, int? id2)
        {
            MusicoBanda mbm = db.MusicoBandas.Where(y => y.Fk_Banda == id2).FirstOrDefault();
            MusicoBanda mbm2 = db.MusicoBandas.Where(z => z.MusicoId == id && z.Fk_Banda == id2).FirstOrDefault();
            if (mbm2 != null)
            {
                TempData["Msg"] = "Erro, voce ja entrou nessa banda!";
                return RedirectToAction("AdicionaMembro");
            }
            else
            {
                TempData["Msg"] = "Voce entrou na Banda!";
                Musico musico = db.Musicos.Find(id);
                mbm.MusicoId = musico.MusicoId;
                db.MusicoBandas.Add(mbm);
                Banda banda = new Banda();
                banda = db.Bandas.Find(id2);
                banda.Quantidade = banda.Quantidade + 1;
                db.Entry(banda).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("TelaMusico", "Musicos");
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
