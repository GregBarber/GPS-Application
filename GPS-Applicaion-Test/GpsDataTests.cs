using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GPS_Application;

namespace GPS_Applicaion_Test
{
    [TestClass]
    public class GpsDataTests
    {

        [TestMethod]
        public void TestGprmcData()
        {
            string test = "$GPRMC,030350.222,A,2741.2288,N,08643.9068,E,0.85,15.12,300413,,,A*58";
            GprmcData data = new GprmcData(test.Split('*')[0].Split(','));

            string output = data.ToString();
            Assert.AreEqual(test, output);
        }

        [TestMethod]
        public void TestGpggaData()
        {
            string test = "$GPGGA,030515.222,2741.2424,N,08643.9129,E,2,03,8.9,2846.6,M,-37.4,M,3.1,0000*68";
            GpggaData data = new GpggaData(test.Split('*')[0].Split(','));

            string output = data.ToString();
            Assert.AreEqual(test, output);
        }

        [TestMethod]
        public void TestGpgsaData()
        {
            string test = "$GPGSA,A,2,22,11,14,,,,,,,,,,8.9,8.9,1.0*37";
            GpgsaData data = new GpgsaData(test.Split('*')[0].Split(','));

            string output = data.ToString();
            Assert.AreEqual(test, output);
        }

        [TestMethod]
        public void TestGpgsvData()
        {
            string test = "$GPGSV,3,3,12,19,07,263,,24,04,038,,01,03,323,,06,01,226,*7A";
            GpgsvData data = new GpgsvData(test.Split('*')[0].Split(','));

            string output = data.ToString();
            Assert.AreEqual(test, output);
        }

        [TestMethod]
        public void TestGpvtgData()
        {
            string test = "$GPVTG,276.02,T,,M,141.56,N,262.2,K,A*0F";
            GpvtgData data = new GpvtgData(test.Split('*')[0].Split(','));

            string output = data.ToString();
            Assert.AreEqual(test, output);
        }


        [TestMethod]
        public void TestGpsData()
        {
            int lines = 0;
            string fileName = "c:\\users\\greg\\documents\\visual studio 2013\\Projects\\GPS-Application\\GPS-Application\\Reference Docs\\GPS_20130430_030350mod.log";

            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = String.Empty;
                DateTime currentTime = new DateTime();  // Note, can only use the time, not date, because date is not included in GPGGA values.  Assume data point is the same if time is the same, independent of date
                GpsData data;

                while ((s = sr.ReadLine()) != null)
                {
                    lines++;
                    data = GpsLogParser.ParseLine(s);

                    if (data != null)
                        Assert.AreEqual(s, data.ToString());
                }

                Assert.AreEqual(7072, lines);
            }
        }
    }
}
