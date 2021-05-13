using System;
using SQLite;

namespace Coach.Mobile.Models
{
    public interface ICoachModel
    {
        [PrimaryKey, AutoIncrement]
        int SQLiteID { get; set; }
        int RemoteID { get; }
        string ModelName { get; }
    }
}
