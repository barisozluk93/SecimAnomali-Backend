using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimIl
    {
        [Key]
        public long Id { get; set; }
        //public string? id { get; set; }
        public string? il_ADI {  get; set; }
        public int il_ID { get; set; }
        public int secilecek_ADAY_SAYISI { get; set; }
        public int secim_CEVRESI_ID { get; set; }
        public int secim_DETAY_ID { get; set; }
        public int yerlesim_YERI_TURU { get; set; }
        public long SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
    }
}
