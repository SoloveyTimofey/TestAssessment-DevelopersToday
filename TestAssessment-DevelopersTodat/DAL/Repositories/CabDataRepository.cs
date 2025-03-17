using Microsoft.EntityFrameworkCore;
using TestAssessment_DevelopersTodat.DAL.Context;
using TestAssessment_DevelopersTodat.DAL.Models;

namespace TestAssessment_DevelopersTodat.DAL.Repositories;

public class CabDataRepository : ICabDataRepository
{
    private readonly ApplicationDbcontext _context;
    public CabDataRepository(ApplicationDbcontext context)
    {
        _context = context;
    }

    public void BulkInsert(List<CabData> cabData)
    {
        _context.CabData.BulkInsert(cabData);
        
    }

    public async Task<bool> ContainsDataAsync() => await _context.CabData.AnyAsync();

    public async Task DeleteAllDataAsync()
    {
        await _context.CabData.ExecuteDeleteAsync();
    }

    public void RemoveRange(List<CabData> cabData)
    {
        _context.CabData.RemoveRange(cabData);
    }

    public IQueryable<CabData> GetAll() => _context.CabData.AsNoTracking();

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}