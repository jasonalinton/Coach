using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Extension
{
    public static class ExtensionMethods
    {
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        /// <summary>
        /// This is only meant for entity framework models. Ignores lists and related models(part of namespace System.Data.Entity.DynamicProxies)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newValue"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public static string GetEFPropertyString<T>(this T newValue, string objectId = "", bool returnBlank = false)
        {

            var fieldInfos = newValue.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            var variances = new List<Variance>();
            foreach (var fieldInfo in fieldInfos.Where(p => 
                (!(p.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(p.PropertyType)) 
                && p.PropertyType.Namespace != "Coach.Data.Model")))
            {
                var value = fieldInfo.GetValue(newValue);
                if (!returnBlank && value == null || 
                    (!returnBlank && value is string && string.IsNullOrEmpty((string)value)))
                    continue;

                variances.Add(new Variance
                {
                    Property = @"<>".Any(fieldInfo.Name.Contains) ? fieldInfo.Name.Split('<')[1].Split('>')[0] : fieldInfo.Name,
                    Value = value
                }); 
            }

            /*variances = variances.ToList();
                .Where(x => x.Property.GetType() != typeof(string) && x.Property.GetType() != typeof(ICollection<object>))
                .Where(v => !v.Property.Equals("PropertyChanging", StringComparison.InvariantCultureIgnoreCase)).ToList();*/
            var jsonString = JsonConvert.SerializeObject(variances, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return jsonString;
        }

        public static string GetVariantString<T>(this T newValue, T oldValue, string objectId = "")
        {
            var variances = newValue.GetVariant(oldValue);

            var jsonString = JsonConvert.SerializeObject(variances, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return jsonString;
        }

        /// <summary>
        /// Compares two objects of same type and returns a list of property variances
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        /// <returns></returns>
        public static List<Variance> GetVariant<T>(this T newValue, T oldValue, string objectId = "")
        {
            var fieldInfos = newValue.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            var variances = new List<Variance>();
            foreach (var fieldInfo in fieldInfos)
            {
                //var oldVal = fieldInfo.GetValue(oldValue) ?? "";
                //var newVal = fieldInfo.GetValue(newValue) ?? "";

                variances.Add(new Variance
                {
                    Property = @"<>".Any(fieldInfo.Name.Contains) ? fieldInfo.Name.Split('<')[1].Split('>')[0] : fieldInfo.Name,
                    //Value = new Variance.Values(fieldInfo.GetValue(oldValue) ?? "", fieldInfo.GetValue(newValue) ?? "")
                    OldValue = fieldInfo.GetValue(oldValue) ?? "",
                    NewValue = fieldInfo.GetValue(newValue) ?? ""
                });
            }

            return variances
                .Where(x => x.Property != "createdAt" && x.Property != "updatedAt")
                .Where(v => !v.OldValue.ToString().Equals(v.NewValue.ToString()) && !v.Property.Equals("PropertyChanging", StringComparison.InvariantCultureIgnoreCase)).ToList();
                //.Where(v => !v.Value.OldValue.ToString().Equals(v.Value.NewValue.ToString()) && !v.Property.Equals("PropertyChanging", StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public class Variance
        {
            public string Property { get; set; }
            public object OldValue { get; set; }
            public object NewValue { get; set; }
            public object Value { get; set; }
            //public Values Value { get; set; }

            //public class Values
            //{
            //    public Values(object oldValue, object newValue)
            //    {
            //        this.OldValue = oldValue;
            //        this.NewValue = newValue;
            //    }
            //    public object OldValue { get; set; }
            //    public object NewValue { get; set; }
            //}

        }
    }

    public static class ExceptionExtensions
    {
        public static string GetFullMessageString(this Exception exception, bool reverse = true)
        {
            var fullMessageString = "";
            var exceptions = exception.FromHierarchy(ex => ex.InnerException).ToList();

            if (reverse)
                exceptions.Reverse();

            foreach (var exceptionTemp in exceptions)
            {
                fullMessageString += exceptionTemp.GetType().FullName + ": " + exceptionTemp.Message;
                fullMessageString += (exceptionTemp != exceptions.Last()) ? " --->\r\n" : "";
            }

            return fullMessageString;
        }

        public static string GetFullStackTraceString(this Exception exception, bool reverse = false)
        {
            var stackTrace = "";
            var exceptions = exception.FromHierarchy(ex => ex.InnerException).ToList();

            if (reverse)
                exceptions.Reverse();

            foreach (var exceptionTemp in exceptions)
            {
                stackTrace += exceptionTemp.GetType().FullName + ": " + exceptionTemp.Message;
                stackTrace += "\r\n";
                stackTrace += exceptionTemp.StackTrace;
                stackTrace += (exceptionTemp != exceptions.Last()) ?
                    $"\r\n--- End of {exceptionTemp.GetType().FullName} stack trace ---\r\n\r\n" : $"\r\n--- End of {exceptionTemp.GetType().FullName} stack trace ---";
            }

            return stackTrace;
        }

        //public static IEnumerable<string> GetFullStackTrace(this Exception exception)
        //{
        //    var exceptions = exception.FromHierarchy(ex => ex.InnerException);

        //    return exceptions.Select(x => x.StackTrace);
        //}

        //public static string GetFullStackTraceString(this Exception exception)
        //{
        //    var stackTraci = exception.GetFullStackTrace().Reverse().ToList();
        //    var stackTraciString = String.Join("\r\n", stackTraci);

        //    return stackTraciString;
        //}


        public static List<string> GetAllMessages(this Exception exception)
        {
            var exceptions = exception.FromHierarchy(ex => ex.InnerException);

            return exceptions.Select(x => x.Message).ToList();
        }

        public static List<string> GetAllMessagesWithClassName(this Exception exception)
        {
            var exceptions = exception.FromHierarchy(ex => ex.InnerException);

            return exceptions.Select(x => x.GetType().Name + ": " + x.Message).ToList();
        }

        //public static string GetFullMessageString1(this Exception exception)
        //{
        //    var messages = exception.GetAllMessages().Reverse().ToList();
        //    var messagesString = String.Join("\r\n", messages);

        //    return messagesString;
        //}
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfDay(this DateTime date)
        {
            return date.Date;
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);
        }

        public static DateTime StartOfWeek(this DateTime date)
        {
            return date.Date.AddDays(-(int)date.DayOfWeek);
        }

        public static DateTime EndOfWeek(this DateTime date)
        {
            return date.Date.AddDays(-(int)date.DayOfWeek).AddDays(6);
        }

        public static string GetMonth(this DateTime date)
        {
            return ((Month)date.Month).ToString();
        }

        public enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
    }

    //public static class EnumExtensions
    //{
    //    public static List<string> ToUppercaseString<T>(this Enum @enum)
    //    {
    //        var @string = String.Empty;

    //        var characters = @enum.ToString().ToList();
    //        foreach (var character in characters)
    //        {
    //            if (@string != "" && Char.IsUpper(character))
    //                @string += " ";

    //            @string += cha
    //        }

    //        for (var i = 0; i < characters.Count; i++)
    //        {
    //            var character
    //            if (i != 0 && Char.IsUpper(character))
    //        }
    //    }
    //}
}
