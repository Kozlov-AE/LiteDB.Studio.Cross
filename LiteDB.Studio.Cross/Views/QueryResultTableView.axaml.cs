using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using LiteDB.Studio.Cross.Models;
using LiteDB.Studio.Cross.ViewModels;
using System;
using System.ComponentModel;

namespace LiteDB.Studio.Cross.Views {
    public partial class QueryResultTableView : UserControl {
        private DataGrid _table;
        private DbTableViewModel _vm;

        public QueryResultTableView() {
            InitializeComponent();
            DataContextChanged += QueryResultTableView_DataContextChanged;
            _table = this.Find<DataGrid>("ResultTable");
        }
        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }


        private void QueryResultTableView_DataContextChanged(object? sender, EventArgs e) {
            if (this.DataContext is DbTableViewModel tvm) {
                _vm = tvm;
                _vm.TableUpdated += () => LoadTable(_vm);
                if (_vm.Rows.Any()) LoadTable(_vm);
            }
        }

        private void LoadTable(DbTableViewModel tvm) {
            _table.Columns.Clear();
            foreach (var prop in tvm.Fields) {
                DataGridColumn col = new DataGridTextColumn() {
                    Header = prop.Name,
                    CanUserSort = true,
                    IsReadOnly = false,
                    Binding = new Binding($"Items[{prop.Name}]")
                };
                _table.Columns.Add(col);
            }

            _table.Items = tvm.Rows;
        } 
    }
}