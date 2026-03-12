using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimIlAdayCinsiyetDagilimi
    {
        public long Id { get; set; }
        public long? SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
        public long? erkek { get; set; }
        public long? kadin { get; set; }
        public string? il_ADI { get; set; }
        public int? il_ID { get; set; }
        public string? parti_ADI { get; set; }
        public string? parti_KISA_ADI { get; set; }
        public string? parti_RENK { get; set; }
    }
}
