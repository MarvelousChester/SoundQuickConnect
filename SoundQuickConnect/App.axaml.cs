using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    private ICollection<string> _pairedDevices = new List<string>();
    private string _selectedQuickConnectDevice= null!;

    private Forms.ToolStripDropDownButton _devicesDropDownMenu = null!;
    private Forms.ToolStripButton _refreshBtn = null!;
    
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
        
        DevicesDropDownInit();
        RefreshDevices();
        RefreshBtnInit();
        InitOnStartUpBtn();
        
    }

    private void InitOnStartUpBtn()
    {
        _startUpToggleBtn  = new Forms.ToolStripMenuItem(StartUpTextUnchecked, null, (sender, args) =>
        {
            
            EnableAppOnStartUp();
            _startUpToggleBtn.Text = StartUpTextChecked;
        });
        if (IsStartUpEnabled())
        {
            _startUpToggleBtn.Text = StartUpTextChecked;
        }
        _notifyIcon.ContextMenuStrip.Items.Add(_startUpToggleBtn);
    }

    private void DevicesDropDownInit()
    {
        _devicesDropDownMenu = new Forms.ToolStripDropDownButton("Devices");
        _notifyIcon.ContextMenuStrip.Items.Add(_devicesDropDownMenu);
    }
    
    private void RefreshBtnInit()
    {
        _refreshBtn = new Forms.ToolStripButton("Refresh", null, (sender, args) =>
        {
            RefreshDevices();
        });
        _notifyIcon.ContextMenuStrip.Items.Add(_refreshBtn);
    }
    
    
    
    private Forms.ToolStripDropDownItem ToDropDownItem(string deviceName)
    {
        return new Forms.ToolStripMenuItem(deviceName, null, (sender, args) =>
        {
            _selectedQuickConnectDevice = deviceName;
            _bluetoothHandler.ConnectToDevice(_selectedQuickConnectDevice);
        });
    }
    
    private void RefreshDevices()
    {
        _bluetoothHandler.FetchBluetoothPairedDevices();
        _pairedDevices = _bluetoothHandler.GetDeviceNames().ToList();

        foreach (string device in _pairedDevices)
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

    private bool IsStartUpEnabled()
    {
        return File.Exists(_shortcutPath);
    }
    
    private void OnExit(object sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _notifyIcon.Dispose();
    }
    
}