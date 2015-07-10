using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GPS_Application
{   
    
    public class GpsLogParser
    {
        string fileName;
        GpsTrack track;


        public GpsLogParser(string fileName)
        {
            this.fileName = fileName;
        }

        public GpsTrack ReadGpsLog()
        {
            using (StreamReader sr = File.OpenText(fileName))
            {
                GpsPoint point = null;
                GpsData data;
                string s = String.Empty;
                double currentTime = 0;  // Note, can only use the time, not date, because date is not included in GPGGA values.  Assume data point is the same if time is the same, independent of date

                while ((s = sr.ReadLine()) != null)
                {
                    data = ParseLine(s);

                    if (data == null)
                        continue;

                    // if the new data is at a different time than the previous then it is a new data point location
                    if (typeof(GpsDataTimeLocation).IsAssignableFrom(data.GetType()) && ((GpsDataTimeLocation)data).Time != currentTime)
                    {
                        point = new GpsPoint((GpsDataTimeLocation)data);

                        // old point is finished, add to the list and start a new one
                        if (point != null)
                        {
                            if (track == null)
                                track = new GpsTrack((GpsDataTimeLocation)data);

                            track.AddPoint(point);
                        }

                        currentTime = ((GpsDataTimeLocation)data).Time;
                    }
                    point.AddData(data);
                }
                track.AddPoint(point);
            }
            return track;
        }

        public static GpsData ParseLine(string s)
        {
            GpsData data;

            string[] checkSumSplit = s.Split('*');
            if (!Util.VerifyChecksum(checkSumSplit))
                return null;

            string[] gpsDataString = checkSumSplit[0].Split(',');
            switch (gpsDataString[0])
            {
                case "$ADVER":
                    data = null;
                    break;
                case "$GPRMC":
                    data = new GprmcData(gpsDataString);
                    break;
                case "$GPGGA":
                    data = new GpggaData(gpsDataString);
                    break;
                case "$GPGSA":
                    data = new GpgsaData(gpsDataString);
                    break;
                case "$GPGSV":
                    data = new GpgsvData(gpsDataString);
                    break;
                case "$GPVTG":
                    data = new GpvtgData(gpsDataString);
                    break;
                default:
                    data = null;
                    // currently have other weird data in some files, perhaps from the 'flag' button.
                    // TODO: Understand mysterious data
                    //break;
                    throw new Exception("New, unhandled data type encountered in GPS log.\n" + s);
            }
            return data;
        }
    }

    #region Common Utilities
    public static class Util
    {
        public static bool VerifyChecksum(string[] checkSumString)
        {
            if (checkSumString.Length < 2)
                return false;

            //byte givenChecksum;
            //byte.TryParse(checkSumString[1], System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out givenChecksum);
            //givenChecksum = byte.Parse(checkSumString[1]);
            //if (givenChecksum != Util.CalculateCheckSum(checkSumString[0]))
            if (checkSumString[1] != Util.CalculateCheckSum(checkSumString[0]))
            {
                return false;

                //TODO: Perhaps log invalid lines?
                throw new Exception("Discrepency in checksum");
            }

            return true;
        }

        public static string CalculateCheckSum(string source)
        {
            source = source.TrimStart('$');
            byte checksum = 0;
            byte[] bytes = Encoding.ASCII.GetBytes(source);

            for (int i = 0; i < bytes.Length; i++)
                checksum = (byte)(checksum ^ bytes[i]);


            /// TODO: Total hack job
            //return byte.Parse(Convert.ToString(checksum, 16));
            return checksum.ToString("X2");// string.Format("X2", Convert.ToString(checksum, 16));
        }

        public static DateTime DateTimeFromString(string date, string timesStr)
        {
            string[] times = timesStr.Split(new char[] { '.' }, 2);
            string time = times[0];
            string milli = times[1];

            if (date.Length != 6 || time.Length != 6 || milli.Length != 3)
                throw new Exception("Invalid date time length in: " + date + " " + times);

            string day = date[0].ToString() + date[1].ToString();
            string month = date[2].ToString() + date[3].ToString();
            string year = "20" + date[4].ToString() + date[5].ToString();  //Assumes dates are from year 2000 or greater

            string hour = time[0].ToString() + time[1].ToString();
            string min = time[2].ToString() + time[3].ToString();
            string sec = time[4].ToString() + time[5].ToString();

            return new DateTime(Convert.ToInt16(year), Convert.ToInt16(month), Convert.ToInt16(day),
                Convert.ToInt16(hour), Convert.ToInt16(min), Convert.ToInt16(sec), Convert.ToInt16(milli), DateTimeKind.Utc);

        }

        public static double? NullableDouble(string input)
        {
            return string.IsNullOrEmpty(input) ? (double?)null : Convert.ToDouble(input);
        }

        public static int? NullableInt(string input)
        {
            return string.IsNullOrEmpty(input) ? (int?)null : Convert.ToInt32(input);
        }

        public static double DegToRad(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public static void ValueToDegreesMinutes(double value, out double degrees, out double minutes)
        {
            degrees = Math.Floor(value);
            minutes = DegDecimalToMinutes(value - degrees);
        }

        public static double DegDecimalToMinutes(double degreeDecimal)
        {
            return degreeDecimal * 60.0;
        }

        public static double DegMinutesToValue(double degrees, double minutes)
        {
            return degrees + minutes / 60.0;
        }
    }
    
    public struct Latitude
    {
        enum Direction { Unknown, North = 'N', South = 'S' }
        double Degrees;
        double Minutes;
        Direction Hemisphere;


        public Latitude(double value)
        {
            double deg, min;
            Util.ValueToDegreesMinutes(value, out deg, out min);

            this.Degrees = deg;
            this.Minutes = min;

            if (this.Degrees >= 0)
                this.Hemisphere = Direction.North;
            else
                this.Hemisphere = Direction.South;
        }

        public Latitude(double degrees, double minutes) : this(Util.DegMinutesToValue(degrees, minutes))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">Latitude position in degrees</param>
        /// <param name="direction">character representation of North/South hemisphere</param>
        /// <returns>Latitude struct</returns>
        public Latitude(string location, string direction)
        {
            string[] loc = location.Split(new char[] { '.' }, 2);

            Degrees = Convert.ToDouble(loc[0].Substring(0, 2));
            if (Degrees > 90 || Degrees < -90) throw new Exception("Invalid Latitude value: " + Degrees);

            Minutes = Convert.ToDouble(loc[0].Substring(2, loc[0].Length - 2)) + Convert.ToDouble(loc[1]) / Math.Pow(10, loc[1].Length);
            if (Minutes < 0 || Minutes > 60) throw new Exception("Latitude minutes invalid value: " + Minutes);

            Hemisphere = Enum.GetValues(typeof(Direction)).Cast<Direction>().FirstOrDefault(a => (char)a == direction[0]);
        }

        public override string ToString()
        {
            return Degrees.ToString("00") + Minutes.ToString("00.0000") + "," + (char)Hemisphere;
        }

        public bool Equals(Latitude lat)
        {
            if (lat.Degrees != this.Degrees || lat.Hemisphere != this.Hemisphere || lat.Minutes != this.Minutes)
                return false;
            return true;
        }

        public double Value
        { get { return this.Degrees + this.Minutes / 60.0; } }
    }

    public struct Longitude
    {
        enum Direction { Unknown, West = 'W', East = 'E' }
        double Degrees;
        double Minutes;
        Direction Hemisphere;

        public Longitude(double value)
        {
            double deg, min;
            Util.ValueToDegreesMinutes(value, out deg, out min);

            this.Degrees = deg;
            this.Minutes = min;

            if (this.Degrees >= 0)
                this.Hemisphere = Direction.East;
            else
                this.Hemisphere = Direction.West;
        }

        public Longitude(double degrees, double minutes)
            : this(Util.DegMinutesToValue(degrees, minutes))
        {
        }

        public Longitude(string location, string direction)
        {
            string[] loc = location.Split(new char[] { '.' }, 2);

            Degrees = Convert.ToDouble(loc[0].Substring(0, 3));
            if (Degrees > 180 || Degrees < -180) 
                throw new Exception("Invalid Longitude value: " + Degrees);

            Minutes = Convert.ToDouble(loc[0].Substring(3, loc[0].Length - 3)) + Convert.ToDouble(loc[1]) / Math.Pow(10, loc[1].Length);
            if (Minutes < 0 || Minutes > 60) 
                throw new Exception("Longitude minutes invalid value: " + Minutes);

            Hemisphere = Enum.GetValues(typeof(Direction)).Cast<Direction>().FirstOrDefault(a => (char)a == direction[0]);
        }

        public override string ToString()
        {
            return Degrees.ToString("000") + Minutes.ToString("00.0000") + "," + (char)Hemisphere;
        }

        public bool Equals(Longitude lon)
        {
            if (lon.Degrees != this.Degrees || lon.Hemisphere != this.Hemisphere || lon.Minutes != this.Minutes)
                return false;
            return true;
        }

        public double Value
        { get { return this.Degrees + this.Minutes/60.0; } }
    }
    #endregion

}
