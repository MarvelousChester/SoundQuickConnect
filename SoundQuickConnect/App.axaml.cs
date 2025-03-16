// NOTE: The reasoning for Avalonia was that original intended it to be UI application but decided not to but keeping as
// If I ever want to do more with it, but for now, it suits my purposes.
using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Forms = System.Windows.Forms;

namespace SoundQuickConnect;

public partial class App : Application
{

    private const string AppName = "SoundQuickConnect";
    
    private Forms.NotifyIcon _notifyIcon = null!;
    private BluetoothHandler _bluetoothHandler = null!;

    private Forms.ToolStripDropDownButton _devicesDropDownMenu = null!;
    private Forms.ToolStripButton _refreshBtn = null!;
    private Forms.ToolStripButton _closeBtn = null!;
    
    // STARTUP Related
    private Forms.ToolStripMenuItem _startUpToggleBtn = null!;
    private const string StartUpTextChecked = "✔ Set On StartUp";
    private const string StartUpTextUnchecked = "Set On StartUp";
    private string _shortcutPath = null!;
    private string? _exePath = null!; 
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Startup += OnStartup;
            desktop.Exit += OnExit;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void OnStartup(object s, ControlledApplicationLifetimeStartupEventArgs e)
    {
        _bluetoothHandler = new BluetoothHandler();
        _notifyIcon = new Forms.NotifyIcon();
        _notifyIcon.Icon = new System.Drawing.Icon("Assets/bluetooth-32.ico");
        _notifyIcon.Visible = true;
        _notifyIcon.Text = AppName;
        _notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
       
        _shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\SoundQuickConnect.lnk";
        _exePath = Environment.ProcessPath; 
        
        InitDevicesDropDown();
        InitRefreshDevices();
        InitRefreshBtn();
        InitOnStartUpBtn();
        InitExitBtn();
    }

    private void InitOnStartUpBtn()
    {
        _startUpToggleBtn  = new Forms.ToolStripMenuItem(StartUpTextUnchecked, null, (sender, args) =>
        {
            if (IsStartUpEnabled())
            {
                DisableAppOnStartUp();
                _startUpToggleBtn.Text = StartUpTextUnchecked;
            }
            else
            {
                EnableAppOnStartUp();
                _startUpToggleBtn.Text = StartUpTextChecked;
            }
        });
        
        if (IsStartUpEnabled())
        {
            _startUpToggleBtn.Text = StartUpTextChecked;
        }
        _notifyIcon.ContextMenuStrip.Items.Add(_startUpToggleBtn);
    }

    private void InitExitBtn()
    {
        _closeBtn = new Forms.ToolStripButton("Exit", null, (sender, args) =>
        {
            Environment.Exit(0);
        });
        _notifyIcon.ContextMenuStrip.Items.Add(_closeBtn);
    }
    
    private void InitDevicesDropDown()
    {
        _devicesDropDownMenu = new Forms.ToolStripDropDownButton("Devices");
        _notifyIcon.ContextMenuStrip.Items.Add(_devicesDropDownMenu);
    }
    
    private void InitRefreshBtn()
    {
        _refreshBtn = new Forms.ToolStripButton("Refresh", null, (sender, args) =>
        {
            InitRefreshDevices();
        });
        _notifyIcon.ContextMenuStrip.Items.Add(_refreshBtn);
    }
    
    private Forms.ToolStripDropDownItem ToDropDownItem(string deviceName)
    {
        return new Forms.ToolStripMenuItem(deviceName, null, (sender, args) =>
        {
            // TODO for future if need be
            /*if (_bluetoothHandler.IsDeviceConnected(deviceName))
            {
                _bluetoothHandler.DisconnectDevice(deviceName);
                return;
            }*/
            _bluetoothHandler.ConnectToDevice(deviceName);
        });
    }
    
    private void InitRefreshDevices()
    {
        _devicesDropDownMenu.DropDownItems.Clear();
        foreach (var device in _bluetoothHandler.GetPairedDeviceNames())
        {
            _devicesDropDownMenu.DropDownItems.Add(ToDropDownItem(device));
        }
    }
    
    private void EnableAppOnStartUp()
    {
        var shortCutCmd =
            $"powershell \"$s=(New-Object -ComObject WScript.Shell).CreateShortcut('{_shortcutPath}'); $s.TargetPath ='{_exePath}'; $s.Save()\"";
        
        var process = new System.Diagnostics.Process();
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            FileName = "cmd.exe",
            Arguments = $"/C {shortCutCmd}"
        };
        process.StartInfo = startInfo;
        process.Start();
    }

    private void DisableAppOnStartUp()
    {
        if (IsStartUpEnabled())
        {
            File.Delete(_shortcutPath);   
        }
    }
    
    private bool IsStartUpEnabled()
    {
        return File.Exists(_shortcutPath);
    }
    
    private void OnExit(object sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _notifyIcon.Dispose();
    }
    
}