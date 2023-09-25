namespace Api_fsc_Entities.Models
{
    public class SmtpSetting
    {
        public bool SendEmail { get; set; }
        public string StmpServerAddress { get; set; }
        public string MailFrom { get; set; }
    }
}