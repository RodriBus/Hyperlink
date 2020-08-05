using System;

namespace Hyperlink.Worker.ControlPanel.Services
{
    public interface ISerialService : IDisposable
    {
        ISerialService Configure();

        ISerialService Open();

        ISerialService OnProcessData(Action<string> action);

        void Write(string text);

        void Close();
    }
}