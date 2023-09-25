
namespace Api_fsc_Interfaces
{
    public interface IOutlookSender
    {
        void SendEmail(string textMessage, string mailSubject, string recipient);
    }
}
