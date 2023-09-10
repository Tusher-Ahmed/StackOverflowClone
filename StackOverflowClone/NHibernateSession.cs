using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflowClone
{
    public class NHibernateSession
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\hibernate.cfg.xml");
            configuration.Configure(configurationPath);
            var clientConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\Client.hbm.xml");
            var questionConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\Question.hbm.xml");
            var answerConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\Answer.hbm.xml");
            configuration.AddFile(clientConfigurationFile);
            configuration.AddFile(questionConfigurationFile);
            configuration.AddFile(answerConfigurationFile);
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}