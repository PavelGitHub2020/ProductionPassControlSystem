using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.Entity
{
    [DataContract]
    public class Photo
    {
        //[DataMember]
        public int ID { get; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public int WorkerId { get; set; }
        public Photo() { }
        public Photo(int id, string path, int workerId)
        {
            ID = id;
            Path = path;
            WorkerId = workerId;
        }
        public override string ToString()
        {
            return $"Path - {Path}";
        }
    }
}
