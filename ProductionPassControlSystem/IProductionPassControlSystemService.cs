using ProductionPassControlSystem.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ProductionPassControlSystem
{
    [ServiceContract]
    public interface IProductionPassControlSystemService
    {
        [OperationContract]
        void AddingDataAboutShifts(ScheduleOfShift sheduleOfShift, int number);

        void CalculationOfDataOnTheShift(ScheduleOfShift sheduleOfShift, int number, SqlDataAdapter adapter, DataTable table, int numberOfHours);

        void FillingInRows(ScheduleOfShift sheduleOfShift, DataRow dataRow);

        void FillingInRowsForFourthShift(ScheduleOfShift sheduleOfShift, DataRow dataRow);

        void CalculationLogic(ScheduleOfShift sheduleOfShift, int numberOfHours);

        void CalculationLogicForFourthShift(ScheduleOfShift sheduleOfShift, int numberOfHours);
    }

    [ServiceContract]
    public interface IInformationAboutShifts
    {
        [OperationContract]
        void GetInformationAboutFirstShift(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm);
        [OperationContract]
        void GetInformationAboutSecondShift(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm);
        [OperationContract]
        void GetInformationAboutThirdShift(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm);
        [OperationContract]
        void GetInformationAboutFourthShift(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm);

        void FillingInWithData(DataTable table, int numOfDays, bool passageControl);

        [OperationContract]
        void SelectingSaturdayOrSunday(string year, string month);

        void GetSpecificDayShiftNumber();

        void GetSpecificNightShiftNumber();

        void GetSpecificStartDayOffNumber();

        void GetSpecificEndDayOffNumber();

        [OperationContract]
        void SelectingYear(string numberOfYear, bool changingInformation, string numberOfYearFromForm);

        [OperationContract]
        void SelectingMonth(string nameOfYear, string nameOfMonths, bool changingInformation,
                            string nameOfYearFromForm, string nameOfMonthFromForm);

        void Requests_2021_First();

        void Requests_2022_First();

        void Requests_2023_First();

        void Requests_2024_First();

        void Requests_2025_First();

        void Requests_2026_First();

        void Requests_2027_First();

        void Requests_2028_First();

        void Requests_2029_First();

        void Requests_2030_First();

        void Requests_2021_Second();

        void Requests_2022_Second();

        void Requests_2023_Second();

        void Requests_2024_Second();

        void Requests_2025_Second();

        void Requests_2026_Second();

        void Requests_2027_Second();

        void Requests_2028_Second();

        void Requests_2029_Second();

        void Requests_2030_Second();

        void Requests_2021_Third();

        void Requests_2022_Third();

        void Requests_2023_Third();

        void Requests_2024_Third();

        void Requests_2025_Third();

        void Requests_2026_Third();

        void Requests_2027_Third();

        void Requests_2028_Third();

        void Requests_2029_Third();

        void Requests_2030_Third();

        void Requests_2021_Fourth();

        void Requests_2022_Fourth();

        void Requests_2023_Fourth();

        void Requests_2024_Fourth();

        void Requests_2025_Fourth();

        void Requests_2026_Fourth();

        void Requests_2027_Fourth();

        void Requests_2028_Fourth();

        void Requests_2029_Fourth();

        void Requests_2030_Fourth();

        [OperationContract]
        void ClearData();

        [OperationContract]
        void ClearData2();

        [OperationContract]
        List<int> NumberOfDayShift();

        [OperationContract]
        List<int> NumberOfNightShift();

        [OperationContract]
        List<int> NumberOfDayOff();

        [OperationContract]
        List<int> NumberOfEndDayOff();

        [OperationContract]
        List<int> NumberOfDay();

        [OperationContract]
        DataTable GetTable1();

        [OperationContract]
        DataTable GetChangedInformation(int year, string month, int workId);

        [OperationContract]
        List<int> SelectingSaturdayOrSundayForChangeInformation(int years, string months);

        [OperationContract]
        void ChangingAWorkerSchedule(int shiftNumber, string year, string month);

        [OperationContract]
        int ChekingForChangedDataInTheDatabase(int year, string month, int workId);

        [OperationContract]
        void UpdateChangedShedule(FieldsForChangingTheWorkShedule fieldsForChangingTheWorkShedule, int id, FieldsForChangingTheWorkShedule[] fields, int year, string month);

        [OperationContract]
        void AddFieldsForChanging(FieldsForChangingTheWorkShedule fieldsForChangingTheWorkShedule, int id, List<FieldsForChangingTheWorkShedule> fields, int year, string month);
    }

    [ServiceContract]
    public interface IAddWorker
    {
        [OperationContract]
        void AddWorker(Worker worker);

        [OperationContract]
        int GetMaxId();

        [OperationContract]
        void AddAddress(Address address);

        [OperationContract]
        void AddPass(Pass pass);

        [OperationContract]
        void AddPhoto(Photo photo);
    }

    [ServiceContract]
    public interface IFindWorker
    {
        [OperationContract]
        void FindByPassNumberAndDepartmentName(string passNumber, string departmentName);

        [OperationContract]
        DataTable GetTable();

        [OperationContract]
        List<Worker> GetListWorker();

        [OperationContract]
        List<Pass> GetListPass();

        [OperationContract]
        List<Address> GetListAddress();

        [OperationContract]
        List<Photo> GetListPhoto();

        [OperationContract]
        List<string> GetListDepName();
    }

    [ServiceContract]
    public interface IGetAll
    {
        [OperationContract]
        int GetNumberOfWorkers();

        [OperationContract]
        int FindTheNumberOfEmployeesInDepartment(string nameOfDepartment);

        [OperationContract]
        DataTable GetAllWorker();

        [OperationContract]
        DataTable GetAllWorkerByDepartment(string departmentName);
    }

    [ServiceContract]
    public interface IRemove
    {
        [OperationContract]
        DataTable GetAllWorker1();

        [OperationContract]
        int AvailabilityOfASpecificWorkerId(int workerId);

        [OperationContract]
        void Remove(int id);

        [OperationContract]
        DataTable GetAllWorkerByDepartment1(string departmentName);
    }

    [ServiceContract]
    public interface IChangingWorkerInformation
    {
        [OperationContract]
        DataTable GetAllWorker2();

        [OperationContract]
        int AvailabilityOfASpecificWorkerId1(int workerId);

        [OperationContract]
        DataTable GetAllWorkerByDepartment2(string departmentName);

        [OperationContract]
        DataTable FindWorkerById1(int id);

        [OperationContract]
        void UpdateWorker(Worker worker, int ID);

        [OperationContract]
        void UpdateAddress(Address address);

        [OperationContract]
        void UpdatePass(Pass pass);

        [OperationContract]
        void UpdatePhoto(Photo photo);
    }

    [ServiceContract]
    public interface IPassageControl
    {
        [OperationContract]
        DataTable GetWorkerIdAndNumberOfShift();

        [OperationContract]
        void Add_Data_Of_The_Use_Of_A_Pass_By_A_Worker(ControlOfTheUseOfThePass controlOfTheUseOfThePass, List<ControlOfTheUseOfThePass> controls, string timeOfUseOfThePass);

        [OperationContract]
        void SelectingYear_PS(string numberOfYear, bool changingInformation, string numberOfYearFromForm);

        [OperationContract]
        void SelectingMonth_PS(string nameOfYear, string nameOfMonths, bool changingInformation,
                            string nameOfYearFromForm, string nameOfMonthFromForm);

        [OperationContract]
        void GetInformationAboutFirstShift_PS(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm);
        [OperationContract]
        void GetInformationAboutSecondShift_PS(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm);
        [OperationContract]
        void GetInformationAboutThirdShift_PS(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm);
        [OperationContract]
        void GetInformationAboutFourthShift_PS(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm);

        [OperationContract]
        DataTable GetWorkerIdFromChangedInformation(int year, string month);

        [OperationContract]
        int AvailabilityOfASpecificWorkerId_PS(int workerId);

        [OperationContract]
        DataTable GetValuesOfTime(int numberOfDay, int year, string month, int workerId);

        [OperationContract]
        List<int> NumberOfDayShift_PS();

        [OperationContract]
        List<int> NumberOfNightShift_PS();

        [OperationContract]
        List<int> NumberOfDayOff_PS();

        [OperationContract]
        List<int> NumberOfEndDayOff_PS();

        [OperationContract]
        void ClearData_PS();

        [OperationContract]
        DataTable GetChangedInformation_PS(int year, string month, int workId);

        [OperationContract]
        List<int> SelectingSaturdayOrSundayForChangeInformation_PS(int years, string months);

        [OperationContract]
        DataTable FindWorkerById_PS(int id);
    }

    [ServiceContract]
    public interface IInformationAboutUseThePass
    {
        [OperationContract]
        DataTable GetAllInformationAboutUseThePass();

        [OperationContract]
        DataTable GetAllInformationAboutUseThePassByWorkerId(int workerId);

        [OperationContract]
        DataTable GetAllInformationAboutUseThePassByWorkerIdYear(int workerId, int year);

        [OperationContract]
        DataTable GetAllInformationAboutUseThePassByWorkerIdYearMonth(int workerId, int year, string month);

        [OperationContract]
        DataTable GetAllInformationAboutUseThePassByWorkerIdYearMonthNumberOfDay(int workerId, int year, string month, int numberOfDay);

        [OperationContract]
        int DeleteAllFromInformationAboutUseThePass();

        [OperationContract]
        int DeleteAllInformationAboutUseThePassByWorkerId(int workerId);

        [OperationContract]
        int DeletemoreSpecificInformationAboutUseThePassByWorkerIdYear(int workerId, int year);

        [OperationContract]
        int DeletemoreSpecificInformationAboutUseThePassByWorkerIdYearMonth(int workerId, int year, string month);

        [OperationContract]
        int DeletemoreSpecificInformationAboutUseThePassByWorkerIdYearMonthNumberOfDay(int workerId, int year, string month, int numberOfDay);

        [OperationContract]
        int TotalNumberOfPassesUsed();

        [OperationContract]
        int TotalNumberOfPassesUsedByWorkerId(int workerId);

        [OperationContract]
        int TotalNumberOfPassesUsedByWorkerIdYear(int workerId, int year);

        [OperationContract]
        int TotalNumberOfPassesUsedByWorkerIdYearMonth(int workerId, int year, string month);

        [OperationContract]
        int TotalNumberOfPassesUsedByWorkerIdYearMonthNumberOfDay(int workerId, int year, string month, int numberOfDay);

        [OperationContract]
        int AvailabilityOfASpecificWorkerId_Pass(int workerId);
    }
}
