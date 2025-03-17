using TestAssessment_DevelopersTodat.DAL.Models;

namespace TestAssessment_DevelopersTodat.DAL.Repositories;

public interface ICabDataRepository
{
    void BulkInsert(List<CabData> cabData);
    IQueryable<CabData> GetAll();
    Task<bool> ContainsDataAsync();
    Task DeleteAllDataAsync();
    void RemoveRange(List<CabData> cabData);

    Task SaveChangesAsync();
}