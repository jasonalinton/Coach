using Coach.Model.Inventory.Physical.Sleep;
using Coach.Service.Inventory.Physical;
using Coach.Service.Logging;
using Coach.Site.Models.ViewModels.Sleep;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coach.Site.Controllers
{
    public class SleepController : BaseController
    {
        ISleepService _sleepService;

        public SleepController(ISleepService sleepService)
        {
            _sleepService = sleepService;
        }

        public ActionResult Sleep()
        {
            return View();
        }

        #region Sleep View
        public JsonResult GetSleepVM(DateTime? date = null)
        {
            try
            {
                var sleepSessions = _sleepService.GetSleepSessions();
                var sleepRequirement = _sleepService.GetSleepRequirement();
                SleepVM sleepVM;
                if (date == null)
                    sleepVM = new SleepVM(sleepSessions, sleepRequirement);
                else
                    sleepVM = new SleepVM(sleepSessions, sleepRequirement, (DateTime)date);

                return Json(new { sleepVM, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving Sleep Session");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult UploadSleepSessionData()
        {
            try
            {

                //List<string> result = new List<string>();
                //List<string> errors = new List<string>();

                //string fileName = string.Empty;
                //string fileContent = string.Empty;
                //string submissionTypeDesc = string.Empty;
                //try
                //{
                //    if (string.IsNullOrWhiteSpace(submissionType))
                //    {
                //        errors.Add("Submission type not selected.");
                //        goto ExitFunction;
                //    }
                //    submissionType = submissionType.Trim().ToLower();
                //    switch (submissionType)
                //    {
                //        case RegistryToolsConstants.SUBMISSION_TYPE_OPT_IN:
                //            submissionTypeDesc = RegistryToolsConstants.SUBMISSION_TYPE_DESCRIPTION_OPT_IN;
                //            break;
                //        case RegistryToolsConstants.SUBMISSION_TYPE_REGISTRY:
                //            submissionTypeDesc = RegistryToolsConstants.SUBMISSION_TYPE_DESCRIPTION_REGISTRY;
                //            break;
                //        default:
                //            errors.Add("Selected Submission type is invalid.");
                //            goto ExitFunction;
                //    }

                //    if ((Request.Files.Count == 0) || (Request.Files[0] == null))
                //    {
                //        errors.Add("No file selected.");
                //        goto ExitFunction;
                //    }

                //    HttpPostedFileBase file = Request.Files[0];

                //    if (file.ContentLength == 0)
                //    {
                //        errors.Add("Selected file is empty.");
                //        goto ExitFunction;
                //    }

                //    if (string.IsNullOrWhiteSpace(file.FileName))
                //    {
                //        errors.Add("Selected file is invalid.");
                //        goto ExitFunction;
                //    }

                //    fileName = file.FileName;
                //    using (Stream submitStream = file.InputStream)
                //    {
                //        using (StreamReader submitReader = new StreamReader(submitStream))
                //        {
                //            fileContent = submitReader.ReadToEnd();
                //        }
                //    }
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error uploading Sleep Session data");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //[ImAuthorize(ImRoles.RegistryAnalyzer)]
        //public FileContentResult GenerateJSONFile(int reportingYear)
        //{
        //    try
        //    {
        //        string fileName = $"ConsolidatedRegistryJSON_{reportingYear.ToString("F0")}.json";
        //        string contentType = "application/json";

        //        bool isErrorContent = false;
        //        string fileContent = _pqrsRegistryService.GenerateJSONOrErrorContent(reportingYear, out isErrorContent);
        //        if (isErrorContent)
        //        {
        //            fileName = $"ConsolidatedRegistryJSONErrors_{reportingYear.ToString("F0")}.txt";
        //            contentType = "text/plain";
        //        }
        //        byte[] fileContentBytes = Encoding.UTF8.GetBytes(fileContent);

        //        FileContentResult returnValue = new FileContentResult(fileContentBytes, contentType);
        //        returnValue.FileDownloadName = fileName;

        //        return (returnValue);
        //    }
        //    catch (Exception ex)
        //    {
        //        Exception exceptionToThrow = new Exception($"An error occurred while generating Registry Tools JSON file for Reporting Year {reportingYear.ToString("F0")}", ex);
        //        throw exceptionToThrow;
        //    }
        //}

        //[HttpPost]
        //[ImAuthorize(ImRoles.RegistryAnalyzer)]
        //public JsonResult SubmitJSONFile(int year, string submissionType = null)
        //{
        //    List<string> result = new List<string>();
        //    List<string> errors = new List<string>();

        //    string fileName = string.Empty;
        //    string fileContent = string.Empty;
        //    string submissionTypeDesc = string.Empty;
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(submissionType))
        //        {
        //            errors.Add("Submission type not selected.");
        //            goto ExitFunction;
        //        }
        //        submissionType = submissionType.Trim().ToLower();
        //        switch (submissionType)
        //        {
        //            case RegistryToolsConstants.SUBMISSION_TYPE_OPT_IN:
        //                submissionTypeDesc = RegistryToolsConstants.SUBMISSION_TYPE_DESCRIPTION_OPT_IN;
        //                break;
        //            case RegistryToolsConstants.SUBMISSION_TYPE_REGISTRY:
        //                submissionTypeDesc = RegistryToolsConstants.SUBMISSION_TYPE_DESCRIPTION_REGISTRY;
        //                break;
        //            default:
        //                errors.Add("Selected Submission type is invalid.");
        //                goto ExitFunction;
        //        }

        //        if ((Request.Files.Count == 0) || (Request.Files[0] == null))
        //        {
        //            errors.Add("No file selected.");
        //            goto ExitFunction;
        //        }

        //        HttpPostedFileBase file = Request.Files[0];

        //        if (file.ContentLength == 0)
        //        {
        //            errors.Add("Selected file is empty.");
        //            goto ExitFunction;
        //        }

        //        if (string.IsNullOrWhiteSpace(file.FileName))
        //        {
        //            errors.Add("Selected file is invalid.");
        //            goto ExitFunction;
        //        }

        //        fileName = file.FileName;
        //        using (Stream submitStream = file.InputStream)
        //        {
        //            using (StreamReader submitReader = new StreamReader(submitStream))
        //            {
        //                fileContent = submitReader.ReadToEnd();
        //            }
        //        }

        //        string userName = User.Identity.Name;
        //        string userEmail = ((ClaimsIdentity)User.Identity).FindFirstValue(ClaimTypes.Email);

        //        SubmitRequestViewModelBase submitVm = null;
        //        switch (submissionType)
        //        {
        //            case RegistryToolsConstants.SUBMISSION_TYPE_OPT_IN:
        //                submitVm = new OptInSubmitRequestViewModel()
        //                {
        //                    Year = year
        //                };
        //                break;
        //            case RegistryToolsConstants.SUBMISSION_TYPE_REGISTRY:
        //                submitVm = new RegistrySubmitRequestViewModel()
        //                {
        //                    Year = year
        //                };
        //                break;
        //        }
        //        if (submitVm != null)
        //        {
        //            submitVm.InitializeRequest(userName, userEmail, fileContent);

        //            ServiceRequestModel requestModel = submitVm.GetServiceRequestModel();
        //            int requestId = _im360Service.CreateServiceRequest(requestModel);
        //            LogInfo($"The {submissionTypeDesc} file {fileName} was submitted for processing", dataTable: "ServiceRequests", dataKey: requestId.ToString("F0"));
        //            result.Add($"The {submissionTypeDesc} file {fileName} was submitted for processing.");
        //        }
        //        else
        //        {
        //            result.Add($"The file {fileName} could not be submitted for processing.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (string.IsNullOrWhiteSpace(fileName))
        //        {
        //            LogError($"An error occurred while submitting the selected {submissionTypeDesc} file for processing", ex, dataKey: "year", dataValue: year.ToString("F0"));
        //            errors.Add($"An error occurred while submitting the selected {submissionTypeDesc} file for processing.");
        //        }
        //        else
        //        {
        //            LogError($"An error occurred while submitting the {submissionTypeDesc} file {fileName} for processing", ex, dataKey: "year", dataValue: year.ToString("F0"));
        //            errors.Add($"An error occurred while submitting the {submissionTypeDesc} file {fileName} for processing.");
        //        }
        //    }

        //    ExitFunction:
        //    bool success = (!(errors.Any()));
        //    if (!(success))
        //    {
        //        result = new List<string>();
        //    }
        //    JsonResult returnValue = new JsonResult()
        //    {
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //        MaxJsonLength = int.MaxValue,
        //        Data = new
        //        {
        //            success = success,
        //            result = result,
        //            errors = errors
        //        }
        //    };
        //    return (returnValue);
        //}
        #endregion

        #region Sleep Session
        public JsonResult GetSleepSession(int idSleepSession)
        {
            try
            {
                var sleepSession = _sleepService.GetSleepSession(idSleepSession);

                return Json(new { SleepSession = sleepSession, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving Sleep Session");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSleepSessions()
        {
            try
            {
                var sleepSessions = _sleepService.GetSleepSessions();

                return Json(new { SleepSessions = sleepSessions, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving Sleep Sessions");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddSleepSession(SleepSessionModel sleepSessionModel)
        {
            try
            {
                var newID = _sleepService.AddSleepSession(sleepSessionModel);

                return Json(new {  newID, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error adding Sleep Session");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddSleepSessions(List<SleepSessionModel> SleepSessionModels)
        {
            try
            {
                var newIDs = _sleepService.AddSleepSessions(SleepSessionModels);

                return Json(new { newIDs, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error adding Sleep Sessions");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateSleepSession(SleepSessionModel sleepSessionModel)
        {
            try
            {
                var sleepSession = _sleepService.UpdateSleepSession(sleepSessionModel);

                return Json(new { SleepSession = sleepSession, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error updating Sleep Session");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateSleepSessions(List<SleepSessionModel> sleepSessionModels)
        {
            try
            {
                var sleepSessions = _sleepService.UpdateSleepSessions(sleepSessionModels);

                return Json(new { SleepSessions = sleepSessions, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error updating Sleep Sessions");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteSleepSession(int id)
        {
            try
            {
                _sleepService.DeleteSleepSession(id);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error deleting Sleep Session");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteSleepSessions(List<int> ids)
        {
            try
            {
                _sleepService.DeleteSleepSessions(ids);

                return Json(new {  success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error deleting Sleep Sessions");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UploadAutoSleepData(IEnumerable<HttpPostedFileBase> csvFiles)
        {
            try
            {
                if (csvFiles != null)
                {
                    foreach (var csvFile in csvFiles)
                        _sleepService.UploadAutoSleepData(csvFile);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error uploading AutoSleep sleep data");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Sleep Requirement
        public JsonResult GetSleepRequirement(int idSleepRequirement)
        {
            try
            {
                var sleepRequirement = _sleepService.GetSleepRequirement(idSleepRequirement);

                return Json(new { SleepRequirement = sleepRequirement, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving Sleep Requirement");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSleepRequirements()
        {
            try
            {
                var sleepRequirements = _sleepService.GetSleepRequirements();

                return Json(new { SleepRequirements = sleepRequirements, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving Sleep Requirements");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddSleepRequirement(SleepRequirementModel sleepRequirementModel)
        {
            try
            {
                var newID = _sleepService.AddSleepRequirement(sleepRequirementModel);

                return Json(new { newID, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error adding Sleep Requirement");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddSleepRequirements(List<SleepRequirementModel> SleepRequirementModels)
        {
            try
            {
                var newIDs = _sleepService.AddSleepRequirements(SleepRequirementModels);

                return Json(new { newIDs, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error adding Sleep Requirements");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateSleepRequirement(SleepRequirementModel sleepRequirementModel)
        {
            try
            {
                var sleepRequirement = _sleepService.UpdateSleepRequirement(sleepRequirementModel);

                return Json(new { SleepRequirement = sleepRequirement, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error updating Sleep Requirement");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateSleepRequirements(List<SleepRequirementModel> sleepRequirementModels)
        {
            try
            {
                var sleepRequirements = _sleepService.UpdateSleepRequirements(sleepRequirementModels);

                return Json(new { SleepRequirements = sleepRequirements, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error updating Sleep Requirements");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteSleepRequirement(int id)
        {
            try
            {
                _sleepService.DeleteSleepRequirement(id);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error deleting Sleep Requirement");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteSleepRequirements(List<int> ids)
        {
            try
            {
                _sleepService.DeleteSleepRequirements(ids);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error deleting Sleep Requirements");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}