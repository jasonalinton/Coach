using Coach.Data.DataAccess.Briefing;
using Coach.Model.Briefing;
using Coach.Service.Briefing;
using Coach.Service.Logging;
using Coach.Site.Models.ViewModels.Briefing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Coach.Site.Controllers
{
    public class BriefingAPIController : ApiController
    {
        IBriefingService _briefingService;

        public BriefingAPIController(IBriefingService briefingService)
        {
            _briefingService = briefingService;
        }

        // GET api/<controller>
        public IEnumerable<BriefingModel> GetBriefings()
        {
            var briefingDAO = new BriefingDAO();
            return briefingDAO.GetBriefings();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]BriefingModel briefing)
        {
            var briefingDAO = new BriefingDAO();
            briefingDAO.AddBriefing(briefing);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}