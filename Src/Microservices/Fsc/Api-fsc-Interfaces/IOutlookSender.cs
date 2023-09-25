
namespace Api_fsc_Interfaces
{
    interface IOutlookSender
    {
        void SendEmail(string textMessage, string mailSubject, string recipient);
    }
}
