using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Teste2.Models;

namespace Teste2.Controllers
{
    public class MusicosController : Controller
    {
        private Teste2Context db = new Teste2Context();

        // GET: Musicos
        public ActionResult Inicio()
        {
            return View();
        }

        //Listar: Musicos Cadastrados
        public ActionResult Cadastrados()
        {
            return View(db.Musicos.ToList());
        }

        //Registrar Musicos
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Musico Musico)
        {
            var mensagem = "";
            if (ModelState.IsValid)
            {
                db.Musicos.Add(Musico);
                db.SaveChanges();
                ModelState.Clear();
                mensagem = "Cadastro efetuado com sucesso!";
                TempData["Mensagem"] = mensagem;
                return RedirectToAction("Inicio");
            }
            return RedirectToAction("Registrar");
        }
        public JsonResult EmailExistente(string email)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!db.Musicos.Any(x => x.Email == email), JsonRequestBehavior.AllowGet);
        }

        //Login do Musico
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Musico Login)
        {
            {
                var Email = db.Musicos.Where(u => u.Email == Login.Email).FirstOrDefault();
                if (Email == null)
                {
                    ViewBag.Cadastro = "Ops! Esse email nao existe no sistema, Realize seu Cadastro!";
                    return View();
                }
                var Senha = db.Musicos.Where(s => s.Senha == Login.Senha && s.Email == Email.Email).FirstOrDefault();
                if (Email != null && Senha != null)
                {
                    var usuario = db.Musicos.Where(x => x.Email == Login.Email && x.Senha == Login.Senha).FirstOrDefault();
                    if (usuario != null)
                    {
                        var mensagem = "Login efetuado com sucesso!";
                        TempData["Mensagem"] = mensagem;
                        Session["MusicoID"] = usuario.MusicoId.ToString();
                        Session["Nome"] = usuario.Nome.ToString();
                        Session["Email"] = usuario.Email.ToString();
                        Session["Idade"] = usuario.Idade.ToString();
                        Session["Senha"] = usuario.Senha.ToString();
                        Session["Categoria"] = usuario.Categoria.ToString();
                        Session["LinkPerfil"] = usuario.LinkPerfil.ToString();
                        return RedirectToAction("TelaMusico");
                    }
                }
                else
                if (Email != null && Senha == null)
                {
                    ViewBag.Errologin = "Senha Incorreta!";
                }
            }
            return View();
        }


        //Tela do Musico
        public ActionResult TelaMusico()
        {
            if (Session["MusicoID"] != null)
            {
                var id2 = Session["MusicoId"];
                ViewBag.Expulsao = db.Expulsaos.Where(e => e.MusicoId.ToString() == id2).Count();
                ViewBag.Convites = db.Convites.Where(c => c.MusicoId.ToString() == id2).Count();
                ViewBag.HabilitarBanda = db.Bandas.Where(b => b.Dono.ToString() == id2).FirstOrDefault();
                ViewBag.EntrarBanda = db.Bandas.Count();
                ViewBag.Solicitacoes = db.Solicitacaos.Where(s => s.Banda.Dono.ToString() == id2).Count();
                ViewBag.Cont = db.Bandas.ToList().Count();
                var nome = Session["Nome"].ToString();
                var banda = db.Bandas.Where(b => b.Dono == nome).FirstOrDefault();
                ViewBag.VerificarBanda = banda;
                var id = Session["MusicoID"].ToString();
                var MusicoBanda = db.MusicoBandas.Where(m => m.MusicoId.ToString() == id).FirstOrDefault();
                var Verificar = db.Bandas.Where(m => m.Dono.ToString() == id).ToList();
                ViewBag.Verificar = Verificar;
                ViewBag.VerificarMusicoBanda = MusicoBanda;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult Deslogar()
        {
            var mensagem = "Login encerrado!";
            TempData["Mensagem"] = mensagem;
            Session.Clear();
            return RedirectToAction("Login");
        }



        //Editar Musico
        public ActionResult Editar(int id)
        {
            Musico musico = db.Musicos.Find(id);
            if (musico == null)
            {
                return HttpNotFound();
            }
            return View(musico);
        }

        [HttpPost]
        public ActionResult Editar(Musico musico)
        {
            if (ModelState.IsValid)
            {
                var mensagem = "";
                var a = 0;
                db.Entry(musico).State = EntityState.Modified;
                var musicos = db.Musicos.ToList();
                foreach(var item in musicos)
                {
                    if(item.Email == musico.Email)
                    {
                        a++;
                    }
                }
                if (a > 1)
                {
                    mensagem = "Email informado ja existe!";
                    TempData["Mensagem"] = mensagem;
                    return RedirectToAction("Editar");
                }
                db.SaveChanges();
                ModelState.Clear();
                mensagem = "Cadastro editado com sucesso!";
                TempData["Mensagem"] = mensagem;
                return RedirectToAction("Deslogar");
            }
            return RedirectToAction("Editar");
        }



        // GET: Musico/Delete
        public ActionResult Deletar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Musico musico = db.Musicos.Find(id);
            if (musico == null)
            {
                return HttpNotFound();
            }
            return View(musico);
        }

        // POST: Musico/Delete
        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletarConfirmed(int id)
        {
            Musico musico = db.Musicos.Find(id);
            Banda b = db.Bandas.Where(b1 => b1.MusicoId == musico.MusicoId).FirstOrDefault();
            if (b != null)
            {
                db.Bandas.Remove(b);
                db.SaveChanges();
            }
            var mb2 = db.MusicoBandas.Where(m => m.MusicoId == musico.MusicoId).Count();
            if (mb2 > 0)
            {
                var mb = db.MusicoBandas.Where(m => m.MusicoId == musico.MusicoId).ToList();
                foreach (var item in mb)
                {
                    if (mb != null)
                    {
                        Banda b1 = db.Bandas.Find(item.Fk_Banda);
                        b1.Quantidade = b1.Quantidade - 1;
                        db.Entry(b1).State = EntityState.Modified;
                    }
                }
                db.Musicos.Remove(musico);
            }
            else
            {
                db.Musicos.Remove(musico);
            }
            db.SaveChanges();
            var mensagem = "Cadastro excluido com sucesso!";
            TempData["Mensagem"] = mensagem;
            return RedirectToAction("Deslogar");
        }


        // GET: Musico/Detalhes/5
        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Musico musico = db.Musicos.Find(id);
            if (musico == null)
            {
                return HttpNotFound();
            }
            return View(musico);
        }
    }
}
