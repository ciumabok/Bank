﻿using Bank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Persistence.SQLServer.EventSourcing
{
    internal record AggregateEvent
    {
        private AggregateEvent() { }

        public required string AggregateId { get; init; }

        public required long AggregateVersion { get; init; }

        public required string EventType { get; init; }

        public required byte[] Data { get; init; }

        public required DateTimeOffset Timestamp { get; init; }

        public static AggregateEvent Create<TKey>(IDomainEvent<TKey> @event, IEventSerializer eventSerializer)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            if (eventSerializer is null)
                throw new ArgumentNullException(nameof(eventSerializer));

            var data = eventSerializer.Serialize(@event);
            var eventType = @event.GetType();

            return new AggregateEvent()
            {
                AggregateId = @event.AggregateId.ToString(),
                AggregateVersion = @event.AggregateVersion,
                EventType = eventType.AssemblyQualifiedName,
                Data = data,
                Timestamp = @event.When
            };
        }
    }
}
