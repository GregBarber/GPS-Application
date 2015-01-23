using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_Application
{
    class GpsLogParser
    {
        string fileName;

        public GpsLogParser()
        {
            this.fileName = "c:\\users\\greg\\documents\\visual studio 2013\\Projects\\GPS-Application\\GPS-Application\\Reference Docs\\GPS_20130430_030350.log";
        }

        public GpsLogParser(string fileName)
        {
            this.fileName = fileName;
        }

        public void ReadGpsLog()
        {
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    //TODO: handle checksum
                    string[] sa = s.Split(',');
                    switch (sa[0])
                    {
                        case "$ADVER":
                            ReadADVER(sa);
                            break;
                        case "$GPRMC": 
                            ReadGPRMC(sa);
                            break;
                        case "$GPGGA": 
                            ReadGPGGA(sa);
                            break;
                        case "$GPGSA": 
                            ReadGPGSA(sa);
                            break;
                        case "$GPGSV": 
                            ReadGPGSV(sa);
                            break;
                        case "$GPGMC": 
                            ReadGPGMC(sa);
                            break;
                        case "$GPVTG": 
                            ReadGPVTG(sa);
                            break;
                        default:
                            // currently have other wierd data in some files, parhaps from the 'flag' button.
                            // TODO: Understand mysterious data
                            int sdf = 34;
                            break;
                            // throw new Exception("New, unhandled data type encountered in GPS log.\n" + s);
                    }
                }
            }

        }

        void ReadADVER(string[] sa) { }
        void ReadGPRMC(string[] sa) 
        {
            GprmcData.FromStringArray(sa);
        }
        void ReadGPGGA(string[] sa) 
        {
            GpggaData.FromStringArray(sa);       
        }
        void ReadGPGSA(string[] sa) { }
        void ReadGPGSV(string[] sa) { }
        void ReadGPGMC(string[] sa) { }
        void ReadGPVTG(string[] sa) { }
    }

    #region Common Utilities
    public class Util
    {
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
    }
    struct Latitude
    {
        enum Direction { Unknown, North = 'N', South = 'S' }
        double Degrees;
        double Minutes;
        Direction Hemisphere;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">Latitude position in degrees</param>
        /// <param name="direction">character representation o North/South hemisphere</param>
        /// <returns>Latitude struct</returns>
        public static Latitude FromStrings(string location, string direction)
        {
            Latitude lat = new Latitude();
            string[] loc = location.Split(new char[] { '.' }, 2);

            lat.Degrees = Convert.ToDouble(loc[0].Substring(0, 2));
            if (lat.Degrees > 90 || lat.Degrees < -90) throw new Exception("Invalid Latitude value: " + lat.Degrees);

            lat.Minutes = Convert.ToDouble(loc[0].Substring(2, loc[0].Length - 2)) + Convert.ToDouble(loc[1]) / Math.Pow(10, loc[1].Length);
            if (lat.Minutes < 0 || lat.Minutes > 60) throw new Exception("Latitude minutes invalid value: " + lat.Minutes);

            lat.Hemisphere = Enum.GetValues(typeof(Direction)).Cast<Direction>().FirstOrDefault(a => (char)a == direction[0]);

            return lat;
        }
    }

    struct Longitude
    {
        enum Direction { Unknown, West = 'W', East = 'E' }
        double Degrees;
        double Minutes;
        Direction Hemisphere;

        public static Longitude FromStrings(string location, string direction)
        {
            Longitude lon = new Longitude();
            string[] loc = location.Split(new char[] { '.' }, 2);

            lon.Degrees = Convert.ToDouble(loc[0].Substring(0, 3));
            if (lon.Degrees > 90 || lon.Degrees < -90) 
                throw new Exception("Invalid Longitude value: " + lon.Degrees);

            lon.Minutes = Convert.ToDouble(loc[0].Substring(3, loc[0].Length - 3)) + Convert.ToDouble(loc[1]) / Math.Pow(10, loc[1].Length);
            if (lon.Minutes < 0 || lon.Minutes > 60) 
                throw new Exception("Longitude minutes invalid value: " + lon.Minutes);

            lon.Hemisphere = Enum.GetValues(typeof(Direction)).Cast<Direction>().FirstOrDefault(a => (char)a == direction[0]);

            return lon;
        }
    }

    #endregion
    #region GPRMC
    struct GprmcData
    {
        struct MagneticVariation
        {
            enum Direction { Unknown, North = 'N', South = 'S', East = 'E', West = 'W' }

            double degrees;
            Direction direction;

            public static MagneticVariation FromStrings(string deg, string direct)
            {
                MagneticVariation mv = new MagneticVariation();

                // TODO: figure out a way to output empty string for Unknown direction
                var x = Convert.ChangeType(mv.direction, mv.direction.GetTypeCode());

                if (deg == "" && direct == "")
                    return mv;

                mv.degrees = Convert.ToDouble(deg);

                mv.direction = Enum.GetValues(typeof(Direction)).Cast<Direction>().FirstOrDefault(a => (char)a == direct[0]);

                return mv;
            }
        }
        enum NavigationReceiverWarning { None, Warning = 'V', ValidPosition = 'A' }

        NavigationReceiverWarning warning;
        DateTime dateTime;
        Latitude latitude;
        Longitude longitude;
        double speedOverGroundKnots;
        double courseMadeGoodDegreesTrue;
        MagneticVariation magneticVariation;

        public static GprmcData FromStringArray(string[] sa)
        {
            GprmcData data = new GprmcData();

            string times = sa[1];
            string navWarning = sa[2];
            data.latitude = Latitude.FromStrings(sa[3], sa[4]);
            data.longitude = Longitude.FromStrings(sa[5], sa[6]);
            data.speedOverGroundKnots = Convert.ToDouble(sa[7]);
            data.courseMadeGoodDegreesTrue = Convert.ToDouble(sa[8]);
            string date = sa[9];

            data.warning = Enum.GetValues(typeof(NavigationReceiverWarning)).Cast<NavigationReceiverWarning>().FirstOrDefault(a => (char)a == navWarning[0]);           
            data.magneticVariation = MagneticVariation.FromStrings(sa[10], sa[11]);
            data.dateTime = Util.DateTimeFromString(date, times);
            
            return data;
        }
    }
    #endregion


    #region GPGGA
    struct GpggaData
    {
        enum FixQuality { Invalid, GpsFix, DGpsFix }
        enum Units {Meters}
        struct Altitude
        {
            double altitude;
            Units units;

            public static Altitude FromStrings(string altitude, string units)
            {
                Altitude alt = new Altitude();
                Double.TryParse(altitude, out alt.altitude);

                alt.units = Enum.GetValues(typeof(Units)).Cast<Units>().FirstOrDefault(a => (char)a == units[0]);           

                return alt;
            }
        }

        Longitude longitude;
        Latitude latitude;
        DateTime dateTime;
        FixQuality qualityIndicator;
        int numberOfSatellites;
        double horizontalDilutionOfPrecision;
        Altitude altitudeAboveSeaLevel, altitudeAboveGeodial;
        double ageOfDifferentialData;
        int idOfDifferentialStation;

        public static GpggaData FromStringArray(string[] sa)
        {
            GpggaData data = new GpggaData();

            string time = sa[1];
            data.latitude = Latitude.FromStrings(sa[2], sa[3]);
            data.longitude = Longitude.FromStrings(sa[4], sa[5]);
            Enum.TryParse<FixQuality>(sa[6], out data.qualityIndicator);
            Int32.TryParse(sa[7], out data.numberOfSatellites);
            Double.TryParse(sa[8], out data.horizontalDilutionOfPrecision);
            data.altitudeAboveSeaLevel = Altitude.FromStrings(sa[9], sa[10]);
            data.altitudeAboveGeodial = Altitude.FromStrings(sa[11], sa[12]);
            Double.TryParse(sa[13], out data.ageOfDifferentialData);
            Int32.TryParse(sa[14], out data.idOfDifferentialStation);

            // use a fake data since none is provided.  Becomes 01.01.2000
            data.dateTime = Util.DateTimeFromString("010100", time);
            return data;

        }
    }

    #endregion
}
