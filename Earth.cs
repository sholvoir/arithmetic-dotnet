using System;
using System.Globalization;

namespace Vultrue.Communication
{
    /// <summary>
    /// 球面及视距相关算法
    /// </summary>
    public static class Earth
    {
        /// <summary>
        /// 地球平均半径(km)
        /// </summary>
        public const double R = 6371.004;

        /// <summary>
        /// 地球上的点
        /// </summary>
        public struct Point
        {
            /// <summary>
            /// 构造
            /// </summary>
            /// <param name="longitude"></param>
            /// <param name="latitude"></param>
            /// <param name="altitude"></param>
            public Point(double longitude, double latitude, double altitude)
            {
                Longitude = longitude;
                Latitude = latitude;
                Altitude = altitude;
            }

            /// <summary>
            /// 经度
            /// </summary>
            public double Longitude;

            /// <summary>
            /// 纬度
            /// </summary>
            public double Latitude;

            /// <summary>
            /// 高度(km)
            /// </summary>
            public double Altitude;

            /// <summary>
            /// 直角坐标X
            /// </summary>
            public double X
            {
                get
                {
                    return R * Math.Cos(Latitude * Math.PI / 180) * Math.Cos(Longitude * Math.PI / 180);
                }
            }

            /// <summary>
            /// 直角坐标Y
            /// </summary>
            public double Y
            {
                get
                {
                    return R * Math.Cos(Latitude * Math.PI / 180) * Math.Sin(Longitude * Math.PI / 180);
                }
            }

            /// <summary>
            /// 直角坐标Z
            /// </summary>
            public double Z
            {
                get
                {
                    return R * Math.Sin(Latitude * Math.PI / 180);
                }
            }
        }

        /// <summary>
        /// 地球上两点的直线距离(km)
        /// </summary>
        /// <param name="a">点A</param>
        /// <param name="b">点B</param>
        /// <returns>距离(km)</returns>
        public static double Distance(Point a, Point b)
        {
            return Math.Sqrt(sqr(a.X - b.X) + sqr(a.Y - b.Y) + sqr(a.Z - b.Z));
        }

        /// <summary>
        /// 地球上高度确定的两点的最大视距距离(km)
        /// </summary>
        /// <param name="a">点A</param>
        /// <param name="b">点B</param>
        /// <returns>最大视距距离(km)</returns>
        public static double MaxStadia(Point a, Point b)
        {
            return Math.Sqrt(a.Altitude * (a.Altitude + 2 * R)) + Math.Sqrt(b.Altitude * (b.Altitude + 2 * R));
        }

        /// <summary>
        ///最大通信半径
        /// </summary>
        /// <param name="p">点P</param>
        /// <param name="signalDistance">点P的最大直线通信距离</param>
        /// <returns>点P的通信范围</returns>
        public static double MaxCommRadius(Point p, double signalDistance)
        {
            double s = signalDistance / 2;
            double h = p.Altitude / 2;
            return Math.Min(2 * Math.Sqrt((R + s + h) * (s + h) * (R - s + h) * (s - h)) / (R + p.Altitude), MaxStadia(p, new Point()));
        }

        /// <summary>
        /// 将10进制经纬度格式转换为度分秒格式
        /// </summary>
        /// <param name="cordinate">十进制经纬度格式</param>
        /// <param name="degree">度</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        public static void ConvertLonLatToDegree(double cordinate, out int degree, out int minute, out int second)
        {
            double s;
            ConvertLonLatToDegree(cordinate, out degree, out minute, out s);
            second = (int)s;
        }

        /// <summary>
        /// 将10进制经纬度格式转换为度分秒格式
        /// </summary>
        /// <param name="cordinate">十进制经纬度格式</param>
        /// <param name="degree">度</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        public static void ConvertLonLatToDegree(double cordinate, out int degree, out int minute, out double second)
        {
            degree = (int)cordinate;
            double dMinute = cordinate - degree;
            dMinute = 60 * dMinute;
            minute = (int)dMinute;
            double dSecond = dMinute - minute;
            second = (60 * dSecond);
        }

        /// <summary>
        /// 将度分秒经纬度格式转换为10进制格式
        /// </summary>
        /// <param name="degree">度</param>
        /// <param name="minute">分</param>
        /// <param name="sencond">秒</param>
        /// <returns>10进制格式的经纬度</returns>
        public static double ConvertDegreeToLonLat(int degree, int minute, int sencond)
        {
            return degree + minute / 60.0 + sencond / 360.0;
        }

        /// <summary>
        /// 将10进制经纬度格式转换为度分秒格式
        /// </summary>
        /// <param name="cordinate">十进制经纬度格式</param>
        /// <returns>转化成度分秒格式后的字符串</returns>
        public static string ConvertLonLatToDegree(double cordinate)
        {
            int degree, minute, second;
            ConvertLonLatToDegree(cordinate, out degree, out minute, out second);

            return string.Format("{0}º{1}'{2}\"", degree, minute, second);
        }

        /// <summary>
        ///  将10进制经纬度格式转换为度分秒格式
        /// </summary>
        /// <param name="cordinate">十进制经纬度格式</param>
        /// <param name="precision">需要保留秒的经度</param>
        /// <returns>转化成度分秒格式后的字符串</returns>
        public static string ConvertLonLatToDegree(double cordinate, int precision)
        {
            int degree, minute;
            double second;
            ConvertLonLatToDegree(cordinate, out degree, out minute, out second);
            return string.Format("{0}º{1}'{2}\"", degree, minute, second.ToString("F" + precision.ToString(), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 平方
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double sqr(double x)
        {
            return x * x;
        }
    }
}