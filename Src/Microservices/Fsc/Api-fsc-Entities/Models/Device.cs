namespace Api_fsc_Entities.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Ip {  get; set; }
        public string Disk { get; set; }
        public float? TotalSpace {  get; set; }
        public float? FreeSpace { get; set; }
        public DateTime DateTime { get; set; }
        public int? UserId { get; set; }
    }
}