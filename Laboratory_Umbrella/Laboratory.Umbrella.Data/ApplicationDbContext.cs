using Laboratory.Umbrella.Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laboratory.Umbrella.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    #region DbSets
    public DbSet<Cliente> Clientes { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCliente(modelBuilder);

    }

    private void ConfigureCliente(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Clientes");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsRequired();

            entity.Property(e => e.fullName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Email)
                .HasMaxLength(255);

            entity.Property(e => e.Phone)
                .HasMaxLength(20);

            entity.Property(e => e.gender)
                .HasMaxLength(10);

            entity.Property(e => e.Address)
                .HasMaxLength(500);

            entity.Property(e => e.Status)
                .IsRequired();

            entity.Property(e => e.bornDate)
                .IsRequired();

            entity.Property(e => e.UserCreated)
                .HasMaxLength(100);

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            entity.Property(e => e.UserUpdated)
                .HasMaxLength(100);

            entity.Property(e => e.UserAnulled)
                .HasMaxLength(100);

            entity.HasIndex(e => e.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL");

            entity.HasIndex(e => e.Phone);

            entity.HasIndex(e => e.Status);
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(500);
    }
}