using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestDataGridView {
    public partial class MainWindow : Window {
        private DataGrid _grid;
        private Type _myType;

        public MainWindow() {
            InitializeComponent();
            _grid = this.Find<DataGrid>("Grid");
            //LoadGrid(GenerateData());
            //LoadGrid(GenerateDataInList());
            LoadGrid(GenerateDataDic());
        }

        public void LoadGrid(BdRequestResult reqRes) {
            foreach (var key in reqRes.Keys) {
                var column = new DataGridTextColumn();
                column.Header = key;
                column.IsReadOnly = false;
                column.Width = new DataGridLength(100);
                column.CanUserSort = true;
                column.SortMemberPath = $"P_{key}";
                column.Binding = new Binding($"P_{key}");
                _grid.Columns.Add(column);
            }

            _myType = ClassGenerator.GenerateType(reqRes.Keys.ToArray(), reqRes.CollectionName);
            List<dynamic> objs = new List<dynamic>();
            foreach (var doc in reqRes.Docs) {
                objs.Add(ClassGenerator.GetObject(_myType, doc.Values));
            }

            var collectionView1 = new DataGridCollectionView(objs);

            _grid.Items = collectionView1;
        }

        public void LoadGrid(BdRequestResultInList reqRes) {
            for (int i = 0; i < reqRes.Keys.Count; i++) {
                var column = new DataGridTextColumn();
                column.Header = reqRes.Keys[i].Key;
                column.IsReadOnly = false;
                column.Width = new DataGridLength(100);
                column.CanUserSort = true;
                //column.SortMemberPath= $"P_{key}";
                column.Binding = new Binding($"Properties[{i}]");
                _grid.Columns.Add(column);
            }

            _grid.Items = reqRes.Docs;
        }
        public void LoadGrid(BdRequestResultDic reqRes) {
            foreach (var key in reqRes.Keys) {
                var column = new DataGridTextColumn();
                column.Header = key.Key;
                column.IsReadOnly = false;
                column.Width = new DataGridLength(100);
                column.CanUserSort = true;
                column.Binding = new Binding($"Properties[{key.Key}]");
                _grid.Columns.Add(column);
            }
            _grid.Items = reqRes.Docs;
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

        private BdRequestResultInList GenerateDataInList() {
            BdRequestResultInList res = new BdRequestResultInList();
            res.CollectionName = "MyCollection";
            res.Keys = new List<(string Key, Type Value)>() {
                new() { Key = "FirstName", Value = typeof(string) },
                new() { Key = "LastName", Value = typeof(string) },
                new() { Key = "Age", Value = typeof(int) }
            };

            BdDocInList doc1 = new BdDocInList();
            doc1.Properties = new List<string>() { "Aleksey", "Kozlov", "35" };

            BdDocInList doc2 = new BdDocInList();
            doc2.Properties = new List<string>() { "Sergey", "Petrov", "30" };

            res.Docs = new List<BdDocInList>() { doc1, doc2 };

            return res;
        }

        private BdRequestResultDic GenerateDataDic() {
            BdRequestResultDic res = new BdRequestResultDic();
            res.CollectionName = "MyCollection";
            res.Keys = new List<(string Key, Type Value)>() {
                new() { Key = "FirstName", Value = typeof(string) },
                new() { Key = "LastName", Value = typeof(string) },
                new() { Key = "Age", Value = typeof(int) }
            };
            
            BdDocDic doc1 = new BdDocDic();
            doc1.Properties.Add("FirstName", "Aleksey");
            doc1.Properties.Add("LastName", "Kozlov");
            doc1.Properties.Add("Age", "35");

            BdDocDic doc2 = new BdDocDic();
            doc2.Properties.Add("FirstName", "Sergey");
            doc2.Properties.Add("LastName", "Petrov");
            doc2.Properties.Add("Age", "30");

            res.Docs = new List<BdDocDic>() { doc1, doc2 };

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
        public int Age { get; set; }

        public static List<User> GetUsers() {
            var ls = new List<User>() {
                new User() { FirstName = "1-FirstName", LastName = "1 - LastName", Age = 12 },
                new User() { FirstName = "2 - FirstName", LastName = "2 - LastName", Age = 50 },
            };
            return ls;
        }
    }

    public class BdRequestResultInList {
        public string CollectionName { get; set; }
        public List<(string Key, Type Value)> Keys { get; set; }
        public List<BdDocInList> Docs { get; set; }
    }

    public class BdDocInList {
        public List<string> Properties { get; set; }
    }
    
    public class BdDocDic {
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
    public class BdRequestResultDic {
        public string CollectionName { get; set; }
        public List<BdDocDic> Docs { get; set; }
        public List<(string Key, Type Value)> Keys { get; set; }
    }
}