using Microsoft.EntityFrameworkCore;
using ElectionManagement.Entity;
using System.Security;

namespace ElectionManagement.DbContexts
{
    public class ElectionManagementDbContext : DbContext
    {
        public ElectionManagementDbContext(DbContextOptions<ElectionManagementDbContext> options) : base(options)
        {
        }
        
        public DbSet<Secim> Secimler { get; set; }
        public DbSet<SecimIl> SecimIller { get; set; }
        public DbSet<SecimIlce> SecimIlceler { get; set; }
        public DbSet<SecimMahalle> SecimMahalleler { get; set; }
        public DbSet<SecimGenelSonuc> SecimGenelSonuclar { get; set; }
        public DbSet<SecimSonuc> SecimSonuclar { get; set; }
        public DbSet<SecimSonucBaslik> SecimSonucBasliklar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Secim>().HasData(
                new Secim { Id = 1, SecimAdi = "28. Dönem Milletvekili Genel Seçimi", SecimTarihi = new DateOnly(2023, 5, 14), SecimId = 20230, SecimIDAsil = 60792, SecimTuru = 8 },
                new Secim { Id = 2, SecimAdi = "27. Dönem Milletvekili Genel Seçimi", SecimTarihi = new DateOnly(2018, 6, 24), SecimId = 16300, SecimIDAsil = 49002, SecimTuru = 8 },
                new Secim { Id = 3, SecimAdi = "26. Dönem Milletvekili Genel Seçimi", SecimTarihi = new DateOnly(2015, 11, 1), SecimId = 14868, SecimIDAsil = 44706, SecimTuru = 8 },
                new Secim { Id = 4, SecimAdi = "25. Dönem Milletvekili Genel Seçimi", SecimTarihi = new DateOnly(2015, 6, 7), SecimId = 13884, SecimIDAsil = 41754, SecimTuru = 8 }
            );
        }
    }
}