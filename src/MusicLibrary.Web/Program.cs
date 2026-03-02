using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using MusicLibrary.Application.Interfaces;
using MusicLibrary.Application.Services;
using MusicLibrary.Domain.Interfaces;
using MusicLibrary.Infrastructure.Data;
using MusicLibrary.Infrastructure.Repositories;
using MusicLibrary.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add Entity Framework
builder.Services.AddDbContext<MusicLibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IMusicFileRepository, MusicFileRepository>();
builder.Services.AddScoped<IScanDirectoryRepository, ScanDirectoryRepository>();

// Add services
builder.Services.AddScoped<IMusicLibraryService, MusicLibraryService>();
builder.Services.AddScoped<IMusicScannerService, MusicScannerService>();

var app = builder.Build();

// Auto-migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MusicLibraryDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
