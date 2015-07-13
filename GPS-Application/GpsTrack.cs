using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_Application
{
    /// <summary>
    /// Contains all the GPS points logged during an entire session or .log file.  
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class GpsTrack
    {
        private List<GpsPoint> points;
        private double startTime = double.MaxValue;
        private double endTime = double.MinValue;
        private int startDate = int.MaxValue;
        private int endDate = int.MinValue;
        private double minLatitude = double.MaxValue;
        private double minLongitude = double.MaxValue;
        private double maxLatitude = double.MinValue;
        private double maxLongitude = double.MinValue;

        public GpsTrack(GpsDataTimeLocation data)
        {
            VerifyInitialInput(data);
            this.points = new List<GpsPoint>();
        }

        public GpsTrack(int startDate, double startTime)
        {
            this.startDate = startDate;
            this.startTime = startTime;
            this.points = new List<GpsPoint>();
        }

        /// <summary>
        /// Verifies the data being added and adjusts the start and/or end end times for the entire track accordingly 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool VerifyInitialInput(GpsDataTimeLocation data)
        {
            if (data.Time == 0)
                throw new Exception("Bogus data to GpsPoint./n" + data.ToString());

            if (data.Time < this.startTime)
                this.startTime = data.Time;
            if (data.Time > this.endTime)
                this.endTime = data.Time;
            if (typeof(GpsDataDate).IsAssignableFrom(data.GetType()))
            {
                GpsDataDate d = (GpsDataDate)data;
                if (d.Date < this.startDate)
                    this.startDate = d.Date;
                if (d.Date > this.endDate)
                    this.endDate = d.Date;
            }
            return true;
        }

        /// <summary>
        /// Adjust parameters specific to this track when new data is entered.  Adjusts values for time span and location
        /// </summary>
        /// <param name="data">New data being added</param>
        private void AdjustForInput(GpsPoint data)
        {
            if (data.Time != 0)
            {
                if (data.Time < this.startTime)
                    this.startTime = data.Time;
                if (data.Time > this.endTime)
                    this.endTime = data.Time;
            }

            if (data.Date != 0)
            {
                if (data.Date < this.startDate)
                    this.startDate = data.Date;
                if (data.Date > this.endDate)
                    this.endDate = data.Date;
            }

            if (data.Latitude.Value > maxLatitude)
                maxLatitude = data.Latitude.Value;
            else if (data.Latitude.Value < minLatitude)
                minLatitude = data.Latitude.Value;

            if (data.Longitude.Value > maxLongitude)
                maxLongitude = data.Longitude.Value;
            else if (data.Longitude.Value < minLongitude)
                minLongitude = data.Longitude.Value;
        }

        public void AddPoint(GpsPoint point)
        {
            AdjustForInput(point);
            points.Add(point); 
        }

        public void SaveToFile(string path)
        {
            StringBuilder sb = new StringBuilder();

            foreach (GpsPoint point in points)
                point.ToString(sb);
            
            File.WriteAllText(path, sb.ToString());
        }

        #region Properties
        public GpsPoint this[int index]
        { get { return points[index]; } }

        public double StartTime
        { get { return this.startTime; } }

        public double EndTime
        { get { return this.endTime; } }

        public int StartDate
        { get { return this.startDate; } }

        public int EndDate
        { get { return this.endDate; } }

        public int PointCount
        { get { return this.points.Count; } }

        public List<GpsPoint> Points
        { get { return this.points; } }

        public double MinLatitude
        { get { return this.minLatitude; } }

        public double MaxLatitude
        { get { return this.maxLatitude; } }

        public double MinLongitude
        { get { return this.minLongitude; } }

        public double MaxLongitude
        { get { return this.maxLongitude; } }

        #region Formatted Properties
        public string StartPretty
        { get { return DateTime.ParseExact(startDate.ToString() + " " + startTime.ToString("000000"), "ddMMyy hhmmss", System.Globalization.CultureInfo.InvariantCulture).ToString(); } }

        public string EndPretty
        { get { return DateTime.ParseExact(endDate.ToString() + " " + endTime.ToString("000000"), "ddMMyy hhmmss", System.Globalization.CultureInfo.InvariantCulture).ToString(); } }

        public string MinLatitudePretty
        { get { return this.minLatitude.ToString("0.000"); } }

        public string MaxLatitudePretty
        { get { return this.maxLatitude.ToString("0.000"); } }

        public string MinLongitudePretty
        { get { return this.minLongitude.ToString("0.000"); } }

        public string MaxLongitudePretty
        { get { return this.maxLongitude.ToString("0.000"); } }
        #endregion
        #endregion

    }
}