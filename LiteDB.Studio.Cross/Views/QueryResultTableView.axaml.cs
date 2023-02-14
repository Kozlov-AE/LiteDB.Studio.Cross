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
        private QueryViewModel _vm;

        public QueryResultTableView() {
            InitializeComponent();
            DataContextChanged += QueryResultTableView_DataContextChanged;
            _table = this.Find<DataGrid>("ResultTable");
        }
        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }


        private void QueryResultTableView_DataContextChanged(object? sender, EventArgs e) {
            if (this.DataContext is QueryViewModel qwm) _vm = qwm;
            _vm.PropertyChanged += VmOnPropertyChanged;
        }

        private void VmOnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case "TableVm":
                    LoadTable(_vm.TableVm);
                    break;
            }
        }


        private void ViewModel_QueryFinished(DbTableViewModel tvm) {
            LoadTable(tvm);
        }

        private void LoadTable(DbTableViewModel tvm) {
            _table.Columns.Clear();
            foreach (var prop in tvm.Fields) {
                DataGridColumn col = new DataGridTextColumn() {
                    Header = prop.Name,
                    CanUserSort = true,
                    IsReadOnly = false,
                    Binding = new Binding($"Values[{prop.Name}]")
                };
                _table.Columns.Add(col);
            }

            _table.Items = tvm.Rows;
        }
    }
}