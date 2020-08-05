using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace Hyperlink.Core.Streaming
{
    public class EliteStream : IEliteStream, IDisposable
    {
        private Subject<EliteMessage> Subject { get; }
        private IList<IDisposable> Subscribers { get; }
        private ILogger<EliteStream> Logger { get; }

        public EliteStream(ILogger<EliteStream> logger)
        {
            Logger = logger;
            Subject = new Subject<EliteMessage>();
            Subscribers = new List<IDisposable>();

            Subscribe(m => Logger.LogInformation("Hardpoint: {Hardpoint}", m.Hardpoint));
        }

        public void Publish(EliteMessage message)
        {
            Subject.OnNext(message);
        }

        public void Subscribe(Action<EliteMessage> action)
        {
            Subscribers.Add(Subject.Subscribe(action));
        }

        public void Dispose()
        {
            Subject?.Dispose();

            foreach (var subscriber in Subscribers)
            {
                subscriber.Dispose();
            }
        }
    }
}