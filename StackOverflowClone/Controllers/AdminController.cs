using NHibernate;
using StackOverflowClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverflowClone.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var question = session.Query<Question>().ToList();
                foreach (var ques in question)
                {
                    NHibernateUtil.Initialize(ques.Client); // Initialize the proxy
                }

                return View(question);
            }
            
        }

        public ActionResult Approve(long id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            using (ISession session = NHibernateSession.OpenSession())
            {
                var question = session.Query<Question>().FirstOrDefault(u => u.Id == id);
                if (question != null)
                {
                    return View(question);
                }

                return RedirectToAction("Index");
            }
        }




        public ActionResult Delete(long id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            using (ISession session = NHibernateSession.OpenSession())
            {
                var question = session.Query<Question>().FirstOrDefault(u=>u.Id==id);
                if(question != null)
                {
                    return View(question);
                }

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(long id, Question ques)
        {
            try
            {
                // TODO: Add delete logic here
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Question question = session.Get<Question>(id);

                    using (ITransaction trans = session.BeginTransaction())
                    {
                        session.Delete(question);
                        trans.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}