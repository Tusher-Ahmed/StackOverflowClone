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
                Dictionary<long,string> result = new Dictionary<long,string>();
                foreach(var item in question)
                {
                    var client=session.Query<Client>().FirstOrDefault(u=>u.Id==item.ClientId);
                    result.Add(item.Id, client.Username);
                }
                QueWClient queWClient = new QueWClient
                {
                    Questions = question,
                    MapedQC = result
                    
                };
                return View(queWClient);
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

                var question = session.Query<Question>().FirstOrDefault(u=>u.Id==id);
                var client=session.Query<Client>().FirstOrDefault(u=>u.Id==question.ClientId);
                var answer= new List<Answer>();
                var answer1 = session.Query<Answer>().Where(u => u.QuestionId == question.Id).FirstOrDefault();
                if (answer1 != null)
                {
                     answer = session.Query<Answer>().Where(u => u.QuestionId == question.Id).ToList();
                }
                else
                {
                    answer = new List<Answer>();
                }
                
                if (question == null)
                {            
                    return RedirectToAction("Index");
                }

                QueWClient queWClient = new QueWClient
                {
                    Question = question,
                    Client = client,
                    Answers = answer
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
                    var questions = session.Query<Question>().Where(u => u.ClientId == profile.Id).ToList();
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
            if (id == 0)
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
           
            return View(que);

        }
        [HttpPost]
        public ActionResult AddComment(string CommentText, long QuestionId)
        {
            if (CommentText == null)
            {
                return RedirectToAction("QuestionDetails", "Question", new {id= QuestionId });
            }
            var email = User.Identity.Name;
            using (ISession session = NHibernateSession.OpenSession())
            {
                var profile = session.Query<Client>().FirstOrDefault(u => u.Email == email);
                Answer answer = new Answer();
                answer.QuestionsAnswer = CommentText;
                answer.Username= profile.Username;
                answer.QuestionId = QuestionId;
                answer.Vote = 0;
                answer.CreatedAt= DateTime.Now;
                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.Save(answer);
                    transaction.Commit();
                    return RedirectToAction("QuestionDetails", "Question",new { id = QuestionId });
                }
            }           
        }
    }
}