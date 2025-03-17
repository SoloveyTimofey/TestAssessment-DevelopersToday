using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestAssessment_DevelopersTodat.DAL.Context;
using TestAssessment_DevelopersTodat.Services;
using TestAssessment_DevelopersTodat.StaticClasses;

var host = ProgramMethods.CreateHostBuilder(args).Build();

using var scope = host.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<ApplicationDbcontext>();
context.Database.Migrate();

var operations = scope.ServiceProvider.GetRequiredService<Operations>();

while(true)
{
    Console.WriteLine("Select an option:" +
    "\n1. Populate or Repopulate db" +
    "\n2. Identify and remove any duplicate records from the dataset based on a combination of `pickup_datetime`, `dropoff_datetime`, and `passenger_count`. Write all removed duplicates into a `duplicates.csv` file." +
    "\n***** Search *****" +
    "\n3.Find out which `PULocationId` (Pick-up location ID) has the highest tip_amount on average." +
    "\n4.Find the top 100 longest fares in terms of `trip_distance`." +
    "\n5.Find the top 100 longest fares in terms of time spent traveling." +
    "\n6.Search, where part of the conditions is `PULocationId`.");

    string userInput = Console.ReadLine() ?? string.Empty;

    if (int.TryParse(userInput, out int selectedOption))
    {
        switch (selectedOption)
        {
            case 1:
                await operations.PopulateOrRepopulateDbAsync();
                break;
            case 2:
                await operations.IdentifyAndRemoveAnyDuplicateRecords();
                break;
            case 3:
                await operations.FindOutWhichPULocationIdHasTheHighestTipAmountOnAverage();
                break;
            case 4:
                await operations.FindTheTop100LongestFaresInTermsOfTripDistance();
                break;
            case 5:
                await operations.FindTheTop100LongestFaresInTermsOfTimeSpentTravelling();
                break;
            case 6:
                await operations.SearchByPULocationId();
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid option\n");
                Console.ResetColor();
                continue;
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nInvalid input\n");
        Console.ResetColor();
        continue;
    }
}
