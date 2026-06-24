using Microsoft.EntityFrameworkCore;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura;

public class SGEContext : DbContext
{
    public DbSet<Expediente> Expedientes{get; set;}
    public DbSet<Tramite> Tramites{get;set;}
    public DbSet<Usuario> Usuarios{get;set;}

    public SGEContext(DbContextOptions<SGEContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.Property(e => e.Caratula)
                .HasConversion(
                    caratula => caratula.Valor,
                    valor => new Caratula(valor))
                .HasColumnName("Caratula");
        });

        modelBuilder.Entity<Tramite>(entity =>
        {
            entity.Property(t => t.Contenido)
                .HasConversion(
                    contenido => contenido.Valor,
                    valor => new Contenido(valor))
                .HasColumnName("Contenido");
        });

        string hashAdmin = ContrasenaUtil.Convertir("admin123"); 
        string hashPrueba = ContrasenaUtil.Convertir("prueba123");

        var adminSemilla = Usuario.Reconstruir(
            id: Guid.Parse("11111111-1111-1111-1111-111111111111"), 
            nombre: "Admin SGE",
            email: "admin@sge.com", 
            contrasenaHash: hashAdmin, 
            esAdmin: true, 
            permisos: [] 
        );

        var usuarioPrueba1 = Usuario.Reconstruir(
            id: Guid.Parse("22222222-2222-2222-2222-222222222222"), 
            nombre: "Usuario Solo Lectura",
            email: "prueba1@sge.com",
            contrasenaHash: hashPrueba,
            esAdmin: false,
            permisos: [] 
        );

        var usuarioPrueba2 = Usuario.Reconstruir(
            id: Guid.Parse("33333333-3333-3333-3333-333333333333"), 
            nombre: "Usuario Con Permisos",
            email: "prueba2@sge.com",
            contrasenaHash: hashPrueba,
            esAdmin: false,
            permisos: [Permiso.ExpedienteAlta, Permiso.TramiteAlta] 
        );

        modelBuilder.Entity<Usuario>().HasData(adminSemilla, usuarioPrueba1, usuarioPrueba2);
    }
}