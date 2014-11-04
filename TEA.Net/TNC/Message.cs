using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal enum Severity
    {
        Error,
        Warning,
        Info
    }

    internal class Message
    {
        string path;
        int line;
        int column;
        Severity severity;
        string message;

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
