using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LiteDB.Studio.Cross.Views.UserControls {
    public partial class DbCollectionsListItemUC : UserControl {
        public DbCollectionsListItemUC() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}