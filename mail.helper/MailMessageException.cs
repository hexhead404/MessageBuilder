using System;
using System.Net.Mail;
using EG.WS.Common.Helpers.ExtensionMethods;

namespace EG.WS.Common.Helpers
{
    /// <summary>
    /// Adds info about the email message to the error message
    /// </summary>
    public class MailMessageException : Exception
    {
        private string to, subject, body, from, cc, bcc, relayServer;

        public MailMessageException(Exception ex) : base(ex.Message, ex)
        {
        }

        public MailMessageException(Exception ex, MailMessage message) : this(ex)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.from = message.From?.DisplayName;
            this.subject = message.Subject;
            this.body = message.Body;
            this.to = message.To.ExpandToStringAddressList();
            this.cc = message.CC.ExpandToStringAddressList();
            this.bcc = message.Bcc.ExpandToStringAddressList();
        }

        public MailMessageException(Exception ex, string from, string subject, string body, string to, string cc = null, string bcc = null) : this(ex)
        {
            this.from = from;
            this.subject = subject;
            this.body = body;
            this.to = to;
            this.cc = cc;
            this.bcc = bcc;
        }

        public MailMessageException(Exception ex, string from, string subject, string body, string[] to, string[] cc = null, string[] bcc = null) : this(ex)
        {
            this.from = from;
            this.subject = subject;
            this.body = body;
            this.to = to.ExpandToStringAddressList();
            this.cc = cc.ExpandToStringAddressList();
            this.bcc = bcc.ExpandToStringAddressList();
        }


        #region Chained setters
        public MailMessageException RelayServer(string value)
        {
            relayServer = value;
            return this;
        }
        public MailMessageException Bcc(string value)
        {
            bcc = value;
            return this;
        }
        public MailMessageException Bcc(MailAddressCollection value)
        {
            bcc = value.ToString();
            return this;
        }
        public MailMessageException Cc(string value)
        {
            cc = value;
            return this;
        }
        public MailMessageException Cc(MailAddressCollection value)
        {
            cc = value.ToString();
            return this;
        }
        public MailMessageException From(string value)
        {
            from = value;
            return this;
        }
        public MailMessageException From(MailAddress value)
        {
            from = value.ToString();
            return this;
        }
        public MailMessageException Body(string value)
        {
            body = value;
            return this;
        }
        public MailMessageException Subject(string value)
        {
            subject = value;
            return this;
        }
        
        public MailMessageException To(string value)
        {

            to = value;
            return this;
        }
        public MailMessageException To(MailAddressCollection value)
        {
            to = value.ToString();
            return this;
        }
        #endregion

        public override string Message
        {
            get
            {
                string messageInfo = string.Format("To: {0}\nSubject: {1}\nBody: {2}\nFrom: {3}\nCC: {4}\nBCC: {5}\nRelay Server: {6}\n", to, subject, body, from, cc, bcc, relayServer);
                return String.Format("{0}\n{1}", base.Message, messageInfo);
            }
        }

        public MailMessageException SetMailMessage(MailMessage msg)
        {
            if(msg != null)
            {
                to = msg.To.ToString();
                from = msg.From.ToString();
                cc = msg.CC.ToString();
                bcc = msg.Bcc.ToString();
                subject = msg.Subject;
                body = msg.Body;
            }
            return this;
        }


    }
}
