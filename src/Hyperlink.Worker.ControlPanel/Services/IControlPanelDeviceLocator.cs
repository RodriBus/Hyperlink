namespace Hyperlink.Worker.ControlPanel.Services
{
    public interface IControlPanelDeviceLocator
    {
        SerialPortConfiguration LocateDevicePort();
    }
}