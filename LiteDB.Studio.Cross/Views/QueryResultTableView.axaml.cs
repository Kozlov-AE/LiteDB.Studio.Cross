using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using LiteDB.Studio.Cross.ViewModels;
using System;

namespace LiteDB.Studio.Cross.Views {
    public partial class QueryResultTableView : UserControl {
        private DataGrid _table;
        private MainWindowViewModel _vm;

        public QueryResultTableView() {
            InitializeComponent();
            DataContextChanged += QueryResultTableView_DataContextChanged;
            _table = this.Find<DataGrid>("ResultTable");
        }
        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }


        private void QueryResultTableView_DataContextChanged(object? sender, EventArgs e) {
            if (this.DataContext is MainWindowViewModel mwm) _vm = mwm;
            _vm.QueryFinished += ViewModel_QueryFinished;
        }


        private void ViewModel_QueryFinished(DbCollectionViewModel cvm) {
            LoadTable(cvm);
        }

        private void LoadTable(DbCollectionViewModel cvm) {
            _table.Columns.Clear();
            foreach (var item in cvm.Fields) {
                DataGridColumn col = new DataGridTextColumn() {
                    Header = item.Name,
                    IsReadOnly = false,
                    Binding = new Binding(item.Name)
                };
                _table.Columns.Add(col);
            }
            _table.Items = cvm.Items;
        }
    }
}