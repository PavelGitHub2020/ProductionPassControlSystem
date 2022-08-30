using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.Entity
{
    [DataContract]
    public class Worker
    {
        public int ID { get; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Patronymic { get; set; }
        [DataMember]
        public string DateOfBirth { get; set; }
        [DataMember]
        public bool Gender { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string Profession { get; set; }
        [DataMember]
        public string DateOfStartToWork { get; set; }
        [DataMember]
        public int DepartmentId { get; set; }
        [DataMember]
        public int NumberOfShift { get; set; }
        public Worker() { }
        public Worker(int id, string surname, string name, string patronymic, string dataOfBirth, bool gender, string phoneNumber,
                      int departmentId, string profession, string dateOfStartToWork, int numberOfShift)
        {
            ID = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            DateOfBirth = dataOfBirth;
            Gender = gender;
            PhoneNumber = phoneNumber;
            DepartmentId = departmentId;
            Profession = profession;
            DateOfStartToWork = dateOfStartToWork;
            NumberOfShift = numberOfShift;
        }
        public override string ToString()
        {
            return $"Last name - {Surname}\n" +
                   $"Name - {Name}\n" +
                   $"Patronymic - {Patronymic}\n" +
                   $"Date of birth - {DateOfBirth}\n" +
                   $"Gender - {Gender}\n" +
                   $"Phone number - {PhoneNumber}\n" +
                   $"Name of department - {DepartmentId}\n" +
                   $"Profession - {Profession}\n" +
                   $"Date of start to work - {DateOfStartToWork}\n" +
                   $"Number of shift - {NumberOfShift}";
        }
    }
}
