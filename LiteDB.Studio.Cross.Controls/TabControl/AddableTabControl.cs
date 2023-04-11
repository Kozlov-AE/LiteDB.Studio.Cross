using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using System.Windows.Input;


namespace LiteDB.Studio.Cross.Controls.TabControl {
    public class AddableTabControl : Avalonia.Controls.TabControl {
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e) {
            base.OnApplyTemplate(e);
            //var addButton = this.FindControl<Button>("PART_AddButton");
            var addButton = e.NameScope.Get<Button>("PART_AddButton");
            addButton.Click += (sender, args) => {
                var e = new RoutedEventArgs(ClickOnAddButtonEvent);
                RaiseEvent(e);
                if (!e.Handled && AddNewTabCommand?.CanExecute(CommandParameter) == true)
                {
                    AddNewTabCommand.Execute(CommandParameter);
                    e.Handled = true;
                }
            };

        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
            base.OnPropertyChanged(change);
            if (change.Property == AddNewTabCommandProperty) {
                
            }
            
        }

        public object? CommandParameter { get; set; }

        /// <summary>
        /// Defines the <see cref="AddNewTabCommand"/> property.
        /// </summary>
        public static readonly StyledProperty<ICommand?> AddNewTabCommandProperty =
            AvaloniaProperty.Register<AddableTabControl, ICommand?>(nameof(AddNewTabCommand), enableDataValidation: true);
        
        public ICommand? AddNewTabCommand
        {
            get => GetValue(AddNewTabCommandProperty);
            set => SetValue(AddNewTabCommandProperty, value);
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e) {
            base.OnAttachedToLogicalTree(e);
            if (AddNewTabCommand != null)
            {
                AddNewTabCommand.CanExecuteChanged += CanExecuteChanged;
                CanExecuteChanged(this, EventArgs.Empty);
            }

        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e) {
            base.OnDetachedFromLogicalTree(e);
            if (AddNewTabCommand != null)
            {
                AddNewTabCommand.CanExecuteChanged -= CanExecuteChanged;
            }
        }

        private void CanExecuteChanged(object? sender, EventArgs e) {
            var canExecute = AddNewTabCommand == null || AddNewTabCommand.CanExecute(CommandParameter);

            if (canExecute != _commandCanExecute)
            {
                _commandCanExecute = canExecute;
                UpdateIsEffectivelyEnabled();
            }
        }


        public static readonly RoutedEvent<RoutedEventArgs> ClickOnAddButtonEvent 
            = RoutedEvent.Register<AddableTabControl, RoutedEventArgs>(nameof(ClickOnAddButtonEvent), RoutingStrategies.Bubble);
        
        private bool _commandCanExecute;

        public event EventHandler<RoutedEventArgs> ClickOnAddButton {
            add => AddHandler(ClickOnAddButtonEvent, value);
            remove => RemoveHandler(ClickOnAddButtonEvent, value);
        }
    }
}