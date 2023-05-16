using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreDemo.Models;
using EFCoreDemo.Models.Dto;
using Omu.ValueInjecter;

namespace EFCoreDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public CoursesController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetCourseAll()
        {
            if (_context.Course == null)
            {
                return NotFound();
            }

            var courses = await _context.Course.Include(p => p.Department)
                .OrderBy(p => p.CourseId).ToListAsync();

            return courses.Select(course => Mapper.Map<CourseResponseDto>(course, null)).ToList();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseResponseDto>> GetCourseById(int id)
        {
            if (_context.Course == null)
            {
                return NotFound();
            }
            
            var course = await _context.Course.Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            var courseDto = Mapper.Map<CourseResponseDto>(course);
            //courseDto.DepartmentName = course.Department.Name;

            return courseDto;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseUpdateDto course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            var courseToUpdate = await _context.Course.FindAsync(id);

            if (courseToUpdate == null)
            {
                return NotFound();
            }

            //courseToUpdate.Title = course.Title;
            //courseToUpdate.Credits = course.Credits;
            courseToUpdate.InjectFrom(course);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(CourseCreateDto course)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ContosoUniversityContext.Course'  is null.");
            }

            var courseToAdd = Mapper.Map<Course>(course);

            _context.Course.Add(courseToAdd);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourseById", new { id = courseToAdd.CourseId }, courseToAdd);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (_context.Course == null)
            {
                return NotFound();
            }
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return (_context.Course?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}
