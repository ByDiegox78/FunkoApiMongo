using FunkoApiMongo.Dto;
using FunkoApiV._2.Error;
using FunkoApiV._2.Service;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace FunkoApiMongo.Controller;

[ApiController]
[Route("api/[controller]")]
public class FunkoController(IFunkoService ser) : ControllerBase {
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FunkoResponseDto>>> GetAll() {
        var funkos = await ser.GetAll();
        return Ok(funkos.ToList());
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<FunkoResponseDto>> GetById(ObjectId id) {
        var resultado = await ser.GetById(id);
        return resultado.IsSuccess
            ? Ok(resultado.Value)
            : resultado.Error.TopHttpResult<FunkoResponseDto>();
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<FunkoResponseDto>> GetDeleted() {
        var get = await ser.GetDeleted();
        return Ok(get.ToList());
    }
    [HttpPost]
    public async Task<ActionResult<FunkoResponseDto>> Create([FromBody] FunkoCreateRequest funko) {
        var result = await ser.Add(funko);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.Error.TopHttpResult<FunkoResponseDto>();
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<FunkoResponseDto>> Update([FromBody] FunkoUpdateRequest funko, ObjectId id) {
        var act = await ser.Update(id, funko);
        return act.IsSuccess
            ? Ok(act.Value)
            : act.Error.TopHttpResult<FunkoResponseDto>();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(ObjectId id) {
        var result = await ser.SoftDelete(id);
        return result.IsSuccess
            ? NoContent()
            : result.Error.TopHttpResult<bool>();
    }
    [HttpPost("{id}/restore")]
    public async Task<ActionResult<bool>> Restore(ObjectId id) {
        var result = await ser.Restore(id);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error.TopHttpResult<bool>();
    }
}
