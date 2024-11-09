using System.Text.Json.Serialization;
using SocialSite.API;
using SocialSite.Application.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddContextWithIdentity(builder.Configuration, builder.Environment);
builder.Services.AddServices();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddControllers()
	.AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
	.AddEndpointValidation();

builder.Services.Configure<JwtSetup>(builder.Configuration.GetSection("JWT"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsBuilder => corsBuilder.WithOrigins(builder.Configuration.GetValue<string>("JWT:ValidAudience") ?? "")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

if (builder.Environment.IsDevelopment())
    builder.Services.AddSwagger();


var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

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
