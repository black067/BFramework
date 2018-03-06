using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFramework.ShootingGame
{
    class Sensor
    {
        public class Information
        {
            private string[] _keys;
            public Information(Dictionary<string, object> dictionary)
            {
                _information = dictionary;
            }

            private Dictionary<string, object> _information;

            public string[] Keys { get => _keys; private set => _keys = value; }

            public object this[string key]
            {
                get
                {
                    if (!_information.ContainsKey(key))
                    {
                        return default(object);
                    }
                    return _information[key];
                }

                private set
                {
                    if (!_information.ContainsKey(key))
                    {
                        _information.Add(key, value);
                    }
                    else
                    {
                        _information[key] = value;
                    }
                }
            }
        }

        private Information _massage;

        internal Information Massage { get => _massage; private set => _massage = value; }

        public Sensor(Information information)
        {
            Massage = information;
        }

        public void Scan()
        {
            //...
            //在此处添加扫描行为
            //...
        }
    }
}
