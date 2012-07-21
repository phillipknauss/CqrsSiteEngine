using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ReadModel
{
    [ProtoContract]
    public class UserIndexItem 
    {
        [ProtoMember(1)]
        public Guid Id { get; set; }
        [ProtoMember(2)]
        public string Username { get; set; }
        [ProtoMember(4)]
        public DateTime TimeStamp { get; set; }

        [ProtoMember(5)]
        public IDictionary<string,UserProperty> Properties { get; set; }

        [ProtoMember(6)]
        public bool Authenticated { get; set; }

        [ProtoMember(7)]
        public List<string> Roles { get; set; }

        public string SerializeRoles()
        {
            if (Roles == null || !Roles.Any())
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();

            foreach (var role in Roles)
            {
                sb.Append(role);
                if (role != Roles.Last())
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        #region Property Pivot
        
        // Due to limitations in Asp.NET MVC's strongly typed views, each property needs to be strongly typed, which isn't really possible with a dictionary,
        // So instead, we pivot the properties we want to display into full properties.

        public string FirstName 
        {
            get
            {
                return GetStringFromProperties("FirstName");
            }

            set
            {
                SetValueOnProperties<string>("FirstName", value, "text", "text");
            }
        }

        public string LastName
        {
            get
            {
                return GetStringFromProperties("LastName");
            }

            set
            {
                SetValueOnProperties<string>("LastName", value, "text", "text");
            }
        }

        public string Email
        {
            get
            {
                return GetStringFromProperties("Email");
            }

            set
            {
                SetValueOnProperties<string>("Email", value, "text", "email");
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (Roles == null)
                {
                    return false;
                }
                return Roles.Contains("Admin");
            }

            set
            {
                if (Roles == null)
                {
                    Roles = new List<string>();
                }
                if (Roles.Contains("Admin"))
                {
                    return;
                }
                Roles.Add("Admin");
            }
        }

        private string GetStringFromProperties(string name)
        {
            if (Properties.ContainsKey(name))
            {
                return Properties[name].Value;
            }
            return null;
        }

        private bool GetBoolFromProperties(string name)
        {
            if (Properties.ContainsKey(name))
            {
                return Properties[name].Value.ToLower() == "true" ? true : false;
            }
            return false;
        }

        private void SetValueOnProperties<T>(string name, T value, string type, string format)
        {
            if (!Properties.ContainsKey(name))
            {
                Properties.Add(name, null);
            }
            Properties[name] = new UserProperty()
                {
                    Name = name,
                    Value = value != null ? value.ToString() : null,
                    Type = type,
                    Format = format
                };
        }

        #endregion

        public UserIndexItem()
        {
            Properties = new Dictionary<string, UserProperty>();
            Roles = new List<string>();
        }

    }

    [ProtoContract]
    public class UserIndexReadModel : IReadModel
    {
        [ProtoMember(1)]public List<UserIndexItem> Items { get; set; }

        public UserIndexReadModel()
        {
            Items = new List<UserIndexItem>();
        }

        public object Get(string identifier)
        {
            return identifier != "items" ? null : Items;
        }
    }
}
