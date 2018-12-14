using EG.WS.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace mail.helper
{
    class Program
    {
        private const string From = "some.body@gmail.com";
        private const string To = "ddebald@gmail.com";

        static void Main(string[] args)
        {
            MailHelper.Send(From, "test", "text body", To, MailHelper.Options.OnException(MailExceptionActionType.LogAndContinue));
            MailHelper.Send(From, "test", "<b>html body</b>", To, MailHelper.Options.IsBodyHtml(true).OnException(MailExceptionActionType.LogAndContinue));
            MailHelper.Send(From, "test", "<b>html, high priority, has an attachment</b>", To, MailHelper.Options.IsBodyHtml(true).Priority(MailPriority.High).AddAttachments(@"C:\data\requests.txt").OnException(MailExceptionActionType.LogAndContinue));
        }
    }
}
