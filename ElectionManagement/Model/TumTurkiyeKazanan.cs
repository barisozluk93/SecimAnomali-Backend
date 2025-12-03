namespace ElectionManagement.Model
{
    public class TumTurkiyeKazanan
    {
        public string Il {  get; set; }
        public int IlId { get; set; }

        public long ToplamOy { get; set; }

        public List<TumTurkiyeKazananPartiler> Partiler { get; set; }
        public string? Legend { get; set; }
    }
}
