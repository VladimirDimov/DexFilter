using DexFilter.AGGrid.DependencyInjection;
using DexFilter.Core.Configuration;
using DexFilter.Examples.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddAgGridServerSide(o => o.SetLicenseDetails(new DexFilterLicenseDetails
{
    Company = "*******",
    Key = "*******",
}));
builder.Services.AddSingleton<StudentsData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

//app.UseSwagger();
//app.UseSwaggerUI();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.UseCors(c =>
{
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
    c.AllowAnyHeader();
});

app.Run();
