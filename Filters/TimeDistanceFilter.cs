using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPS_Application;

namespace Filters
{
    /// <summary>
    /// Filters based off time and distance.  New data that has not passed the minimum distance since that last data is discarded.
    /// New data that has exceeded the maximum time value is not filtered independent of distance.
    /// </summary>
    public class TimeDistanceFilter : Filter
    {
        double minDistanceMiles;
        double maxTimeSeconds;
        GpsTrack filteredTrack;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minDistance">Minimum separation distance between points in miles</param>
        /// <param name="maxTime">Maximum difference in time between filtered points in seconds</param>
        /// <remarks>Assumes data being filtered has been sorted by time</remarks>
        public TimeDistanceFilter(GpsTrack track, double minDistance, double maxTime)
        {
            this.minDistanceMiles = minDistance;
            this.maxTimeSeconds = maxTime;

            this.filteredTrack = new GpsTrack(track.StartDate, track.StartTime);

            foreach (GpsPoint point in track.Points)
                AddPoint(point);
        }

        private bool AddPoint(GpsPoint point)
        {
            if (point == null)
                return false;

            // first point added by default
            if (filteredTrack.PointCount == 0)
            {
                filteredTrack.AddPoint(point);
                return true;
            }
            // check to see if teh maximum time has elapsed, if so add the point no matter what the distance
            else if (point.Time - filteredTrack.Points.Last().Time > maxTimeSeconds)
            {
                filteredTrack.AddPoint(point);
                return true;
            }
            // finally only add teh point if the min distance is exceeded
            else if (TimeDistanceFilter.Distance(point, filteredTrack.Points.Last()) > minDistanceMiles)
            {
                filteredTrack.AddPoint(point);
                return true;
            }
            
            return false;
        }

        public void SaveTrack(string path)
        {
            filteredTrack.SaveToFile(path);
        }

        /// <summary>
        /// Distance, in miles, between two latitude, longitude points on the Earth using the haversine formula
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns>Distance in miles</returns>
        public static double Distance(GpsPoint point1, GpsPoint point2)
        {
            double deg2rad = Math.PI / 180.0;
            double radius = 3959;  //average Earth radius in miles
            double phi1 = point1.Latitude.Value * deg2rad;
            double phi2 = point2.Latitude.Value * deg2rad;
            double deltaPhi = (point2.Latitude.Value - point1.Latitude.Value) * deg2rad;
            double deltalambda = (point2.Longitude.Value - point1.Longitude.Value) * deg2rad;

            double sinPhiOver2 = Math.Sin(deltaPhi / 2);
            double sinLambdaOver2 = Math.Sin(deltalambda / 2);

            double a = sinPhiOver2 * sinPhiOver2 + Math.Cos(phi1) * Math.Cos(phi2) * sinLambdaOver2 * sinLambdaOver2;
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return radius * c;
        }
    }
}
