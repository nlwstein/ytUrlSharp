using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yturlSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Instantiate the class
            var yt = new ytUrl();
            
            //Example request: 
            var TestQuery = yt.GetStreamUrl("https://www.youtube.com/watch?v=lWzdORxIbNk");

            //Output the first streamable URL that has a bitrate higher than 96kbps.
            Console.WriteLine(TestQuery.First(x => x.Key.aBitrate > 96).Value);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
