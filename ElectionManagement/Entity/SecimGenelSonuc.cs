using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimGenelSonuc
    {
        public long Id { get; set; }
        public long acilan_SANDIK_SAYISI { get; set; }
        public long acilan_SECMEN_SAYISI { get; set; }
        public long birlestirme_TUTANAGI_DIS_TEMSILCILIK { get; set; }
        public long birlestirme_TUTANAGI_GUMRUKLER { get; set; }
        public int birlestirme_TUTANAGI_GUMRUK_ILCE { get; set; }
        public int birlestirme_TUTANAGI_GUMRUK_KURUL { get; set; }
        public int birlestirme_TUTANAGI_GUMRUK_RUMUZ { get; set; }
        public int birlestirme_TUTANAGI_IL { get; set; }
        public int birlestirme_TUTANAGI_ILCE { get; set; }
        public int birlestirme_TUTANAGI_KURUL { get; set; }
        public int birlestirme_TUTANAGI_TUMDUNYA { get; set; }
        public int birlestirme_TUTANAGI_ULKE { get; set; }
        public int birlestirme_TUTANAGI_ULKELER { get; set; }
        public long gecerli_OY_TOPLAMI { get; set; }
        public long gecersiz_OY_TOPLAMI { get; set; }
        public long itirazli_GECERLI_OY_SAYISI { get; set; }
        public long itirazsiz_GECERLI_OY_SAYISI { get; set; }
        public long oy_KULLANAN_SECMEN_SAYISI { get; set; }
        public int secilecek_ADAY_SAYISI { get; set; }
        public long secmen_SAYISI { get; set; }
        public long toplam_SANDIK_SAYISI { get; set; }
        public long SecimId {  get; set; }
        
        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
        public long? IlId { get; set; }
        public long? IlceId { get; set; }
        public long? MahalleId { get; set; }
    }
}
