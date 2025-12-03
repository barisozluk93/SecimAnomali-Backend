namespace ElectionManagement.Model
{
    public class IlKazanan
    {
        public string Ilce {  get; set; }
        public int IlceId { get; set; }

        public long ToplamOy { get; set; }

        public List<IlKazananPartiler> Partiler { get; set; }
        public string? Legend { get; set; }
    }
}
