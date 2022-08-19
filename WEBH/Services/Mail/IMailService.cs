using System.Threading.Tasks;
using WebH.Models;

namespace WebH.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

    }
}
