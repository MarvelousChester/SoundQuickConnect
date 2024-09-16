using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using InTheHand.Net.Sockets;

namespace SoundQuickConnect;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Trace.Listeners.Add(new ConsoleTraceListener());
        
        
        var bluetoothClient = new BluetoothClient();
        var bluetoothDevices = bluetoothClient.PairedDevices;
        Dictionary<string, BluetoothDeviceInfo> deviceInfosDict = new Dictionary<string, BluetoothDeviceInfo>();
        foreach (var device in bluetoothDevices)
        {
            deviceInfosDict.Add(device.DeviceName, device);
        }
        bluetoothDevicesListBox.ItemsSource = deviceInfosDict.Keys;
    }
    
    


}