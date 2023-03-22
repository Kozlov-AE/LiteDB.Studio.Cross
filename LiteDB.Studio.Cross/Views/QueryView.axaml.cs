using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LiteDB.Studio.Cross.Views {
    public partial class QueryView : UserControl {
        public QueryView() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}