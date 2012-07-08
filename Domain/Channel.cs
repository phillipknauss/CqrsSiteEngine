using System;
using Ncqrs.Domain;
using Events;

namespace Domain
{
    public class Channel : AggregateRootMappedByConvention 
    {
        private Guid _id;
        private string _name;
        private DateTime _timestamp;
        
        public Channel() { }

        public Channel(Guid id, string name)
        {
            var e = new ChannelCreatedEvent
            {
                ID = id,
                Name = name,
                TimeStamp = DateTime.UtcNow
            };

            ApplyEvent(e);
        }
        
        protected void OnChannelCreated(ChannelCreatedEvent e)
        {
            _id = e.ID;
            _name = e.Name;
            _timestamp = e.TimeStamp;
        }

    }

}
