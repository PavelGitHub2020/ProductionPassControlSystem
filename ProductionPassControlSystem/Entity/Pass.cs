using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.Entity
{
    [DataContract]
    public class Pass
    {
        //[DataMember]
        public int ID { get; }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public bool Condition { get; set; }
        [DataMember]
        public int WorkerId { get; set; }
        public Pass() { }
        public Pass(int id, string number, bool condition, int workerId)
        {
            ID = id;
            Number = number;
            Condition = condition;
            WorkerId = workerId;
        }
        public override string ToString()
        {
            return $"Number - {Number}\n" +
                   $"Condition - {Condition}";
        }
    }
}
