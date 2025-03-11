using System.Collections.Generic;
using Avalonia.Controls;
using InTheHand.Net.Bluetooth;
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
    
    public void FetchBluetoothPairedDevices()
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
    

    public bool ConnectToDevice(string deviceName)
    {
        if (!deviceInfosDict.TryGetValue(deviceName, out var device))
        {
            return false;
        }

        device.Refresh();
        if (device.Connected)
        {
            return true;
        }
        bluetoothClient.Connect(device.DeviceAddress, BluetoothService.SerialPort);
        return true;

    }
}