using Microsoft.EntityFrameworkCore;
using MusicLibrary.Domain.Entities;
using MusicLibrary.Domain.Interfaces;
using MusicLibrary.Infrastructure.Data;

namespace MusicLibrary.Infrastructure.Repositories;

public class ScanDirectoryRepository : IScanDirectoryRepository
{
    private readonly MusicLibraryDbContext _context;

    public ScanDirectoryRepository(MusicLibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ScanDirectory>> GetAllAsync()
    {
        return await _context.ScanDirectories
            .Include(d => d.MusicFiles)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<ScanDirectory?> GetByIdAsync(int id)
    {
        return await _context.ScanDirectories
            .Include(d => d.MusicFiles)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<ScanDirectory> AddAsync(ScanDirectory directory)
    {
        _context.ScanDirectories.Add(directory);
        await _context.SaveChangesAsync();
        return directory;
    }

    public async Task UpdateAsync(ScanDirectory directory)
    {
        _context.ScanDirectories.Update(directory);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var directory = await _context.ScanDirectories.FindAsync(id);
        if (directory != null)
        {
            _context.ScanDirectories.Remove(directory);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(string path)
    {
        return await _context.ScanDirectories.AnyAsync(d => d.Path == path);
    }
}