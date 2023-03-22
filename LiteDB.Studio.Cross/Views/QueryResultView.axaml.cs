using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LiteDB.Studio.Cross.Views {
    public partial class QueryResultView : UserControl {
        public QueryResultView() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}