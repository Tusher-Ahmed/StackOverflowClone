using NHibernate;
using StackOverflowClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverflowClone.Controllers
{
    [Authorize]
   // [AllowAnonymous]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var question = session.Query<Question>().Where(u=>u.Approve==false).ToList();                
                return View(question);
            }
            
        }

        public ActionResult Approve(long id)
        {
            if (id == 0)
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
        [HttpPost]
        public ActionResult Approve(long id, Question ques)
        {
            try
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    session.Clear();
                    var question = session.Get<Question>(id);
                    question.Approve = true;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(question);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public ActionResult ApprovedQuestionList()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var question = session.Query<Question>().Where(u => u.Approve == true).ToList();
                return View(question);
            }
        }

        public ActionResult Delete(long id)
        {
            if (id == 0)
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
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Question question = session.Get<Question>(id);

                    if (question.Approve == true)
                    {
                        using (ITransaction trans = session.BeginTransaction())
                        {
                            session.Delete(question);
                            trans.Commit();
                            return RedirectToAction("ApprovedQuestionList","Admin");
                        }
                    }
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
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }

        public ActionResult Users()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var client = session.Query<Client>().ToList();
                return View(client);
            }
        }

        public ActionResult DeleteUser(long id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            using (ISession session = NHibernateSession.OpenSession())
            {
                var client = session.Query<Client>().FirstOrDefault(u => u.Id == id);
                if (client != null)
                {
                    return View(client);
                }

                return RedirectToAction("Users","Admin");
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(long id, Client cli)
        {
            try
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Client client = session.Get<Client>(id);

                    
                    using (ITransaction trans = session.BeginTransaction())
                    {
                        session.Delete(client);
                        trans.Commit();
                    }
                }
                return RedirectToAction("Users","Admin");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }

        public ActionResult RoleProvider()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var roles = session.Query<UserRoles>().Where(u => u.Role != "Admin").ToList();
                return View(roles);
            }
        }
        public ActionResult RoleProviderEdit(long id)
        {
            if (id == 0)
            {
                return RedirectToAction("RoleProvider", "Admin");
            }
            UserRoles userRoles=new UserRoles();
            using (ISession session = NHibernateSession.OpenSession())
            {
                userRoles = session.Query<UserRoles>().Where(b => b.Id == id).FirstOrDefault();
            }


            return View(userRoles);

        }
        [HttpPost]
        public ActionResult RoleProviderEdit(long id,UserRoles ur)
        {
            try
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    session.Clear();
                    var userRoles = session.Get<UserRoles>(id);
                    userRoles.Role =ur.Role ;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(userRoles);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("RoleProvider", "Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        public ActionResult RoleProviderAdmin()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var roles = session.Query<UserRoles>().Where(u => u.Role == "Admin").ToList();
                return View(roles);
            }
        }
        public ActionResult RoleProviderAdminEdit(long id)
        {
            if (id == 0)
            {
                return RedirectToAction("RoleProviderAdmin", "Admin");
            }
            UserRoles userRoles = new UserRoles();
            using (ISession session = NHibernateSession.OpenSession())
            {
                userRoles = session.Query<UserRoles>().Where(b => b.Id == id).FirstOrDefault();
            }


            return View(userRoles);

        }
        [HttpPost]
        public ActionResult RoleProviderAdminEdit(long id, UserRoles ur)
        {
            try
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    session.Clear();
                    var userRoles = session.Get<UserRoles>(id);
                    userRoles.Role = ur.Role;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(userRoles);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("RoleProviderAdmin", "Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

    }
}