using EcoMercaditoAPI.Models.EcoMercadito;
using Microsoft.EntityFrameworkCore;
using EcoMercaditoAPI.Models.Usuarios;

namespace EcoMercaditoAPI.Concretes.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    // ==================== DbSets - Catálogo ====================

    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Municipio> Municipios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Categoria> Categorias { get; set; }

    // ==================== DbSets - Principales ====================

    public DbSet<EcoMercaditoAPI.Models.Usuarios.Usuario> Usuarios { get; set; }
    public DbSet<Credencial> Credenciales { get; set; }
    public DbSet<Negocio> Negocios { get; set; }
    public DbSet<OfertaExcedente> OfertasExcedentes { get; set; }

    // ==================== DbSets - View ====================

    public DbSet<UsuarioCompleto> UsuariosCompletos { get; set; }
    public DbSet<OfertaCompleta> OfertasCompletas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.ToTable("Departamento");
            entity.HasKey(e => e.DepartamentoId);

            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(e => e.Nombre)
                .IsUnique();

            entity.HasMany(e => e.Municipios)
                .WithOne(m => m.Departamento)
                .HasForeignKey(m => m.DepartamentoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Municipio>(entity =>
        {
            entity.ToTable("Municipio");
            entity.HasKey(e => e.MunicipioId);

            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(e => new { e.DepartamentoId, e.Nombre })
                .IsUnique();

            entity.HasOne(e => e.Departamento)
                .WithMany(d => d.Municipios)
                .HasForeignKey(e => e.DepartamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Usuarios)
                .WithOne(u => u.Municipio)
                .HasForeignKey(u => u.MunicipioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Negocios)
                .WithOne(n => n.Municipio)
                .HasForeignKey(n => n.MunicipioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol");
            entity.HasKey(e => e.RolId);

            entity.Property(e => e.NombreRol)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.NombreRol)
                .IsUnique();

            entity.HasMany(e => e.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categoria");
            entity.HasKey(e => e.CategoriaId);

            entity.Property(e => e.NombreCategoria)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.IconoClase)
                .HasMaxLength(100);

            entity.HasIndex(e => e.NombreCategoria)
                .IsUnique();

            entity.HasMany(e => e.OfertasExcedentes)
                .WithOne(o => o.Categoria)
                .HasForeignKey(o => o.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EcoMercaditoAPI.Models.Usuarios.Usuario>(entity =>
        {
            entity.ToTable("Usuario");
            entity.HasKey(e => e.UsuarioId);

            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Telefono)
                .HasMaxLength(20);

            entity.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Activo");

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.HasIndex(e => new { e.RolId, e.Estado });

            entity.HasOne(e => e.Municipio)
                .WithMany(m => m.Usuarios)
                .HasForeignKey(e => e.MunicipioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(e => e.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Credencial)
                .WithOne(c => c.Usuario)
                .HasForeignKey<Credencial>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Negocio)
                .WithOne(n => n.Usuario)
                .HasForeignKey<Negocio>(n => n.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Check constraint para Estado
            entity.HasCheckConstraint("CK_Usuario_Estado", "[Estado] IN (N'Activo', N'Inactivo', N'Suspendido')");
        });

        modelBuilder.Entity<Credencial>(entity =>
        {
            entity.ToTable("Credencial");
            entity.HasKey(e => e.CredencialId);

            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.ProveedorAuth)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Local");

            entity.Property(e => e.ProveedorUserId)
                .HasMaxLength(255);

            entity.HasIndex(e => e.UsuarioId)
                .IsUnique();

            entity.HasOne(e => e.Usuario)
                .WithOne(u => u.Credencial)
                .HasForeignKey<Credencial>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Check constraint para ProveedorAuth
            entity.HasCheckConstraint("CHK_Credencial_ProveedorAuth", "[ProveedorAuth] IN (N'Local', N'Google', N'Apple')");
        });

        modelBuilder.Entity<Negocio>(entity =>
        {
            entity.ToTable("Negocio");
            entity.HasKey(e => e.NegocioId);

            entity.Property(e => e.NombreComercial)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Descripcion)
                .HasMaxLength(1000);

            entity.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Activo");

            entity.HasIndex(e => e.UsuarioId)
                .IsUnique();

            entity.HasIndex(e => new { e.MunicipioId, e.Estado });

            entity.HasOne(e => e.Usuario)
                .WithOne(u => u.Negocio)
                .HasForeignKey<Negocio>(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Municipio)
                .WithMany(m => m.Negocios)
                .HasForeignKey(e => e.MunicipioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.OfertasExcedentes)
                .WithOne(o => o.Negocio)
                .HasForeignKey(o => o.NegocioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Check constraint para Estado
            entity.HasCheckConstraint("CHK_Negocio_Estado", "[Estado] IN (N'Activo', N'Inactivo')");
        });

        modelBuilder.Entity<OfertaExcedente>(entity =>
        {
            entity.ToTable("OfertaExcedente");
            entity.HasKey(e => e.OfertaId);

            entity.Property(e => e.Titulo)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.PrecioOriginal)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            entity.Property(e => e.PrecioDescuento)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            entity.HasIndex(e => new { e.Disponible, e.CategoriaId, e.FechaLimite });
            entity.HasIndex(e => e.NegocioId);

            entity.HasOne(e => e.Negocio)
                .WithMany(n => n.OfertasExcedentes)
                .HasForeignKey(e => e.NegocioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Categoria)
                .WithMany(c => c.OfertasExcedentes)
                .HasForeignKey(e => e.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Check constraints
            entity.HasCheckConstraint("CHK_OfertaExcedente_Precios", "[PrecioDescuento] <= [PrecioOriginal]");
            entity.HasCheckConstraint("CHK_OfertaExcedente_Cantidad", "[CantidadDisponible] >= 0");
        });

        modelBuilder.Entity<UsuarioCompleto>(entity =>
        {
            entity.ToView("vw_Usuario_Completo");
            entity.HasNoKey();
            entity.HasNoDiscriminator();

            entity.Ignore(e => e.UsuarioId);
        });

        modelBuilder.Entity<OfertaCompleta>(entity =>
        {
            entity.ToView("vw_Oferta_Completa");
            entity.HasNoKey();
            entity.HasNoDiscriminator();

            entity.Ignore(e => e.OfertaId);
        });
    }
}
