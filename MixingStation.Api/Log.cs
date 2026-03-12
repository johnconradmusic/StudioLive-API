//using System;

//namespace MixingStation.Api
//{
//    public static class Log
//    {
//        private static void Write(string level, string messageTemplate, params object[] propertyValues)
//        {
//            var timestamp = DateTime.Now.ToString("HH:mm:ss");

//            var message = propertyValues != null && propertyValues.Length > 0
//                ? string.Format(messageTemplate, propertyValues)
//                : messageTemplate;

//            Console.WriteLine($"[{timestamp}] [{level}] {message}");
//        }

//        public static void Information(string messageTemplate, params object[] propertyValues)
//        {
//            Write("INFO", messageTemplate, propertyValues);
//        }

//        public static void Debug(string messageTemplate, params object[] propertyValues)
//        {
//            Write("DEBUG", messageTemplate, propertyValues);
//        }

//        public static void Warning(string messageTemplate, params object[] propertyValues)
//        {
//            Write("WARN", messageTemplate, propertyValues);
//        }

//        public static void Error(Exception exception, string messageTemplate, params object[] propertyValues)
//        {
//            var message = propertyValues != null && propertyValues.Length > 0
//                ? string.Format(messageTemplate, propertyValues)
//                : messageTemplate;

//            Write("ERROR", $"{message} | {exception}");
//        }
//    }
//}