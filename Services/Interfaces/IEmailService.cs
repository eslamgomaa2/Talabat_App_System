namespace Repository.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmail(string mailto, string subject, string body);
    }
}
