using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Items.GoogleTask
{
    public class GoogleTaskModel
    {
        public GoogleTaskModel(string title, DateTime? due)
        {
            Title = title;
            Due = due;
        }
        public GoogleTaskModel(string id, string title, DateTime? due)
        {
            Id = id;
            Title = title;
            Due = due;
        }

        string notes;
        int? points;
        string newName;
        string medium;
        string note;
        bool wasAttempted;
        bool isVisible = true;
        bool isActive = true;
        DateTime? deadline;
        DateTime? dueDate;
        DateTime? recommendedDate;

        #region API Properties
        /// <summary>
        /// Last modification time of the task (as a RFC 3339 timestamp).
        /// </summary>
        public virtual string UpdatedRaw { get; set; }
        /// <summary>
        /// Title of the task.
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// Status of the task. This is either "needsAction" or "completed".
        /// </summary>
        public virtual string Status { get; set; }
        /// <summary>
        /// URL pointing to this task. Used to retrieve, update, or delete this task.
        /// </summary>
        public virtual string SelfLink { get; set; }
        /// <summary>
        /// String indicating the position of the task among its sibling tasks under the
        /// same parent task or at the top level. If this string is greater than another
        ///  ask's corresponding position string according to lexicographical ordering, the
        ///  task is positioned after the other task under the same parent task (or at the
        ///  top level). This field is read-only. Use the "move" method to move the task to
        /// </summary>
        public virtual string Position { get; set; }
        /// <summary>
        /// Parent task identifier. This field is omitted if it is a top-level task. This
        /// field is read-only. Use the "move" method to move the task under a different
        /// parent or to the top level.
        /// </summary>
        public virtual string Parent { get; set; }
        /// <summary>
        /// Notes describing the task. Optional.
        /// </summary>
        public virtual string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                try
                {
                    this.notes = value;

                    var extentionModel = JsonConvert.DeserializeObject<GoogleMapExtension>(value);
                    this.Points = extentionModel.Points;
                    this.NewName = extentionModel.NewName;
                    this.Medium = extentionModel.Medium;
                    this.Note = extentionModel.Note;
                    this.WasAttempted = extentionModel.WasAttempted;
                    this.IsVisible = extentionModel.IsVisible;
                    this.IsActive = extentionModel.IsActive;
                    this.Deadline = extentionModel.Deadline;
                    this.DueDate = extentionModel.DueDate;
                    this.RecommendedDate = extentionModel.RecommendedDate;
                }
                catch (Exception ex)
                {

                }

            }
        }
        /// <summary>
        /// Collection of links. This collection is read-only.
        /// </summary>
        public virtual IList<LinksData> Links { get; set; }
        /// <summary>
        /// Type of the resource. This is always "tasks#task".
        /// </summary>
        public virtual string Kind { get; set; } = "tasks#task";
        /// <summary>
        /// Task identifier.
        /// </summary>
        public virtual string Id { get; set; }
        /// <summary>
        /// Flag indicating whether the task is hidden. This is the case if the task had
        /// been marked completed when the task list was last cleared. The default is False.
        /// This field is read-only.
        /// </summary>
        public virtual bool? Hidden { get; set; }
        /// <summary>
        /// ETag of the resource.
        /// </summary>
        public virtual string ETag { get; set; }
        /// <summary>
        /// System.DateTime representation of Google.Apis.Tasks.v1.Data.Task.DueRaw.
        /// </summary>
        public virtual DateTime? Due { get; set; }
        /// <summary>
        /// Due date of the task (as a RFC 3339 timestamp). Optional. The due date only records
        /// date information; the time portion of the timestamp is discarded when setting
        /// the due date. It isn't possible to read or write the time that a task is due
        /// via the API.
        /// </summary>
        public virtual string DueRaw { get; set; }
        /// <summary>
        /// Flag indicating whether the task has been deleted. The default if False.
        /// </summary>
        public virtual bool? Deleted { get; set; }
        /// <summary>
        /// System.DateTime representation of Google.Apis.Tasks.v1.Data.Task.CompletedRaw.
        /// </summary>
        public virtual DateTime? Completed { get; set; }
        /// <summary>
        /// Completion date of the task (as a RFC 3339 timestamp). This field is omitted
        /// if the task has not been completed.
        /// </summary>
        public virtual string CompletedRaw { get; set; }
        /// <summary>
        /// System.DateTime representation of Google.Apis.Tasks.v1.Data.Task.UpdatedRaw.
        /// </summary>
        public virtual DateTime? Updated { get; set; }

        public DateTime? ScheduledDate => Due?.ToUniversalTime() ?? null;


        public bool IsComplete
        {
            get
            {
                if (Status == "needsAction")
                    return false;
                else if (Status == "completed")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    Status = "completed";
                else if (!value)
                    Status = "needsAction";
            }
        }

        public class LinksData
        {
            /// <summary>
            /// The description. In HTML speak: Everything between and .
            /// </summary>
            public virtual string Description { get; set; }
            /// <summary>
            /// The URL.
            /// </summary>
            public virtual string Link { get; set; }
            /// <summary>
            /// Type of the link, e.g. "email".
            /// </summary>
            public virtual string Type { get; set; }
        }
        #endregion

        #region Extension Properties

        public int? Points
        {
            get { return this.points; }
            set
            {
                var extentionModel = JsonConvert.DeserializeObject<GoogleMapExtension>(this.Notes);
                extentionModel.Points = value;
                this.notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                this.points = value;
            }
        }
        public string Text => this.Title;
        public string NewName
        {
            get { return this.newName; }
            set
            {
                var extentionModel = JsonConvert.DeserializeObject<GoogleMapExtension>(this.Notes);
                extentionModel.NewName = value;
                this.notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                //this.GoogleMapExtensionModel.Medium = value;
                this.newName = value;
            }
        }
        public string Medium
        {
            get { return this.medium; }
            set
            {
                var extentionModel = JsonConvert.DeserializeObject<GoogleMapExtension>(this.Notes);
                extentionModel.Medium = value;
                this.notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                //this.GoogleMapExtensionModel.Medium = value;
                this.medium = value;
            }
        }
        public string Note
        {
            get { return this.note; }
            set
            {
                var extentionModel = JsonConvert.DeserializeObject<GoogleMapExtension>(this.Notes);
                extentionModel.Note = value;
                this.notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                //this.GoogleMapExtensionModel.Note = value;
                this.note = value;
            }
        }
        public bool IsVisible
        {
            get { return this.isVisible; }
            set
            {
                var extentionModel = JsonConvert.DeserializeObject<GoogleMapExtension>(this.Notes);
                extentionModel.IsVisible = value;
                this.notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                //this.GoogleMapExtensionModel.IsVisible = value;
                this.isVisible = value;
            }
        }
        public bool IsActive
        {
            get { return this.isActive; }
            set
            {
                var extentionModel = JsonConvert.DeserializeObject<GoogleMapExtension>(this.Notes);
                extentionModel.IsActive = value;
                this.notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                //this.GoogleMapExtensionModel.IsActive = value;
                this.isActive = value;
            }
        }
        public bool WasAttempted
        {
            get { return this.wasAttempted; }
            set
            {
                var extentionModel = JsonConvert.DeserializeObject<GoogleMapExtension>(this.Notes);
                extentionModel.WasAttempted = value;
                this.notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                //this.GoogleMapExtensionModel.WasAttempted = value;
                this.wasAttempted = value;
            }
        }
        public DateTime? Deadline
        {
            get { return this.deadline; }
            set
            {
                var extentionModel = JsonConvert.DeserializeObject<GoogleMapExtension>(this.Notes);
                extentionModel.Deadline = value;
                this.notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                //this.GoogleMapExtensionModel.Deadline = value;
                this.deadline = value;
            }
        }
        public DateTime? DueDate
        {
            get { return this.dueDate; }
            set
            {
                var extentionModel = JsonConvert.DeserializeObject<GoogleMapExtension>(this.Notes);
                extentionModel.DueDate = value;
                this.notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                //this.GoogleMapExtensionModel.DueDate = value;
                this.dueDate = value;
            }
        }
        public DateTime? RecommendedDate
        {
            get { return this.recommendedDate; }
            set
            {
                var extentionModel = JsonConvert.DeserializeObject<GoogleMapExtension>(this.Notes);
                extentionModel.RecommendedDate = value;
                this.notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                //this.GoogleMapExtensionModel.RecommendedDate = value;
                this.recommendedDate = value;
            }
        }
        /// <summary>
        /// The date this task needs to be completed. Based on deadline, due date, and recommended date
        /// </summary>
        public DateTime? CompletionDate
        {
            get
            {
                DateTime? completionDate = null;
                var now = DateTime.Now;

                if (this.Deadline != null && this.Deadline > now)
                    completionDate = this.Deadline;
                else if (this.DueDate != null && this.DueDate > now)
                    completionDate = this.DueDate;
                else if (this.RecommendedDate != null && this.RecommendedDate > now)
                    completionDate = this.RecommendedDate;

                return completionDate;
            }
        }
        #endregion

        public class GoogleMapExtension
        {
            public int? Points { get; set; }
            public string NewName { get; set; }
            public string Medium { get; set; }
            public string Note { get; set; }
            public bool IsVisible { get; set; } = true;
            public bool IsActive { get; set; } = true;
            public bool WasAttempted { get; set; }
            public DateTime? Deadline { get; set; }
            public DateTime? DueDate { get; set; }
            public DateTime? RecommendedDate { get; set; }
            /// <summary>
            /// The date this task needs to be completed. Based on deadline, due date, and recommended date
            /// </summary>
            public DateTime? CompletionDate
            {
                get
                {
                    DateTime? completionDate = null;
                    var now = DateTime.Now;

                    if (this.Deadline != null && this.Deadline > now)
                        completionDate = this.Deadline;
                    else if (this.DueDate != null && this.DueDate > now)
                        completionDate = this.DueDate;
                    else if (this.RecommendedDate != null && this.RecommendedDate > now)
                        completionDate = this.RecommendedDate;

                    return completionDate;
                }
            }
        }

        public override string ToString()
        {
            return $"Google Task: {this.Text}";
        }
    }
}
