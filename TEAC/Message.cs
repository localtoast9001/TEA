//-----------------------------------------------------------------------
// <copyright file="Message.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal enum Severity
    {
        Error,
        Warning,
        Info
    }

    internal class Message
    {
        private readonly string path;
        private readonly int line;
        private readonly int column;
        private readonly Severity severity;
        private readonly string message;

        public Message(string path, int line, int column, Severity sev, string message)
        {
            this.path = path;
            this.line = line;
            this.column = column;
            this.severity = sev;
            this.message = message;
        }

        public Severity Severity
        {
            get { return this.severity; }
        }

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
