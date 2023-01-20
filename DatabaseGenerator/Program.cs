// See https://aka.ms/new-console-template for more information

using DatabaseGenerator;
using LiteDB;

Console.WriteLine("Creating database from 1_county_level_confirmed_cases.csv...");
var countyLevelConfirmedCases = CountryLevelConfirmedCase.LoadMe("1_county_level_confirmed_cases.csv");
using (var db = new LiteDatabase(@"CoronavirusCaseTracker.db")) {
    if (db.CollectionExists("CountyLevelConfirmedCase")){
        var col = db.GetCollection<CountryLevelConfirmedCase>();
        col.EnsureIndex(x => x.Id, true);
        col.Insert(countyLevelConfirmedCases);
    } else {
        var col = db.GetCollection<CountryLevelConfirmedCase>();
        col.EnsureIndex(x => x.Id, true);
        col.Insert(countyLevelConfirmedCases);
    }
}
Console.WriteLine("1_county_level_confirmed_cases.csv loaded!");