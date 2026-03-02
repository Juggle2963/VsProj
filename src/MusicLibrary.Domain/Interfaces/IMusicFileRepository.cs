using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Domain.Interfaces;

public interface IMusicFileRepository
{
    Task<IEnumerable<MusicFile>> GetAllAsync();
    Task<MusicFile?> GetByIdAsync(int id);
    Task<IEnumerable<MusicFile>> GetByDirectoryAsync(int directoryId);
    Task<MusicFile?> GetByPathAsync(string filePath);
    Task<MusicFile> AddAsync(MusicFile musicFile);
    Task UpdateAsync(MusicFile musicFile);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(string filePath);
}