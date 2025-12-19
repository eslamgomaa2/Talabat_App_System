using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmail(string mailto, string subject, string body);
    }
}
