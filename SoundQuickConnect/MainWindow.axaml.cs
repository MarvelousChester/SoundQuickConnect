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
        
        
    }
    
    


}