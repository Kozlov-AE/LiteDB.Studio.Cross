using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LiteDB.Studio.Cross.Views {
    public partial class RigthSideView : UserControl {
        public RigthSideView() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}