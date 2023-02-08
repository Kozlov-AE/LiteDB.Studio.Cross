using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LiteDB.Studio.Cross.Views {
    public partial class DataBaseWorkspaseView : UserControl {
        public DataBaseWorkspaseView() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}