using System;

namespace Hyperlink.Core.Streaming
{
    public interface IEliteStream
    {
        void Publish(EliteMessage message);

        void Subscribe(Action<EliteMessage> action);
    }
}