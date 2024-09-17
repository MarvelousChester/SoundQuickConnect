using System.Collections.Generic;
using Avalonia.Controls;
using InTheHand.Net.Sockets;
namespace SoundQuickConnect;

public class BluetoothHandler
{
    private BluetoothClient bluetoothClient;
    private Dictionary<string, BluetoothDeviceInfo> deviceInfosDict = new Dictionary<string, BluetoothDeviceInfo>();

    public BluetoothHandler()
    {
        bluetoothClient = new BluetoothClient();
        FetchBluetoothPairedDevices();
    }
    
    private void FetchBluetoothPairedDevices()
    {
        var bluetoothDevices = bluetoothClient.PairedDevices;
        deviceInfosDict = new Dictionary<string, BluetoothDeviceInfo>();
        foreach (var device in bluetoothDevices)
        {
            deviceInfosDict.Add(device.DeviceName, device);
        }
    }
    
    public ICollection<string> GetDeviceNames()
    {
        return deviceInfosDict.Keys;
    }
    
    //TODO Create a method to refresh dictionary/Devices
    
}