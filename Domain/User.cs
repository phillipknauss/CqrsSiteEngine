using System;
using System.Collections.Generic;
using Events;
using Ncqrs.Domain;

namespace Domain
{
    public class User : AggregateRootMappedByConvention 
    {
        private string _name;
        private string _password;

        private IDictionary<string,UserProperty> _properties;

        private UserState _userState;

        public User() { }

        public User(string name)
        {
            var e = new UserCreatedEvent
            {
                Name = name,
                TimeStamp = DateTime.UtcNow
            };

            ApplyEvent(e);
        }

        public void Delete(Guid userID)
        {
            var e = new UserDeletedEvent(Guid.NewGuid(), Guid.Empty, Version+1, DateTime.UtcNow)
            {
                UserID = userID,
                TimeStamp = DateTime.UtcNow
            };

            ApplyEvent(e);
        }

        public void SetProperty(Guid userID, string name, string value)
        {
            var e = new UserPropertySetEvent(Guid.NewGuid(), Guid.Empty, this.Version + 1, DateTime.UtcNow)
            {
                UserID = userID,
                Name = name,
                Value = value,
                TimeStamp = DateTime.UtcNow
            };

            ApplyEvent(e);
        }

        public void SetPassword(Guid userID, string password)
        {
            var e = new UserPasswordSetEvent(Guid.NewGuid(), Guid.Empty, Version + 1, DateTime.UtcNow)
            {
                UserID = userID,
                Password = password,
                TimeStamp = DateTime.UtcNow
            };

            ApplyEvent(e);
        }

        protected void OnUserCreated(UserCreatedEvent e)
        {
            _name = e.Name;
        }

        protected void OnUserDeleted(UserDeletedEvent e)
        {
            _userState = UserState.Deleted;
        }

        protected void OnUserPropertySet(UserPropertySetEvent e)
        {
            if (_properties == null)
            {
                _properties = new Dictionary<string, UserProperty>();
            }

            if (!_properties.ContainsKey(e.Name))
            {
                _properties.Add(e.Name, new UserProperty
                                            {
                                                Name = e.Name,
                                                Value = e.Value,
                                                Type = e.Type,
                                                Format = e.Format
                                            });
            }

            _properties[e.Name] = new UserProperty
                                      {
                                          Name = e.Name,
                                          Value = e.Value,
                                          Type = e.Type,
                                          Format = e.Format
                                      };
        }

        protected void OnUserPasswordSet(UserPasswordSetEvent e)
        {
            _password = e.Password;
        }

    }

    public enum UserState
    {
        Normal=0,
        Deleted=1,
        Authenticated=2
    }
}
