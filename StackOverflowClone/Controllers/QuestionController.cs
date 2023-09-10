using NHibernate;
using NHibernate.Criterion;
using NHibernate.Mapping.ByCode;
using StackOverflowClone.Models;
using StackOverflowClone.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverflowClone.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        // GET: Question
        [AllowAnonymous]
        public ActionResult Index()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var question=session.Query<Question>().Where(u=>u.Approve==true).ToList();
                foreach (var ques in question)
                {
                    NHibernateUtil.Initialize(ques.Client); // Initialize the proxy
                }

                return View(question);
            }
                
        }
        [AllowAnonymous]
        public ActionResult QuestionDetails(long id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            using (ISession session = NHibernateSession.OpenSession())
            {

                var question = session.CreateCriteria<Question>().Add(Restrictions.IdEq(id))
                         .SetFetchMode("Client", FetchMode.Eager)
                         .UniqueResult<Question>();

                if (question == null)
                {            
                    return RedirectToAction("NotFound");
                }

                QueWClient queWClient = new QueWClient
                {
                    Question = question,
                    Client = question.Client
                };

                return View(queWClient);

            }

        }

        public ActionResult Profile()
        {
            if (User.Identity.IsAuthenticated)
            {
                string username = User.Identity.Name;
                
                
                using (ISession session = NHibernateSession.OpenSession())
                {
                    var profile = session.Query<Client>().Where(u => u.Email == username).FirstOrDefault();
                    IList<Question> questions = session.QueryOver<Question>()
                                                .JoinQueryOver(q => q.Client)
                                                .Where(c => c.Id == profile.Id)
                                                .List();
                    QueWClient queWClient = new QueWClient();
                    queWClient.Client = profile;
                    queWClient.Questions = questions;
                    return View(queWClient);
                }

            }
            return RedirectToAction("Index");
        }

        public ActionResult EditProfile(long id)
        {
            if (id == null)
            {
                return RedirectToAction("Profile", "Question");
            }
            Client client = new Client();
            using (ISession session = NHibernateSession.OpenSession())
            {
                client = session.Query<Client>().Where(b => b.Id == id).FirstOrDefault();
            }


            return View(client);

        }
        [HttpPost]
        public ActionResult EditProfile(long id, Client cli)
        {
            try
            {
           
                // TODO: Add insert logic here
                using (ISession session = NHibernateSession.OpenSession())
                {
                    session.Clear();
                    var client = session.Get<Client>(id);
                    //Client client = new Client();              
                    client.Username = cli.Username;
                    
                    client.AboutMe = cli.AboutMe;
                    
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(client);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Profile","Question");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        //[Authorize]
        [AllowAnonymous]
        public ActionResult AskQuestion()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult AskQuestion(Question que)
        {

            if(ModelState.IsValid)
            {
                var email=User.Identity.Name;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    var profile=session.Query<Client>().FirstOrDefault(u => u.Email == email);
                    Question question = new Question();
                    question.Title = que.Title;
                    question.Body = que.Body;
                    question.ExpectResult = que.ExpectResult;
                    question.Tags = que.Tags;
                    question.ClientId = profile.Id;
                    question.CreatedAt = DateTime.Now;
                    question.Approve=false;

                    using (ITransaction transaction = session.BeginTransaction())
                    {  
                       
                        session.Save(question);
                        transaction.Commit();
                        return RedirectToAction("Index", "Question");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid Input");
            return View(que);

        }
    }
}