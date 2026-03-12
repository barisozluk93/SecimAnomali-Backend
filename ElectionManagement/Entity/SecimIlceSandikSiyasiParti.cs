using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimIlceSandikSiyasiParti
    {
        public long Id { get; set; }
        public long? SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
        public string? ilce_ADI { get; set; }
        public string? ilce_KODU { get; set; }
        public string? il_ADI { get; set; }
        public int? il_ID { get; set; }
        public string? parti_ADI { get; set; }
        
    }
}
