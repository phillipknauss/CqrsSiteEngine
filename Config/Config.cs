using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Configuration;

namespace Config
{
    public static class Config
    {
        public static string DefaultPath
        {
            get
            {
                return "d:\\store\\settings.xml";
            }
        }

        private static IDictionary<string,string> _settings;

        public static void Init(IDictionary<string, string> settings)
        {
            _settings = settings;
        }

        public static void InitFromXml(string xml)
        {
            _settings = new Dictionary<string, string>();

            var root = XElement.Parse(xml);

            foreach (var elem in root.Elements())
            {
                _settings.Add(elem.Attribute("key").Value, elem.Attribute("value").Value);
            }
        }

        public static void LoadFromXml(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                string data = sr.ReadToEnd();
                InitFromXml(data);
            }
        }

        public static string Get(string name)
        {
            if (_settings.ContainsKey(name))
            {
                return _settings[name];
            }
            return null;
        }
    }
}
