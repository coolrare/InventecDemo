using EFCoreDemo.Models;
using EFCoreDemo.Models.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Omu.ValueInjecter;
using System.Reflection;
using System.Text;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("EFCoreDemo.Controllers", LogEventLevel.Verbose)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq(serverUrl: "http://localhost:5341", apiKey: "o1uLOtqp6Hu0R0bPjBje")
    .CreateLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.AddJsonConsole();

    Mapper.AddMap<Course, CourseResponseDto>(course =>
    {
        var courseDto = new CourseResponseDto();
        courseDto.InjectFrom(course);
        courseDto.DepartmentName = course.Department.Name;
        return courseDto;
    });

    Mapper.AddMap<List<Course>, List<CourseResponseDto>>(courses =>
    {
        return courses.Select(course => Mapper.Map<CourseResponseDto>(course, null)).ToList();
    });

    // Add services to the container.

    builder.Services.Configure<JwtSettings>(
        builder.Configuration.GetSection("JwtSettings"));

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            // �����ҥ��ѮɡA�^�����Y�|�]�t WWW-Authenticate ���Y�A�o�̷|��ܥ��Ѫ��Բӿ��~��]
            options.IncludeErrorDetails = true; // �w�]�Ȭ� true�A���ɷ|�S�O����

            options.TokenValidationParameters = new TokenValidationParameters
            {
                // �z�L�o���ŧi�A�N�i�H�q "sub" ���Ȩó]�w�� User.Identity.Name
                NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                // �z�L�o���ŧi�A�N�i�H�q "roles" ���ȡA�åi�� [Authorize] �P�_����
                RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                // �@��ڭ̳��|���� Issuer
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),

                // �q�`���ӻݭn���� Audience
                ValidateAudience = false,
                //ValidAudience = "JwtAuthDemo", // �����ҴN���ݭn��g

                // �@��ڭ̳��|���� Token �����Ĵ���
                ValidateLifetime = true,

                // �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
                ValidateIssuerSigningKey = false,

                // "1234567890123456" ���ӱq IConfiguration ���o
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")!))
            };
        });

    builder.Services.AddAuthorization();

    builder.Services.AddSingleton<JwtHelpers>();

    builder.Services.AddCors(builder =>
    {
        /*
             fetch('https://localhost:5001/api/courses')
                .then(response => response.json())
                .then(jsonData => {
                    console.log(jsonData);
                })
         */
        builder.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("https://blog.miniasp.com")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
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

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    builder.Services.AddDbContext<ContosoUniversityContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Host.UseSerilog();

    var app = builder.Build();

    if (app.Environment.IsProduction())
    {
        app.UseExceptionHandler("/error");
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();

    app.UseCors();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers().RequireAuthorization();

    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
