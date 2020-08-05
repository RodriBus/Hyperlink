using System;
using System.IO.Ports;

namespace Hyperlink.Worker.ControlPanel.Services
{
    public class SerialService : ISerialService
    {
        private IControlPanelDeviceLocator Locator { get; }
        private SerialPort Port { get; set; }

        public event SerialDataReceivedEventHandler OnDataReceived
        {
            add { Port.DataReceived += value; }
            remove { Port.DataReceived -= value; }
        }

        public SerialService(IControlPanelDeviceLocator locator)
        {
            Locator = locator;
            Configure();
        }

        public ISerialService Configure()
        {
            var portConfiguration = Locator.LocateDevicePort();
            Port = portConfiguration.ToSerialPort();
            Port.Handshake = Handshake.None;
            return this;
        }

        public ISerialService OnProcessData(Action<string> action)
        {
            Port.DataReceived += (object sender, SerialDataReceivedEventArgs e) => action(Port.ReadLine());
            return this;
        }

        public void Write(string text)
        {
            Port.WriteLine(text);
        }

        public ISerialService Open()
        {
            Port.Open();
            Port.ReadExisting();
            return this;
        }

        public void Close()
        {
            Port.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}