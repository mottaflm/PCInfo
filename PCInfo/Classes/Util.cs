using System;
using System.Management;

namespace PCInfo.Classes
{
    class Util
    {
        public const byte MAX_DAYS = 2;
        public const byte MAX_CPU = 70;
        public const byte MAX_NETWORK = 10;

        public const byte MIN_DISK = 10;
        public const byte MIN_MEMORY = 1;

        public static DateTime ToDateTime(string value)
        {
            return ManagementDateTimeConverter.ToDateTime(value);
        }

        public static string ToFormatedTimeSpan(TimeSpan value)
        {
            return $"{value.Days:D2}d:{value.Hours:D2}h:{value.Minutes:D2}m:{value.Seconds:D2}s";
        }

        public static string FormatOSVersion(string value)
        {
            switch (value)
            {
                case "10.0.10240":
                    return "1507";
                case "10.0.10586":
                    return "1511";
                case "10.0.14393":
                    return "1607";
                case "10.0.15063":
                    return "1703";
                case "10.0.16299":
                    return "1709";
                case "10.0.17134":
                    return "1803";
                case "10.0.17763":
                    return "1809";
                case "10.0.18362":
                    return "1903";
                case "10.0.18363":
                    return "1909";
                case "10.0.19041":
                    return "2004";
                default:
                    return value;
            }
        }

        public static double ConvertToLowerDouble(double value, ConvertType type)
        {
            try
            {
                double newValue = value;

                switch(type)
                {
                    case ConvertType.ClockSpeed:

                        newValue /= 1000;
                        break;

                    case ConvertType.Memory:

                        newValue /= 1048576;
                        break;

                    case ConvertType.DiskSpace:

                        newValue /= 1073741824;
                        break;

                    case ConvertType.Network:

                        newValue /= 1048576;
                        break;
                }

                newValue = Math.Round(newValue, 2);
                return newValue;
            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
                return value;
            }
        }

        public static double ToDouble(string value)
        {
            try
            {
                var newValue = double.Parse(value);
                return newValue;
            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
                return 0;
            }
        }

        public static byte ToByte(string value)
        {
            try
            {
                var newValue = (byte)Int32.Parse(value);
                return newValue;
            }
            catch (Exception e)
            {
                Program.ErrorDialog(e.Message);
                return 0;
            }
        }
    }

    public enum ConvertType
    {
        ClockSpeed,
        Memory,
        DiskSpace,
        Network,
    }

}
