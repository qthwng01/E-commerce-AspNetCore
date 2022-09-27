using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Email.Services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridKey { get; set; }
        public string SendGridUser { get; set; }
    }
}
