using Avalonia;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit;
using System;

namespace LiteDB.Studio.Cross.Behaviors {
    public class CustomizeEditorBehavior : Behavior<TextEditor> {
        private double originalMinWidth = 0;
        private IDisposable? _boundsChangedObservable;
        protected override void OnAttachedToVisualTree() {
            base.OnAttachedToVisualTree();
            originalMinWidth = AssociatedObject.MinWidth;
            _boundsChangedObservable = AssociatedObject.GetObservable(Visual.BoundsProperty)
                .Subscribe(OnBoundsChanged);
        }
        private void OnBoundsChanged(Rect obj) {
            AssociatedObject.MinWidth = Math.Clamp(obj.Width, AssociatedObject.MinWidth, AssociatedObject.MaxWidth);
        }

        protected override void OnDetachedFromVisualTree() {
            _boundsChangedObservable?.Dispose();
            AssociatedObject.MinWidth = originalMinWidth;
            base.OnDetachedFromVisualTree();
        }
    }
}