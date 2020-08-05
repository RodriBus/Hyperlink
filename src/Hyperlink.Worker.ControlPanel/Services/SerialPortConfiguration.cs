using System.IO.Ports;

namespace Hyperlink.Worker.ControlPanel.Services
{
    public class SerialPortConfiguration
    {
        public string Port { get; set; }
        public int Baudrate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }

        public SerialPort ToSerialPort() => new SerialPort(Port, Baudrate, Parity, DataBits, StopBits);
    }
}