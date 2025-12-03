using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimMahalle
    {
        [Key]
        public long Id { get; set; }
        //public string? id { get; set; }
        public string? il_ADI {  get; set; }
        public int il_ID { get; set; }
        public string? ilce_ADI { get; set; }
        public int ilce_ID { get; set; }
        public string? min_SANDIK_NO { get; set; }
        public string? max_SANDIK_NO { get; set; }
        public string? muhtarlik_ADI { get; set; }
        public int muhtarlik_ID { get; set; }
        public int secilecek_ADAY_SAYISI { get; set; }
        public int secim_CEVRESI_ID { get; set; }
        public int secim_DETAY_ID { get; set; }
        public int yerlesim_YERI_TURU { get; set; }
        public int belde_ID { get; set; }
        public int birim_ID { get; set; }
        public int cezaevi_ID { get; set; }
        public long SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
    }
}
