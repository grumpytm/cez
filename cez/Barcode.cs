using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;

/* Third-party library */
using RestSharp;

namespace cez
{
    class Barcode
    {
        /* Fetch orders */
        public class Orders
        {
            public string Serie { get; set; }
            public string Client { get; set; }
            public string Produs { get; set; }
        }

        /* Product information */
        public class Check
        {
            public bool success { get; set; }
            public string message { get; set; }
            public List<Details> details { get; set; }
        }

        public class Details
        {
            public string barcode { get; set; }
            public string articol { get; set; }
            public string producator { get; set; }
        }

        /* Save */
        public class Save
        {
            public bool success { get; set; }
            public List<Details> details { get; set; }
        }

        /* Parameters */
        public class myParameters
        {
            public List<Item> items { get; set; }
        }

        public class Item
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public bool TimeoutCheck(IRestResponse response)
        {
            if (response.StatusCode == 0)
                return true;
            return false;
        }

        /* Test RestSharp response */
        public static object TestResponse(IRestResponse response)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            /* Connection timedout or error 404 */
            if (response.IsSuccessful.Equals(false))
            {
                if (response.StatusCode == 0)
                {
                    dict.Add("error", "1");
                    dict.Add("message", "Error: Connection timed out.");
                }
                if (response.StatusCode.ToString().Equals("NotFound"))
                {
                    dict.Add("error", "1");
                    dict.Add("message", "Error 404: File not found.");
                }
            }
            
            /* Answer is not in a valid JSON format */
            else if (!response.Content.IsJson())
            {
                dict.Add("error", "1");
                dict.Add("message", "Not a valid JSON answer!");
            }
            else if (response.ErrorException != null)
            {
                    dict.Add("error", "1");
                    dict.Add("message", "Exception: " + response.ErrorMessage);
            }

            if (!dict.ContainsKey("error"))
            {
                dict.Add("error", "0");
                dict.Add("message", "Success");
            }

            return dict;
        }

        /* Fetch json result */
        public static IRestResponse fetchJson(List<Item> items)
        {
            var client = new RestClient();
            // client.BaseUrl = new Uri("http://10.0.0.1/menu/api/");
            client.BaseUrl = new Uri(GlobalVar.baseUrl);
            client.AddDefaultHeader("Content-type", "application/json");
            client.Timeout = BarcodeVar.clientTimeout;
            var request = new RestRequest(items[0].value, Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "text/json"; };
            for (int i = 1; i < items.Count; i++)
                request.AddUrlSegment(items[i].name, items[i].value);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("User-Agent", "RestSharp");
            request.Timeout = BarcodeVar.requestTimeout;
            return client.Execute(request);
        }
    }
}