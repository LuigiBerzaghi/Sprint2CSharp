
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Sprint1CSharp.Application.Common;
using Sprint1CSharp.Application.Dtos;
using Sprint1CSharp.Data;
using Sprint1CSharp.Models;

namespace Sprint1CSharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatiosController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly LinkGenerator _links;
    public PatiosController(AppDbContext db, LinkGenerator links)
    {
        _db = db; _links = links;
    }
    /// <summary>Lista pátios com paginação.</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<PatioDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var query = _db.Patios.AsNoTracking().OrderBy(p => p.Id);
        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var dtos = items.Select(p => ToDto(p));

        return Ok(new PagedResult<PatioDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalItems = total
        });
    }
    /// <summary>Lista pátios por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PatioDto>> GetById(int id)
    {
        var p = await _db.Patios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) return NotFound();
        return Ok(ToDto(p));
    }

    /// <summary>Lista pátios com paginação.</summary>
    [HttpPost]
    public async Task<ActionResult<PatioDto>> Create([FromBody] CreatePatioDto dto)
    {
        var entity = new Patio { Nome = dto.Nome, Endereco = dto.Endereco };
        _db.Patios.Add(entity);
        await _db.SaveChangesAsync();
        var result = ToDto(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
    }

    /// <summary>Atualiza um pátio.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePatioDto dto)
    {
        var entity = await _db.Patios.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Nome = dto.Nome;
        entity.Endereco = dto.Endereco;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Exclui um pátio.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Patios.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Patios.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private PatioDto ToDto(Patio p)
    {
        var self = _links.GetPathByAction(HttpContext, nameof(GetById), values: new { id = p.Id }) ?? $"/api/patios/{p.Id}";
        var links = new[]
        {
            new LinkDto("self", self, "GET"),
            new LinkDto("update", self, "PUT"),
            new LinkDto("delete", self, "DELETE")
        };
        return new PatioDto(p.Id, p.Nome ?? string.Empty, p.Endereco, links);
    }
}
