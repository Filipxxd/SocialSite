using SocialSite.API.Extensions;
using SocialSite.Core.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddContextWithIdentity(builder.Configuration, builder.Environment);
builder.Services.AddCoreServices();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.Configure<JwtSetup>(builder.Configuration.GetSection("JWT"));
builder.Services.AddApplicationServices();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwagger();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
