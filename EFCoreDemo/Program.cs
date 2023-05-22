using EFCoreDemo.Models;
using EFCoreDemo.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "Inventec EFCoreDemo API Server", 
        Version = "v1",
        Contact = new OpenApiContact()
        {
            Name = "Will",
            Email = "will@example.com",
            Url = new Uri("https://www.example.com")
        },
        Description = "Inventec EFCoreDemo API Server",
        License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        },
        TermsOfService = new Uri("https://www.example.com")
    });
});

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
