using System;
using System.Collections.Generic;

namespace LiteDB.Studio.Cross.Models {
    public class DbQuerryResultModel {
        public DbCollectionModel Collection { get; set; } = new ();
        public List<CollectionItem> Items { get; set; } = new ();
    }
}