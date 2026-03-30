using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PayBille.Api.Configuration;
using PayBille.Api.DTOs;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure;
using PayBille.Api.Interfaces;
using PayBille.Api.Services;
using PayBille.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

// ── MongoDB ────────────────────────────────────────────────────────────────
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettings.SectionName));

builder.Services.AddSingleton<MongoDbContext>();

// Register generic repository factory
builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

// Register specific repositories
builder.Services.AddScoped<PayBille.Api.Infrastructure.Repositories.PersonaRepository>();
builder.Services.AddScoped<PayBille.Api.Infrastructure.Repositories.EmpresaRepository>();

// ── Imagenes ───────────────────────────────────────────────────────────────
builder.Services.Configure<ImagenSettings>(
    builder.Configuration.GetSection(ImagenSettings.SectionName));

// ── JWT ────────────────────────────────────────────────────────────────────
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName));

var jwtSettings = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>()!;

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSettings.Issuer,
            ValidAudience            = jwtSettings.Audience,
            IssuerSigningKey         = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew                = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode  = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(
                    ApiRespDto<object>.Error(AppErrors.AuthSinToken()));
            }
        };
    });

builder.Services.AddAuthorization();

// ── Application services ───────────────────────────────────────────────────
builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<PayBille.Api.Interfaces.IPersonaService, PayBille.Api.Services.PersonaService>();
builder.Services.AddScoped<PayBille.Api.Interfaces.IEmpresaService, PayBille.Api.Services.EmpresaService>();
builder.Services.AddScoped<PayBille.Api.Interfaces.IImagenService, PayBille.Api.Services.ImagenService>();
builder.Services.AddScoped<PayBille.Api.Infrastructure.Services.MongoDbInitializerService>();

// ── MVC / Swagger ──────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── FluentValidation ──────────────────────────────────────────────────────
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<PersonaReqDtoValidator>();
builder.Services.Configure<ApiBehaviorOptions>(o => o.SuppressModelStateInvalidFilter = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PayBille API", Version = "v1" });

    // Allow Swagger UI to send the Bearer token
    var scheme = new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter your JWT access token here."
    };
    c.AddSecurityDefinition("Bearer", scheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("PayBilleFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ── Exception handling ─────────────────────────────────────────────────────
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// ── Exception handler must be first in the pipeline ───────────────────────
app.UseExceptionHandler();

// ── Initialize MongoDB ─────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<PayBille.Api.Infrastructure.Services.MongoDbInitializerService>();
    await initializer.InitializeAsync(CancellationToken.None);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("PayBilleFrontend");

// ── Archivos estáticos (imágenes subidas) ──────────────────────────────────
var imagenSettings = app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<ImagenSettings>>().Value;
var imagenBaseDir  = Path.IsPathRooted(imagenSettings.DirectorioBase)
    ? imagenSettings.DirectorioBase
    : Path.Combine(app.Environment.ContentRootPath, imagenSettings.DirectorioBase);
Directory.CreateDirectory(imagenBaseDir);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagenBaseDir),
    RequestPath  = "/imagenes",
    ContentTypeProvider = new FileExtensionContentTypeProvider(),
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
