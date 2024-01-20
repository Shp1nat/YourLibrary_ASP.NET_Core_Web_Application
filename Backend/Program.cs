using Backend.Api.Services;
using Backend.Configuration;
using DatabaseAccessLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ConnectionStringsOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<JwtAuthenticationOptions>(builder.Configuration.GetSection("JwtAuthentication"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
});


builder.Services.AddDbContext<LibraryWebDbContex>(x => x.UseSqlServer(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ConnectionStringsOptions>>().Value.LibraryWebConnectionString));
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<AuthorService>();
builder.Services.AddTransient<GenreService>();
builder.Services.AddTransient<TypeBkService>();
builder.Services.AddTransient<PublisherService>();
builder.Services.AddTransient<BookService>();
builder.Services.AddTransient<ExampleService>();
builder.Services.AddTransient<VacancyService>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<JwtService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOptions =>
{
    var config = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtAuthenticationOptions>>().Value;

    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config.Issuer,
        ValidAudience = config.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthentication();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();