//-----------------------------------------------------------------------
// <copyright file="MessageLog.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Stream for compiler log messages.
    /// </summary>
    public class MessageLog
    {
        private List<Message> messages = new List<Message>();
        private bool hasErrors;

        /// <summary>
        /// Gets a value indicating whether or not the log has any errors.
        /// </summary>
        public bool HasErrors
        {
            get { return this.hasErrors; }
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        public IEnumerable<Message> Messages
        {
            get
            {
                return this.messages;
            }
        }

        /// <summary>
        /// Writes a message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Write(Message message)
        {
            if (message.Severity == Severity.Error)
            {
                this.hasErrors = true;
            }

            this.messages.Add(message);
        }
    }
}
