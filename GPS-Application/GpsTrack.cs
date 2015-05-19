using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_Application
{
    public class GpsTrack
    {
        List<GpsPoint> points;
        double startTime = double.MaxValue;
        double endTime = double.MinValue;
        int startDate = int.MaxValue;
        int endDate = int.MinValue;
        int pointCount = 0;
        
        public GpsTrack(GpsDataTimeLocation data)
        {
            VerifyInitialInput(data);
            this.points = new List<GpsPoint>();
        }

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

        private void AdjustForInput(GpsPoint data)
        {
            if (data.Time < this.startTime)
                this.startTime = data.Time;
            if (data.Time > this.endTime)
                this.endTime = data.Time;
            if (data.Date < this.startDate)
                this.startDate = data.Date;
            if (data.Date > this.endDate)
                this.endDate = data.Date;
        }

        public void AddPoint(GpsPoint point)
        {
            AdjustForInput(point);
            points.Add(point); 
        }

        #region Properties
        public GpsPoint this[int index]
        { get { return points[index]; } }

        public double StartTime
        { get { return this.startTime; } }

        public double EndTime
        { get { return this.endTime; } }

        public double StartDate
        { get { return this.startDate; } }

        public double EndDate
        { get { return this.endDate; } }
        #endregion

    }





}
