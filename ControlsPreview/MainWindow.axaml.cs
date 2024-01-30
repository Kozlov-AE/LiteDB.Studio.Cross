using Avalonia.Controls;
using Avalonia.Interactivity;
using ControlsPreview.treeviewTest;

namespace ControlsPreview;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
        this.DataContext = new TVTestVM();
    }
}