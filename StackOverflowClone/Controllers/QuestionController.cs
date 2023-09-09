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
    
    public class QuestionController : Controller
    {
        // GET: Question
        [AllowAnonymous]
        public ActionResult Index()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var question=session.Query<Question>().ToList();
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
            //Question question = new Question();
            //Client client = new Client();
            //QueWClient queWClient = new QueWClient();
            using (ISession session = NHibernateSession.OpenSession())
            {
                //question = session.Query<Question>().Where(b => b.Id == id).FirstOrDefault();
                //client=session.Query<Client>().Where(u=>u.Id==question.ClientId).FirstOrDefault();                
                //queWClient.Question = question;
                //queWClient.Client = client;
                //return View(queWClient);

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

                    using (ITransaction transaction = session.BeginTransaction())
                    {  
                       
                        session.Save(question);
                        transaction.Commit();
                        return RedirectToAction("Index", "Question");
                    }
                }
            }
            return View(que);

        }
    }
}