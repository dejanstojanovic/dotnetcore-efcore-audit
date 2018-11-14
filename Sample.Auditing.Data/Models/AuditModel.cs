using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Sample.Auditing.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Auditing.Data.Models
{
    public class AuditModel
    {
        public String Username { get; set; }
        public String TableName { get; set; }
        public EntityState Action { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public Audit ToAudit()
        {
            return new Audit()
            {
                TableName = TableName,
                DateTime = DateTime.UtcNow,
                Action = Enum.GetName(typeof(EntityState), this.Action),
                KeyValues = JsonConvert.SerializeObject(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues)
            };
        }
    }
}
