using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_Application
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "c:\\users\\greg\\documents\\visual studio 2013\\Projects\\GPS-Application\\GPS-Application\\Reference Docs\\GPS_20130430_030350.log";

            GpsLogParser test = new GpsLogParser(fileName);
            test.ReadGpsLog();
        }

        
    }
}
