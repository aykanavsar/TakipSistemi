namespace TakipSistemi.Models
{
    public class ReminderItem
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string AssetName { get; set; }
        public string AssetType { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public int ValidityMonths { get; set; }
        public DateTime EndDate { get; set; }
        public int RemainingDays { get; set; }
    }
}
