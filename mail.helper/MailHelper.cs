// <copyright file="MailHelper.cs" company="Essent Guaranty, Inc">
// Copyright (c) Essent Guaranty, Inc. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using EG.WS.Common.Helpers.ExtensionMethods;

namespace EG.WS.Common.Helpers
{
    public static class MailHelper
    {
        public static string EmailServer = "127.0.0.1";

        /// <summary>
        /// Gets an instance of <see cref="MailMessageOptions/> to allow customization of the mail message
        /// </summary>
        public static MailMessageOptions Options => new MailMessageOptions();

        /// <summary>
        /// Sends an email using the specified <see cref="MailMessageOptions"/>
        /// </summary>
        /// <remarks>Exceptions are written to the event log</remarks>
        /// <param name="from">The from email address</param>
        /// <param name="subject">The email subject</param>
        /// <param name="body">The email body</param>
        /// <param name="options">The message options</param>
        /// <param name="relayServer">Optional relay server to use</param>
        public static void Send(string from, string subject, string body, string to, MailMessageOptions options)
        {
            MailMessage message = new MailMessage();
            try
            {
                // Build the message
                try
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.From = new MailAddress(from);
                    message.To.AddAddresses(to);
                    options?.ApplyOptions(message);
                }
                catch (Exception ex)
                {
                    // Errors here are due to arguments (ie bad email addresses)
                    throw new ArgumentException(ex.Message, ex);
                }

                // Ensure there are recipients
                if (!message.To.Any() && !message.CC.Any() && !message.Bcc.Any())
                {
                    throw new ArgumentException($"No recipients were specified");
                }

                // Send the message
                Send(message, options.RelayServer);
            }
            catch (Exception ex)
            {
                ex = new MailMessageException(ex, message);
                //EventHelper.LogError(ex, false);
                if (options.ExceptionAction == MailExceptionActionType.LogAndThrow)
                {
                    throw ex;
                }
            }
        }

        #region Private Methods

        private static void Send(MailMessage msg, string relayServer = null)
        {
            relayServer = relayServer ?? EmailServer;
            using (var client = new SmtpClient(relayServer))
            {
                try
                {
                    client.Send(msg);
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion

    }
}
