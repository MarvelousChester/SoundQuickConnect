using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Forms = System.Windows.Forms;

namespace SoundQuickConnect;

public partial class App : Application
{

    private Forms.NotifyIcon _notifyIcon;
    
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
    }
    
    
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.Startup += OnStartup;
            desktop.Exit += OnExit;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void OnStartup(object s, ControlledApplicationLifetimeStartupEventArgs e)
    {
        _notifyIcon = new Forms.NotifyIcon();
        _notifyIcon.Icon = new System.Drawing.Icon("Assets/bluetooth-32.ico");
        _notifyIcon.Visible = true;
    }

    private void OnExit(object sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _notifyIcon.Dispose();
    } 
    

    

    
    
}