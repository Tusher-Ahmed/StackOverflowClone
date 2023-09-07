using NHibernate;
using StackOverflowClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverflowClone.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            IList<Client> users;
            using (ISession session = NHibernateSession.OpenSession())
            {
                users = session.Query<Client>().ToList();
            }
            return View(users);
        }
        public ActionResult Details(int id)
        {
            Client user= new Client();
            using (ISession session = NHibernateSession.OpenSession())
            {
                user = session.Query<Client>().Where(b => b.Id == id).FirstOrDefault();
            }

            return View(user);
        }
    }
}