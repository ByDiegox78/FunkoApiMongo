using CSharpFunctionalExtensions;
using FunkoApiMongo.Models;
using FunkoApiV._2.Entity;
using FunkoApiV._2.Error;
using FunkoApiV._2.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FunkoApiMongo.Repository;

public class FunkoEFRepository(AppDbContext context) : IFunkoRepository, IScopedService {

    public async Task<IReadOnlyList<Funko>> GetAll() =>
        await context.Funkos.Where(p => !p.IsDeleted).ToListAsync();
    

    public async Task<Result<Funko, DomainError>> GetById(ObjectId id) {
        var funko = await context.Funkos.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        return funko is not null
            ? Result.Success<Funko, DomainError>(funko)
            : Result.Failure<Funko, DomainError>(NotFound.FromId(id, "Funko"));
    }
    public async Task<IReadOnlyList<Funko>> GetAllDeleted() {
        return await context.Funkos.IgnoreQueryFilters()
            .AsNoTracking()
            .Where(f => f.IsDeleted)
            .ToListAsync();
    }

    public async Task<Result<Funko, DomainError>> Add(Funko funko) {
        var nuevo = funko with {
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
        await context.Funkos.AddAsync(nuevo);
        await context.SaveChangesAsync();
        return Result.Success<Funko, DomainError>(nuevo);
    }

    public async Task<Result<Funko, DomainError>> Update(ObjectId id, Funko funko) {
        var existente = await context.Funkos
            .FirstOrDefaultAsync(f => f.Id == id);
        if (existente is null || existente.IsDeleted)
            return Result.Failure<Funko, DomainError>(NotFound.FromId(id, "Funko"));
        existente.Name = funko.Name;
        existente.ProveedorId = funko.ProveedorId;
        existente.Precio = funko.Precio;
        existente.Categoria = funko.Categoria;
        existente.Stock = funko.Stock;
        existente.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Result.Success<Funko, DomainError>(existente);
    }
    public async Task<Result<bool, DomainError>> SoftDelete(ObjectId id) {
        var funko = await context.Funkos.FindAsync(id);
        if (funko is null) 
            return Result.Failure<bool, DomainError>(NotFound.FromId(id, "Funko"));
        funko.IsDeleted = true;
        funko.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Result.Success<bool, DomainError>(true);
    }

    public async Task<Result<bool, DomainError>> Restore(ObjectId id) {
        var funko = await context.Funkos.IgnoreQueryFilters()
            .FirstOrDefaultAsync(f => f.Id  == id && f.IsDeleted);
        if (funko is null) 
            return Result.Failure<bool, DomainError>(NotFound.FromId(id, "Funko"));
        funko.IsDeleted = false;
        funko.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Result.Success<bool, DomainError>(true);
    }
}