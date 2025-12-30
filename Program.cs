using System.Text.Json;
using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Mapping;
using ExpenseTracker.Api.Middlewares;
using ExpenseTracker.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------- Configure Npgsql legacy timestamp behavior (Postgres) ----------
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.WebHost.UseUrls("http://0.0.0.0:" +
    (Environment.GetEnvironmentVariable("PORT") ?? "8080"));

// ----------  Add configuration + connection string ----------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// If environment variable is used, it will override the appsettings value automatically.
// Example env var key: ConnectionStrings__DefaultConnection

// ---------- Register DbContext (EF Core + Npgsql) ----------
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// ----------  Add controllers ----------
builder.Services.AddControllers();

// ---------- Swagger / OpenAPI (useful in Development) ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Expense Tracker API",
        Version = "v1",
        Description = "Learning ASP.NET Core Web API with Swagger"
    });
    // Add Bearer Auth
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Paste your JWT token here. No need for 'Bearer ' prefix."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// ---------- CORS (allow local frontend during development) ----------
builder.Services.AddCors(
    options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:4200", "http://192.168.1.7:4200");
    });
    options.AddPolicy("ProdCors", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithOrigins("https://pet-finance.vercel.app", "https://personal-expense-tracker-kappa-dusky.vercel.app");
    });
}
);

// ---------- Add scoped services / repositories / automapper etc. ----------
var clerk = builder.Configuration.GetSection("Clerk");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.MapInboundClaims = false;
    x.Authority = clerk["Issuer"];
    x.MetadataAddress = clerk["JWKSUrl"] ?? "";
    x.TokenValidationParameters = new TokenValidationParameters()
    {

        ValidateIssuer = true,
        ValidIssuer = clerk["Issuer"],

        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        // Basic expiration validation
        ValidateLifetime = true,
        // ClockSkew = TimeSpan.Zero, // no 5-minute grace period
        NameClaimType = "sub"
    };
    // 2. Helpful for debugging: print why auth failed in the console
    x.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
           {
               Console.WriteLine("JWT failed: " + context.Exception.Message);
               return Task.CompletedTask;
           },

        //         OnMessageReceived = context =>
        // {
        //     // Force API to ONLY use Authorization header and ignore Clerk cookies
        //     if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        //     {
        //         var header = authHeader.ToString();
        //         if (header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        //         {
        //             context.Token = header.Substring("Bearer ".Length).Trim();
        //             return Task.CompletedTask;
        //         }
        //     }

        //     // If there's no Authorization header, DO NOT authenticate cookie session
        //     context.NoResult();
        //     return Task.CompletedTask;
        // },

        // OnTokenValidated = context =>
        // {
        //     Console.WriteLine("==== TOKEN CLAIMS ====");
        //     foreach (var c in context.Principal.Claims)
        //         Console.WriteLine($"{c.Type}: {c.Value}");
        //     return Task.CompletedTask;
        // },
        OnTokenValidated = context =>
        {
            Console.WriteLine("JWT validated successfully!");
            return Task.CompletedTask;
        }
    };
});


// This handles API behavior Controllers Accepts Type and Model Validation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        // 1. Get the FIRST error message only
        var firstError = context.ModelState
            .SelectMany(x => x.Value!.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault();

        Console.WriteLine(firstError);

        // 2. Fallback message (for safety)
        var message = string.IsNullOrWhiteSpace(firstError)
            ? "Invalid request payload"
            : firstError;

        // 3. Normalize JSON parsing error
        if (message.Contains("JSON", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("trailing comma", StringComparison.OrdinalIgnoreCase))
        {
            message = "Invalid JSON payload";
        }

        var response = new ApiResponse<object>
        {
            Data = null,
            Error = new ApiError
            {
                Message = message,
                StatusCode = StatusCodes.Status400BadRequest
            }
        };

        return new BadRequestObjectResult(response);
    };
});



builder.Services.AddAuthorization();

// builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// ---------- Build the app ----------
var app = builder.Build();

// ---------- Apply middleware ----------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCors");
}

if (app.Environment.IsProduction())
{
    app.UseCors("ProdCors");
}

// app.UseHttpsRedirection();


app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// // ---------- Helpful health-check endpoint ----------
// app.MapGet("/health", () => Results.Ok(new { status = "ok", env = app.Environment.EnvironmentName })).RequireAuthorization();

app.Run();