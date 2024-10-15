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
    class BluetoothDevice
    {
        public string Name { get; set; }

        public BluetoothDevice()
        {
            
        }
    }
    
    private BluetoothHandler bluetoothHandler;
    private ICollection<string> pairedDevices = new List<string>();
    private ObservableCollection<string> observableCollectionPairedDevices;
    private ObservableCollection<string> QuickConnectDevices = new ObservableCollection<string>();
    
    public class MainWindowViewModel
    {
        public string Greeting => "Welcome to Avalonia!";
    }
    
    public MainWindow()
    {
        InitializeComponent();

        Trace.Listeners.Add(new ConsoleTraceListener());
        bluetoothHandler = new BluetoothHandler();
        RefreshDevices();
        quickConnectListBox.ItemsSource = QuickConnectDevices;
        observableCollectionPairedDevices = new ObservableCollection<string>(pairedDevices);
        bluetoothDevicesListBox.ItemsSource = observableCollectionPairedDevices;

    }

    private void RefreshDevices()
    {
        bluetoothHandler.FetchBluetoothPairedDevices();
        pairedDevices = bluetoothHandler.GetDeviceNames();
    }

    private void AddToQuickConnectBox(string deviceName)
    {
        quickConnectListBox.Items.Add(deviceName);
    }

    private void RemoveDeviceFromPairedOptions(string deviceName)
    {
        bluetoothDevicesListBox.Items.Remove(deviceName);
    }
    
    
    private void RefreshBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        RefreshDevices();
    }


    private void AddQuickMenuBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        string selectedDevice = bluetoothDevicesListBox.SelectedItem.ToString();
        AddToQuickConnectBox(selectedDevice);
        RemoveDeviceFromPairedOptions(selectedDevice);
    }

    private void BluetoothDevicesListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        string selectedDevice = bluetoothDevicesListBox.SelectedItem.ToString();
        QuickConnectDevices.Add(selectedDevice);
        quickConnectListBox.ItemsSource = QuickConnectDevices;
    }
}