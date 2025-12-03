namespace ElectionManagement.Entity
{
    public class Secim
    {
        public long Id { get; set; }
        public long SecimId { get; set; }
        public string SecimAdi { get; set; }
        public DateOnly SecimTarihi { get; set; }
        public long SecimIDAsil { get; set; }

        public int SecimTuru { get; set; }
    }
}
