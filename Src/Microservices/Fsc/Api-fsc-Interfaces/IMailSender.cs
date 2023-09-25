namespace Api_fsc_Interfaces
{
    public interface IMailSender
    {
        void SendEmail(string textMessage, string mailSubject, string mailAddress, string smtpServer, string mailFrom);
    }
}