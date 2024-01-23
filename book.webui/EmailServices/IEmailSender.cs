using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace book.webui.EmailServices
{
    public interface IEmailSender
    {
        
        System.Threading.Tasks.Task SendEmailAsync(string email , string subject, string htmlMessage);
    }
}