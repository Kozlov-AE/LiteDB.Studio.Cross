using System.Collections.Generic;
using System.Xml;

namespace LiteDB.Studio.Cross.Contracts.DTO {
    public record DataBaseDto {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> SysCollections { get; set; }
        public IEnumerable<string> DbCollections { get; set; }
    }
}

