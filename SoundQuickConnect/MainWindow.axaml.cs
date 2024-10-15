using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using InTheHand.Net.Sockets;

namespace SoundQuickConnect;



public partial class MainWindow : Window
{

    
    private BluetoothHandler bluetoothHandler;
    private ICollection<string> pairedDevices = new List<string>();

    private string selectedQuickConnectDevice; 
    
    public MainWindow()
    {
        InitializeComponent();
        Trace.Listeners.Add(new ConsoleTraceListener());
        bluetoothHandler = new BluetoothHandler();
        RefreshDevices();
    }

    private void RefreshDevices()
    {
        bluetoothHandler.FetchBluetoothPairedDevices();
        pairedDevices = bluetoothHandler.GetDeviceNames().ToList();
        bluetoothDevicesListBox.ItemsSource = pairedDevices;
    }
    
    private void RefreshBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        RefreshDevices();
    }
    
    private void BluetoothDevicesListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        string selectedDevice = bluetoothDevicesListBox.SelectedItem.ToString();
        SelectedQuickConnectDevice.Text = selectedDevice;
        selectedQuickConnectDevice = selectedDevice;
    }


    private void ConnectBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        // TODO, make sure to refresh the C# devices so that we can latest info and also verify not connected already before
        // triggering this, actually do that in the bluetoothHandler, this file is mainly for the UI section
        if (selectedQuickConnectDevice == "")
        {
            // TODO
            // Error, no device selected
        }
        bluetoothHandler.ConnectToDevice(selectedQuickConnectDevice);
    }
}