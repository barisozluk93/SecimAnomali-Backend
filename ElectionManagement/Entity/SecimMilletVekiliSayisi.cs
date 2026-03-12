using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimMilletVekiliSayisi
    {
        [Key]
        public long Id { get; set; }
        public string? belde_ADI { get; set; }
        public string? il_ADI {  get; set; }
        public int? il_KODU { get; set; }
        public string? ilce_ADI { get; set; }
        public int? ilce_KODU { get; set; }
        public int? secilecek_ADAY_SAYISI { get; set; }
        public int? secim_CEVRESI_ID { get; set; }
        public long SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
    }
}
