using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LiteDB.Studio.Cross.ViewModels;

namespace LiteDB.Studio.Cross.Views {
    public partial class ConnectionView : UserControl {
        private Button _openFileButton;
        private Button _cancelButton;
        private TextBox _selectedFile;
        private ComboBox _cultureBox;
        private ComboBox _sortBox;
        private MainWindowViewModel _viewModel;

        private Window GetWindow() {
            return VisualRoot as Window ?? throw new NullReferenceException("Invalid Owner");
        }
        
        
        public ConnectionView() {
            InitializeComponent();
            InitControlsProps();
        }

        private void InitControlsProps() {
            _openFileButton = this.Find<Button>("OpenFileButton");
            _cancelButton = this.Find<Button>("CancelBtn");
            _cultureBox = this.Find<ComboBox>("CultureBox");
            _sortBox = this.Find<ComboBox>("SortBox");
            _selectedFile = this.Find<TextBox>("SelectedFile");
            
            _openFileButton.Click += OpenFileButtonOnClick;
            _cancelButton.Click += HideConnectionView;

            _cultureBox.ItemsSource = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Select(x => x.LCID)
                .Distinct()
                .Where(x => x != 4096)
                .Select(x => CultureInfo.GetCultureInfo(x).Name)
                .ToList();
            _sortBox.ItemsSource = Enum.GetNames(typeof(CompareOptions));
        }

        protected override void OnDataContextChanged(EventArgs e) {
            base.OnDataContextChanged(e);
            if (DataContext is MainWindowViewModel vm) _viewModel = vm;
        }

        private void HideConnectionView(object? sender, RoutedEventArgs e) {
            _viewModel.IsLoadDatabaseNeeded = false;
        }

        private async void OpenFileButtonOnClick(object? sender, RoutedEventArgs e) {
            var uri = Assembly.GetEntryAssembly()?.GetModules().FirstOrDefault()?.FullyQualifiedName;
            var initialFileName = uri == null ? null : System.IO.Path.GetFileName(uri);
            var initialDirectory = uri == null ? null : System.IO.Path.GetDirectoryName(uri);

            var result =
                await new OpenFileDialog() {
                    Title = "Open file", Directory = initialDirectory, InitialFileName = initialFileName
                }.ShowAsync(GetWindow());
            if (result?.Length == 0) return;
            _selectedFile.Text = result?.First();
        }
    }
}