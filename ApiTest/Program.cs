using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ApiTest
{
    class JsonModel
    {
        public string id;
        public string firstName;
        public string lastName;
        public string email;
        public string dob;
        public string favouriteColour;
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var webClient = new System.Net.WebClient())
            {
                var jsonStr = webClient.DownloadString("https://recruitment.highfieldqualifications.com/api/test");
                var jsonList = JsonConvert.DeserializeObject<List<JsonModel>>(jsonStr);
                jsonList = jsonList.OrderBy(o => o.favouriteColour).ToList<JsonModel>();

                var colors = new List<string>();

                foreach (var json in jsonList)
                    colors.Add(json.favouriteColour);

                var colorsfreq = new List<KeyValuePair<string, int>>();

                int count = 1;
                for (int i = 0; i < colors.Count; ++i)
                {
                    if (colors[i] == colors[i + 1])
                    {
                        ++count;
                        if (i + 1 == colors.Count - 1)
                        {
                            colorsfreq.Add(new KeyValuePair<string, int>(colors[i + 1], count));
                            break;
                        }
                    }
                    else
                    {
                        colorsfreq.Add(new KeyValuePair<string, int>(colors[i], count));
                        count = 1;
                    }
                }//

                Console.WriteLine("Colors by name:");

                foreach (var color in colorsfreq)                             // order by names
                    Console.WriteLine($"{color.Key} : {color.Value}");

                colorsfreq = colorsfreq.OrderBy(o => o.Value).ToList<KeyValuePair<string, int>>();

                Console.WriteLine("\nColors by frequency:");

                foreach (var color in colorsfreq)                             // order by frequency
                    Console.WriteLine($"{color.Key} : {color.Value}");


                Console.WriteLine("\nUsers above 20:");
                foreach (var json in jsonList)
                {
                    var date = DateTime.Parse(json.dob);
                    var age = DateTime.Now.Year - date.Year;
                    if (age > 20)
                        Console.WriteLine($"{json.firstName} {json.lastName} : {age}");
                }

            }

        }//
    }
}
