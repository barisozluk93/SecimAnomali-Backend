using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimSiyasiParti
    {
        [Key]
        public long Id { get; set; }
        [NotMapped]
        public string? id { get; set; }
        public string? parti_ADI {  get; set; }
        public string? parti_KISA_ADI { get; set; }
        public string? parti_RENK { get; set; }
        public long SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
    }
}
