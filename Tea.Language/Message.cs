//-----------------------------------------------------------------------
// <copyright file="Message.cs" company="Jon Rowlett">
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
    /// Message severity.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// Error severity.
        /// </summary>
        Error,

        /// <summary>
        /// Warning severity.
        /// </summary>
        Warning,

        /// <summary>
        /// Info severity.
        /// </summary>
        Info,
    }

    /// <summary>
    /// Compiler output message.
    /// </summary>
    public class Message
    {
        private readonly string path;
        private readonly int line;
        private readonly int column;
        private readonly Severity severity;
        private readonly string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="path">The source file path.</param>
        /// <param name="line">The source line.</param>
        /// <param name="column">The source column.</param>
        /// <param name="sev">The message severity.</param>
        /// <param name="message">The message text.</param>
        public Message(string path, int line, int column, Severity sev, string message)
        {
            this.path = path;
            this.line = line;
            this.column = column;
            this.severity = sev;
            this.message = message;
        }

        /// <summary>
        /// Gets the message severity.
        /// </summary>
        public Severity Severity
        {
            get { return this.severity; }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "{0}({1},{2}):{3}:{4}",
                this.path,
                this.line,
                this.column,
                this.Severity,
                this.message);
        }
    }
}
