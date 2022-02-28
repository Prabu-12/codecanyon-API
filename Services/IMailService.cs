using System.Collections.Generic;
using System.Threading.Tasks;
using QuizplusApi.ViewModels.Email;

namespace QuizplusApi.Services
{
    public interface IMailService
    {
        Task SendWelcomeEmailAsync(WelcomeRequest request);
        Task SendPasswordEmailAsync(ForgetPassword request);
        Task SendInvitationEmailAsync(List<Invitation> listOfAddress);
        Task SendReportEmailAsync(ReExamRequest report);
    }
}