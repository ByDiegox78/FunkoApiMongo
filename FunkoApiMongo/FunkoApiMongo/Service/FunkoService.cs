using System.Collections.Generic;
using CSharpFunctionalExtensions;
using FluentValidation;
using FunkoApiMongo.Dto;
using FunkoApiMongo.Mapper;
using FunkoApiMongo.Repository;
using FunkoApiV._2.Error;
using FunkoApiV._2.Infrastructure.Interfaces;
using MongoDB.Bson;

namespace FunkoApiV._2.Service;

public class FunkoService(
    IFunkoRepository repo, 
    ILogger<FunkoService> log,
    IValidator<FunkoCreateRequest> createValidation,
    IValidator<FunkoUpdateRequest> updateValidation
    ) : IFunkoService, IScopedService {
    public async Task<IReadOnlyList<FunkoResponseDto>> GetAll() {
        log.LogInformation("Getting all funkos");
        var funkos = await repo.GetAll();
        return funkos.Select(t => t.ToDto()).ToList();
    }

    public async Task<IReadOnlyCollection<FunkoResponseDto>> GetDeleted() {
        log.LogInformation("Getting deleted funkos");
        var deleted = await repo.GetAllDeleted();
        return deleted.Select(t => t.ToDto()).ToList();
    }

    public async Task<Result<FunkoResponseDto, DomainError>> GetById(ObjectId id) {
        log.LogInformation("Getting funko by id: {@Id}", id);
        var funko = await repo.GetById(id);
        return funko.Map(p => p.ToDto());
    }

    public async Task<Result<FunkoResponseDto, DomainError>> Add(FunkoCreateRequest funko) {
        log.LogInformation("Adding funko: {@Funko}", funko);
        var validation = await createValidation.ValidateAsync(funko);
        if (!validation.IsValid) {
            log.LogWarning("Validación fallida al crear funko: {@Errors}", validation.Errors);
            return Result.Failure<FunkoResponseDto, DomainError>(
                new ValidationError(validation.Errors[0].ErrorMessage));
        }
        var creado = await repo.Add(funko.ToModel());
        return creado.Map(p => p.ToDto());
    }
    public async Task<Result<FunkoResponseDto, DomainError>> Update(ObjectId id, FunkoUpdateRequest funko) {
        log.LogInformation("Updating funko {Id} with: {@Funko}", id, funko);

        var validation = await updateValidation.ValidateAsync(funko);
        if (!validation.IsValid) {
            log.LogWarning("Validación fallida al actualizar funko {Id}: {@Errors}", id, validation.Errors);
            return Result.Failure<FunkoResponseDto, DomainError>(
                new ValidationError(validation.Errors[0].ErrorMessage));
        }
        log.LogInformation("Realizando actualizacion del funko con id: {@Id}", id);
        return (await repo.Update(id, funko.ToModel())).Map(p => p.ToDto());
    }

    public async Task<Result<bool, DomainError>> SoftDelete(ObjectId id) {
        log.LogInformation("Soft deleting funko: {@Id}", id);
        return await repo.SoftDelete(id);
    }

    public async Task<Result<bool, DomainError>> Restore(ObjectId id) {
        log.LogInformation("Restoring funko: {@Id}", id);
        return await repo.Restore(id);
    }

}