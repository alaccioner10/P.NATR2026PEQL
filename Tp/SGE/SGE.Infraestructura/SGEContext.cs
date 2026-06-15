using Microsoft.EntityFrameworkCore;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;

public class SGEContext : DbContext
{
    public DbSet<Expediente> Expedientes{get; set;}
    public DbSet<Tramite> Tramites{get;set;}
    public DbSet<Usuario> Usuarios{get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("data source=SGE.sqlite");
    }
}