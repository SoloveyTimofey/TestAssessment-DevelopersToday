//using CsvHelper;
//using CsvHelper.Configuration;
//using CsvHelper.TypeConversion;
//using System.Globalization;
//using TestAssessment_DevelopersTodat.DAL.Models;
//using TimeZoneConverter;

//namespace TestAssessment_DevelopersTodat.Services;

//public class CsvProcessor
//{
//    public List<CabData> ReadCsvFile(string filePath)
//    {
//        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
//        {
//            HasHeaderRecord = true,
//            Delimiter = ",",
//            PrepareHeaderForMatch = args => args.Header.Trim(),
//            MissingFieldFound = null,
//            BadDataFound = context =>
//            {
//                Console.WriteLine($"Bad data encountered: {context.RawRecord}");
//            },
//        };

//        using var reader = new StreamReader(filePath);
//        using var csvReader = new CsvReader(reader, config);

//        csvReader.Context.TypeConverterCache.AddConverter<StatusType>(new StatusTypeConverter());
//        csvReader.Context.TypeConverterCache.AddConverter<string>(new TrimmedStringConverter());
//        csvReader.Context.RegisterClassMap<CabDataMap>();

//        var records = csvReader.GetRecords<CabData>().ToList();

//        var estTimeZone = TZConvert.GetTimeZoneInfo("Eastern Standard Time");

//        foreach (var record in records)
//        {
//            record.Id = Guid.NewGuid();

//            record.TpepPickupDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(record.TpepPickupDateTimeUtc, estTimeZone);
//            record.TpepDropoffDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(record.TpepDropoffDateTimeUtc, estTimeZone);
//        }

//        return records;
//    }

//    public void WriteDuplicates(string duplicatesFilePath, List<CabData> duplicates)
//    {
//        using var writer = new StreamWriter(duplicatesFilePath);
//        using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

//        csvWriter.Context.RegisterClassMap<CabDataMap>();

//        csvWriter.WriteRecords(duplicates);
//    }
//}

//internal class StatusTypeConverter : DefaultTypeConverter
//{
//    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
//    {
//        if (text is null) return null;

//        return text?.ToUpper() switch
//        {
//            "Y" => StatusType.Yes,
//            "N" => StatusType.No,
//            _ => throw new InvalidOperationException($"Unknown status type: {text}")
//        };
//    }
//}

//internal class TrimmedStringConverter : DefaultTypeConverter
//{
//    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
//    {
//        if (text is null) throw new ArgumentNullException(nameof(text));

//        return text.Trim();
//    }
//}

//internal class CabDataMap : ClassMap<CabData>
//{
//    public CabDataMap()
//    {
//        Map(m => m.TpepPickupDateTimeUtc).Name("tpep_pickup_datetime");
//        Map(m => m.TpepDropoffDateTimeUtc).Name("tpep_dropoff_datetime");
//        Map(m => m.PassengerCount).Name("passenger_count");
//        Map(m => m.TripDistance).Name("trip_distance");
//        Map(m => m.StoreAndFwdFlag).Name("store_and_fwd_flag");
//        Map(m => m.PULocationId).Name("PULocationID");
//        Map(m => m.DOLocationId).Name("DOLocationID");
//        Map(m => m.FareAmount).Name("fare_amount");
//        Map(m => m.TipAmount).Name("tip_amount");
//    }
//}

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;
using TestAssessment_DevelopersTodat.DAL.Models;
using TimeZoneConverter;

namespace TestAssessment_DevelopersTodat.Services;

public class CsvProcessor
{
    public List<CabData> ReadCsvFile(string filePath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            PrepareHeaderForMatch = args => args.Header.Trim(),
            MissingFieldFound = null,
            BadDataFound = context =>
            {
                Console.WriteLine($"Bad data encountered: {context.RawRecord}");
            }
        };

        var validRecords = new List<CabData>();

        try
        {
            using var reader = new StreamReader(filePath);
            using var csvReader = new CsvReader(reader, config);

            csvReader.Context.TypeConverterCache.AddConverter<StatusType>(new StatusTypeConverter());
            csvReader.Context.TypeConverterCache.AddConverter<string>(new TrimmedStringConverter());
            csvReader.Context.RegisterClassMap<CabDataMap>();

            var estTimeZone = TZConvert.GetTimeZoneInfo("Eastern Standard Time");

            foreach (var record in csvReader.GetRecords<CabData>())
            {
                try
                {
                    record.Id = Guid.NewGuid();

                    record.TpepPickupDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(record.TpepPickupDateTimeUtc, estTimeZone);
                    record.TpepDropoffDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(record.TpepDropoffDateTimeUtc, estTimeZone);

                    validRecords.Add(record);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing record: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to read CSV file: {ex.Message}");
        }

        return validRecords;
    }

    public void WriteDuplicates(string duplicatesFilePath, List<CabData> duplicates)
    {
        try
        {
            using var writer = new StreamWriter(duplicatesFilePath);
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csvWriter.Context.RegisterClassMap<CabDataMap>();

            csvWriter.WriteRecords(duplicates);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write duplicates to file: {ex.Message}");
        }
    }
}

internal class StatusTypeConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;

        return text.ToUpper() switch
        {
            "Y" => StatusType.Yes,
            "N" => StatusType.No,
            _ => throw new InvalidOperationException($"Unknown status type: {text}")
        };
    }
}

internal class TrimmedStringConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text is null) throw new ArgumentNullException(nameof(text));

        return text.Trim();
    }
}

internal class CabDataMap : ClassMap<CabData>
{
    public CabDataMap()
    {
        Map(m => m.TpepPickupDateTimeUtc).Name("tpep_pickup_datetime");
        Map(m => m.TpepDropoffDateTimeUtc).Name("tpep_dropoff_datetime");
        Map(m => m.PassengerCount).Name("passenger_count").Default(0);
        Map(m => m.TripDistance).Name("trip_distance").TypeConverterOption.NullValues("0");
        Map(m => m.StoreAndFwdFlag).Name("store_and_fwd_flag");
        Map(m => m.PULocationId).Name("PULocationID").Default(0);
        Map(m => m.DOLocationId).Name("DOLocationID").Default(0);
        Map(m => m.FareAmount).Name("fare_amount").Default(0.0);
        Map(m => m.TipAmount).Name("tip_amount").Default(0.0);
    }
}
