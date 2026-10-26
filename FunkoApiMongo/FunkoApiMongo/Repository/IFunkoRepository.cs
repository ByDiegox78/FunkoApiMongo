using CSharpFunctionalExtensions;
using FunkoApiMongo.Models;
using FunkoApiV._2.Error;
using MongoDB.Bson;

namespace FunkoApiMongo.Repository;

public interface IFunkoRepository {
    Task<IReadOnlyList<Funko>> GetAll();

    Task<Result<Funko, DomainError>> GetById(ObjectId id);

    Task<IReadOnlyList<Funko>> GetAllDeleted();
    
    Task<Result<Funko, DomainError>> Add(Funko funko);

    Task<Result<Funko, DomainError>> Update(ObjectId id, Funko funko);
    
    Task<Result<bool, DomainError>> SoftDelete(ObjectId id);
    
    Task<Result<bool, DomainError>> Restore(ObjectId id);
}