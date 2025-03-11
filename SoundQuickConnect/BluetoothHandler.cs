using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
namespace SoundQuickConnect;

public class BluetoothHandler
{
    private readonly BluetoothClient _bluetoothClient; // Point of ReadOnly
                                                       // indicates that assignment to the field can only occur as
                                                       // part of the declaration or in a constructor in the same class
                                                       // more you know!
                                                       // The point is for Immutability once initialised is the only reason for use.
                                                       // As a developer it also demonstrates intent
    private Dictionary<string, BluetoothDeviceInfo> _bluetoothDevicesDict = new Dictionary<string, BluetoothDeviceInfo>();

    public BluetoothHandler()
    {
        _bluetoothClient = new BluetoothClient();
        FetchBluetoothPairedDevices();
    }
    
    public void FetchBluetoothPairedDevices()
    {
        var bluetoothDevices = _bluetoothClient.PairedDevices;
        _bluetoothDevicesDict = new Dictionary<string, BluetoothDeviceInfo>();
        foreach (var device in bluetoothDevices)
        {
            _bluetoothDevicesDict.Add(device.DeviceName, device);
        }
    }
    
    public ICollection<string> GetDeviceNames()
    {
        return _bluetoothDevicesDict.Keys;
    }
    
    /// <summary>
    /// Connects to device
    /// </summary>
    /// <param name="deviceName"></param>
    /// <returns></returns>
    public bool ConnectToDevice(string deviceName)
    {
        if (!_bluetoothDevicesDict.TryGetValue(deviceName, out var device) || !IsDeviceConnected(device))
        {
            return false;
        }
        
        _bluetoothClient.Connect(device.DeviceAddress, BluetoothService.SerialPort);
        return true;
    }

    /// <summary>
    ///  Additional safety check to ensure always retrieving latest device information when check if connected
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    public bool IsDeviceConnected(BluetoothDeviceInfo device)
    {
        device.Refresh();
        return device.Connected;
    }
}