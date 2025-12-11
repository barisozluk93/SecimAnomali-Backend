namespace ElectionManagement.Model
{
    public class IlceKazanan
    {
        public string Mahalle {  get; set; }
        public int MahalleId { get; set; }

        public long ToplamOy { get; set; }

        public List<IlceKazananPartiler> Partiler { get; set; }
        public string? Legend { get; set; }
    }
}
