using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_Application
{
    public abstract class GpsData
    {
        public abstract override string ToString();
    }

    public abstract class GpsDataTimeLocation : GpsData
    {
        public abstract double Time { get; }

        public abstract Latitude Latitude { get; }

        public abstract Longitude Longitude { get; }
    }

    public abstract class GpsDataDate : GpsDataTimeLocation
    {
        public abstract int Date { get; }
    }

    #region Common
    public enum ModeIndicator { NotValid = 'N', Autonomous = 'A', Differential = 'D', Estimated = 'E' }
    #endregion

    #region GPRMC
    public class GprmcData : GpsDataDate
    {
        #region Supporting Data Structures
        struct MagneticVariation
        {
            enum Direction { Unknown = 'U', North = 'N', South = 'S', East = 'E', West = 'W' }

            double? degrees;
            Direction direction;

            public static MagneticVariation FromStrings(string deg, string direct)
            {
                MagneticVariation mv = new MagneticVariation();

                //TODO: hack for empty value
                if (direct == "")
                    direct = "U";

                mv.degrees = Util.NullableDouble(deg);
                mv.direction = Enum.GetValues(typeof(Direction)).Cast<Direction>().FirstOrDefault(a => (char)a == direct[0]);

                return mv;
            }

            public override string ToString()
            {
                string dir = "";

                if (direction != Direction.Unknown)
                    dir = ((char)direction).ToString();

                return string.Format("{0:000.0}", degrees) + "," + dir;
            }
        }
        enum NavigationReceiverWarning { None, Warning = 'V', ValidPosition = 'A' }
        #endregion

        string head;
        NavigationReceiverWarning warning;
        DateTime dateTime;
        Latitude latitude;
        Longitude longitude;
        double speedOverGroundKnots;
        double courseMadeGoodDegreesTrue;
        MagneticVariation magneticVariation;
        ModeIndicator modeIndicator;

        public GprmcData(string[] sa)
        {
            head = "GPRMC";
            string times = sa[1];
            string navWarning = sa[2];
            latitude = new Latitude(sa[3], sa[4]);
            longitude = new Longitude(sa[5], sa[6]);
            speedOverGroundKnots = Convert.ToDouble(sa[7]);
            courseMadeGoodDegreesTrue = Convert.ToDouble(sa[8]);
            string date = sa[9];
            string mode = sa[12];

            warning = Enum.GetValues(typeof(NavigationReceiverWarning)).Cast<NavigationReceiverWarning>().FirstOrDefault(a => (char)a == navWarning[0]);
            magneticVariation = MagneticVariation.FromStrings(sa[10], sa[11]);
            dateTime = Util.DateTimeFromString(date, times);
            modeIndicator = Enum.GetValues(typeof(ModeIndicator)).Cast<ModeIndicator>().FirstOrDefault(a => (char)a == mode[0]);
        }

        public override string ToString()
        {
            string data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                head, dateTime.ToString("HHmmss.fff"), (char)warning, latitude.ToString(), longitude.ToString(),
                speedOverGroundKnots.ToString("0.00"), courseMadeGoodDegreesTrue.ToString("0.00"), dateTime.ToString("ddMMyy"),
                magneticVariation.ToString(), (char)modeIndicator);

            return "$" + data + "*" + Util.CalculateCheckSum(data);
        }


        #region Properties
        public override double Time
        {
            //TODO: ya, converting back and forth
            get { return Convert.ToDouble(dateTime.ToString("HHmmss.fff")); }
        }

        public override Latitude Latitude
        {
            get { return this.latitude; }
        }

        public override Longitude Longitude
        {
            get { return this.longitude; }
        }

        public override int Date
        {
            //TODO: ya, converting back and forth
            get { return Convert.ToInt32(dateTime.ToString("ddMMyy")); }
        }
        #endregion
    }
    #endregion

    #region GPGGA
    public class GpggaData : GpsDataTimeLocation
    {
        #region Supporting Data Structures
        enum FixQuality { Invalid, GpsFix, DGpsFix }
        enum Units { Meters = 'M' }
        struct Altitude
        {
            double altitude;
            Units units;

            public Altitude(string alt, string unit)
            {
                Double.TryParse(alt, out altitude);
                units = Enum.GetValues(typeof(Units)).Cast<Units>().FirstOrDefault(a => (char)a == unit[0]);
            }

            public override string ToString()
            {
                return altitude.ToString("0.0") + "," + (char)units;
            }
        }
        #endregion

        string head;
        Longitude longitude;
        Latitude latitude;
        DateTime dateTime;
        FixQuality qualityIndicator;
        int numberOfSatellites;
        double horizontalDilutionOfPrecision;
        Altitude altitudeAboveSeaLevel, altitudeAboveGeodial;
        double? ageOfDifferentialData;
        int? idOfDifferentialStation;

        public GpggaData(string[] sa)
        {
            head = "GPGGA";
            string time = sa[1];
            latitude = new Latitude(sa[2], sa[3]);
            longitude = new Longitude(sa[4], sa[5]);
            Enum.TryParse<FixQuality>(sa[6], out qualityIndicator);
            Int32.TryParse(sa[7], out numberOfSatellites);
            Double.TryParse(sa[8], out horizontalDilutionOfPrecision);
            altitudeAboveSeaLevel = new Altitude(sa[9], sa[10]);
            altitudeAboveGeodial = new Altitude(sa[11], sa[12]);
            ageOfDifferentialData = Util.NullableDouble(sa[13]);
            idOfDifferentialStation = Util.NullableInt(sa[14]);

            // use a fake date since none is provided.  Becomes 01.01.2000, but time is correct
            dateTime = Util.DateTimeFromString("010100", time);
        }

        public override string ToString()
        {
            string data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9:0.0},{10:0000}",
                head, dateTime.ToString("HHmmss.fff"), latitude.ToString(), longitude.ToString(),
                (int)qualityIndicator, numberOfSatellites.ToString("00"), horizontalDilutionOfPrecision.ToString("0.0"),
                altitudeAboveSeaLevel.ToString(), altitudeAboveGeodial.ToString(), ageOfDifferentialData, idOfDifferentialStation);
            return "$" + data + "*" + Util.CalculateCheckSum(data);
        }

        #region Properties
        public override double Time
        {
            //TODO: ya, converting back and forth
            get { return Convert.ToDouble(dateTime.ToString("HHmmss.fff")); }
        }

        public override Latitude Latitude
        {
            get { return this.latitude; }
        }

        public override Longitude Longitude
        {
            get { return this.longitude; }
        }
        #endregion
    }
    #endregion

    #region GPGSA
    public class GpgsaData : GpsData
    {
        #region Supporting Data Structures
        enum ModeSetting { Unknown, Manual = 'M', Automatic = 'A' }
        enum ModeType { Unknown, FixNotAvailable = 1, TwoD = 2, ThreeD = 3 }
        #endregion

        string head;
        ModeSetting modeSetting;
        ModeType modeType;
        int?[] satellitePRN;
        double? positionDilutionOfPrecision, hoizontalDilutionOfPrecision, verticalDilutionOfPrecision;

        public GpgsaData(string[] sa)
        {
            head = "GPGSA";
            satellitePRN = new int?[12];

            modeSetting = Enum.GetValues(typeof(ModeSetting)).Cast<ModeSetting>().FirstOrDefault(a => (char)a == sa[1][0]);
            Enum.TryParse<ModeType>(sa[2], out modeType);

            positionDilutionOfPrecision = Util.NullableDouble(sa[15]);
            hoizontalDilutionOfPrecision = Util.NullableDouble(sa[16]);
            verticalDilutionOfPrecision = Util.NullableDouble(sa[17]); ;

            for (int i = 0; i < satellitePRN.Length; i++)
                satellitePRN[i] = Util.NullableInt(sa[i + 3]);
        }

        public override string ToString()
        {
            string prns = string.Format("{0:00}", satellitePRN[0]); //.ToString();
            for (int i = 1; i < satellitePRN.Length; i++)
                prns += "," + string.Format("{0:00}", satellitePRN[i]); // satellitePRN[i].ToString();

            string data = string.Format("{0},{1},{2},{3},{4:0.0},{5:0.0},{6:0.0}",
                    head, (char)modeSetting, (int)modeType, prns, positionDilutionOfPrecision,
                    hoizontalDilutionOfPrecision, verticalDilutionOfPrecision);

            return "$" + data + "*" + Util.CalculateCheckSum(data);
        }
    }
    #endregion

    #region GPGSV
    public class GpgsvData : GpsData
    {
        #region Supporting Data Structures
        public struct Message
        {
            int? prnNumber;
            int? elevation;
            int? azimuth;
            int? snr;

            public Message(string[] sa)
            {
                this.prnNumber = Util.NullableInt(sa[0]);
                this.elevation = Util.NullableInt(sa[1]);
                this.azimuth = Util.NullableInt(sa[2]);
                this.snr = Util.NullableInt(sa[3]);
            }

            public override string ToString()
            {
                return string.Format("{0:00},{1:00},{2:000},{3:00}",
                    prnNumber, elevation, azimuth, snr);
            }
        }
        #endregion

        string head;
        int totalMessagesInCycle;
        int messageNumber;
        int totalSatellitesInView;
        List<Message> messages;

        public GpgsvData(string[] sa)
        {
            head = "GPGSV";
            messages = new List<Message>();
            totalMessagesInCycle = Convert.ToInt32(sa[1]);
            messageNumber = Convert.ToInt32(sa[2]);
            totalSatellitesInView = Convert.ToInt32(sa[3]);

            string[] messageContent = new string[4];
            int messageCount = (sa.Length - 4) / 4;
            for (int i = 0; i < messageCount; i++)
            {
                Array.Copy(sa, 4 * (i + 1), messageContent, 0, 4);
                messages.Add(new Message(messageContent));
            }
        }

        public override string ToString()
        {
            string m = messages[0].ToString();
            for (int i = 1; i < messages.Count; i++)
                m += "," + messages[i].ToString();

            string data = string.Format("{0},{1},{2},{3:00},{4}", head,
                totalMessagesInCycle, messageNumber, totalSatellitesInView, m);

            return "$" + data + "*" + Util.CalculateCheckSum(data);
        }
    }
    #endregion

    #region GPVTG
    public class GpvtgData : GpsData
    {
        string head;
        char trueCourseMark, magneticCourseMark, knotsSpeedMark, kphSpeedMark;
        double? trueCourseOverGroundDeg;
        double? magneticCourseDeg;
        double? groundSpeedKnots;
        double? groundSpeedKilometersPerHour;
        ModeIndicator modeIndicator;

        int courseDigits;
        int knotsDigits;
        int kiloDigits;

        public GpvtgData(string[] data)
        {
            head = "GPVTG";
            
            this.trueCourseOverGroundDeg = Util.NullableDouble(data[1]);
            this.trueCourseMark = Convert.ToChar(data[2]);
            this.magneticCourseDeg = Util.NullableDouble(data[3]);
            this.magneticCourseMark = Convert.ToChar(data[4]);
            this.groundSpeedKnots = Util.NullableDouble(data[5]);
            this.knotsSpeedMark = Convert.ToChar(data[6]);
            this.groundSpeedKilometersPerHour = Util.NullableDouble(data[7]);
            this.kphSpeedMark = Convert.ToChar(data[8]);
            modeIndicator = Enum.GetValues(typeof(ModeIndicator)).Cast<ModeIndicator>().FirstOrDefault(a => (char)a == data[9][0]);
        

            //pulling my hair out over this one, can't be bothered to find an elegent solution right now
            this.courseDigits = data[1].Split('.')[1].Length;
            this.knotsDigits = data[5].Split('.')[1].Length;
            this.kiloDigits = data[7].Split('.')[1].Length;
        }

        public override string ToString()
        {
            string data = string.Format("{0},{1:0." + Digits(courseDigits) + "},{2},{3},{4},{5:0." + Digits(knotsDigits) + "},{6},{7:0." + Digits(kiloDigits) + "},{8},{9}", head,
                 trueCourseOverGroundDeg, (char)trueCourseMark, magneticCourseDeg, (char)magneticCourseMark,
                 groundSpeedKnots, (char)knotsSpeedMark, groundSpeedKilometersPerHour, (char)kphSpeedMark, (char)modeIndicator);

            return "$" + data + "*" + Util.CalculateCheckSum(data);
        }

        /// <summary>
        /// Formats the correct number of digits (sig figs perhaps?) for the value
        /// </summary>
        /// <remarks>This is a inelegant workaround so the input data matches exactly the output.  There is not a consistent number of digits used for this data.  Perhaps is it connected to the accuracy of the 
        /// device where an extra digit is used only if it is known to be 0.  Wouldl like to extract the format of the input to apply to the output</remarks>
        /// <param name="num">Number of digits</param>
        /// <returns>Format string</returns>
        string Digits(int num)
        {
            string val = "";
            for (int i = 0; i < num; i++)
                val += "0";
            return val;
        }
    }
    #endregion
}
