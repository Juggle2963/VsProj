using Microsoft.EntityFrameworkCore;
using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Infrastructure.Data;

public class MusicLibraryDbContext : DbContext
{
    public MusicLibraryDbContext(DbContextOptions<MusicLibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<MusicFile> MusicFiles { get; set; }
    public DbSet<ScanDirectory> ScanDirectories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MusicFile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).HasMaxLength(500).IsRequired();
            entity.Property(e => e.FilePath).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.Artist).HasMaxLength(500);
            entity.Property(e => e.Album).HasMaxLength(500);
            entity.Property(e => e.Genre).HasMaxLength(100);
            entity.Property(e => e.FileExtension).HasMaxLength(10);
            entity.HasIndex(e => e.FilePath).IsUnique();
            
            entity.HasOne(e => e.ScanDirectory)
                  .WithMany(s => s.MusicFiles)
                  .HasForeignKey(e => e.ScanDirectoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ScanDirectory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Path).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.HasIndex(e => e.Path).IsUnique();
        });
    }
}