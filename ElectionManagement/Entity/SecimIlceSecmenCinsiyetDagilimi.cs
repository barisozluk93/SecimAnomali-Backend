using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimIlceSecmenCinsiyetDagilimi
    {
        public long Id { get; set; }

        public long? SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }

        public long? erkek {  get; set; }

        public long? erkek_ORAN { get; set; }
        public long? kadin { get; set; }

        public long? kadin_ORAN { get; set; }
        public long? toplam { get; set; }

        public string? il_ADI { get; set; }
        public int? il_ID { get; set; }
        public string? ilce_ADI { get; set; }
        public int? ilce_ID { get; set; }
    }
}
