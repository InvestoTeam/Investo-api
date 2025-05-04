using Microsoft.EntityFrameworkCore;
using Investo.DataAccess.Entities;

namespace Investo.DataAccess.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("character varying");
                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("character varying");
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnType("text");
                entity.Property(e => e.AvatarUrl)
                    .HasColumnType("text");
                entity.Property(e => e.WalletAddress)
                    .HasColumnType("text");
                entity.Property(e => e.KycVerified)
                    .HasDefaultValue(false);
                entity.Property(e => e.TwoFactorEnabled)
                    .HasDefaultValue(false);
                entity.Property(e => e.UserType)
                    .IsRequired();
                entity.Property(e => e.JoinDate)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.LastLoginAt)
                    .HasColumnType("timestamp with time zone");
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
} 