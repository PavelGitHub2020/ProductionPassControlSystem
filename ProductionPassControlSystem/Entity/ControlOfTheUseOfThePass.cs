using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.Entity
{
    [DataContract]
    public class ControlOfTheUseOfThePass : FieldsForChangingTheWorkShedule
    {
        [DataMember]
        public string Year { get; set; }
        [DataMember]
        public string Month { get; set; }
        [DataMember]
        public int WorkerId { get; set; }
        [DataMember]
        public string TimeOfUseOfThePass { get; set; }
        [DataMember]
        public string TheResultOfUsingThePass { get; set; }
        public ControlOfTheUseOfThePass() { }
        public ControlOfTheUseOfThePass(string year, string month, int id, string timeOfTheUseOfThePass, string theResultOfUsingThePass,
                                        int numOfDay, string condition, string sicneWhatTime, string tillWhatTime,
                                        string value) : base(numOfDay, condition, sicneWhatTime, tillWhatTime, value)
        {
            Year = year;
            Month = month;
            WorkerId = id;
            TimeOfUseOfThePass = timeOfTheUseOfThePass;
            TheResultOfUsingThePass = theResultOfUsingThePass;
        }
    }
}
