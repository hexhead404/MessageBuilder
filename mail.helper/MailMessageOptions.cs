// <copyright file="MailMessageOptions.cs" company="Essent Guaranty, Inc">
// Copyright (c) Essent Guaranty, Inc. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using EG.WS.Common.Helpers.ExtensionMethods;

namespace EG.WS.Common.Helpers
{
    /// <summary>
    /// A class for setting <see cref="MailMessage"/> properties
    /// </summary>
    public class MailMessageOptions
    {
        #region Fields

        private List<Action<MailMessage>> actions = new List<Action<MailMessage>>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the relay server to use for sending the message (default is the
        /// configured EmailServer)
        /// </summary>
        public string RelayServer { get; set; } = MailHelper.EmailServer;

        /// <summary>
        /// Gets or sets the action to take when mail <see cref="Exception"/>s occur
        /// </summary>
        public MailExceptionActionType ExceptionAction { get; set; } = MailExceptionActionType.LogAndContinue;

        #endregion

        #region Public Methods

        /// <summary>
        /// Use the specified relay server when sending the message (default is the configured EmailServer)
        /// </summary>
        /// <param name="relayServer">The relay server to use</param>
        /// <returns>The <see cref="MailMessageOptions"/> to allow method chaining</returns>
        public MailMessageOptions UsingRelay(string relayServer)
        {
            this.RelayServer = relayServer;
            return this;
        }

        /// <summary>
        /// Sets the action to take when <see cref="Exception"/>s occur (default
        /// is <see cref="MailExceptionActionType.LogAndContinue"/>)
        /// </summary>
        /// <param name="action"></param>
        /// <returns>The <see cref="MailMessageOptions"/> to allow method chaining</returns>
        public MailMessageOptions OnException(MailExceptionActionType action)
        {
            this.ExceptionAction = action;
            return this;
        }

        /// <summary>
        /// Adds the specified email address(es) to the message's To collection
        /// </summary>
        /// <param name="addresses">The address(es) to add</param>
        /// <returns>The <see cref="MailMessageOptions"/> to allow method chaining</returns>
        public MailMessageOptions AddTo(params string[] addresses)
        {
            this.actions.Add(m => m.To.AddAddresses(addresses));
            return this;
        }

        /// <summary>
        /// Adds the specified email address(es) to the message's CC collection
        /// </summary>
        /// <param name="addresses">The address(es) to add</param>
        /// <returns>The <see cref="MailMessageOptions"/> to allow method chaining</returns>
        public MailMessageOptions AddCc(params string[] addresses)
        {
            this.actions.Add(m => m.CC.AddAddresses(addresses));
            return this;
        }

        /// <summary>
        /// Adds the specified email address(es) to the message's BCC collection
        /// </summary>
        /// <param name="addresses">The address(es) to add</param>
        /// <returns>The <see cref="MailMessageOptions"/> to allow method chaining</returns>
        public MailMessageOptions AddBcc(params string[] addresses)
        {
            this.actions.Add(m => m.Bcc.AddAddresses(addresses));
            return this;
        }

        /// <summary>
        /// Adds the specified attachments to the message
        /// </summary>
        /// <param name="attachments">The attachment(s) to add</param>
        /// <returns>The <see cref="MailMessageOptions"/> to allow method chaining</returns>
        public MailMessageOptions AddAttachments(params Attachment[] attachments)
        {
            this.actions.Add(m => m.AddAttachments(attachments));
            return this;
        }

        /// <summary>
        /// Attaches the files specified by their paths
        /// </summary>
        /// <param name="filePaths">The path(s) of the files to attach</param>
        /// <returns>The <see cref="MailMessageOptions"/> to allow method chaining</returns>
        public MailMessageOptions AddAttachments(params string[] filePaths)
        {
            this.actions.Add(m => m.AddAttachments(filePaths));
            return this;
        }

        /// <summary>
        /// Sets the message priority
        /// </summary>
        /// <param name="priority">The <see cref="MailPriority"/></param>
        /// <returns>The <see cref="MailMessageOptions"/> to allow method chaining</returns>
        public MailMessageOptions Priority(MailPriority priority)
        {
            this.actions.Add(m => m.Priority = priority);
            return this;
        }

        /// <summary>
        /// States whether the body of the message is HTML
        /// </summary>
        /// <param name="value">Whether the body of the message is HTML</param>
        /// <returns>The <see cref="MailMessageOptions"/> to allow method chaining</returns>
        public MailMessageOptions IsBodyHtml(bool value)
        {
            this.actions.Add(m => m.IsBodyHtml = true);
            return this;
        }

        /// <summary>
        /// Indicates the <see cref="Encoding"/> used for the body of the message
        /// </summary>
        /// <returns>The <see cref="MailMessageOptions"/> to allow method chaining</returns>
        public MailMessageOptions BodyEncoding(Encoding encoding)
        {
            this.actions.Add(m => m.BodyEncoding = encoding);
            return this;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Applies the options to the <see cref="MailMessage"/>
        /// </summary>
        /// <param name="message">The <see cref="MailMessage"/></param>
        internal void ApplyOptions(MailMessage message)
        {
            var exceptions = new List<Exception>();
            foreach (var action in this.actions)
            {
                try
                {
                    action(message);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Any())
            { 
                throw new AggregateException("Error setting mail message options", exceptions);
            }
        }

        #endregion
    }
}
