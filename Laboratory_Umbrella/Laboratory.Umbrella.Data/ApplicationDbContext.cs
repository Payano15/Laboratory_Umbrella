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
    public DbSet<Usuarios> Usuarios { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Opciones> Opciones { get; set; }
    public DbSet<Secciones> Secciones { get; set; }
    public DbSet<ProfileOptionPermission> ProfileOptionPermissions { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCliente(modelBuilder);
        ConfigureUsuarios(modelBuilder);
        ConfigureUserProfiles(modelBuilder);
        ConfigureProfiles(modelBuilder);
        ConfigureOpciones(modelBuilder);
        ConfigureSecciones(modelBuilder);
        ConfigureProfileOptionPermissions(modelBuilder);

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

    private void ConfigureUsuarios(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(36).IsRequired();
            entity.Property(e => e.fullName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.UserName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PasswordHash).HasMaxLength(300).IsRequired();
            entity.Property(e => e.hashSalt).HasMaxLength(200);
        });
    }

    private void ConfigureUserProfiles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("UserProfiles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(36).IsRequired();
            entity.Property(e => e.UserId).HasMaxLength(36).IsRequired();
            entity.Property(e => e.ProfileId).HasMaxLength(36).IsRequired();
        });
    }

    private void ConfigureProfiles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.ToTable("Profiles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(36).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(200).IsRequired();
        });
    }

    private void ConfigureOpciones(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Opciones>(entity =>
        {
            entity.ToTable("Opciones");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(36).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Type).HasMaxLength(50).IsRequired();
            entity.Property(e => e.SectionId).HasMaxLength(36).IsRequired();
        });
    }

    private void ConfigureSecciones(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Secciones>(entity =>
        {
            entity.ToTable("Secciones");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(36).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(200).IsRequired();
        });
    }

    private void ConfigureProfileOptionPermissions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProfileOptionPermission>(entity =>
        {
            entity.ToTable("ProfileOptionPermissions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(36).IsRequired();
            entity.Property(e => e.ProfileId).HasMaxLength(36).IsRequired();
            entity.Property(e => e.OptionId).HasMaxLength(36).IsRequired();
            entity.Ignore(e => e.Permission);
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