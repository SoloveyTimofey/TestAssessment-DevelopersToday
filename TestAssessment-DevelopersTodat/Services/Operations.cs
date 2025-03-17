using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestAssessment_DevelopersTodat.DAL.Models;
using TestAssessment_DevelopersTodat.DAL.Repositories;

namespace TestAssessment_DevelopersTodat.Services;

public class Operations
{
    private readonly CsvProcessor _csvProcessor;
    private readonly ICabDataRepository _repository;

    public Operations(CsvProcessor csvProcessor, ICabDataRepository repository)
    {
        _csvProcessor = csvProcessor;
        _repository = repository;
    }

    public async Task PopulateOrRepopulateDbAsync()
    {
        if (await _repository.ContainsDataAsync())
        {
            Console.WriteLine("\nDb is not empty, deleting data...");
            await _repository.DeleteAllDataAsync();
        }

        List<CabData> models;
        try
        {
            models = _csvProcessor.ReadCsvFile(@"..\..\..\Csvs\sample-cab-data.csv");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error occured: " + ex.Message);
            Console.ResetColor();
            return;
        }

        _repository.BulkInsert(models);

        Console.WriteLine("\nAdded data\n");
    }

    public async Task FindOutWhichPULocationIdHasTheHighestTipAmountOnAverage()
    {
        var cabData = await _repository.GetAll()
            .GroupBy(c => c.PULocationId)
            .Select(g => new
            {
                PULocationId = g.Key,
                AverageTipAmount = g.Average(c => c.TipAmount)
            })
            .OrderByDescending(x => x.AverageTipAmount)
            .FirstOrDefaultAsync();

        if (cabData is null)
        {
            Console.WriteLine("Db is empty");
            return;
        }

        Console.WriteLine($"\nPULocationId with the highest tip amount on average: {cabData.PULocationId} (average: {cabData.AverageTipAmount})\n");
    }

    public async Task FindTheTop100LongestFaresInTermsOfTripDistance()
    {
        var _100LongestFares = await _repository.GetAll()
            .OrderByDescending(c => c.TripDistance)
            .Take(100)
            .ToListAsync();

        Console.WriteLine();
        for (var i = 0; i < _100LongestFares.Count(); i++)
        {
            Console.WriteLine($"{i + 1}. TripDistance: {_100LongestFares[i].TripDistance}");
        }
        Console.WriteLine();
    }

    public async Task FindTheTop100LongestFaresInTermsOfTimeSpentTravelling()
    {
        var _100LongestFares = await _repository.GetAll()
            .OrderByDescending(c => EF.Functions.DateDiffMinute(c.TpepPickupDateTimeUtc, c.TpepDropoffDateTimeUtc))
            .Take(100)
            .ToListAsync();

        Console.WriteLine();
        for (var i = 0; i < _100LongestFares.Count(); i++)
        {
            var cabData = _100LongestFares[i];
            var timeSpentTravelling = (cabData.TpepDropoffDateTimeUtc - cabData.TpepPickupDateTimeUtc).TotalMinutes;
            Console.WriteLine($"{i + 1}. TimeSpentTravelling: {Math.Round(timeSpentTravelling, 1)} min");
        }
        Console.WriteLine();
    }

    public async Task SearchByPULocationId()
    {
        Console.Write("\nEnter PULocationId: ");
        string userInput = Console.ReadLine() ?? string.Empty;

        if (int.TryParse(userInput, out int puLocationId))
        {
            var cabData = await _repository.GetAll()
                .Where(c => c.PULocationId == puLocationId)
                .ToListAsync();

            Console.WriteLine();

            if (cabData.Any() is false)
            {
                Console.WriteLine("There is no cab data with specified PULocationId\n");
                return;
            }

            foreach (var cabDataItem in cabData)
            {
                Console.WriteLine(cabDataItem.ToString());
            }
            Console.WriteLine();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nInvalid input\n");
            Console.ResetColor();
        }
    }

    public async Task IdentifyAndRemoveAnyDuplicateRecords()
    {
        var allRecords = await _repository.GetAll().ToListAsync();
        var duplicates = allRecords
            .GroupBy(c => new { c.TpepPickupDateTimeUtc, c.TpepDropoffDateTimeUtc, c.PassengerCount })
            .Where(g => g.Count() > 1)
            .SelectMany(g => g)
            .ToList();

        if (duplicates.Count() == 0)
        {
            Console.WriteLine("\nThere is no duplicates.\n");
            return;
        }
        Console.WriteLine($"\nDetected {duplicates.Count()} duplicates");

        _csvProcessor.WriteDuplicates(@"..\..\..\Csvs\duplicates.csv", duplicates);
        Console.WriteLine("Duplicates written to the file");

        _repository.RemoveRange(duplicates);
        await _repository.SaveChangesAsync();
        Console.WriteLine("Duplicates removed from the db");
    }
}