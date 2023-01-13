using Avalonia.Controls;
using System.Collections.Generic;

namespace TestDataGridView {
    public partial class MainWindow : Window {
        private DataGrid _grid;
        public MainWindow() {
            InitializeComponent();
            _grid = Grid;
        }

        public void LoadGrid(BdDocument doc) {
        }
    }

    public class BdDocument {
        public List<string> Keys { get; set; }
        public List<(string key,string value)> Values { get; set; }
    }
}