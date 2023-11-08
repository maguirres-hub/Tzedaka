using System;
using System.Collections.Generic;
using System.Text;

namespace Tzedaka
{

    public static class Settings
    {
        private const string _url = "http://192.168.0.104:8545/";
        public static string Url
        {
            get
            {
                return _url;
            }
        }
    }
}
