// <copyright file="MailMessageExtensions.cs" company="Essent Guaranty, Inc">
// Copyright (c) Essent Guaranty, Inc. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace EG.WS.Common.Helpers.ExtensionMethods
{
    /// <summary>
    /// Extension methods for building <see cref="MailMessage"/>s
    /// </summary>
    public static class MailMessageExtensions
    {
        /// <summary>
        /// Parses specified <paramref name="addresses"/> and adds zero-to-many <see cref="MailAddress"/>s to the <see cref="MailAddressCollection"/>
        /// </summary>
        /// <param name="collection">The <see cref="MailAddressCollection"/></param>
        /// <param name="addresses">The address(es) to add</param>
        /// <exception cref="ArgumentNullException">When <paramref name="collection"/> is null</exception>
        public static void AddAddresses(this MailAddressCollection collection, params string[] addresses)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var address in addresses ?? Enumerable.Empty<string>())
            {
                address?.Replace("\r", string.Empty).Replace("\n", string.Empty)
                    .Split(';').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s))
                    .ToList().ForEach(collection.Add);
            }
        }

        /// <summary>
        /// Expands the <paramref name="collection"/> into a string of addresses
        /// </summary>
        /// <param name="collection">The <see cref="MailAddressCollection"/></param>
        public static string ExpandToStringAddressList(this MailAddressCollection collection)
        {
            return collection == null ? string.Empty : string.Join(";", collection.Select(a => a.DisplayName));
        }

        /// <summary>
        /// Expands the <paramref name="addresses"/> into a string of addresses
        /// </summary>
        /// <param name="addresses">The addresses</param>
        public static string ExpandToStringAddressList(this string[] addresses)
        {
            return addresses == null ? string.Empty : string.Join(";", addresses);
        }

        /// <summary>
        /// Adds the specified <paramref name="attachmentPaths"/> to the <see cref="MailMessage"/>
        /// </summary>
        /// <param name="message">The <see cref="MailMessage"/></param>
        /// <param name="attachmentPaths">The path(s) of the attachment(s) to add</param>
        /// <exception cref="ArgumentNullException">When <paramref name="message"/> is null</exception>
        public static void AddAttachments(this MailMessage message, params string[] attachmentPaths)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            foreach (var path in attachmentPaths ?? Enumerable.Empty<string>())
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Attachment file '{path}' not found");
                }
                message.Attachments.Add(new Attachment(path));
            }
        }

        /// <summary>
        /// Adds the specified <paramref name="attachments"/> to the <see cref="MailMessage"/>
        /// </summary>
        /// <param name="message">The <see cref="MailMessage"/></param>
        /// <param name="attachments">The attachment(s) to add</param>
        /// <exception cref="ArgumentNullException">When <paramref name="message"/> is null</exception>
        public static void AddAttachments(this MailMessage message, params Attachment[] attachments)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            foreach (var attachment in attachments ?? Enumerable.Empty<Attachment>())
            {
                message.Attachments.Add(attachment);
            }
        }

        /// <summary>
        /// Gets the <see cref="MailPriority"/> associated with a <see cref="EventLogEntryType"/>
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns>The <see cref="MailPriority"/></returns>
        public static MailPriority Priority(this EventLogEntryType eventType)
        {
            return eventType == EventLogEntryType.Error ? MailPriority.High : MailPriority.Normal;
        }
                
        /// <summary>
        /// Gets the keys used for duplicate checking
        /// </summary>
        /// <param name="message">The <see cref="MailMessage"/></param>
        /// <param name="relayServer">The relay server</param>
        /// <returns>The keys</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="message"/> is null</exception>
        internal static IEnumerable<string> GetKeys(this MailMessage message, string relayServer)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            yield return message.From?.DisplayName;
            yield return message.Subject;
            yield return message.Body;
            yield return message.To?.ExpandToStringAddressList();
            yield return message.CC?.ExpandToStringAddressList();
            yield return message.Bcc?.ExpandToStringAddressList();
            yield return message.Priority.ToString();
        }
    }
}
