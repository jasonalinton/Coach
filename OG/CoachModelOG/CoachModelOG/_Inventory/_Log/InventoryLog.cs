using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachModel._Inventory._Log
{
    public class LogItem
    {
        public LogItem()
        {
            this.Fields = new List<ItemField>();
        }

        public int idLogItem { get; set; }
        public int idInventoryItem { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string InventoryItemName { get; set; }
        public int Position { get; set; }
        public bool IsActive { get; set; }
        public bool WasSaved { get; set; }
        public List<ItemField> Fields { get; set; }
        public List<FieldValue> UnSavedValues
        {
            get
            {
                var unSavedFields = new List<FieldValue>();
                foreach (var field in this.Fields)
                {
                    unSavedFields = unSavedFields.Union(field.UnSavedValues).ToList();
                }
                return unSavedFields;
            }
        }
        public List<FieldValue> EditedValues
        {
            get
            {
                var editedValues = new List<FieldValue>();
                foreach (var field in this.Fields)
                {
                    editedValues = editedValues.Union(field.EditedValues).ToList();
                }
                return editedValues;
            }
        }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ItemField
    {
        public ItemField()
        {
            this.Attributes = new List<Attribute>();
            this.Values = new List<FieldValue>();
        }

        public int idLogItemField { get; set; }
        public int idType { get; set; }
        public int idType_Data { get; set; }
        public string Name { get; set; }
        public string DateType { get; set; }
        public string Type { get; set; }
        public int Position { get; set; }
        public bool IsActive { get; set; }
        public string Value { get; set; }
        public bool WasEdited { get; set; }
        public List<FieldValue> UnSavedValues  { get { return this.Values.Where(x => !x.WasSaved).ToList(); } }
        public List<FieldValue> EditedValues { get { return this.Values.Where(x => x.WasEdited).ToList(); } }
        public List<Attribute> Attributes { get; set; }
        public List<FieldValue> Values { get; set; }
        public string[] UniqueValues // Used for dropdown datasource
        {
            get
            {
                return this.Values.Select(x => x.Value).Distinct().OrderBy(x => x).ToArray();
            }
        }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class Attribute
    {
        public Attribute()
        {
            this.TypeIDs = new List<int>();
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public List<int> TypeIDs { get; set; }
    }

    public class FieldValue
    {
        public int idField { get; set; }
        public int idEntry { get; set; }
        public int idLogEntry_LogItemField { get; set; }
        public string Value { get; set; }
        public bool WasEdited { get; set; }
        public bool WasSaved { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
