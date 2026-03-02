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
builder.Services.AddScoped<MusicLibrary.Web.Services.PlayerState>();

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

// Music file streaming endpoint
app.MapGet("/api/music/stream/{id:int}", async (int id, MusicLibraryDbContext db) =>
{
    var musicFile = await db.MusicFiles.FindAsync(id);
    if (musicFile == null)
        return Results.NotFound();
    
    if (!System.IO.File.Exists(musicFile.FilePath))
        return Results.NotFound("File not found on disk");

    var contentType = musicFile.FileExtension?.ToLower() switch
    {
        ".mp3" => "audio/mpeg",
        ".flac" => "audio/flac",
        ".wav" => "audio/wav",
        ".m4a" => "audio/mp4",
        ".aac" => "audio/aac",
        ".ogg" => "audio/ogg",
        ".wma" => "audio/x-ms-wma",
        _ => "application/octet-stream"
    };

    var stream = System.IO.File.OpenRead(musicFile.FilePath);
    return Results.File(stream, contentType, enableRangeProcessing: true);
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
