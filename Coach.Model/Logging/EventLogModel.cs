using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Logging
{
    public class EventLogModel
    {
        public EventLogModel(string message, int dataKey, string dataTable, string dataValue, string crud, string endpoint = null, string username = null)
        {
            this.LogMessage = message;
            this.DataKey = dataKey;
            this.DataTable = dataTable;
            this.DataValue = dataValue;
            this.CRUD = crud;
            this.Endpoint = endpoint;
            this.Username = username;
        }
        
        public string LogMessage { get; set; }
        public int DataKey { get; set; }
        public string DataTable { get; set; }
        public string DataValue { get; set; }
        public string CRUD { get; set; }
        public string Endpoint { get; set; }
        public string Username { get; set; }
        public string MachineName { get; set; }
        public string EventLevel { get; set; }
    }
}
