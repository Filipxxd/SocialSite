using System.Text.Json.Serialization;
using SocialSite.API;
using SocialSite.Application.Utilities;
using SocialSite.Data.EF;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddContextWithIdentity(builder.Configuration, builder.Environment);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddControllers()
	.AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
	.AddEndpointValidation();
builder.Services.AddSignalR();

builder.Services.Configure<JwtSetup>(builder.Configuration.GetSection("JWT"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsBuilder => corsBuilder.WithOrigins(builder.Configuration.GetValue<string>("JWT:ValidAudience") ?? "")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

builder.Services.AddSwagger();


var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    var shouldSeed = builder.Configuration.GetValue<bool>("Seeding");
    if (shouldSeed)
    {
	    using var scope = app.Services.CreateScope();
	    await using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
	    
	    var seeder = new TestDataSeeder(context);

	    await seeder.SeedAsync();
    }
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHubs();

app.Run();
