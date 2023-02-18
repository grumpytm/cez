using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace cez
{
    /* general settings */
    public static class GlobalVar
    {
        public const string serverIP = "10.0.0.1";
        public const string baseUrl = "http://10.0.0.1/menu/api/";
        
        /* live feed */
        // public const string baseUrl = "http://10.0.0.1/menu/api/scan/check/";

        //public const string serverIP = "192.168.1.249";
        //public const string baseUrl = "http://192.168.2.249/menu/api/";
    }
    
    /* barcode stuff */
    public static class BarcodeVar
    {
        public const int clientTimeout = 1000;
        public const int requestTimeout = 1000;
        public const string check = "?barcode={barcode}";
        public const string orders = "orders.json";

        /* Live feed */
        // public const string check = "{barcode}";
    }
}