using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GPS_Application;
using Filters;

namespace FilterGpsTest
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestKalmanImport()
        {
            GpsLogParser p = new GpsLogParser("c:\\users\\greg\\documents\\visual studio 2013\\Projects\\GPS-Application\\GPS-Application\\Reference Docs\\ShortGPS.txt");
            GpsTrack t = p.ReadGpsLog();

            KalmanImport.Kalman1D kLat = new KalmanImport.Kalman1D();
            kLat.Reset(0.1, 0.1, 0.1, 400, 0);
            KalmanImport.Kalman1D kLon = new KalmanImport.Kalman1D();
            kLon.Reset(0.1, 0.1, 0.1, 400, 0);

            int points = t.PointCount;
            // Assume we get to see every other measurement we calculated, and use
            // the others as the points to compare for estimates.
            // Run the filter, note our time unit is 1.
            double[] kalmanLat = new double[points];
            double[] velLat = new double[points];
            double[] kGainLat = new double[points];

            double[] kalmanLon = new double[points];
            double[] velLon = new double[points];
            double[] kGainLon = new double[points];


            double lastTime = 0;
            for (int i = 0; i < points; i++)
            {
                if (i == 0)
                {
                    lastTime = t[i].Time;
                    kalmanLat[0] = t[i].Latitude.Value;
                    velLat[0] = kLat.Velocity;
                    kGainLat[0] = kLat.LastGain;
                    kalmanLat[1] = kLat.Predicition(1);
                    velLat[1] = kLat.Velocity;
                    kGainLat[1] = kLat.LastGain;

                    kalmanLon[0] = t[i].Longitude.Value;
                    velLon[0] = kLon.Velocity;
                    kGainLon[0] = kLon.LastGain;
                    kalmanLon[1] = kLon.Predicition(1);
                    velLon[1] = kLon.Velocity;
                    kGainLon[1] = kLon.LastGain;
                }
                else
                {
                    kalmanLat[i] = kLat.Update(t[i].Latitude.Value, t[i].Time - lastTime);
                    velLat[i] = kLat.Velocity;
                    kGainLat[i] = kLat.LastGain;

                    kalmanLon[i] = kLon.Update(t[i].Longitude.Value, t[i].Time - lastTime);
                    velLon[i] = kLon.Velocity;
                    kGainLon[i] = kLon.LastGain;

                    lastTime = t[i].Time;
                }


            }

            double[] deltaLat = new double[points];
            double[] deltaLon = new double[points];
            for (int j = 0; j < points; j++)
            {
                deltaLat[j] = kalmanLat[j] - t.Points[j].Latitude.Value;
                deltaLon[j] = kalmanLon[j] - t.Points[j].Longitude.Value;
            }



            System.IO.StreamWriter w = new System.IO.StreamWriter("c:\\users\\greg\\documents\\visual studio 2013\\Projects\\GPS-Application\\GPS-Application\\Reference Docs\\ShortGpsMinKalman.log");

            for (int i = 0; i < t.PointCount; i++ )
                foreach (GpsData d in t.Points[i].Data)
                    if (typeof(GprmcData).IsAssignableFrom(d.GetType()))
                    {
                        string[] s = d.ToString().Split(',');

                        string[] tmp = kalmanLat[i].ToString("###0.0###").Split('.');
                        string latDeg = double.Parse(tmp[0]).ToString("00");
                        double latFraction = double.Parse("." + tmp[1]) * 60;

                        s[3] = latDeg + latFraction.ToString("00.0000");

                        tmp = kalmanLon[i].ToString("####0.0###").Split('.');
                        string lonDeg = double.Parse(tmp[0]).ToString("000");
                        double lonFraction = double.Parse("." + tmp[1]) * 60;

                        s[5] = lonDeg + lonFraction.ToString("00.0000"); 
                        

                        GprmcData data = new GprmcData(s);
                        w.WriteLine(data.ToString());
                    }

            w.Close();
        }
   
    /// <summary>
    /// Test distance calculations with results obtained from online calculator.
    /// </summary>
        [TestMethod]
        public void TestDistance()
        {
            GpsPoint p1 = new GpsPoint(0, 87, 15);
            GpsPoint p2 = new GpsPoint(0, 32, -67);
            double distance = TimeDistanceFilter.Distance(p1, p2);

            Assert.AreEqual(6408*0.621371, distance, 0.5);


            p1 = new GpsPoint(0, 50.851, 5.985);
            p2 = new GpsPoint(0, 58.148, 3.08);
            double distance2 = TimeDistanceFilter.Distance(p1, p2);

            Assert.AreEqual(832.6 * 0.621371, distance2, 0.5);
        }
    }
}
