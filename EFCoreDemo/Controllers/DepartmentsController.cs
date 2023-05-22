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
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public DepartmentsController(ContosoUniversityContext context)
        {
            _context = context;
        }

        [HttpGet(Name = nameof(GetDepartmentsAll))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartmentsAll()
        {
            if (_context.Department == null)
            {
                return NotFound();
            }
            return await _context.Department.ToListAsync();
        }

        [HttpGet("{id}", Name = nameof(GetDepartmentById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Department>> GetDepartmentById(int id)
        {
            if (_context.Department == null)
            {
                return NotFound();
            }
            var department = await _context.Department.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        [HttpGet("{id}/courses", Name = nameof(GetCoursesByDepartmentId))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetCoursesByDepartmentId(int id)
        {
            if (_context.Department == null)
            {
                return NotFound();
            }
            var department = await _context.Department.Include(p => p.Course)
                .FirstOrDefaultAsync(p => p.DepartmentId == id);

            if (department == null)
            {
                return NotFound();
            }

            var courses = department.Course.ToList();

            if (courses.Count == 0)
            {
                return NotFound();
            }

            return Mapper.Map<List<CourseResponseDto>>(courses);
        }



        private bool DepartmentExists(int id)
        {
            return (_context.Department?.Any(e => e.DepartmentId == id)).GetValueOrDefault();
        }
    }
}
