using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DACS.Models;

public partial class DbDacSanQueHuongContext : IdentityDbContext
{
    public DbDacSanQueHuongContext()
    {
    }

    public DbDacSanQueHuongContext(DbContextOptions<DbDacSanQueHuongContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CtdonHang> CtdonHangs { get; set; }

    public virtual DbSet<DacSan> DacSans { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }


    public virtual DbSet<LoaiDacSan> LoaiDacSans { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=ADMIN-PC\\CHIANH;Database=DB_DacSanQueHuong;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CtdonHang>(entity =>
        {
            entity.HasKey(e => e.MaCtdonHang).HasName("PK_CTDatHang");

            entity.ToTable("CTDonHang");

            entity.Property(e => e.MaCtdonHang)
                .ValueGeneratedNever()
                .HasColumnName("MaCTDonHang");
            entity.Property(e => e.MaDonHang).HasMaxLength(50);
            entity.Property(e => e.SldacSan).HasColumnName("SLDacSan");


            entity.HasOne(d => d.MaDacSanNavigation).WithMany(p => p.CtdonHangs)
                .HasForeignKey(d => d.MaDacSan)
                .HasConstraintName("FK_CTDonHang_DacSan");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.CtdonHangs)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDonHang_DonHang");
        });

        modelBuilder.Entity<DacSan>(entity =>
        {
            entity.HasKey(e => e.MaDacSan);

            entity.ToTable("DacSan");

            entity.Property(e => e.MaDacSan).ValueGeneratedNever();
            entity.Property(e => e.Anh1).HasMaxLength(50);
            entity.Property(e => e.Anh2).HasMaxLength(50);
            entity.Property(e => e.Anh3).HasMaxLength(50);
            entity.Property(e => e.QueHuong).HasMaxLength(50);
            entity.Property(e => e.TenDacSan).HasMaxLength(50);

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.DacSans)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DacSan_LoaiDacSan");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang);

            entity.ToTable("DonHang");

            entity.Property(e => e.MaDonHang).HasMaxLength(50);
            entity.Property(e => e.NgayGiao).HasColumnType("datetime");
            entity.Property(e => e.NgayNhanDon).HasColumnType("datetime");

        });


        modelBuilder.Entity<LoaiDacSan>(entity =>
        {
            entity.HasKey(e => e.MaLoai);

            entity.ToTable("LoaiDacSan");

            entity.Property(e => e.MaLoai).ValueGeneratedNever();
            entity.Property(e => e.AnhLoai).HasMaxLength(50);
            entity.Property(e => e.MoTaLoai).HasMaxLength(50);
            entity.Property(e => e.TenLoai).HasMaxLength(50);
        });
        base.OnModelCreating(modelBuilder);
        OnModelCreatingPartial(modelBuilder);

        modelBuilder.HasSequence("donhangseq");
        modelBuilder.HasSequence("ctdonhangseq");
        modelBuilder.HasSequence("dacsanseq");
        modelBuilder.HasSequence("loaiseq");
    }

    public int GetDonHangSequence()
    {
        SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
        {
            Direction = System.Data.ParameterDirection.Output
        };

        Database.ExecuteSqlRaw(
            "SELECT @result = (NEXT VALUE FOR donhangseq)", result);


        return (int)result.Value;
    }

    public int GetCTDonHangSequence()
    {
        SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
        {
            Direction = System.Data.ParameterDirection.Output
        };

        Database.ExecuteSqlRaw(
            "SELECT @result = (NEXT VALUE FOR ctdonhangseq)", result);


        return (int)result.Value;
    }

    public int GetDacSanSequence()
    {
        SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
        {
            Direction = System.Data.ParameterDirection.Output
        };

        Database.ExecuteSqlRaw(
            "SELECT @result = (NEXT VALUE FOR dacsanseq)", result);


        return (int)result.Value;
    }

    public int GetLoaiSequence()
    {
        SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
        {
            Direction = System.Data.ParameterDirection.Output
        };

        Database.ExecuteSqlRaw(
            "SELECT @result = (NEXT VALUE FOR loaiseq)", result);


        return (int)result.Value;
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
