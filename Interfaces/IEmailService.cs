namespace IngetinGwAPI.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
        Task SendMailpitAsync(string to, string subject, string body);
    }
}
