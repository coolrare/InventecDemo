﻿using System;
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
        [HttpGet(Name = nameof(GetCourseAll))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
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
        [HttpGet("{id}", Name = nameof(GetCourseById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
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
        [HttpPut("{id}", Name = "UpdateCourse")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
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

            //courseToUpdate.ModifiedOn = DateTime.Now;

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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        [HttpPost(Name = "CreateCourse")]
        public async Task<ActionResult<CourseCreateResponseDto>> PostCourse(CourseCreateDto course)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ContosoUniversityContext.Course'  is null.");
            }

            var courseToAdd = Mapper.Map<Course>(course);

            _context.Course.Add(courseToAdd);

            await _context.SaveChangesAsync();

            var courseDto = Mapper.Map<CourseCreateResponseDto>(courseToAdd);

            return CreatedAtAction("GetCourseById", new { id = courseDto.CourseId }, courseDto);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}", Name = "DeleteCourse")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
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
