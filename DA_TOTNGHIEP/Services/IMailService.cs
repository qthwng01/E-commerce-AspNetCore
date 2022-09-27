using DA_TOTNGHIEP.MailSetting;
using DA_TOTNGHIEP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DA_TOTNGHIEP.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendWelcomeEmailAsync(Invoices invoices);
    }
}
