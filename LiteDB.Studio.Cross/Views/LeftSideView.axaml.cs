using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using LiteDB.Studio.Cross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace LiteDB.Studio.Cross.Views {
    public partial class LeftSideView : UserControl {
        private ListBox _dbList;
        private object _selectedDb;
        private object _selectedCollection;
        
        public LeftSideView() {
            InitializeComponent();
            _dbList = this.FindControl<ListBox>("DbList");
            _dbList.SelectionChanged += DbListOnSelectionChanged;
            _dbList.Items.CollectionChanged += DbListOnCollectionChanged;
            _dbList.LayoutUpdated += DbListOnLayoutUpdated;
            
            var sysColl = this.Find<TreeView>("SysCollectionsTree");
            var dbColl = this.Find<TreeView>("DbCollectionsTree");
        }

        private void DbListOnLayoutUpdated(object? sender, EventArgs e) {
            var childs = _dbList.GetLogicalChildren();
            foreach (var ch in childs) {
                var se = ch.Find<Expander>("SysCollectionsExpander");
                var st = ch.Find<TreeView>("SysCollectionsTree");
                var dt = ch.Find<TreeView>("DbCollectionsTree");
                var sysExp = _dbList.FindControl<TreeView>("SysCollectionsExpander");
                var sysTree = _dbList.FindControl<TreeView>("SysCollectionsTree");
                var dbTree = _dbList.FindControl<TreeView>("DbCollectionsTree");
            }
        }

        private void DbListOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
            if (e.NewItems != null) {
                var childs = _dbList.GetVisualChildren();
                foreach (var ch in childs) {
                    var se = ch.Find<Expander>("SysCollectionsExpander");
                    var st = ch.Find<TreeView>("SysCollectionsTree");
                    var dt = ch.Find<TreeView>("DbCollectionsTree");
                    var sysExp = _dbList.FindControl<TreeView>("SysCollectionsExpander");
                    var sysTree = _dbList.FindControl<TreeView>("SysCollectionsTree");
                    var dbTree = _dbList.FindControl<TreeView>("DbCollectionsTree");
                }
            } 
        }

        private void DbListOnSelectionChanged(object? sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count > 0) {
                _selectedDb = e.AddedItems[0];
            }
        }
    }
}