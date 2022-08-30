using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.Entity
{
    [DataContract]
    public class Address
    {
       // [DataMember]
        public int ID { get; }

        [DataMember]
        public string NameOfTheCity { get; set; }

        [DataMember]
        public string NameOfTheStreet { get; set; }

        [DataMember]
        public string HouseNumber { get; set; }

        [DataMember]
        public int WorkerId { get; set; }
        public Address() { }

        public Address(int id, string nameOfTheCity, string nameOfTheStreet, string houseNumber, int workerId)
        {
            ID = id;
            NameOfTheCity = nameOfTheCity;
            NameOfTheStreet = nameOfTheStreet;
            HouseNumber = houseNumber;
            WorkerId = workerId;
        }

        public override string ToString()
        {
            return $"Name of the city - {NameOfTheCity}\n" +
                   $"Name of the street - {NameOfTheStreet}\n" +
                   $"House number - {HouseNumber}";
        }
    }
}

