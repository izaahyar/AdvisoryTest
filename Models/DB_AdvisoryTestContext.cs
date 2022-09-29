using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AdvisoryTest.Models
{
    public partial class DB_AdvisoryTestContext : IdentityDbContext
    {
        public DB_AdvisoryTestContext()
        {
        }

        public DB_AdvisoryTestContext(DbContextOptions<DB_AdvisoryTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblMBuku> TblMBuku { get; set; }
        public virtual DbSet<TblTPeminjamanDetail> TblTPeminjamanDetail { get; set; }
        public virtual DbSet<TblTPeminjamanHeader> TblTPeminjamanHeader { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-OLC72DS;Database=DB_AdvisoryTest;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TblMBuku>(entity =>
            {
                entity.ToTable("TblM_Buku");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DelDate).HasColumnType("datetime");

                entity.Property(e => e.HargaPerHari).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.Judul)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Penerbit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Pengarang)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblTPeminjamanDetail>(entity =>
            {
                entity.ToTable("TblT_PeminjamanDetail");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BukuId).HasColumnName("BukuID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DelDate).HasColumnType("datetime");

                entity.Property(e => e.PeminjamanHeaderId).HasColumnName("PeminjamanHeaderID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblTPeminjamanHeader>(entity =>
            {
                entity.ToTable("TblT_PeminjamanHeader");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DelDate).HasColumnType("datetime");

                entity.Property(e => e.Peminjam)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TanggalPengembalian).HasColumnType("datetime");

                entity.Property(e => e.TanggalPinjam).HasColumnType("datetime");

                entity.Property(e => e.TotalBiaya).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });
        }
    }
}
