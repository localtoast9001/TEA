using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class MessageLog
    {
        private List<Message> messages = new List<Message>();
        private bool hasErrors;

        public bool HasErrors
        {
            get { return this.hasErrors; }
        }

        public IEnumerable<Message> Messages
        {
            get
            {
                return this.messages;
            }
        }

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
