
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint1CSharp.Application.Common;
using Sprint1CSharp.Data;
using Sprint1CSharp.Models;

namespace Sprint1CSharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _db;
    public ClientesController(AppDbContext db) => _db = db;

    /// <summary>Lista clientes com paginação e filtro opcional por nome.</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<Cliente>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nome = null)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var query = _db.Clientes.AsNoTracking().Include(c => c.Veiculos).AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
        {
            var filtro = nome.ToUpperInvariant();
            query = query.Where(c => c.Nome != null && c.Nome.ToUpper()!.Contains(filtro));
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        Response.Headers["X-Total-Count"] = total.ToString();

        return Ok(new PagedResult<Cliente>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = total
        });
    }

    /// <summary>Busca cliente pelo ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cliente>> GetById(int id)
    {
        var entity = await _db.Clientes.AsNoTracking().Include(c => c.Veiculos).FirstOrDefaultAsync(c => c.Id == id);
        return entity is null ? NotFound() : Ok(entity);
    }

    /// <summary>Cria um cliente.</summary>
    [HttpPost]
    public async Task<ActionResult<Cliente>> Create([FromBody] Cliente dto)
    {
        if (dto.Veiculos is not null)
        {
            foreach (var v in dto.Veiculos) v.Cliente = null;
        }

        _db.Clientes.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    /// <summary>Atualiza um cliente.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Cliente dto)
    {
        if (id != dto.Id) return BadRequest("ID do caminho difere do corpo.");

        var exists = await _db.Clientes.AnyAsync(c => c.Id == id);
        if (!exists) return NotFound();

        var entry = _db.Entry(dto);
        entry.State = EntityState.Modified;
        entry.Property(c => c.Veiculos).IsModified = false;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove um cliente.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Clientes.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Clientes.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
