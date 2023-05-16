using EFCoreDemo.Models;
using EFCoreDemo.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;

var builder = WebApplication.CreateBuilder(args);


Mapper.AddMap<Course, CourseResponseDto>(course =>
{
    var courseDto = new CourseResponseDto();
    courseDto.InjectFrom(course);
    courseDto.DepartmentName = course.Department.Name;
    return courseDto;
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContosoUniversityContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
