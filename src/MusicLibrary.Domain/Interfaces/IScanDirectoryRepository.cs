using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Domain.Interfaces;

public interface IScanDirectoryRepository
{
    Task<IEnumerable<ScanDirectory>> GetAllAsync();
    Task<ScanDirectory?> GetByIdAsync(int id);
    Task<ScanDirectory> AddAsync(ScanDirectory directory);
    Task UpdateAsync(ScanDirectory directory);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(string path);
}