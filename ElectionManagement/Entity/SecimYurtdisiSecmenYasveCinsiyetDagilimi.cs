using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionManagement.Entity
{
    public class SecimYurtdisiSecmenYasveCinsiyetDagilimi
    {
        public long Id { get; set; }

        public long? SecimId { get; set; }

        [ForeignKey("SecimId")]
        public Secim? Secim { get; set; }
        public string? yas_GRUBU { get; set; }
        public long erkek { get; set; }
        public long kadin { get; set; }
    }
}
