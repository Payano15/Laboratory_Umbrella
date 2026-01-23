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
    // Agrega más DbSets según tus entidades
    // public DbSet<Producto> Productos { get; set; }
    // public DbSet<Orden> Ordenes { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar Cliente
        ConfigureCliente(modelBuilder);

        // Configura más entidades según necesites
        // ConfigureProducto(modelBuilder);
    }

    private void ConfigureCliente(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            // Nombre de la tabla
            entity.ToTable("Clientes");

            // Clave primaria
            entity.HasKey(e => e.Id);

            // Propiedades
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

            // Campos de auditoría
            entity.Property(e => e.UserCreated)
                .HasMaxLength(100);

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            entity.Property(e => e.UserUpdated)
                .HasMaxLength(100);

            entity.Property(e => e.UserAnulled)
                .HasMaxLength(100);

            // Índices
            entity.HasIndex(e => e.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL"); // Permite nulls pero únicos si existe

            entity.HasIndex(e => e.Phone);

            entity.HasIndex(e => e.Status);
        });
    }

    // Método para configurar conversiones globales
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        // Configuración global para strings (si quieres)
        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(500); // Tamaño por defecto para strings sin especificar
    }
}