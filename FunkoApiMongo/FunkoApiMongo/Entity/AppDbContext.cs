using FunkoApiMongo.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace FunkoApiV._2.Entity;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<Funko> Funkos => Set<Funko>();
    public DbSet<Proveedor> Proveedor =>  Set<Proveedor>();


    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Funko>().ToCollection("funkos");
        modelBuilder.Entity<Proveedor>().ToCollection("proveedors");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }
}