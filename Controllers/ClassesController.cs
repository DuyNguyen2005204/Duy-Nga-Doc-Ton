using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Data;
using QuanLySinhVien.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ClassesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClassesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/classes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
    {
        return await _context.Classes.ToListAsync();
    }

    // POST: api/classes
    [HttpPost]
    public async Task<ActionResult<Class>> PostClass(Class newClass)
    {
        _context.Classes.Add(newClass);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetClasses", new { id = newClass.Id }, newClass);
    }
}