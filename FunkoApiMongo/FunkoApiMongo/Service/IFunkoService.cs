using System.Collections.Generic;
using CSharpFunctionalExtensions;
using FunkoApiMongo.Dto;
using FunkoApiV._2.Error;
using MongoDB.Bson;

namespace FunkoApiV._2.Service;

public interface IFunkoService {
    Task<IReadOnlyList<FunkoResponseDto>> GetAll();
    Task<IReadOnlyCollection<FunkoResponseDto>> GetDeleted();
    Task<Result<FunkoResponseDto, DomainError>> GetById(ObjectId id);
    Task<Result<FunkoResponseDto, DomainError>> Add(FunkoCreateRequest funko);
    Task<Result<FunkoResponseDto, DomainError>> Update(ObjectId id, FunkoUpdateRequest funko);
    Task<Result<bool, DomainError>> SoftDelete(ObjectId id);
    Task<Result<bool, DomainError>> Restore(ObjectId id);
}