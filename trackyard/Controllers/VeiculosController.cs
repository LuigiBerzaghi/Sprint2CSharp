
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint1CSharp.Application.Common;
using Sprint1CSharp.Data;
using Sprint1CSharp.Models;
using Sprint1CSharp.Application.Dtos;

namespace Sprint1CSharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly AppDbContext _db;

    public VeiculosController(AppDbContext db) => _db = db;

    /// <summary>Lista veículos com paginação</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<Veiculo>>> GetAll(
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

        return Ok(new PagedResult<Veiculo> {
            Items = data, Page = page, PageSize = pageSize, TotalItems = total
        });
    }

    /// <summary>Busca um veículo por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _db.Veiculos.AsNoTracking().Include(v => v.Cliente).FirstOrDefaultAsync(x => x.Id == id);
        return entity is null ? NotFound() : Ok(entity);
    }
    /// <summary>Cria um veículo.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Veiculo dto)
    {
        // garante vínculo coerente
        if (dto.Cliente is not null)
        {
            // se vier cliente aninhado, usamos apenas o ClienteId
            dto.Cliente = null;
        }

        _db.Veiculos.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
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
}
