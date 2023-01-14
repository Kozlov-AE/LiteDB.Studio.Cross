using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using System.Collections.Generic;

namespace TestDataGridView {
    public partial class MainWindow : Window {
        private DataGrid _grid;
        public MainWindow() {
            InitializeComponent();
            _grid = Grid;
        }

        public void LoadGrid(BdDocument doc) {
            foreach (var key in doc.Keys) {
                var column = new DataGridTextColumn();
                column.Header = key;
                column.IsReadOnly = false;
                _grid.Columns.Add(column);
            }

            var row = new DataGridRow();

        }
    }

    public class BdDocument {
        public List<string> Keys { get; set; }
        public List<(string key,string value)> Values { get; set; }
    }
}