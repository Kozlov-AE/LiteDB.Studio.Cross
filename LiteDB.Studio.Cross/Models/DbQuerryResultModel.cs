using System;
using System.Collections.Generic;

namespace LiteDB.Studio.Cross.Models {
    public class DbQuerryResultModel {
        public DbCollectionModel Collection { get; set; } = new DbCollectionModel();
        public List<CollectionItem> Items { get; set; } = new List<CollectionItem>();
    }

    public class DbCollectionModel {
        public string CollectionName { get; set; }
        public HashSet<PropertyModel> Properties { get; set; } = new HashSet<PropertyModel>();
    }

    public class CollectionItem {
        public Dictionary<string, dynamic> Values { get; set; } = new Dictionary<string, dynamic>();
    }
}