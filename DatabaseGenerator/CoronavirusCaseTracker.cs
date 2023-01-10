using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace DatabaseGenerator {
    public class CoronavirusCaseTracker {
    }

    public class CountyLevelConfirmedCase {
        public string Id { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LocationType { get; set; }
        public string State { get; set; }
        public string CountyName { get; set; }
        public string CountyNameLong { get; set; }
        public int FipsCode { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string NchsUrbanization { get; set; }
        public double TotalPopulation { get; set; }
        public int Confirmed { get; set; }
        public double ConfirmedPer100000 { get; set; }
        public int Deaths { get; set; }
        public double DeathsPer100000 { get; set; }

        public static List<CountyLevelConfirmedCase> LoadMe(string path) {
            var result = new List<CountyLevelConfirmedCase>(3300);
            using (TextFieldParser parser = new TextFieldParser(path)) {
                int i = 1;
                parser.TrimWhiteSpace = true;
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData) {
                    string[] fields = parser.ReadFields();
                    if (i > 0){
                        i--;
                    }
                    else{
                        var @case = new CountyLevelConfirmedCase();
                        var @date = fields[0][..^4];
                        @case.LastUpdate = DateTime.Parse(@date);
                        @case.LocationType = fields[1];
                        @case.State = fields[2];
                        @case.CountyName = fields[3];
                        @case.CountyNameLong = fields[4];
                        @case.FipsCode = int.TryParse(fields[5], CultureInfo.InvariantCulture, out var val) ? val : 0;
                        @case.TotalPopulation = double.TryParse(fields[9], CultureInfo.InvariantCulture, out var dval) ? dval : 0;
                        @case.Confirmed = int.TryParse(fields[10], CultureInfo.InvariantCulture, out val) ? val : 0;
                        @case.ConfirmedPer100000 = double.TryParse(fields[11], CultureInfo.InvariantCulture, out dval) ? dval : 0;
                        @case.Deaths = int.TryParse(fields[12], CultureInfo.InvariantCulture, out val) ? val : 0;
                        @case.Lat = double.TryParse(fields[6], CultureInfo.InvariantCulture, out dval) ? dval : 0;
                        @case.Lon = double.TryParse(fields[7], CultureInfo.InvariantCulture, out dval) ? dval : 0;
                        @case.NchsUrbanization = fields[8];
                        @case.DeathsPer100000 = double.TryParse(fields[13], CultureInfo.InvariantCulture, out dval) ? dval : 0;
                        @case.Id = Guid.NewGuid().ToString();
                        result.Add(@case);
                    }
                }
            }
            return result;
        } 
    }
}