using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using InTheHand.Net.Sockets;

namespace SoundQuickConnect;

public partial class MainWindow : Window
{
    private BluetoothHandler bluetoothHandler;
    
    public MainWindow()
    {
        InitializeComponent();

        Trace.Listeners.Add(new ConsoleTraceListener());
        bluetoothHandler = new BluetoothHandler();
        RefreshDevices();
        // TODO add a button
        // TODO then add a connect button that will connect to device
        // TODO Add a refresh button

        // REFACTOR 
        // Make into a small icon thing 

        // TODO add start up option
    }

    private void RefreshDevices()
    {
        bluetoothHandler.FetchBluetoothPairedDevices();
        bluetoothDevicesListBox.ItemsSource = bluetoothHandler.GetDeviceNames();
    }

    private void RefreshBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        RefreshDevices();
    }
}