using System;
using System.Collections.Generic;
using System.Text;

namespace Tzedaka
{

    public static class Settings
    {
        private const string _url = "https://tzedakamobile.pw/";
        public static string Url
        {
            get
            {
                return _url;
            }
        }
    }
}
