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

        //Secim il,ilce,mahalle entityleri
        public DbSet<SecimIl> SecimIller { get; set; }
        public DbSet<SecimIlce> SecimIlceler { get; set; }
        public DbSet<SecimMahalle> SecimMahalleler { get; set; }

        //Secim secmen entityleri
        public DbSet<SecimIlceSecmenCinsiyetDagilimi> SecimIlceSecmenCinsiyetDagilimlari { get; set; }
        public DbSet<SecimIlceSecmenVeSandikSayisi> SecimIlceSecmenVeSandikSayilari { get; set; }
        public DbSet<SecimYurtdisiSecmenYasveCinsiyetDagilimi> SecimYurtdisiSecmenYasveCinsiyetDagilimlari { get; set; }

        //Secim siyasi parti entityleri
        public DbSet<SecimSiyasiParti> SecimKatilanSiyasiPartiler {  get; set; }
        public DbSet<SecimIlceSandikSiyasiParti> SecimIlceSandikSiyasiPartiler { get; set; }

        //Secim aday entityleri
        public DbSet<SecimIlAdayYasDagilimi> SecimIlAdayYasDagilimlari { get; set; }
        public DbSet<SecimIlAdayOgrenimDurumuDagilimi> SecimIlAdayOgrenimDurumuDagilimlari { get; set; }
        public DbSet<SecimMilletVekiliSayisi> SecimMilletVekiliSayilari { get; set; }
        public DbSet<SecimAday> SecimAdaylar { get; set; }
        public DbSet<SecimIlAdayCinsiyetDagilimi> SecimIlAdayCinsiyetDagilimlari { get; set; }

        //Secim gorevli entityleri
        public DbSet<SecimIlceSiyasiPartiSandikGorevlisiSayisi> SecimIlceSiyasiPartiSandikGorevlisiSayilari { get; set; }

        //Secim diğer sonuc entityleri --
        public DbSet<SecimGumrukSonuc> SecimGumrukSonuclari { get; set; }
        public DbSet<SecimTemsilcilikSonuc> SecimTemsilcilikSonuclari { get; set; }
        public DbSet<SecimTemsilcilikListesi> SecimTemsilcilikListeleri { get; set; }
        public DbSet<SecimGumrukListesi> SecimGumrukListeleri { get; set; }

        //Secim sonuc entityleri
        public DbSet<SecimGenelSonuc> SecimGenelSonuclar { get; set; }
        public DbSet<SecimSonuc> SecimSonuclar { get; set; }
        public DbSet<SecimSonucBaslik> SecimSonucBasliklar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Secim>().HasData(
                new Secim { Id = 1, SecimAdi = "28. Dönem Milletvekili Genel Seçimi", SecimTarihi = new DateOnly(2023, 5, 14), SecimId = 20230, SecimIDAsil = 60792, SecimTuru = 8 },
                new Secim { Id = 2, SecimAdi = "27. Dönem Milletvekili Genel Seçimi", SecimTarihi = new DateOnly(2018, 6, 24), SecimId = 16300, SecimIDAsil = 49002, SecimTuru = 8 },
                new Secim { Id = 3, SecimAdi = "26. Dönem Milletvekili Genel Seçimi", SecimTarihi = new DateOnly(2015, 11, 1), SecimId = 14868, SecimIDAsil = 44706, SecimTuru = 8 },
                new Secim { Id = 4, SecimAdi = "25. Dönem Milletvekili Genel Seçimi", SecimTarihi = new DateOnly(2015, 6, 7), SecimId = 13884, SecimIDAsil = 41754, SecimTuru = 8 }
            );
        
            modelBuilder.Entity<SecimGumrukSonuc>(entity =>
            {
                entity.ToTable("SecimGumrukSonuclari");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                entity.HasOne(x => x.Secim)
                    .WithMany()
                    .HasForeignKey(x => x.SecimId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(x => x.gumruk_SANDIK_TARIHI)
                    .HasColumnType("timestamp without time zone");

                entity.Property(x => x.son_ISLEM_TARIHI)
                    .HasColumnType("timestamp with time zone");
            });
        }
    }
}