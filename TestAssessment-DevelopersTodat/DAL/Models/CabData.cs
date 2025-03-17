namespace TestAssessment_DevelopersTodat.DAL.Models;

public sealed class CabData
{
    public Guid Id { get; set; }
    public DateTime TpepPickupDateTimeUtc {get;set;}
    public DateTime TpepDropoffDateTimeUtc {get;set;}
    public int? PassengerCount { get; set;}
    public double TripDistance { get; set;}
    public StatusType? StoreAndFwdFlag { get; set; }
    public int PULocationId { get; set; }
    public int DOLocationId { get; set; }
    public double FareAmount { get; set; }
    public double TipAmount { get; set; }

    public override string ToString()
        => $"Id: {Id}, TpepPickupDateTime: {TpepPickupDateTimeUtc}, TpepDropoffDateTime: {TpepDropoffDateTimeUtc}, PassengerCount: {PassengerCount}, TripDistance: {TripDistance}, StoreAndFwdFlag: {StoreAndFwdFlag}, PULocationId: {PULocationId}, DOLocationId: {DOLocationId}, FareAmount: {FareAmount}, TipAmount: {TipAmount}";
}
