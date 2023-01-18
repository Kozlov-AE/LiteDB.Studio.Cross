using System;

namespace LiteDB.Studio.Cross.Models {
    public record PropertyModel(string Name, Type @Type) {
        public override int GetHashCode() {
            return Name.GetHashCode();
        }
    }
}