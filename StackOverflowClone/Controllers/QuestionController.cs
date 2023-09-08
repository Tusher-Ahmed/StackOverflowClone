using NHibernate;
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
        public ActionResult QuestionDetails(long id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Question question = new Question();
            Client client = new Client();
            QueWClient queWClient = new QueWClient();
            using (ISession session = NHibernateSession.OpenSession())
            {
                question = session.Query<Question>().Where(b => b.Id == id).FirstOrDefault();
                client=session.Query<Client>().Where(u=>u.Id==question.ClientId).FirstOrDefault();                
                queWClient.Question = question;
                queWClient.Client = client;
                return View(queWClient);

            }

        }
    }
}