using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Forms = System.Windows.Forms;

namespace SoundQuickConnect;

public partial class App : Application
{

    private Forms.NotifyIcon _notifyIcon;
    private BluetoothHandler bluetoothHandler;
    private ICollection<string> pairedDevices = new List<string>();

    private string selectedQuickConnectDevice;
    private Forms.ToolStripDropDownButton devicesDropDownMenu;
    private Forms.ToolStripButton refreshBtn;
    private Forms.ToolStripMenuItem startUpToggleBtn;
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
        bluetoothHandler = new BluetoothHandler();
        _notifyIcon = new Forms.NotifyIcon();
        _notifyIcon.Icon = new System.Drawing.Icon("Assets/bluetooth-32.ico");
        _notifyIcon.Visible = true;
        _notifyIcon.Text = "SoundQuickConnect";
        _notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
       
        DevicesDropDownInit();
        RefreshDevices();
        RefreshBtnInit();
        
        // TODO Add a check, if shortcut already created, means that startup was enabled, however, if not that means disabled.
        startUpToggleBtn  = new Forms.ToolStripMenuItem("Set On StartUp", null, (sender, args) =>
        {
            EnableAppOnStartUp();
        });
        _notifyIcon.ContextMenuStrip.Items.Add(startUpToggleBtn);

    }

    private void DevicesDropDownInit()
    {
        devicesDropDownMenu = new Forms.ToolStripDropDownButton("Devices");
        _notifyIcon.ContextMenuStrip.Items.Add(devicesDropDownMenu);
    }
    
    private void RefreshBtnInit()
    {
        refreshBtn = new Forms.ToolStripButton("Refresh", null, (sender, args) =>
        {
            RefreshDevices();
        });
        _notifyIcon.ContextMenuStrip.Items.Add(refreshBtn);
    }
    
    
    
    private Forms.ToolStripDropDownItem ToDropDownItem(string deviceName)
    {
        return new Forms.ToolStripMenuItem(deviceName, null, (sender, args) =>
        {
            selectedQuickConnectDevice = deviceName;
            bluetoothHandler.ConnectToDevice(selectedQuickConnectDevice);
        });
    }
    
    private void RefreshDevices()
    {
        bluetoothHandler.FetchBluetoothPairedDevices();
        pairedDevices = bluetoothHandler.GetDeviceNames().ToList();

        foreach (string device in pairedDevices)
        {
            devicesDropDownMenu.DropDownItems.Add(ToDropDownItem(device));
        }
    }

    private void EnableAppOnStartUp()
    {
        string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\SoundQuickConnect.lnk";
        string exePath = System.Environment.ProcessPath; 
        var dir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        var createShortCutCMD =
            $"powershell \"$s=(New-Object -ComObject WScript.Shell).CreateShortcut('{shortcutPath}'); $s.TargetPath ='{exePath}'; $s.Save()\"";
        
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = $"/C {createShortCutCMD}";
        process.StartInfo = startInfo;
        process.Start();
    }
    
    private void AppShortcutToDesktop(string linkURL)
    {
        string startUpDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        using (StreamWriter writer = new StreamWriter(startUpDir + "\\" + linkURL + ".url"))
        {
            writer.WriteLine("[InternetShortcut]");
            writer.WriteLine("URL=" + linkURL);
        }
    }
    private void OnExit(object sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _notifyIcon.Dispose();
    }
    

    

    
    
}