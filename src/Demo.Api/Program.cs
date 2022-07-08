using Demo.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration);
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureAutoMapper();

var app = builder.Build();
app.UseServices();
app.UseAuthentication();