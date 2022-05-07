using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OAuth2_Identity.Recourses.Filters;
using OAuth2_Identity.Recourses.Middlewares;
using OAuth2_Identity.Resources.Core.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EFDBContext>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo {Title = "OAuth2_Resources", Version = "v1"});
    options.OperationFilter<AddAuthHeaderOperationFilter>();
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme.\"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<TokenCheckMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();