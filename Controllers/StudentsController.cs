using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Data;
using QuanLySinhVien.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/students
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        return await _context.Students.Include(s => s.Class).ToListAsync();
    }

    // GET: api/classes/{classId}/students
    [HttpGet("/api/classes/{classId}/students")]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByClass(int classId)
    {
        var students = await _context.Students
            .Where(s => s.ClassId == classId)
            .ToListAsync();

        if (students == null)
        {
            return NotFound();
        }

        return students;
    }

    // POST: api/students
    [HttpPost]
    public async Task<ActionResult<Student>> PostStudent(Student student)
    {
        // Kiểm tra xem ClassId có tồn tại không
        var classExists = await _context.Classes.AnyAsync(c => c.Id == student.ClassId);
        if (!classExists)
        {
            return BadRequest("ClassId không hợp lệ.");
        }

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetStudents", new { id = student.Id }, student);
    }

    // PUT: api/students/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(int id, Student student)
    {
        if (id != student.Id)
        {
            return BadRequest();
        }

        var existingStudent = await _context.Students.FindAsync(id);
        if (existingStudent == null)
        {
            return NotFound();
        }

        // Cập nhật thông tin, không cho đổi lớp
        existingStudent.Name = student.Name;
        existingStudent.DateOfBirth = student.DateOfBirth;
        // existingStudent.ClassId = student.ClassId; // KHÔNG cập nhật ClassId

        _context.Entry(existingStudent).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Students.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
}