using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
namespace SoundQuickConnect;

public class BluetoothHandler
{
    private BluetoothClient _bluetoothClient;
    private Dictionary<string, BluetoothDeviceInfo> _bluetoothDevicesDict = new Dictionary<string, BluetoothDeviceInfo>();

    public BluetoothHandler()
    {
        _bluetoothClient = new BluetoothClient();
        FetchBluetoothPairedDevices();
    }
    
    
    private void FetchBluetoothPairedDevices()
    {
        var bluetoothDevices = _bluetoothClient.PairedDevices;
        _bluetoothDevicesDict = new Dictionary<string, BluetoothDeviceInfo>();
        foreach (var device in bluetoothDevices)
        {
            _bluetoothDevicesDict.Add(device.DeviceName, device);
        }
    }
    
    public ICollection<string> GetPairedDeviceNames()
    {
        FetchBluetoothPairedDevices();
        return _bluetoothDevicesDict.Keys;
    }
    
    /// <summary>
    /// Connects to device
    /// </summary>
    /// <param name="deviceName"></param>
    /// <returns></returns>
    public bool ConnectToDevice(string deviceName)
    {
        if (!_bluetoothDevicesDict.TryGetValue(deviceName, out var device) || IsDeviceConnected(device))
        {
            return false;
        }
        
        _bluetoothClient.Connect(device.DeviceAddress, BluetoothService.SerialPort);
        return true;
    }

    /// <summary>
    /// TODO METHOD
    /// </summary>
    /// <param name="deviceName"></param>
    public void DisconnectDevice(string deviceName)
    {
        // TODO, this will likely require to switch to another library and do it in different way
        // See following : https://github.com/inthehand/32feet/issues/132
        // For my purposes, I mainly wanted way to quickly connect so I am gonna leave it as be.
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
    
    /// <summary>
    ///  Additional safety check to ensure always retrieving latest device information when check if connected
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    public bool IsDeviceConnected(string deviceName)
    {
        if (!_bluetoothDevicesDict.TryGetValue(deviceName, out var device))
        {
            return false;
        }
        return IsDeviceConnected(device);
    }
    
}