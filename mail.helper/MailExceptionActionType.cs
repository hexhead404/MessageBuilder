// <copyright file="MailExceptionActionType.cs" company="Essent Guaranty, Inc">
// Copyright (c) Essent Guaranty, Inc. All rights reserved.
// </copyright>

namespace EG.WS.Common.Helpers
{
    /// <summary>
    /// An enumeration of actions to take when mail <see cref="Exception"/>s occur
    /// </summary>
    public enum MailExceptionActionType
    {
        /// <summary>
        /// Log the exception and continue
        /// </summary>
        LogAndContinue = 0,

        /// <summary>
        /// Log the exception and throw
        /// </summary>
        LogAndThrow = 1
    }
}
