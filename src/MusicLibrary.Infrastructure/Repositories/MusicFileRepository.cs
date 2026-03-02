using Microsoft.EntityFrameworkCore;
using MusicLibrary.Domain.Entities;
using MusicLibrary.Domain.Interfaces;
using MusicLibrary.Infrastructure.Data;

namespace MusicLibrary.Infrastructure.Repositories;

public class MusicFileRepository : IMusicFileRepository
{
    private readonly MusicLibraryDbContext _context;

    public MusicFileRepository(MusicLibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MusicFile>> GetAllAsync()
    {
        return await _context.MusicFiles
            .Include(m => m.ScanDirectory)
            .OrderBy(m => m.Artist)
            .ThenBy(m => m.Album)
            .ThenBy(m => m.Track)
            .ToListAsync();
    }

    public async Task<MusicFile?> GetByIdAsync(int id)
    {
        return await _context.MusicFiles
            .Include(m => m.ScanDirectory)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<MusicFile>> GetByDirectoryAsync(int directoryId)
    {
        return await _context.MusicFiles
            .Where(m => m.ScanDirectoryId == directoryId)
            .OrderBy(m => m.FileName)
            .ToListAsync();
    }

    public async Task<MusicFile?> GetByPathAsync(string filePath)
    {
        return await _context.MusicFiles
            .FirstOrDefaultAsync(m => m.FilePath == filePath);
    }

    public async Task<MusicFile> AddAsync(MusicFile musicFile)
    {
        _context.MusicFiles.Add(musicFile);
        await _context.SaveChangesAsync();
        return musicFile;
    }

    public async Task UpdateAsync(MusicFile musicFile)
    {
        _context.MusicFiles.Update(musicFile);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var musicFile = await _context.MusicFiles.FindAsync(id);
        if (musicFile != null)
        {
            _context.MusicFiles.Remove(musicFile);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(string filePath)
    {
        return await _context.MusicFiles.AnyAsync(m => m.FilePath == filePath);
    }
}