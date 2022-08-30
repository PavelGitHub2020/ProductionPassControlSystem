using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.Entity
{
    [DataContract]
    public class FieldsForChangingTheWorkShedule
    {
        [DataMember]
        public int NumberOfDay { get; set; }
        [DataMember]
        public string Condition { get; set; }
        [DataMember]
        public string SinceWhatTime { get; set; }
        [DataMember]
        public string TillWhatTime { get; set; }
        [DataMember]
        public string Value { get; set; }
        public FieldsForChangingTheWorkShedule() { }
        public FieldsForChangingTheWorkShedule(int numOfDay, string condition, string sinceWhatTime, string tillWhatTime, string value)
        {
            NumberOfDay = numOfDay;
            Condition = condition;
            SinceWhatTime = sinceWhatTime;
            TillWhatTime = tillWhatTime;
            Value = value;
        }
    }
}
