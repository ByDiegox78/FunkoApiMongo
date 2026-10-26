using CSharpFunctionalExtensions;
using FunkoApiMongo.Models;
using FunkoApiV._2.Error;
using FunkoApiV._2.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FunkoApiMongo.Repository;

public class FunkoNativeRepository(IMongoCollection<Funko> collection) : IFunkoRepository, IScopedService{
    public async Task<IReadOnlyList<Funko>> GetAll() {
        return await collection.Find(p => !p.IsDeleted).ToListAsync();
    }

    public async Task<Result<Funko, DomainError>> GetById(ObjectId id) {
        var funko = await collection.Find(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
        return funko is not null
            ? Result.Success<Funko, DomainError>(funko)
            : Result.Failure<Funko, DomainError>(NotFound.FromId(id, "Funko"));
    }

    public async Task<IReadOnlyList<Funko>> GetAllDeleted() {
        return await collection
            .Find(f => f.IsDeleted)
            .ToListAsync();    
    }

    public async Task<Result<Funko, DomainError>> Add(Funko funko) {
        var nuevo = funko with {
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
        await collection.InsertOneAsync(nuevo);
        return Result.Success<Funko, DomainError>(nuevo);
    }

    public async Task<Result<Funko, DomainError>> Update(ObjectId id, Funko funko) {
        var existente = await collection.Find(f => f.Id == id).FirstOrDefaultAsync();
        if (existente is null || existente.IsDeleted)
            return Result.Failure<Funko, DomainError>(NotFound.FromId(id, "Funko"));
        existente.Name = funko.Name;
        existente.ProveedorId = funko.ProveedorId;
        existente.Precio = funko.Precio;
        existente.Categoria = funko.Categoria;
        existente.Stock = funko.Stock;
        existente.UpdatedAt = DateTime.UtcNow;
        await collection.ReplaceOneAsync(f => f.Id == id, existente);
        return Result.Success<Funko, DomainError>(existente);
    }

    public async Task<Result<bool, DomainError>> SoftDelete(ObjectId id) {
        var funko = await collection.Find(f => f.Id == id).FirstOrDefaultAsync();
        if (funko is null) 
            return Result.Failure<bool, DomainError>(NotFound.FromId(id, "Funko"));
        funko.IsDeleted = true;
        funko.UpdatedAt = DateTime.UtcNow;
        await collection.ReplaceOneAsync(f => f.Id == id, funko);
        return Result.Success<bool, DomainError>(true);
    }

    public async Task<Result<bool, DomainError>> Restore(ObjectId id) {
        var funko = await collection.Find(f => f.Id  == id && f.IsDeleted).FirstOrDefaultAsync();
        if (funko is null) 
            return Result.Failure<bool, DomainError>(NotFound.FromId(id, "Funko"));
        funko.IsDeleted = false;
        funko.UpdatedAt = DateTime.UtcNow;
        await collection.ReplaceOneAsync(f => f.Id == id, funko);
        return Result.Success<bool, DomainError>(true);
    }
}