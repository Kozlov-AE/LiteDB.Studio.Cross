using System.ComponentModel.DataAnnotations;

namespace LiteDB.Studio.Cross.Contracts.DTO {
    public class ConnectionParametersDto {
        public bool IsShared { get; set; }
        [Required(ErrorMessage = "The database path is not specified")]
        public string?  DbPath { get; set; }
        public string? Password { get; set; }
        public string? InitSize { get; set; }
        public string? Culture { get; set; }
        public string? Sort { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsUpgrade { get; set; }
    }
}

