
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
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly LinkGenerator _links;
    public ClientesController(AppDbContext db, LinkGenerator links)
    {
        _db = db; _links = links;
    }

    /// <summary>Lista clientes com paginação e filtro opcional por nome.</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<ClienteDto>>> GetAll(
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

        var dtos = items.Select(ToDto);

        return Ok(new PagedResult<ClienteDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalItems = total
        });
    }

    /// <summary>Busca cliente pelo ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        var entity = await _db.Clientes.AsNoTracking().Include(c => c.Veiculos).FirstOrDefaultAsync(c => c.Id == id);
        return entity is null ? NotFound() : Ok(ToDto(entity));
    }

    /// <summary>Cria um cliente.</summary>
    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Create([FromBody] CreateClienteDto dto)
    {
        var entity = new Cliente
        {
            Nome = dto.Nome,
            CPF = dto.CPF,
            Email = dto.Email,
            Endereco = dto.Endereco
        };
        _db.Clientes.Add(entity);
        await _db.SaveChangesAsync();
        var result = ToDto(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
    }

    /// <summary>Atualiza um cliente.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClienteDto dto)
    {
        var entity = await _db.Clientes.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Nome    = dto.Nome;
        entity.CPF     = dto.CPF;
        entity.Email   = dto.Email;
        entity.Endereco = dto.Endereco;

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

    private ClienteDto ToDto(Cliente c)
    {
        var self = _links.GetPathByAction(HttpContext, nameof(GetById), values: new { id = c.Id }) ?? $"/api/clientes/{c.Id}";
        var links = new[]
        {
            new LinkDto("self", self, "GET"),
            new LinkDto("update", self, "PUT"),
            new LinkDto("delete", self, "DELETE")
        };
        return new ClienteDto(c.Id, c.Nome ?? string.Empty, c.CPF ?? string.Empty, c.Email ?? string.Empty, c.Endereco, links);
    }
}
