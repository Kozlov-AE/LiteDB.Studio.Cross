using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace LiteDB.Studio.Cross.Views;

public partial class ConnectionView : UserControl
{
    private Button _openFileButton;
    private TextBox _selectedFile;
    private ComboBox _cultureBox;
    private ComboBox _sortBox;
    
    Window GetWindow() => this.VisualRoot as Window ?? throw new NullReferenceException("Invalid Owner");
    public ConnectionView()
    {
        InitializeComponent();
        _openFileButton = this.Find<Button>("OpenFileButton");
        if (_openFileButton != null) _openFileButton.Click += OpenFileButtonOnClick;
        _cultureBox = this.Find<ComboBox>("CultureBox");
        if (_cultureBox != null) _cultureBox.Items = CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Select(x => x.LCID)
            .Distinct()
            .Where(x => x != 4096)
            .Select(x => CultureInfo.GetCultureInfo(x).Name)
            .ToList();
        _sortBox = this.Find<ComboBox>("SortBox");
        if (_sortBox != null) _sortBox.Items = Enum.GetNames(typeof(CompareOptions));
    }

    private async void OpenFileButtonOnClick(object? sender, RoutedEventArgs e)
    {
        var uri = Assembly.GetEntryAssembly()?.GetModules().FirstOrDefault()?.FullyQualifiedName;
        var initialFileName = uri == null ? null : System.IO.Path.GetFileName(uri);
        var initialDirectory = uri == null ? null : System.IO.Path.GetDirectoryName(uri);

        var result = await new OpenFileDialog()
        {
            Title = "Open file",
            Directory = initialDirectory,
            InitialFileName = initialFileName
        }.ShowAsync(GetWindow());
        _selectedFile = this.Find<TextBox>("SelectedFile");
        if (_selectedFile != null) _selectedFile.Text = result?.First();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}