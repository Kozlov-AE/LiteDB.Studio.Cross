using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LiteDB.Studio.Cross.Views {
    public partial class QueryResultTableView : UserControl {
        private DataGrid _table;
        public QueryResultTableView() {
            InitializeComponent();
            _table = this.Find<DataGrid>("ResultTable");
        }

        private void LoadTable() {
            
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}