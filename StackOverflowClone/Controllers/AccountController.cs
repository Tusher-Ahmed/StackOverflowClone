using NHibernate;
using StackOverflowClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
namespace StackOverflowClone.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        { 

            return View();
        }
        [HttpPost]
        public ActionResult Login(Client client)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                if(ModelState.IsValid)
                {
                    var isValid = session.Query<Client>().Where(b => b.Email == client.Email && b.Password == client.Password).FirstOrDefault();
                    ;
                    if (isValid != null) {
                        var name = isValid.Email;                       
                        FormsAuthentication.SetAuthCookie(name, false);
                        return RedirectToAction("Index", "Question");
                    }
                    
                }                  
                    ModelState.AddModelError("", "Invalid Email or Password.");
                    return View();   
            }
            
        }
        public ActionResult SignUp()
        {          
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Client cli)
        {
            try
            {
                Client client = new Client();  
                client.Email = cli.Email;
                client.Password = cli.Password;
                client.Username= cli.Username;
                
             
                using (ISession session = NHibernateSession.OpenSession())
                {
                    var present= session.Query<Client>().Where(b => b.Email == client.Email).FirstOrDefault();
                    if (present != null)
                    {
                        ModelState.AddModelError("", "Email is already registered");
                        return View();
                    }
                    using (ITransaction transaction = session.BeginTransaction())   
                    {
                        session.Save(client); 
                        transaction.Commit();   
                    }
                }
                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                ViewBag.Exception=e.Message;
                return View();
            }
            
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index","Question");
        }

    }
}