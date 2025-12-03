using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimSonucBaslik
    {
        public long Id { get; set; } 
        public string? ad {  get; set; }
        public string? column_NAME { get; set; }
        public int sira_NO { get; set; }
        public long SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }

        public string? Legend { get; set; }
        public string? LogoPath { get; set; }
        public long? IlId { get; set; }


    }
}
