using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_Application
{
    /// <summary>
    /// GPSPoint is a collection of the logged GPS position and time as well as any other generated GPSData at that point
    /// </summary>
    public class GpsPoint
    {
        double time;
        int date;
        Latitude latitude;
        Longitude longitude;
        List<GpsData> gpsData = new List<GpsData>();

        public GpsPoint(double time, double latitude, double longitude)
        {
            this.time = time;
            this.latitude = new Latitude(latitude);
            this.longitude = new Longitude(longitude);
        }

        public GpsPoint(GpsDataTimeLocation data)
        {
            VerifyInitialInput(data);

            this.time = data.Time;
            this.latitude = data.Latitude;
            this.longitude = data.Longitude;

            if (typeof(GpsDataDate).IsAssignableFrom(data.GetType()))
                this.date = ((GpsDataDate)data).Date;
        }

        public void AddData(GpsData data)
        {
            if (VerifyInput(data))
                gpsData.Add(data);
        }

        private bool VerifyInitialInput(GpsDataTimeLocation data)
        {
            if (data.Time == 0)
                throw new Exception("Bogus data to GpsPoint./n" + data.ToString());
            else if (this.time != 0 && data.Time != this.time)
                throw new Exception("Bogus data to GpsPoint./n" + data.ToString());

            return true;
        }

        private bool VerifyInput(GpsData data)
        {
            if (typeof(GpsDataTimeLocation).IsAssignableFrom(data.GetType()))
            {
                GpsDataTimeLocation d1 = (GpsDataTimeLocation)data;
                if (!d1.Latitude.Equals(latitude) || !d1.Longitude.Equals(this.longitude) || d1.Time != this.time)
                    throw new Exception("Bogus data to GpsPoint./n" + data.ToString());

                if (typeof(GpsDataDate).IsAssignableFrom(data.GetType()))
                {
                    GpsDataDate d2 = (GpsDataDate)data;
                    if (this.date != 0 && d2.Date != this.date)
                        throw new Exception("Bogus data for date to GpsPoint./n" + data.ToString());
                }
            }
            return true;
        }

        /// <summary>
        /// Simple override for displaying during debugging
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return date.ToString() + "," + time.ToString();
        }

        /// <summary>
        /// Writes all GPS data contained in the point to the file provided
        /// </summary>
        /// <param name="sb"></param>
        public void ToString(StringBuilder sb)
        {
            foreach (GpsData data in gpsData)
                sb.AppendLine(data.ToString());
        }

        #region Properties
        public GpsData this[int index]
        { get { return gpsData[index]; } }

        public int Date
        { get { return this.date; } }

        public double Time
        { get { return this.time; } }

        public Latitude Latitude
        { get { return this.latitude; } }

        public Longitude Longitude
        { get { return this.longitude; } }

        public List<GpsData> Data
        { get { return this.gpsData; } }
        #endregion
    }
}
