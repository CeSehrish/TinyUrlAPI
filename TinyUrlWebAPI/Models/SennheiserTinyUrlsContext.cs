using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TinyUrlWebAPI.Models;

public partial class SennheiserTinyUrlsContext : DbContext
{
    public SennheiserTinyUrlsContext()
    {
    }

    public SennheiserTinyUrlsContext(DbContextOptions<SennheiserTinyUrlsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UrlLink> UrlLinks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SKTP;Database=SennheiserTinyURLs;Integrated Security=true;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrlLink>(entity =>
        {
            entity.ToTable("urlLinks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alias)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("alias");
            entity.Property(e => e.LongUrl).HasColumnName("longUrl");
            entity.Property(e => e.TinyUrl)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("tinyUrl");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.UrlLinks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_urlLinks_userId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
