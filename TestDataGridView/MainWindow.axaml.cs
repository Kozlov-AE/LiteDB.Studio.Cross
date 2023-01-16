using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using System;
using System.Collections.Generic;

namespace TestDataGridView {
    public partial class MainWindow : Window {
        private DataGrid _grid;
        private Type _myType;
        public MainWindow() {
            InitializeComponent();
            _grid = this.Find<DataGrid>("Grid");
            LoadGrid(GenerateData());
        }

        public void LoadGrid(BdRequestResult reqRes) {
            foreach (var key in reqRes.Keys) {
                var column = new DataGridTextColumn();
                column.Header = key;
                column.IsReadOnly = false;
                column.Width = new DataGridLength(100);
                column.Binding = new Binding($"P_{key}");
                //column.Binding = new Binding(key);
                _grid.Columns.Add(column);
            }
/*
            var dict1 = new Dictionary<string, string>();
            dict1.Add("FirstName", "Aleksey");
            dict1.Add("LastName", "Kozlov");
            dict1.Add("Age", "35");

            var dict2 = new Dictionary<string, string>();
            dict2.Add("FirstName", "Sergey");
            dict2.Add("LastName", "Petrov");
            dict2.Add("Age", "30");

            /var lo = new List<Dictionary<string, string>>() { dict1, dict2 };

            var rr = ClassGenerator.GenerateType(reqRes.Keys.ToArray(), reqRes.CollectionName, lo);
*/
            
            _myType = ClassGenerator.GenerateType(reqRes.Keys.ToArray(), reqRes.CollectionName);
            List<object> objs = new List<object>();
            foreach (var doc in reqRes.Docs) {
                objs.Add(ClassGenerator.GetObject(_myType, doc.Values));
            }
            var collectionView1 = new DataGridCollectionView(objs);

            _grid.Items = collectionView1;
        }
        private BdRequestResult GenerateData() {
            BdRequestResult res = new BdRequestResult();
            res.CollectionName = "MyCollection";
            res.Keys = new List<string>() { "FirstName", "LastName", "Age" };

            BdDocument doc1 = new BdDocument();
            doc1.Values.Add("FirstName", "Aleksey");
            doc1.Values.Add("LastName", "Kozlov");
            doc1.Values.Add("Age", "35");
            
            BdDocument doc2 = new BdDocument();
            doc2.Values.Add("FirstName", "Sergey");
            doc2.Values.Add("LastName", "Petrov");
            doc2.Values.Add("Age", "30");
            
            res.Docs = new List<BdDocument>() { doc1, doc2 };

            return res;
        }
    }

    public class BdDocument {
        public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
    }

    public class BdRequestResult {
        public string CollectionName { get; set; }
        public List<string> Keys { get; set; }
        public List<BdDocument> Docs { get; set; }
    }

    public class User {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }

        public static List<User> GetUsers() {
            var ls = new List<User>() {
                new User() { FirstName = "1-FirstName", LastName = "1 - LastName", Age = "12" },
                new User() { FirstName = "2 - FirstName", LastName = "2 - LastName", Age = "50" },
            };
            return ls;
        }
    }

}