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


        BluetoothHandler bluetoothHandler = new BluetoothHandler();
        
        bluetoothDevicesListBox.ItemsSource = bluetoothHandler.GetDeviceNames();
        
        
        // TODO add a button
        // TODO then add a connect button that will connect to device
        // TODO Add a refresh button
        
        // REFACTOR 
        // Make into a small icon thing 

        // TODO add start up option
    }
    
    


}