using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using LiteDB.Studio.Cross.ViewModels;
using LiteDB.Studio.Cross.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LiteDB.Studio.Cross {
    public partial class App : Application {
        public IServiceProvider Services { get; private set; }
        public override void Initialize() {
            Services = IoC.ConfigureServices();
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted() {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
                var mv = Services.GetRequiredService<MainWindowViewModel>();
                desktop.MainWindow = new MainWindow { DataContext = mv };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}