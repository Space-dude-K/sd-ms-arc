namespace Api_fsc_Interfaces
{
    interface IMailSender
    {
        void SendEmail(string textMessage, string mailSubject, string mailAddress, string smtpServer, string mailFrom);
    }
}