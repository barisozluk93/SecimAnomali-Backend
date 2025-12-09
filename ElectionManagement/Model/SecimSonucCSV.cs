using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Model
{
    public class SecimSonucCSV
    {
        public string Secim { get; set; }
        public string Tarih { get; set; }

        public string? Il { get; set; }
        public string? Ilce { get; set; }

        //public long gecerli_OY_TOPLAMI { get; set; }
        //public long gecersiz_OY_TOPLAMI { get; set; }

        //public string? muhtarlik_ADI { get; set; }
        //public int muhtarlik_ID { get; set; }

        public long ToplamKullanilanOy { get; set; }

        public long Akp { get; set; }
        public long Chp { get; set; }
        public long Mhp { get; set; }
        public long Dem { get; set; }
        public long Saadet { get; set; }
        
    }
}
