using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;


namespace LiteDB.Studio.Cross.Controls.TabControl {
    public class AddableTabControl : Avalonia.Controls.TabControl{
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e) {
            base.OnApplyTemplate(e);
            var addButton = this.FindControl<Button>("PART_AddButton");
            if (addButton != null) {
                addButton.Click += (sender, args) => {
                    var e_ = new RoutedEventArgs(ClickOnAddButtonEvent);
                    RaiseEvent(e_);
                    e_.Handled = true;
                };
            }
        }

        public static readonly RoutedEvent<RoutedEventArgs> ClickOnAddButtonEvent 
            = RoutedEvent.Register<AddableTabControl, RoutedEventArgs>(nameof(ClickOnAddButtonEvent), RoutingStrategies.Bubble);
        public event EventHandler<RoutedEventArgs> ClickOnAddButton {
            add => AddHandler(ClickOnAddButtonEvent, value);
            remove => RemoveHandler(ClickOnAddButtonEvent, value);
        }
    }
}