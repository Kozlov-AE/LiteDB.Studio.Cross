using System.Collections.Generic;

namespace LiteDB.Studio.Cross.Models {
    public class DbCollectionModel {
        public string CollectionName { get; set; }
        public HashSet<PropertyModel> Properties { get; set; } = new HashSet<PropertyModel>();
    }
}