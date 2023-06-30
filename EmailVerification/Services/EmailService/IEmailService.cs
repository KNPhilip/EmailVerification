namespace EmailVerification.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}