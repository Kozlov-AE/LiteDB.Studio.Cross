using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LiteDB.Studio.Cross.Views {
    public partial class LeftSideView : UserControl {
        public LeftSideView() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}