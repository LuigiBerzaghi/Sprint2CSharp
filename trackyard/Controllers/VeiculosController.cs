
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Sprint1CSharp.Application.Common;
using Sprint1CSharp.Data;
using Sprint1CSharp.Models;
using Sprint1CSharp.Application.Dtos;

namespace Sprint1CSharp.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = "ApiKey")]
[Route("api/v{version:apiVersion}/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly LinkGenerator _links;

    public VeiculosController(AppDbContext db, LinkGenerator links)
    {
        _db = db; _links = links;
    }

    /// <summary>Lista veículos com paginação</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<VeiculoDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? placa = null)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var query = _db.Veiculos.AsNoTracking().Include(v => v.Cliente).AsQueryable();

        if (!string.IsNullOrWhiteSpace(placa))
        {
            var filtro = placa.ToUpperInvariant();
            query = query.Where(v => v.Placa != null && v.Placa.ToUpper()!.Contains(filtro));
        }

        var total = await query.CountAsync();
        var data  = await query.OrderBy(v => v.Id)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        Response.Headers["X-Total-Count"] = total.ToString();

        var dtos = data.Select(ToDto);

        return Ok(new PagedResult<VeiculoDto> {
            Items = dtos, Page = page, PageSize = pageSize, TotalItems = total
        });
    }

    /// <summary>Busca um veículo por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<VeiculoDto>> GetById(int id)
    {
        var entity = await _db.Veiculos.AsNoTracking().Include(v => v.Cliente).FirstOrDefaultAsync(x => x.Id == id);
        return entity is null ? NotFound() : Ok(ToDto(entity));
    }
    /// <summary>Cria um veículo.</summary>
    [HttpPost]
    public async Task<ActionResult<VeiculoDto>> Create([FromBody] CreateVeiculoDto dto)
    {
        var entity = new Veiculo
        {
            Modelo = dto.Modelo,
            Placa = dto.Placa,
            Cor = dto.Cor,
            Ano = dto.Ano,
            ClienteId = dto.ClienteId
        };

        _db.Veiculos.Add(entity);
        await _db.SaveChangesAsync();
        var result = ToDto(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
    }

    /// <summary>Atualiza um veículo.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVeiculoDto dto)
    {
        var entity = await _db.Veiculos.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Modelo = dto.Modelo;
        entity.Placa  = dto.Placa;
        entity.Cor    = dto.Cor;
        entity.Ano    = dto.Ano;
        if (dto.ClienteId.HasValue) entity.ClienteId = dto.ClienteId.Value;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Exclui um veículo.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Veiculos.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Veiculos.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private VeiculoDto ToDto(Veiculo v)
    {
        var self = _links.GetPathByAction(HttpContext, nameof(GetById), values: new { id = v.Id }) ?? $"/api/veiculos/{v.Id}";
        var links = new[]
        {
            new LinkDto("self", self, "GET"),
            new LinkDto("update", self, "PUT"),
            new LinkDto("delete", self, "DELETE")
        };
        return new VeiculoDto(v.Id, v.Modelo ?? string.Empty, v.Placa ?? string.Empty, v.Cor ?? string.Empty, v.Ano ?? string.Empty, v.ClienteId, links);
    }
}
