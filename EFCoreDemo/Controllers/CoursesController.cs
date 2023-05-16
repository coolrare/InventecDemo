﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreDemo.Models;
using EFCoreDemo.Models.Dto;

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

            return await _context.Course.Include(p => p.Department)
                .Select(course => new CourseResponseDto()
                {
                    CourseId = course.CourseId,
                    Title = course.Title,
                    Credits = course.Credits,
                    DepartmentId = course.DepartmentId,
                    DepartmentName = course.Department.Name
                }).ToListAsync();
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

            return new CourseResponseDto()
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentId = course.DepartmentId,
                DepartmentName = course.Department.Name
            };
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

            courseToUpdate.Title = course.Title;
            courseToUpdate.Credits = course.Credits;

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
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ContosoUniversityContext.Course'  is null.");
            }
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
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
