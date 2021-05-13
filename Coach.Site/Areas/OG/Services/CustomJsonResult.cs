using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Coach.Site.Areas.OG.Services
{
    /// <summary>
    /// customJsonview
    /// </summary>
    public class CustomJsonResult : JsonResult
    {
        /// <summary>
        /// format string
        /// </summary>
        public string FormateStr
        {
            get;
            set;
        }

        /// <summary>
        /// Override execution view
        /// </summary>
        /// <param name="context">context</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.Data != null)
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 2147483647;
                string jsonString = jss.Serialize(Data);

                /* Convert dates after 1970 */
                string patternNew = @"\\/Date\((\d+)\)\\/";
                MatchEvaluator matchEvaluatorNew = new MatchEvaluator(this.ConvertJsonDateToDateString);
                Regex regNew = new Regex(patternNew);
                jsonString = regNew.Replace(jsonString, matchEvaluatorNew);

                /* Convert dates before 1970 */
                string patternOld = @"\\/Date\((-\d+)\)\\/";
                MatchEvaluator matchEvaluatorOld = new MatchEvaluator(this.ConvertJsonDateToOldDateString);
                Regex regOld = new Regex(patternOld);
                jsonString = regOld.Replace(jsonString, matchEvaluatorOld);

                response.Write(jsonString);
            }
        }

        /// <summary> 
        /// takeJsonTime of serialization/Date(1294499956278)Converted to a string .
        /// </summary> 
        /// <param name="m">regular expression matching</param>
        /// <returns>Formatted string</returns>
        private string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString(FormateStr);
            return result;
        }

        /*THIS ASSUMES THAT USER WILL NEVER WANT TO SERISALIZE A DATE BEFORE JANUARY 1, 1970 */
        private string ConvertJsonDateToOldDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime();
            dt = dt.ToLocalTime();
            result = dt.ToString(FormateStr);
            return result;
        }

        //protected internal JsonResult Json(object data)
        //{
        //    return this.Json(data, null, null, JsonRequestBehavior.DenyGet);
        //}

        //protected internal JsonResult Json(object data, string contentType)
        //{
        //    return this.Json(data, contentType, null, JsonRequestBehavior.DenyGet);
        //}

        //protected internal JsonResult Json(object data, JsonRequestBehavior behavior)
        //{
        //    return this.Json(data, null, null, behavior);
        //}

        //protected internal virtual JsonResult Json(object data, string contentType, Encoding contentEncoding)
        //{
        //    return this.Json(data, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
        //}

        //protected internal JsonResult Json(object data, string contentType, JsonRequestBehavior behavior)
        //{
        //    return this.Json(data, contentType, null, behavior);
        //}

        //protected internal virtual JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        //{
        //    return new JsonResult { Data = data, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behavior };
        //}
    }
}