using System;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;

namespace Hyperlink.Worker.ControlPanel.Services
{
    public class ControlPanelDeviceLocator : IControlPanelDeviceLocator
    {
        private const string DeviceName = "Arduino Uno";

        public SerialPortConfiguration LocateDevicePort()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new NotSupportedException($"OS '{RuntimeInformation.OSDescription}' not supported.");

            using var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort");
            string[] portnames = SerialPort.GetPortNames();
            var ports = searcher.Get()
                .Cast<ManagementBaseObject>()
                .Select(o => new SerialPortInfo(o))
                .ToList();

            var devicePort = ports.Find(p => p.Description == DeviceName);

            if (devicePort is null)
                throw new Exception($"Device '{DeviceName}' not found.");

            return new SerialPortConfiguration
            {
                Port = devicePort.DeviceID,
                Baudrate = 9600,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
            };
        }
    }
}