namespace IngetinGwAPI.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
        Task<bool> SendMailpitAsync(string to, string subject, string body);
    }
}
