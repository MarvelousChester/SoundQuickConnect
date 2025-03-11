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
    private BluetoothHandler bluetoothHandler;
    private ICollection<string> pairedDevices = new List<string>();

    private string selectedQuickConnectDevice;
    private Forms.ToolStripDropDownButton devicesDropDownMenu;
    private Forms.ToolStripButton refreshButton;
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
    }

    private void DevicesDropDownInit()
    {
        devicesDropDownMenu = new Forms.ToolStripDropDownButton("Devices");
        _notifyIcon.ContextMenuStrip.Items.Add(devicesDropDownMenu);
    }
    
    private void RefreshBtnInit()
    {
        refreshButton = new Forms.ToolStripButton("Refresh", null, (sender, args) =>
        {
            RefreshDevices();
        });
        _notifyIcon.ContextMenuStrip.Items.Add(refreshButton);
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
    
    private void OnExit(object sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _notifyIcon.Dispose();
    }
    

    

    
    
}