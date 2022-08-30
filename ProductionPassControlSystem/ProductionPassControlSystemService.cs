using ProductionPassControlSystem.DAL;
using ProductionPassControlSystem.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows;

namespace ProductionPassControlSystem
{
    public class ProductionPassControlSystemService : BaseDAO, IProductionPassControlSystemService,
                                                               IInformationAboutShifts, IAddWorker,
                                                               IFindWorker, IGetAll, IRemove,
                                                               IChangingWorkerInformation,
                                                               IPassageControl,
                                                               IInformationAboutUseThePass
    {
        IDbConnection connection;

        SqlCommand command;

        public ProductionPassControlSystemService()
        {
            connection = GetConnection();
        }

        ///FillinfShifts//////////////////////////////////////////////////////////////////
        private static int condition = 0;
        private static DateTime date = new DateTime(2021, 1, 1);

        private const string SELECT_FIRST_SHIFTS = "SELECT * FROM First_Shift";
        private const string SELECT_SECOND_SHIFTS = "SELECT * FROM Second_Shift";
        private const string SELECT_THIRD_SHIFTS = "SELECT * FROM Third_Shift";
        private const string SELECT_FOURTH_SHIFTS = "SELECT * FROM Fourth_Shift";
        public void AddingDataAboutShifts(ScheduleOfShift sheduleOfShift, int number)
        {
            try
            {
                SqlCommand first = new SqlCommand();
                first.CommandText = SELECT_FIRST_SHIFTS;

                SqlCommand second = new SqlCommand();
                second.CommandText = SELECT_SECOND_SHIFTS;

                SqlCommand third = new SqlCommand();
                third.CommandText = SELECT_THIRD_SHIFTS;

                SqlCommand fourth = new SqlCommand();
                fourth.CommandText = SELECT_FOURTH_SHIFTS;

                first.Connection = (SqlConnection)connection;
                second.Connection = (SqlConnection)connection;
                third.Connection = (SqlConnection)connection;
                fourth.Connection = (SqlConnection)connection;

                SqlDataAdapter firstAdapter = new SqlDataAdapter(first);
                DataTable firstTable = new DataTable();
                firstAdapter.Fill(firstTable);

                SqlDataAdapter secondAdapter = new SqlDataAdapter(second);
                DataTable secondTable = new DataTable();
                secondAdapter.Fill(secondTable);

                SqlDataAdapter thirdAdapter = new SqlDataAdapter(third);
                DataTable thirdTable = new DataTable();
                thirdAdapter.Fill(thirdTable);

                SqlDataAdapter fourthAdapter = new SqlDataAdapter(fourth);
                DataTable fourthTable = new DataTable();
                fourthAdapter.Fill(fourthTable);

                CalculationOfDataOnTheShift(sheduleOfShift, number, firstAdapter, firstTable, 8);
                CalculationOfDataOnTheShift(sheduleOfShift, number, secondAdapter, secondTable, 32);
                CalculationOfDataOnTheShift(sheduleOfShift, number, thirdAdapter, thirdTable, 56);
                CalculationOfDataOnTheShift(sheduleOfShift, number, fourthAdapter, fourthTable, 20);

                ReleaseConnection(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void FillingInRows(ScheduleOfShift sheduleOfShift, DataRow dataRow)
        {
            try
            {
                dataRow[1] = sheduleOfShift.NameOfMonth;
                dataRow[2] = sheduleOfShift.DayOfWeek_D;
                dataRow[3] = sheduleOfShift.DayShift;
                dataRow[4] = sheduleOfShift.StartDayShift;
                dataRow[5] = sheduleOfShift.EndDayShift;
                dataRow[6] = sheduleOfShift.Day_Of_Week_N;
                dataRow[7] = sheduleOfShift.NightShift;
                dataRow[8] = sheduleOfShift.StartNightShift;
                dataRow[9] = sheduleOfShift.EndNightShift;
                dataRow[10] = sheduleOfShift.Day_Of_Week_Off;
                dataRow[11] = sheduleOfShift.DayOff;
                dataRow[12] = sheduleOfShift.StartDayOff;
                dataRow[13] = sheduleOfShift.Day_Of_Week_End;
                dataRow[14] = sheduleOfShift.EndDayOff;
                dataRow[15] = sheduleOfShift.EndDayOffTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void FillingInRowsForFourthShift(ScheduleOfShift sheduleOfShift, DataRow dataRow)
        {
            try
            {
                dataRow[1] = sheduleOfShift.NameOfMonth;
                dataRow[2] = sheduleOfShift.Day_Of_Week_N;
                dataRow[3] = sheduleOfShift.NightShift;
                dataRow[4] = sheduleOfShift.StartNightShift;
                dataRow[5] = sheduleOfShift.EndNightShift;
                dataRow[6] = sheduleOfShift.Day_Of_Week_Off;
                dataRow[7] = sheduleOfShift.DayOff;
                dataRow[8] = sheduleOfShift.StartDayOff;
                dataRow[9] = sheduleOfShift.Day_Of_Week_End;
                dataRow[10] = sheduleOfShift.EndDayOff;
                dataRow[11] = sheduleOfShift.EndDayOffTime;
                dataRow[12] = sheduleOfShift.DayOfWeek_D;
                dataRow[13] = sheduleOfShift.DayShift;
                dataRow[14] = sheduleOfShift.StartDayShift;
                dataRow[15] = sheduleOfShift.EndDayShift;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CalculationLogic(ScheduleOfShift sheduleOfShift, int numberOfHours)
        {
            try
            {
                if (condition == 0)
                {
                    date = new DateTime(2021, 1, 1);
                    date = date.AddHours(numberOfHours);
                    condition++;
                }

                sheduleOfShift.NameOfMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
                sheduleOfShift.DayOfWeek_D = date.DayOfWeek.ToString();
                sheduleOfShift.DayShift = date.ToLongDateString();
                sheduleOfShift.StartDayShift = date.ToShortTimeString();
                date = date.AddHours(12);
                sheduleOfShift.EndDayShift = date.ToShortTimeString();
                date = date.AddHours(24);

                sheduleOfShift.Day_Of_Week_N = date.DayOfWeek.ToString();
                sheduleOfShift.NightShift = date.ToLongDateString();
                sheduleOfShift.StartNightShift = date.ToShortTimeString();
                date = date.AddHours(12);
                sheduleOfShift.EndNightShift = date.ToShortTimeString();

                sheduleOfShift.Day_Of_Week_Off = date.DayOfWeek.ToString();
                sheduleOfShift.DayOff = date.ToLongDateString();
                sheduleOfShift.StartDayOff = date.ToShortTimeString();
                date = date.AddHours(39);

                sheduleOfShift.Day_Of_Week_End = date.DayOfWeek.ToString();
                sheduleOfShift.EndDayOff = date.ToLongDateString();
                sheduleOfShift.EndDayOffTime = date.ToShortTimeString();
                date = date.AddHours(9);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CalculationLogicForFourthShift(ScheduleOfShift sheduleOfShift, int numberOfHours)
        {
            try
            {
                if (condition == 0)
                {
                    date = new DateTime(2021, 1, 1);
                    date = date.AddHours(numberOfHours);
                    condition++;
                }

                sheduleOfShift.NameOfMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
                sheduleOfShift.Day_Of_Week_N = date.DayOfWeek.ToString();
                sheduleOfShift.NightShift = date.ToLongDateString();
                sheduleOfShift.StartNightShift = date.ToShortTimeString();
                date = date.AddHours(12);
                sheduleOfShift.EndNightShift = date.ToShortTimeString();

                sheduleOfShift.Day_Of_Week_Off = date.DayOfWeek.ToString();
                sheduleOfShift.DayOff = date.ToLongDateString();
                sheduleOfShift.StartDayOff = date.ToShortTimeString();
                date = date.AddHours(39);
                sheduleOfShift.Day_Of_Week_End = date.DayOfWeek.ToString();
                sheduleOfShift.EndDayOff = date.ToLongDateString();
                sheduleOfShift.EndDayOffTime = date.ToShortTimeString();
                date = date.AddHours(9);

                sheduleOfShift.DayOfWeek_D = date.DayOfWeek.ToString();
                sheduleOfShift.DayShift = date.ToLongDateString();
                sheduleOfShift.StartDayShift = date.ToShortTimeString();
                date = date.AddHours(12);
                sheduleOfShift.EndDayShift = date.ToShortTimeString();
                date = date.AddHours(24);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CalculationOfDataOnTheShift(ScheduleOfShift sheduleOfShift, int number, SqlDataAdapter adapter, DataTable table, int numberOfHours)
        {
            try
            {
                int i = 1;

                if (numberOfHours == 20)
                {
                    number = 1218;

                    for (; i <= number; i++)
                    {
                        DataRow dataRow = table.NewRow();

                        CalculationLogicForFourthShift(sheduleOfShift, numberOfHours);
                        FillingInRowsForFourthShift(sheduleOfShift, dataRow);

                        table.Rows.Add(dataRow);

                        adapter.UpdateCommand = new SqlCommandBuilder(adapter).GetUpdateCommand();

                        adapter.Update(table);
                    }
                }
                else
                {
                    for (; i <= number; i++)
                    {
                        DataRow dataRow = table.NewRow();

                        CalculationLogic(sheduleOfShift, numberOfHours);
                        FillingInRows(sheduleOfShift, dataRow);

                        table.Rows.Add(dataRow);

                        adapter.UpdateCommand = new SqlCommandBuilder(adapter).GetUpdateCommand();

                        adapter.Update(table);
                    }
                }

                if (i > number)
                {
                    condition = 0;
                    date = new DateTime(2021, 1, 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// InformationAboutShifts//////////////////////////////////////////////////////////////////////////
        /// 

        private int numOfDayOfMonth = 0;

        private string dayShift = "";
        private string nightShift = "";
        private string dayOff = "";
        private string endDayOff = "";

        private string firstCommandText = "";
        private string secondCommandText = "";
        private string thirdCommandText = "";
        private string fourthCommandText = "";

        public List<int> numberOfDayShift = new List<int>();
        public List<int> numberOfNightShift = new List<int>();
        public List<int> numberOfDayOff = new List<int>();
        public List<int> numberOfEndDayOff = new List<int>();
        private List<int> numberOfDay = new List<int>();

        private string[] arrayOfRequestsFirst_2021 = new string[12];//возможно перенести в метод и там выделять память
        private string[] arrayOfRequestsFirst_2022 = new string[12];
        private string[] arrayOfRequestsFirst_2023 = new string[12];
        private string[] arrayOfRequestsFirst_2024 = new string[12];
        private string[] arrayOfRequestsFirst_2025 = new string[12];
        private string[] arrayOfRequestsFirst_2026 = new string[12];
        private string[] arrayOfRequestsFirst_2027 = new string[12];
        private string[] arrayOfRequestsFirst_2028 = new string[12];
        private string[] arrayOfRequestsFirst_2029 = new string[12];
        private string[] arrayOfRequestsFirst_2030 = new string[12];

        private string[] arrayOfRequestsSecond_2021 = new string[12];
        private string[] arrayOfRequestsSecond_2022 = new string[12];
        private string[] arrayOfRequestsSecond_2023 = new string[12];
        private string[] arrayOfRequestsSecond_2024 = new string[12];
        private string[] arrayOfRequestsSecond_2025 = new string[12];
        private string[] arrayOfRequestsSecond_2026 = new string[12];
        private string[] arrayOfRequestsSecond_2027 = new string[12];
        private string[] arrayOfRequestsSecond_2028 = new string[12];
        private string[] arrayOfRequestsSecond_2029 = new string[12];
        private string[] arrayOfRequestsSecond_2030 = new string[12];

        private string[] arrayOfRequestsThird_2021 = new string[12];
        private string[] arrayOfRequestsThird_2022 = new string[12];
        private string[] arrayOfRequestsThird_2023 = new string[12];
        private string[] arrayOfRequestsThird_2024 = new string[12];
        private string[] arrayOfRequestsThird_2025 = new string[12];
        private string[] arrayOfRequestsThird_2026 = new string[12];
        private string[] arrayOfRequestsThird_2027 = new string[12];
        private string[] arrayOfRequestsThird_2028 = new string[12];
        private string[] arrayOfRequestsThird_2029 = new string[12];
        private string[] arrayOfRequestsThird_2030 = new string[12];

        private string[] arrayOfRequestsFourth_2021 = new string[12];
        private string[] arrayOfRequestsFourth_2022 = new string[12];
        private string[] arrayOfRequestsFourth_2023 = new string[12];
        private string[] arrayOfRequestsFourth_2024 = new string[12];
        private string[] arrayOfRequestsFourth_2025 = new string[12];
        private string[] arrayOfRequestsFourth_2026 = new string[12];
        private string[] arrayOfRequestsFourth_2027 = new string[12];
        private string[] arrayOfRequestsFourth_2028 = new string[12];
        private string[] arrayOfRequestsFourth_2029 = new string[12];
        private string[] arrayOfRequestsFourth_2030 = new string[12];

        public void GetInformationAboutFirstShift(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm)
        {
            SqlCommand first = new SqlCommand();
            first.CommandText = firstCommandText;
            first.Connection = (SqlConnection)connection;

            SqlDataAdapter adapterFirst = new SqlDataAdapter(first);
            DataTable tableFirst = new DataTable();
            adapterFirst.Fill(tableFirst);

            if (passageControl == true)
            {
                FillingInWithData(tableFirst, numOfDays, true);
            }
            else
            {
                FillingInWithData(tableFirst, 0, false);
            }
        }

        public void GetInformationAboutSecondShift(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm)
        {
            SqlCommand second = new SqlCommand();
            second.CommandText = secondCommandText;
            second.Connection = (SqlConnection)connection;

            SqlDataAdapter adapterSecond = new SqlDataAdapter(second);
            DataTable tableSecond = new DataTable();
            adapterSecond.Fill(tableSecond);

            if (passageControl == true)
            {
                FillingInWithData(tableSecond, numOfDays, true);
            }
            else
            {
                FillingInWithData(tableSecond, 0, false);
            }
        }

        public void GetInformationAboutThirdShift(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm)
        {
            SqlCommand third = new SqlCommand();
            third.CommandText = thirdCommandText;
            third.Connection = (SqlConnection)connection;

            SqlDataAdapter adapterThird = new SqlDataAdapter(third);
            DataTable tableThird = new DataTable();
            adapterThird.Fill(tableThird);

            if (passageControl == true)
            {
                FillingInWithData(tableThird, numOfDays, true);
            }
            else
            {
                FillingInWithData(tableThird, 0, false);
            }
        }

        public void GetInformationAboutFourthShift(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm)
        {
            SqlCommand fourth = new SqlCommand();
            fourth.CommandText = fourthCommandText;
            fourth.Connection = (SqlConnection)connection;

            SqlDataAdapter adapterFourth = new SqlDataAdapter(fourth);
            DataTable tableFourth = new DataTable();
            adapterFourth.Fill(tableFourth);

            if (passageControl == true)
            {
                FillingInWithData(tableFourth, numOfDays, true);
            }
            else
            {
                FillingInWithData(tableFourth, 0, false);
            }

            ReleaseConnection(connection);
        }
        public void FillingInWithData(DataTable table, int numOfDays, bool passageControl)
        {
            try
            {
                int number = 0;//the number of iterations depending on the number of days in the month

                if (passageControl == true)
                {
                    if (numOfDays == 31 || numOfDays == 28 || numOfDays == 29)
                    {
                        number = 7;
                    }
                    if (numOfDays == 30)
                    {
                        number = 8;
                    }
                }
                else
                {
                    if (numOfDayOfMonth == 31 || numOfDayOfMonth == 28 || numOfDayOfMonth == 29)
                    {
                        number = 7;
                    }
                    if (numOfDayOfMonth == 30)
                    {
                        number = 8;
                    }
                }

                foreach (DataRow row in table.Rows)
                {
                    number--;
                    dayShift = row[0].ToString();
                    GetSpecificDayShiftNumber();
                    int day = Convert.ToInt32(dayShift);
                    numberOfDayShift.Add(day);

                    nightShift = row[1].ToString();
                    GetSpecificNightShiftNumber();
                    int night = Convert.ToInt32(nightShift);
                    numberOfNightShift.Add(night);

                    dayOff = row[2].ToString();
                    GetSpecificStartDayOffNumber();
                    int dayOff1 = Convert.ToInt32(dayOff);
                    numberOfDayOff.Add(dayOff1);

                    if (number >= 0)
                    {
                        endDayOff = row[3].ToString();
                        GetSpecificEndDayOffNumber();
                        int endDayOff1 = Convert.ToInt32(endDayOff);
                        numberOfEndDayOff.Add(endDayOff1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SelectingSaturdayOrSunday(string year, string month)
        {
            int year_ = 0;

            int numberOfMonth = 0;

            year_ = Convert.ToInt32(year);

            switch (month)
            {
                case "January":
                    numberOfMonth = 1;
                    break;
                case "February":
                    numberOfMonth = 2;
                    break;
                case "March":
                    numberOfMonth = 3;
                    break;
                case "April":
                    numberOfMonth = 4;
                    break;
                case "May":
                    numberOfMonth = 5;
                    break;
                case "June":
                    numberOfMonth = 6;
                    break;
                case "July":
                    numberOfMonth = 7;
                    break;
                case "August":
                    numberOfMonth = 8;
                    break;
                case "September":
                    numberOfMonth = 9;
                    break;
                case "October":
                    numberOfMonth = 10;
                    break;
                case "November":
                    numberOfMonth = 11;
                    break;
                case "December":
                    numberOfMonth = 12;
                    break;
                default:
                    break;
            }

            DateTime date = new DateTime(year_, numberOfMonth, 1);

            numOfDayOfMonth = DateTime.DaysInMonth(year_, numberOfMonth);

            for (int i = 0; i < numOfDayOfMonth; i++)
            {
                if (date.DayOfWeek.Equals(DayOfWeek.Saturday) || date.DayOfWeek.Equals(DayOfWeek.Sunday))
                {
                    numberOfDay.Add(i + 1);
                }

                date = date.AddDays(1);
            }
        }

        public void GetSpecificDayShiftNumber()
        {
            try
            {
                dayShift = dayShift.Substring(0, dayShift.Length - 16);

                if (dayShift.Equals("01") || dayShift.Equals("02") || dayShift.Equals("03") || dayShift.Equals("04")
                    || dayShift.Equals("05") || dayShift.Equals("06") || dayShift.Equals("07") || dayShift.Equals("08")
                    || dayShift.Equals("09"))
                {
                    dayShift = dayShift.Substring(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetSpecificNightShiftNumber()
        {
            try
            {
                nightShift = nightShift.Substring(0, nightShift.Length - 16);

                if (nightShift.Equals("01") || nightShift.Equals("02") || nightShift.Equals("03") || nightShift.Equals("04")
                    || nightShift.Equals("05") || nightShift.Equals("06") || nightShift.Equals("07") || nightShift.Equals("08")
                    || nightShift.Equals("09"))
                {
                    nightShift = nightShift.Substring(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetSpecificStartDayOffNumber()
        {
            try
            {
                dayOff = dayOff.Substring(0, dayOff.Length - 16);

                if (dayOff.Equals("01") || dayOff.Equals("02") || dayOff.Equals("03") || dayOff.Equals("04")
                    || dayOff.Equals("05") || dayOff.Equals("06") || dayOff.Equals("07") || dayOff.Equals("08")
                    || dayOff.Equals("09"))
                {
                    dayOff = dayOff.Substring(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetSpecificEndDayOffNumber()
        {
            try
            {
                endDayOff = endDayOff.Substring(0, endDayOff.Length - 16);

                if (endDayOff.Equals("01") || endDayOff.Equals("02") || endDayOff.Equals("03") || endDayOff.Equals("04")
                    || endDayOff.Equals("05") || endDayOff.Equals("06") || endDayOff.Equals("07") || endDayOff.Equals("08")
                    || endDayOff.Equals("09"))
                {
                    endDayOff = endDayOff.Substring(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SelectingYear(string numberOfYear, bool changingInformation, string numberOfYearFromForm)
        {
            try
            {
                string year_ = "";

                if (changingInformation == true)
                {
                    year_ = numberOfYear;
                }
                else
                {
                    year_ = numberOfYearFromForm;
                }

                switch (year_)
                {
                    case "2021":
                        Requests_2021_First();
                        Requests_2021_Second();
                        Requests_2021_Third();
                        Requests_2021_Fourth();
                        break;
                    case "2022":
                        Requests_2022_First();
                        Requests_2022_Second();
                        Requests_2022_Third();
                        Requests_2022_Fourth();
                        break;
                    case "2023":
                        Requests_2023_First();
                        Requests_2023_Second();
                        Requests_2023_Third();
                        Requests_2023_Fourth();
                        break;
                    case "2024":
                        Requests_2024_First();
                        Requests_2024_Second();
                        Requests_2024_Third();
                        Requests_2024_Fourth();
                        break;
                    case "2025":
                        Requests_2025_First();
                        Requests_2025_Second();
                        Requests_2025_Third();
                        Requests_2025_Fourth();
                        break;
                    case "2026":
                        Requests_2026_First();
                        Requests_2026_Second();
                        Requests_2026_Third();
                        Requests_2026_Fourth();
                        break;
                    case "2027":
                        Requests_2027_First();
                        Requests_2027_Second();
                        Requests_2027_Third();
                        Requests_2027_Fourth();
                        break;
                    case "2028":
                        Requests_2028_First();
                        Requests_2028_Second();
                        Requests_2028_Third();
                        Requests_2028_Fourth();
                        break;
                    case "2029":
                        Requests_2029_First();
                        Requests_2029_Second();
                        Requests_2029_Third();
                        Requests_2029_Fourth();
                        break;
                    case "2030":
                        Requests_2030_First();
                        Requests_2030_Second();
                        Requests_2030_Third();
                        Requests_2030_Fourth();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SelectingMonth(string nameOfYear, string nameOfMonths, bool changingInformation,
                                   string nameOfYearFromForm, string nameOfMonthFromForm)
        {
            try
            {
                string month = "";
                string year_ = "";

                if (changingInformation == true)
                {
                    month = nameOfMonths;
                    year_ = nameOfYear;
                }
                else
                {
                    month = nameOfMonthFromForm;
                    year_ = nameOfYearFromForm;
                }

                switch (month)
                {
                    case "January":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[0];
                                secondCommandText = arrayOfRequestsSecond_2021[0];
                                thirdCommandText = arrayOfRequestsThird_2021[0];
                                fourthCommandText = arrayOfRequestsFourth_2021[0];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[0];
                                secondCommandText = arrayOfRequestsSecond_2022[0];
                                thirdCommandText = arrayOfRequestsThird_2022[0];
                                fourthCommandText = arrayOfRequestsFourth_2022[0];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[0];
                                secondCommandText = arrayOfRequestsSecond_2023[0];
                                thirdCommandText = arrayOfRequestsThird_2023[0];
                                fourthCommandText = arrayOfRequestsFourth_2023[0];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[0];
                                secondCommandText = arrayOfRequestsSecond_2024[0];
                                thirdCommandText = arrayOfRequestsThird_2024[0];
                                fourthCommandText = arrayOfRequestsFourth_2024[0];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[0];
                                secondCommandText = arrayOfRequestsSecond_2025[0];
                                thirdCommandText = arrayOfRequestsThird_2025[0];
                                fourthCommandText = arrayOfRequestsFourth_2025[0];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[0];
                                secondCommandText = arrayOfRequestsSecond_2026[0];
                                thirdCommandText = arrayOfRequestsThird_2026[0];
                                fourthCommandText = arrayOfRequestsFourth_2026[0];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[0];
                                secondCommandText = arrayOfRequestsSecond_2027[0];
                                thirdCommandText = arrayOfRequestsThird_2027[0];
                                fourthCommandText = arrayOfRequestsFourth_2027[0];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[0];
                                secondCommandText = arrayOfRequestsSecond_2028[0];
                                thirdCommandText = arrayOfRequestsThird_2028[0];
                                fourthCommandText = arrayOfRequestsFourth_2028[0];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[0];
                                secondCommandText = arrayOfRequestsSecond_2029[0];
                                thirdCommandText = arrayOfRequestsThird_2029[0];
                                fourthCommandText = arrayOfRequestsFourth_2029[0];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[0];
                                secondCommandText = arrayOfRequestsSecond_2030[0];
                                thirdCommandText = arrayOfRequestsThird_2030[0];
                                fourthCommandText = arrayOfRequestsFourth_2030[0];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "February":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[1];
                                secondCommandText = arrayOfRequestsSecond_2021[1];
                                thirdCommandText = arrayOfRequestsThird_2021[1];
                                fourthCommandText = arrayOfRequestsFourth_2021[1];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[1];
                                secondCommandText = arrayOfRequestsSecond_2022[1];
                                thirdCommandText = arrayOfRequestsThird_2022[1];
                                fourthCommandText = arrayOfRequestsFourth_2022[1];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[1];
                                secondCommandText = arrayOfRequestsSecond_2023[1];
                                thirdCommandText = arrayOfRequestsThird_2023[1];
                                fourthCommandText = arrayOfRequestsFourth_2023[1];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[1];
                                secondCommandText = arrayOfRequestsSecond_2024[1];
                                thirdCommandText = arrayOfRequestsThird_2024[1];
                                fourthCommandText = arrayOfRequestsFourth_2024[1];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[1];
                                secondCommandText = arrayOfRequestsSecond_2025[1];
                                thirdCommandText = arrayOfRequestsThird_2025[1];
                                fourthCommandText = arrayOfRequestsFourth_2025[1];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[1];
                                secondCommandText = arrayOfRequestsSecond_2026[1];
                                thirdCommandText = arrayOfRequestsThird_2026[1];
                                fourthCommandText = arrayOfRequestsFourth_2026[1];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[1];
                                secondCommandText = arrayOfRequestsSecond_2027[1];
                                thirdCommandText = arrayOfRequestsThird_2027[1];
                                fourthCommandText = arrayOfRequestsFourth_2027[1];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[1];
                                secondCommandText = arrayOfRequestsSecond_2028[1];
                                thirdCommandText = arrayOfRequestsThird_2028[1];
                                fourthCommandText = arrayOfRequestsFourth_2028[1];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[1];
                                secondCommandText = arrayOfRequestsSecond_2029[1];
                                thirdCommandText = arrayOfRequestsThird_2029[1];
                                fourthCommandText = arrayOfRequestsFourth_2029[1];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[1];
                                secondCommandText = arrayOfRequestsSecond_2030[1];
                                thirdCommandText = arrayOfRequestsThird_2030[1];
                                fourthCommandText = arrayOfRequestsFourth_2030[1];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "March":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[2];
                                secondCommandText = arrayOfRequestsSecond_2021[2];
                                thirdCommandText = arrayOfRequestsThird_2021[2];
                                fourthCommandText = arrayOfRequestsFourth_2021[2];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[2];
                                secondCommandText = arrayOfRequestsSecond_2022[2];
                                thirdCommandText = arrayOfRequestsThird_2022[2];
                                fourthCommandText = arrayOfRequestsFourth_2022[2];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[2];
                                secondCommandText = arrayOfRequestsSecond_2023[2];
                                thirdCommandText = arrayOfRequestsThird_2023[2];
                                fourthCommandText = arrayOfRequestsFourth_2023[2];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[2];
                                secondCommandText = arrayOfRequestsSecond_2024[2];
                                thirdCommandText = arrayOfRequestsThird_2024[2];
                                fourthCommandText = arrayOfRequestsFourth_2024[2];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[2];
                                secondCommandText = arrayOfRequestsSecond_2025[2];
                                thirdCommandText = arrayOfRequestsThird_2025[2];
                                fourthCommandText = arrayOfRequestsFourth_2025[2];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[2];
                                secondCommandText = arrayOfRequestsSecond_2026[2];
                                thirdCommandText = arrayOfRequestsThird_2026[2];
                                fourthCommandText = arrayOfRequestsFourth_2026[2];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[2];
                                secondCommandText = arrayOfRequestsSecond_2027[2];
                                thirdCommandText = arrayOfRequestsThird_2027[2];
                                fourthCommandText = arrayOfRequestsFourth_2027[2];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[2];
                                secondCommandText = arrayOfRequestsSecond_2028[2];
                                thirdCommandText = arrayOfRequestsThird_2028[2];
                                fourthCommandText = arrayOfRequestsFourth_2028[2];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[2];
                                secondCommandText = arrayOfRequestsSecond_2029[2];
                                thirdCommandText = arrayOfRequestsThird_2029[2];
                                fourthCommandText = arrayOfRequestsFourth_2029[2];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[2];
                                secondCommandText = arrayOfRequestsSecond_2030[2];
                                thirdCommandText = arrayOfRequestsThird_2030[2];
                                fourthCommandText = arrayOfRequestsFourth_2030[2];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "April":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[3];
                                secondCommandText = arrayOfRequestsSecond_2021[3];
                                thirdCommandText = arrayOfRequestsThird_2021[3];
                                fourthCommandText = arrayOfRequestsFourth_2021[3];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[3];
                                secondCommandText = arrayOfRequestsSecond_2022[3];
                                thirdCommandText = arrayOfRequestsThird_2022[3];
                                fourthCommandText = arrayOfRequestsFourth_2022[3];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[3];
                                secondCommandText = arrayOfRequestsSecond_2023[3];
                                thirdCommandText = arrayOfRequestsThird_2023[3];
                                fourthCommandText = arrayOfRequestsFourth_2023[3];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[3];
                                secondCommandText = arrayOfRequestsSecond_2024[3];
                                thirdCommandText = arrayOfRequestsThird_2024[3];
                                fourthCommandText = arrayOfRequestsFourth_2024[3];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[3];
                                secondCommandText = arrayOfRequestsSecond_2025[3];
                                thirdCommandText = arrayOfRequestsThird_2025[3];
                                fourthCommandText = arrayOfRequestsFourth_2025[3];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[3];
                                secondCommandText = arrayOfRequestsSecond_2026[3];
                                thirdCommandText = arrayOfRequestsThird_2026[3];
                                fourthCommandText = arrayOfRequestsFourth_2026[3];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[3];
                                secondCommandText = arrayOfRequestsSecond_2027[3];
                                thirdCommandText = arrayOfRequestsThird_2027[3];
                                fourthCommandText = arrayOfRequestsFourth_2027[3];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[3];
                                secondCommandText = arrayOfRequestsSecond_2028[3];
                                thirdCommandText = arrayOfRequestsThird_2028[3];
                                fourthCommandText = arrayOfRequestsFourth_2028[3];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[3];
                                secondCommandText = arrayOfRequestsSecond_2029[3];
                                thirdCommandText = arrayOfRequestsThird_2029[3];
                                fourthCommandText = arrayOfRequestsFourth_2029[3];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[3];
                                secondCommandText = arrayOfRequestsSecond_2030[3];
                                thirdCommandText = arrayOfRequestsThird_2030[3];
                                fourthCommandText = arrayOfRequestsFourth_2030[3];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "May":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[4];
                                secondCommandText = arrayOfRequestsSecond_2021[4];
                                thirdCommandText = arrayOfRequestsThird_2021[4];
                                fourthCommandText = arrayOfRequestsFourth_2021[4];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[4];
                                secondCommandText = arrayOfRequestsSecond_2022[4];
                                thirdCommandText = arrayOfRequestsThird_2022[4];
                                fourthCommandText = arrayOfRequestsFourth_2022[4];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[4];
                                secondCommandText = arrayOfRequestsSecond_2023[4];
                                thirdCommandText = arrayOfRequestsThird_2023[4];
                                fourthCommandText = arrayOfRequestsFourth_2023[4];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[4];
                                secondCommandText = arrayOfRequestsSecond_2024[4];
                                thirdCommandText = arrayOfRequestsThird_2024[4];
                                fourthCommandText = arrayOfRequestsFourth_2024[4];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[4];
                                secondCommandText = arrayOfRequestsSecond_2025[4];
                                thirdCommandText = arrayOfRequestsThird_2025[4];
                                fourthCommandText = arrayOfRequestsFourth_2025[4];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[4];
                                secondCommandText = arrayOfRequestsSecond_2026[4];
                                thirdCommandText = arrayOfRequestsThird_2026[4];
                                fourthCommandText = arrayOfRequestsFourth_2026[4];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[4];
                                secondCommandText = arrayOfRequestsSecond_2027[4];
                                thirdCommandText = arrayOfRequestsThird_2027[4];
                                fourthCommandText = arrayOfRequestsFourth_2027[4];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[4];
                                secondCommandText = arrayOfRequestsSecond_2028[4];
                                thirdCommandText = arrayOfRequestsThird_2028[4];
                                fourthCommandText = arrayOfRequestsFourth_2028[4];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[4];
                                secondCommandText = arrayOfRequestsSecond_2029[4];
                                thirdCommandText = arrayOfRequestsThird_2029[4];
                                fourthCommandText = arrayOfRequestsFourth_2029[4];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[4];
                                secondCommandText = arrayOfRequestsSecond_2030[4];
                                thirdCommandText = arrayOfRequestsThird_2030[4];
                                fourthCommandText = arrayOfRequestsFourth_2030[4];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "June":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[5];
                                secondCommandText = arrayOfRequestsSecond_2021[5];
                                thirdCommandText = arrayOfRequestsThird_2021[5];
                                fourthCommandText = arrayOfRequestsFourth_2021[5];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[5];
                                secondCommandText = arrayOfRequestsSecond_2022[5];
                                thirdCommandText = arrayOfRequestsThird_2022[5];
                                fourthCommandText = arrayOfRequestsFourth_2022[5];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[5];
                                secondCommandText = arrayOfRequestsSecond_2023[5];
                                thirdCommandText = arrayOfRequestsThird_2023[5];
                                fourthCommandText = arrayOfRequestsFourth_2023[5];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[5];
                                secondCommandText = arrayOfRequestsSecond_2024[5];
                                thirdCommandText = arrayOfRequestsThird_2024[5];
                                fourthCommandText = arrayOfRequestsFourth_2024[5];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[5];
                                secondCommandText = arrayOfRequestsSecond_2025[5];
                                thirdCommandText = arrayOfRequestsThird_2025[5];
                                fourthCommandText = arrayOfRequestsFourth_2025[5];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[5];
                                secondCommandText = arrayOfRequestsSecond_2026[5];
                                thirdCommandText = arrayOfRequestsThird_2026[5];
                                fourthCommandText = arrayOfRequestsFourth_2026[5];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[5];
                                secondCommandText = arrayOfRequestsSecond_2027[5];
                                thirdCommandText = arrayOfRequestsThird_2027[5];
                                fourthCommandText = arrayOfRequestsFourth_2027[5];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[5];
                                secondCommandText = arrayOfRequestsSecond_2028[5];
                                thirdCommandText = arrayOfRequestsThird_2028[5];
                                fourthCommandText = arrayOfRequestsFourth_2028[5];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[5];
                                secondCommandText = arrayOfRequestsSecond_2029[5];
                                thirdCommandText = arrayOfRequestsThird_2029[5];
                                fourthCommandText = arrayOfRequestsFourth_2029[5];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[5];
                                secondCommandText = arrayOfRequestsSecond_2030[5];
                                thirdCommandText = arrayOfRequestsThird_2030[5];
                                fourthCommandText = arrayOfRequestsFourth_2030[5];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "July":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[6];
                                secondCommandText = arrayOfRequestsSecond_2021[6];
                                thirdCommandText = arrayOfRequestsThird_2021[6];
                                fourthCommandText = arrayOfRequestsFourth_2021[6];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[6];
                                secondCommandText = arrayOfRequestsSecond_2022[6];
                                thirdCommandText = arrayOfRequestsThird_2022[6];
                                fourthCommandText = arrayOfRequestsFourth_2022[6];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[6];
                                secondCommandText = arrayOfRequestsSecond_2023[6];
                                thirdCommandText = arrayOfRequestsThird_2023[6];
                                fourthCommandText = arrayOfRequestsFourth_2023[6];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[6];
                                secondCommandText = arrayOfRequestsSecond_2024[6];
                                thirdCommandText = arrayOfRequestsThird_2024[6];
                                fourthCommandText = arrayOfRequestsFourth_2024[6];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[6];
                                secondCommandText = arrayOfRequestsSecond_2025[6];
                                thirdCommandText = arrayOfRequestsThird_2025[6];
                                fourthCommandText = arrayOfRequestsFourth_2025[6];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[6];
                                secondCommandText = arrayOfRequestsSecond_2026[6];
                                thirdCommandText = arrayOfRequestsThird_2026[6];
                                fourthCommandText = arrayOfRequestsFourth_2026[6];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[6];
                                secondCommandText = arrayOfRequestsSecond_2027[6];
                                thirdCommandText = arrayOfRequestsThird_2027[6];
                                fourthCommandText = arrayOfRequestsFourth_2027[6];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[6];
                                secondCommandText = arrayOfRequestsSecond_2028[6];
                                thirdCommandText = arrayOfRequestsThird_2028[6];
                                fourthCommandText = arrayOfRequestsFourth_2028[6];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[6];
                                secondCommandText = arrayOfRequestsSecond_2029[6];
                                thirdCommandText = arrayOfRequestsThird_2029[6];
                                fourthCommandText = arrayOfRequestsFourth_2029[6];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[6];
                                secondCommandText = arrayOfRequestsSecond_2030[6];
                                thirdCommandText = arrayOfRequestsThird_2030[6];
                                fourthCommandText = arrayOfRequestsFourth_2030[6];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "August":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[7];
                                secondCommandText = arrayOfRequestsSecond_2021[7];
                                thirdCommandText = arrayOfRequestsThird_2021[7];
                                fourthCommandText = arrayOfRequestsFourth_2021[7];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[7];
                                secondCommandText = arrayOfRequestsSecond_2022[7];
                                thirdCommandText = arrayOfRequestsThird_2022[7];
                                fourthCommandText = arrayOfRequestsFourth_2022[7];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[7];
                                secondCommandText = arrayOfRequestsSecond_2023[7];
                                thirdCommandText = arrayOfRequestsThird_2023[7];
                                fourthCommandText = arrayOfRequestsFourth_2023[7];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[7];
                                secondCommandText = arrayOfRequestsSecond_2024[7];
                                thirdCommandText = arrayOfRequestsThird_2024[7];
                                fourthCommandText = arrayOfRequestsFourth_2024[7];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[7];
                                secondCommandText = arrayOfRequestsSecond_2025[7];
                                thirdCommandText = arrayOfRequestsThird_2025[7];
                                fourthCommandText = arrayOfRequestsFourth_2025[7];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[7];
                                secondCommandText = arrayOfRequestsSecond_2026[7];
                                thirdCommandText = arrayOfRequestsThird_2026[7];
                                fourthCommandText = arrayOfRequestsFourth_2026[7];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[7];
                                secondCommandText = arrayOfRequestsSecond_2027[7];
                                thirdCommandText = arrayOfRequestsThird_2027[7];
                                fourthCommandText = arrayOfRequestsFourth_2027[7];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[7];
                                secondCommandText = arrayOfRequestsSecond_2028[7];
                                thirdCommandText = arrayOfRequestsThird_2028[7];
                                fourthCommandText = arrayOfRequestsFourth_2028[7];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[7];
                                secondCommandText = arrayOfRequestsSecond_2029[7];
                                thirdCommandText = arrayOfRequestsThird_2029[7];
                                fourthCommandText = arrayOfRequestsFourth_2029[7];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[7];
                                secondCommandText = arrayOfRequestsSecond_2030[7];
                                thirdCommandText = arrayOfRequestsThird_2030[7];
                                fourthCommandText = arrayOfRequestsFourth_2030[7];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "September":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[8];
                                secondCommandText = arrayOfRequestsSecond_2021[8];
                                thirdCommandText = arrayOfRequestsThird_2021[8];
                                fourthCommandText = arrayOfRequestsFourth_2021[8];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[8];
                                secondCommandText = arrayOfRequestsSecond_2022[8];
                                thirdCommandText = arrayOfRequestsThird_2022[8];
                                fourthCommandText = arrayOfRequestsFourth_2022[8];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[8];
                                secondCommandText = arrayOfRequestsSecond_2023[8];
                                thirdCommandText = arrayOfRequestsThird_2023[8];
                                fourthCommandText = arrayOfRequestsFourth_2023[8];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[8];
                                secondCommandText = arrayOfRequestsSecond_2024[8];
                                thirdCommandText = arrayOfRequestsThird_2024[8];
                                fourthCommandText = arrayOfRequestsFourth_2024[8];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[8];
                                secondCommandText = arrayOfRequestsSecond_2025[8];
                                thirdCommandText = arrayOfRequestsThird_2025[8];
                                fourthCommandText = arrayOfRequestsFourth_2025[8];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[8];
                                secondCommandText = arrayOfRequestsSecond_2026[8];
                                thirdCommandText = arrayOfRequestsThird_2026[8];
                                fourthCommandText = arrayOfRequestsFourth_2026[8];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[8];
                                secondCommandText = arrayOfRequestsSecond_2027[8];
                                thirdCommandText = arrayOfRequestsThird_2027[8];
                                fourthCommandText = arrayOfRequestsFourth_2027[8];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[8];
                                secondCommandText = arrayOfRequestsSecond_2028[8];
                                thirdCommandText = arrayOfRequestsThird_2028[8];
                                fourthCommandText = arrayOfRequestsFourth_2028[8];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[8];
                                secondCommandText = arrayOfRequestsSecond_2029[8];
                                thirdCommandText = arrayOfRequestsThird_2029[8];
                                fourthCommandText = arrayOfRequestsFourth_2029[8];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[8];
                                secondCommandText = arrayOfRequestsSecond_2030[8];
                                thirdCommandText = arrayOfRequestsThird_2030[8];
                                fourthCommandText = arrayOfRequestsFourth_2030[8];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "October":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[9];
                                secondCommandText = arrayOfRequestsSecond_2021[9];
                                thirdCommandText = arrayOfRequestsThird_2021[9];
                                fourthCommandText = arrayOfRequestsFourth_2021[9];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[9];
                                secondCommandText = arrayOfRequestsSecond_2022[9];
                                thirdCommandText = arrayOfRequestsThird_2022[9];
                                fourthCommandText = arrayOfRequestsFourth_2022[9];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[9];
                                secondCommandText = arrayOfRequestsSecond_2023[9];
                                thirdCommandText = arrayOfRequestsThird_2023[9];
                                fourthCommandText = arrayOfRequestsFourth_2023[9];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[9];
                                secondCommandText = arrayOfRequestsSecond_2024[9];
                                thirdCommandText = arrayOfRequestsThird_2024[9];
                                fourthCommandText = arrayOfRequestsFourth_2024[9];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[9];
                                secondCommandText = arrayOfRequestsSecond_2025[9];
                                thirdCommandText = arrayOfRequestsThird_2025[9];
                                fourthCommandText = arrayOfRequestsFourth_2025[9];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[9];
                                secondCommandText = arrayOfRequestsSecond_2026[9];
                                thirdCommandText = arrayOfRequestsThird_2026[9];
                                fourthCommandText = arrayOfRequestsFourth_2026[9];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[9];
                                secondCommandText = arrayOfRequestsSecond_2027[9];
                                thirdCommandText = arrayOfRequestsThird_2027[9];
                                fourthCommandText = arrayOfRequestsFourth_2027[9];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[9];
                                secondCommandText = arrayOfRequestsSecond_2028[9];
                                thirdCommandText = arrayOfRequestsThird_2028[9];
                                fourthCommandText = arrayOfRequestsFourth_2028[9];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[9];
                                secondCommandText = arrayOfRequestsSecond_2029[9];
                                thirdCommandText = arrayOfRequestsThird_2029[9];
                                fourthCommandText = arrayOfRequestsFourth_2029[9];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[9];
                                secondCommandText = arrayOfRequestsSecond_2030[9];
                                thirdCommandText = arrayOfRequestsThird_2030[9];
                                fourthCommandText = arrayOfRequestsFourth_2030[9];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "November":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[10];
                                secondCommandText = arrayOfRequestsSecond_2021[10];
                                thirdCommandText = arrayOfRequestsThird_2021[10];
                                fourthCommandText = arrayOfRequestsFourth_2021[10];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[10];
                                secondCommandText = arrayOfRequestsSecond_2022[10];
                                thirdCommandText = arrayOfRequestsThird_2022[10];
                                fourthCommandText = arrayOfRequestsFourth_2022[10];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[10];
                                secondCommandText = arrayOfRequestsSecond_2023[10];
                                thirdCommandText = arrayOfRequestsThird_2023[10];
                                fourthCommandText = arrayOfRequestsFourth_2023[10];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[10];
                                secondCommandText = arrayOfRequestsSecond_2024[10];
                                thirdCommandText = arrayOfRequestsThird_2024[10];
                                fourthCommandText = arrayOfRequestsFourth_2024[10];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[10];
                                secondCommandText = arrayOfRequestsSecond_2025[10];
                                thirdCommandText = arrayOfRequestsThird_2025[10];
                                fourthCommandText = arrayOfRequestsFourth_2025[10];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[10];
                                secondCommandText = arrayOfRequestsSecond_2026[10];
                                thirdCommandText = arrayOfRequestsThird_2026[10];
                                fourthCommandText = arrayOfRequestsFourth_2026[10];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[10];
                                secondCommandText = arrayOfRequestsSecond_2027[10];
                                thirdCommandText = arrayOfRequestsThird_2027[10];
                                fourthCommandText = arrayOfRequestsFourth_2027[10];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[10];
                                secondCommandText = arrayOfRequestsSecond_2028[10];
                                thirdCommandText = arrayOfRequestsThird_2028[10];
                                fourthCommandText = arrayOfRequestsFourth_2028[10];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[10];
                                secondCommandText = arrayOfRequestsSecond_2029[10];
                                thirdCommandText = arrayOfRequestsThird_2029[10];
                                fourthCommandText = arrayOfRequestsFourth_2029[10];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[10];
                                secondCommandText = arrayOfRequestsSecond_2030[10];
                                thirdCommandText = arrayOfRequestsThird_2030[10];
                                fourthCommandText = arrayOfRequestsFourth_2030[10];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "December":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[11];
                                secondCommandText = arrayOfRequestsSecond_2021[11];
                                thirdCommandText = arrayOfRequestsThird_2021[11];
                                fourthCommandText = arrayOfRequestsFourth_2021[11];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[11];
                                secondCommandText = arrayOfRequestsSecond_2022[11];
                                thirdCommandText = arrayOfRequestsThird_2022[11];
                                fourthCommandText = arrayOfRequestsFourth_2022[11];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[11];
                                secondCommandText = arrayOfRequestsSecond_2023[11];
                                thirdCommandText = arrayOfRequestsThird_2023[11];
                                fourthCommandText = arrayOfRequestsFourth_2023[11];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[11];
                                secondCommandText = arrayOfRequestsSecond_2024[11];
                                thirdCommandText = arrayOfRequestsThird_2024[11];
                                fourthCommandText = arrayOfRequestsFourth_2024[11];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[11];
                                secondCommandText = arrayOfRequestsSecond_2025[11];
                                thirdCommandText = arrayOfRequestsThird_2025[11];
                                fourthCommandText = arrayOfRequestsFourth_2025[11];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[11];
                                secondCommandText = arrayOfRequestsSecond_2026[11];
                                thirdCommandText = arrayOfRequestsThird_2026[11];
                                fourthCommandText = arrayOfRequestsFourth_2026[11];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[11];
                                secondCommandText = arrayOfRequestsSecond_2027[11];
                                thirdCommandText = arrayOfRequestsThird_2027[11];
                                fourthCommandText = arrayOfRequestsFourth_2027[11];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[11];
                                secondCommandText = arrayOfRequestsSecond_2028[11];
                                thirdCommandText = arrayOfRequestsThird_2028[11];
                                fourthCommandText = arrayOfRequestsFourth_2028[11];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[11];
                                secondCommandText = arrayOfRequestsSecond_2029[11];
                                thirdCommandText = arrayOfRequestsThird_2029[11];
                                fourthCommandText = arrayOfRequestsFourth_2029[11];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[11];
                                secondCommandText = arrayOfRequestsSecond_2030[11];
                                thirdCommandText = arrayOfRequestsThird_2030[11];
                                fourthCommandText = arrayOfRequestsFourth_2030[11];
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Requests_2021_First()
        {
            arrayOfRequestsFirst_2021[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-01-01' and '2021-01-31'";
            arrayOfRequestsFirst_2021[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-02-01' and '2021-02-28'";
            arrayOfRequestsFirst_2021[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-03-01' and '2021-03-31'";
            arrayOfRequestsFirst_2021[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-03-30' and '2021-04-30'";
            arrayOfRequestsFirst_2021[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-05-01' and '2021-05-31'";
            arrayOfRequestsFirst_2021[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-05-29' and '2021-06-29'";
            arrayOfRequestsFirst_2021[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-06-30' and '2021-07-31'";
            arrayOfRequestsFirst_2021[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-08-01' and '2021-08-31'";
            arrayOfRequestsFirst_2021[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-09-01' and '2021-09-29'";
            arrayOfRequestsFirst_2021[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-09-30' and '2021-10-31'";
            arrayOfRequestsFirst_2021[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-10-28' and '2021-11-28'";
            arrayOfRequestsFirst_2021[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-11-29' and '2021-12-30'";
        }

        public void Requests_2022_First()
        {
            arrayOfRequestsFirst_2022[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2021-12-31' and '2022-01-31'";
            arrayOfRequestsFirst_2022[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-01-31' and '2022-02-28'";
            arrayOfRequestsFirst_2022[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-02-28' and '2022-03-31'";
            arrayOfRequestsFirst_2022[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-04-01' and '2022-04-29'";
            arrayOfRequestsFirst_2022[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-04-30' and '2022-05-31'";
            arrayOfRequestsFirst_2022[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-05-31' and '2022-06-28'";
            arrayOfRequestsFirst_2022[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-06-29' and '2022-07-30'";
            arrayOfRequestsFirst_2022[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-07-31' and '2022-08-31'";
            arrayOfRequestsFirst_2022[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-09-01' and '2022-09-28'";
            arrayOfRequestsFirst_2022[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-09-29' and '2022-10-30'";
            arrayOfRequestsFirst_2022[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-10-27' and '2022-11-27'";
            arrayOfRequestsFirst_2022[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-11-29' and '2022-12-30'";
        }

        public void Requests_2023_First()
        {
            arrayOfRequestsFirst_2023[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2022-12-29' and '2023-01-30'";
            arrayOfRequestsFirst_2023[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2023-02-01' and '2023-02-28'";
            arrayOfRequestsFirst_2023[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2023-02-28' and '2023-03-31'";
            arrayOfRequestsFirst_2023[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2023-04-01' and '2023-04-28'";
            arrayOfRequestsFirst_2023[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2023-04-29' and '2023-05-30'";
            arrayOfRequestsFirst_2023[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2023-05-27' and '2023-06-27'";
            arrayOfRequestsFirst_2023[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2023-06-30' and '2023-07-31'";
            arrayOfRequestsFirst_2023[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2023-07-29' and '2023-08-30'";
            arrayOfRequestsFirst_2023[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2023-08-27' and '2023-09-27'";
            arrayOfRequestsFirst_2023[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2023-09-30' and '2023-10-31'";
            arrayOfRequestsFirst_2023[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2023-10-30' and '2023-11-30'";
            arrayOfRequestsFirst_2023[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2023-11-29' and '2023-12-30'";
        }

        public void Requests_2024_First()
        {
            arrayOfRequestsFirst_2024[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-01-01' and '2024-01-30'";
            arrayOfRequestsFirst_2024[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-02-01' and '2024-02-29'";
            arrayOfRequestsFirst_2024[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-02-28' and '2024-03-31'";
            arrayOfRequestsFirst_2024[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-03-29' and '2024-04-30'";
            arrayOfRequestsFirst_2024[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-04-29' and '2024-05-30'";
            arrayOfRequestsFirst_2024[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-05-27' and '2024-06-27'";
            arrayOfRequestsFirst_2024[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-06-30' and '2024-07-31'";
            arrayOfRequestsFirst_2024[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-07-29' and '2024-08-30'";
            arrayOfRequestsFirst_2024[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-08-27' and '2024-09-27'";
            arrayOfRequestsFirst_2024[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-09-30' and '2024-10-31'";
            arrayOfRequestsFirst_2024[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-10-30' and '2024-11-28'";
            arrayOfRequestsFirst_2024[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-11-29' and '2024-12-30'";
        }

        public void Requests_2025_First()
        {
            arrayOfRequestsFirst_2025[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2024-12-31' and '2025-01-30'";
            arrayOfRequestsFirst_2025[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-01-30' and '2025-02-28'";
            arrayOfRequestsFirst_2025[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-02-28' and '2025-03-31'";
            arrayOfRequestsFirst_2025[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-03-29' and '2025-04-29'";
            arrayOfRequestsFirst_2025[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-04-29' and '2025-05-30'";
            arrayOfRequestsFirst_2025[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-06-01' and '2025-06-28'";
            arrayOfRequestsFirst_2025[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-06-28' and '2025-07-30'";
            arrayOfRequestsFirst_2025[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-07-29' and '2025-08-30'";
            arrayOfRequestsFirst_2025[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-09-01' and '2025-09-28'";
            arrayOfRequestsFirst_2025[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-09-28' and '2025-10-30'";
            arrayOfRequestsFirst_2025[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-10-30' and '2025-11-30'";
            arrayOfRequestsFirst_2025[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-11-29' and '2025-12-30'";
        }

        public void Requests_2026_First()
        {
            arrayOfRequestsFirst_2026[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2025-12-29' and '2026-01-30'";
            arrayOfRequestsFirst_2026[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2026-01-30' and '2026-02-28'";
            arrayOfRequestsFirst_2026[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2026-02-28' and '2026-03-31'";
            arrayOfRequestsFirst_2026[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2026-04-01' and '2026-04-28'";
            arrayOfRequestsFirst_2026[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2026-04-29' and '2026-05-30'";
            arrayOfRequestsFirst_2026[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2026-05-29' and '2026-06-28'";
            arrayOfRequestsFirst_2026[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2026-07-02' and '2026-07-31'";
            arrayOfRequestsFirst_2026[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2026-07-29' and '2026-08-30'";
            arrayOfRequestsFirst_2026[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2026-08-29' and '2026-09-30'";
            arrayOfRequestsFirst_2026[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2026-10-02' and '2026-10-31'";
            arrayOfRequestsFirst_2026[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2026-10-30' and '2026-11-27'";
            arrayOfRequestsFirst_2026[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2026-11-29' and '2026-12-30'";
        }

        public void Requests_2027_First()
        {
            arrayOfRequestsFirst_2027[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-01-01' and '2027-01-30'";
            arrayOfRequestsFirst_2027[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-01-29' and '2027-02-28'";
            arrayOfRequestsFirst_2027[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-02-27' and '2027-03-30'";
            arrayOfRequestsFirst_2027[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-03-29' and '2027-04-28'";
            arrayOfRequestsFirst_2027[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-04-29' and '2027-05-30'";
            arrayOfRequestsFirst_2027[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-05-29' and '2027-06-30'";
            arrayOfRequestsFirst_2027[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-07-01' and '2027-07-31'";
            arrayOfRequestsFirst_2027[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-08-02' and '2027-08-30'";
            arrayOfRequestsFirst_2027[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-08-29' and '2027-09-30'";
            arrayOfRequestsFirst_2027[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-10-01' and '2027-10-31'";
            arrayOfRequestsFirst_2027[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-11-01' and '2027-11-29'";
            arrayOfRequestsFirst_2027[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2027-11-29' and '2027-12-30'";
        }

        public void Requests_2028_First()
        {
            arrayOfRequestsFirst_2028[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-01-01' and '2028-01-30'";
            arrayOfRequestsFirst_2028[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-02-01' and '2028-02-26'";
            arrayOfRequestsFirst_2028[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-02-27' and '2028-03-30'";
            arrayOfRequestsFirst_2028[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-03-29' and '2028-04-28'";
            arrayOfRequestsFirst_2028[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-04-29' and '2028-05-30'";
            arrayOfRequestsFirst_2028[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-05-29' and '2028-06-30'";
            arrayOfRequestsFirst_2028[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-07-01' and '2028-07-31'";
            arrayOfRequestsFirst_2028[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-07-29' and '2028-08-31'";
            arrayOfRequestsFirst_2028[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-08-29' and '2028-09-30'";
            arrayOfRequestsFirst_2028[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-10-01' and '2028-10-31'";
            arrayOfRequestsFirst_2028[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-10-29' and '2028-11-30'";
            arrayOfRequestsFirst_2028[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-11-29' and '2028-12-30'";
        }

        public void Requests_2029_First()
        {
            arrayOfRequestsFirst_2029[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2028-12-29' and '2029-01-30'";
            arrayOfRequestsFirst_2029[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-02-01' and '2029-02-28'";
            arrayOfRequestsFirst_2029[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-02-27' and '2029-03-30'";
            arrayOfRequestsFirst_2029[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-03-29' and '2029-04-28'";
            arrayOfRequestsFirst_2029[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-04-29' and '2029-05-30'";
            arrayOfRequestsFirst_2029[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-05-29' and '2029-06-28'";
            arrayOfRequestsFirst_2029[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-07-01' and '2029-07-31'";
            arrayOfRequestsFirst_2029[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-07-29' and '2029-08-31'";
            arrayOfRequestsFirst_2029[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-08-29' and '2029-09-30'";
            arrayOfRequestsFirst_2029[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-10-01' and '2029-10-30'";
            arrayOfRequestsFirst_2029[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-10-29' and '2029-11-30'";
            arrayOfRequestsFirst_2029[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-11-29' and '2029-12-30'";
        }

        public void Requests_2030_First()
        {
            arrayOfRequestsFirst_2030[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2029-12-29' and '2030-01-30'";
            arrayOfRequestsFirst_2030[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2030-02-01' and '2030-02-28'";
            arrayOfRequestsFirst_2030[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2030-02-27' and '2030-03-30'";
            arrayOfRequestsFirst_2030[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2030-03-29' and '2030-04-28'";
            arrayOfRequestsFirst_2030[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2030-04-29' and '2030-05-30'";
            arrayOfRequestsFirst_2030[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2030-05-29' and '2030-06-28'";
            arrayOfRequestsFirst_2030[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2030-07-01' and '2030-07-31'";
            arrayOfRequestsFirst_2030[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2030-07-29' and '2030-08-31'";
            arrayOfRequestsFirst_2030[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2030-08-29' and '2030-09-30'";
            arrayOfRequestsFirst_2030[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2030-10-01' and '2030-10-30'";
            arrayOfRequestsFirst_2030[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2030-11-02' and '2030-11-30'";
            arrayOfRequestsFirst_2030[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM First_Shift WHERE Day_Shift between '2030-11-29' and '2030-12-30'";
        }

        public void Requests_2021_Second()
        {
            arrayOfRequestsSecond_2021[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-01-01' and '2021-01-31'";///
            arrayOfRequestsSecond_2021[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-02-01' and '2021-02-28'";
            arrayOfRequestsSecond_2021[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-02-27' and '2021-03-31'";
            arrayOfRequestsSecond_2021[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-03-27' and '2021-04-27'";
            arrayOfRequestsSecond_2021[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-05-01' and '2021-05-31'";
            arrayOfRequestsSecond_2021[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-05-29' and '2021-06-29'";
            arrayOfRequestsSecond_2021[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-07-01' and '2021-07-31'";
            arrayOfRequestsSecond_2021[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-08-01' and '2021-08-31'";
            arrayOfRequestsSecond_2021[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-08-27' and '2021-09-30'";
            arrayOfRequestsSecond_2021[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-09-30' and '2021-10-31'";
            arrayOfRequestsSecond_2021[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-10-28' and '2021-11-28'";
            arrayOfRequestsSecond_2021[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2021-11-29' and '2021-12-31'";
        }

        public void Requests_2022_Second()
        {
            arrayOfRequestsSecond_2022[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-01-01' and '2022-02-01'";
            arrayOfRequestsSecond_2022[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-01-31' and '2022-03-01'";
            arrayOfRequestsSecond_2022[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-02-28' and '2022-03-31'";
            arrayOfRequestsSecond_2022[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-03-29' and '2022-04-30'";
            arrayOfRequestsSecond_2022[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-05-01' and '2022-06-01'";
            arrayOfRequestsSecond_2022[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-05-31' and '2022-06-29'";
            arrayOfRequestsSecond_2022[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-06-30' and '2022-07-31'";
            arrayOfRequestsSecond_2022[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-08-01' and '2022-09-01'";
            arrayOfRequestsSecond_2022[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-08-29' and '2022-09-29'";
            arrayOfRequestsSecond_2022[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-09-30' and '2022-10-31'";
            arrayOfRequestsSecond_2022[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-10-28' and '2022-11-28'";
            arrayOfRequestsSecond_2022[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-11-29' and '2022-12-30'";
        }

        public void Requests_2023_Second()
        {
            arrayOfRequestsSecond_2023[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2022-12-29' and '2023-01-31'";
            arrayOfRequestsSecond_2023[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-02-01' and '2023-02-28'";
            arrayOfRequestsSecond_2023[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-02-28' and '2023-03-31'";
            arrayOfRequestsSecond_2023[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-04-01' and '2023-04-29'";
            arrayOfRequestsSecond_2023[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-04-29' and '2023-05-31'";
            arrayOfRequestsSecond_2023[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-05-31' and '2023-06-28'";
            arrayOfRequestsSecond_2023[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-06-29' and '2023-07-30'";
            arrayOfRequestsSecond_2023[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-07-31' and '2023-08-31'";
            arrayOfRequestsSecond_2023[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-08-31' and '2023-09-28'";
            arrayOfRequestsSecond_2023[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-09-29' and '2023-10-30'";
            arrayOfRequestsSecond_2023[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-10-29' and '2023-11-30'";
            arrayOfRequestsSecond_2023[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-11-29' and '2023-12-30'";
        }

        public void Requests_2024_Second()
        {
            arrayOfRequestsSecond_2024[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2023-12-29' and '2024-01-30'";
            arrayOfRequestsSecond_2024[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2024-01-30' and '2024-02-27'";
            arrayOfRequestsSecond_2024[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2024-02-28' and '2024-03-31'";
            arrayOfRequestsSecond_2024[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2024-03-29' and '2024-04-30'";
            arrayOfRequestsSecond_2024[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2024-04-29' and '2024-05-31'";
            arrayOfRequestsSecond_2024[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2024-05-29' and '2024-06-29'";
            arrayOfRequestsSecond_2024[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2024-06-29' and '2024-07-30'";
            arrayOfRequestsSecond_2024[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2024-07-31' and '2024-08-31'";
            arrayOfRequestsSecond_2024[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2024-08-29' and '2024-09-28'";
            arrayOfRequestsSecond_2024[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2024-09-29' and '2024-10-30'";
            arrayOfRequestsSecond_2024[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2024-11-01' and '2024-11-29'";
            arrayOfRequestsSecond_2024[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2024-11-29' and '2024-12-30'";
        }

        public void Requests_2025_Second()
        {
            arrayOfRequestsSecond_2025[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-01-01' and '2025-01-30'";
            arrayOfRequestsSecond_2025[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-01-30' and '2025-02-28'";
            arrayOfRequestsSecond_2025[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-02-28' and '2025-03-31'";
            arrayOfRequestsSecond_2025[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-03-29' and '2025-04-27'";
            arrayOfRequestsSecond_2025[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-04-29' and '2025-05-31'";
            arrayOfRequestsSecond_2025[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-06-02' and '2025-06-30'";
            arrayOfRequestsSecond_2025[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-06-29' and '2025-07-30'";
            arrayOfRequestsSecond_2025[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-07-31' and '2025-08-31'";
            arrayOfRequestsSecond_2025[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-08-29' and '2025-09-28'";
            arrayOfRequestsSecond_2025[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-09-29' and '2025-10-30'";
            arrayOfRequestsSecond_2025[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-11-01' and '2025-11-28'";
            arrayOfRequestsSecond_2025[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-11-29' and '2025-12-30'";
        }

        public void Requests_2026_Second()
        {
            arrayOfRequestsSecond_2026[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2025-12-29' and '2026-01-30'";
            arrayOfRequestsSecond_2026[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-01-30' and '2026-02-28'";
            arrayOfRequestsSecond_2026[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-02-28' and '2026-03-31'";
            arrayOfRequestsSecond_2026[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-03-29' and '2026-04-27'";
            arrayOfRequestsSecond_2026[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-04-29' and '2026-05-31'";
            arrayOfRequestsSecond_2026[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-05-29' and '2026-06-28'";
            arrayOfRequestsSecond_2026[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-06-29' and '2026-07-30'";
            arrayOfRequestsSecond_2026[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-07-31' and '2026-08-31'";
            arrayOfRequestsSecond_2026[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-08-29' and '2026-09-28'";
            arrayOfRequestsSecond_2026[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-09-29' and '2026-10-30'";
            arrayOfRequestsSecond_2026[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-10-29' and '2026-11-28'";
            arrayOfRequestsSecond_2026[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-11-29' and '2026-12-30'";
        }

        public void Requests_2027_Second()
        {
            arrayOfRequestsSecond_2027[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2026-12-29' and '2027-01-30'";
            arrayOfRequestsSecond_2027[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2027-01-30' and '2027-02-27'";
            arrayOfRequestsSecond_2027[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2027-02-28' and '2027-03-31'";
            arrayOfRequestsSecond_2027[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2027-04-01' and '2027-04-29'";
            arrayOfRequestsSecond_2027[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2027-04-29' and '2027-05-31'";
            arrayOfRequestsSecond_2027[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2027-05-29' and '2027-06-30'";
            arrayOfRequestsSecond_2027[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2027-06-29' and '2027-07-30'";
            arrayOfRequestsSecond_2027[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2027-07-31' and '2027-08-31'";
            arrayOfRequestsSecond_2027[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2027-08-29' and '2027-09-28'";
            arrayOfRequestsSecond_2027[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2027-09-29' and '2027-10-30'";
            arrayOfRequestsSecond_2027[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2027-10-29' and '2027-11-30'";
            arrayOfRequestsSecond_2027[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2027-11-29' and '2027-12-30'";
        }

        public void Requests_2028_Second()
        {
            arrayOfRequestsSecond_2028[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-01-01' and '2028-01-30'";
            arrayOfRequestsSecond_2028[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-02-03' and '2028-02-29'";
            arrayOfRequestsSecond_2028[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-02-28' and '2028-03-31'";
            arrayOfRequestsSecond_2028[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-04-01' and '2028-04-29'";
            arrayOfRequestsSecond_2028[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-04-29' and '2028-05-31'";
            arrayOfRequestsSecond_2028[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-05-29' and '2028-06-28'";
            arrayOfRequestsSecond_2028[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-06-29' and '2028-07-30'";
            arrayOfRequestsSecond_2028[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-07-29' and '2028-08-31'";
            arrayOfRequestsSecond_2028[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-08-29' and '2028-09-28'";
            arrayOfRequestsSecond_2028[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-09-29' and '2028-10-30'";
            arrayOfRequestsSecond_2028[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-10-29' and '2028-11-30'";
            arrayOfRequestsSecond_2028[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-11-29' and '2028-12-30'";
        }

        public void Requests_2029_Second()
        {
            arrayOfRequestsSecond_2029[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2028-12-29' and '2029-01-30'";
            arrayOfRequestsSecond_2029[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-02-01' and '2029-02-28'";
            arrayOfRequestsSecond_2029[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-02-28' and '2029-03-31'";
            arrayOfRequestsSecond_2029[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-04-01' and '2029-04-29'";
            arrayOfRequestsSecond_2029[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-04-29' and '2029-05-31'";
            arrayOfRequestsSecond_2029[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-05-29' and '2029-06-30'";
            arrayOfRequestsSecond_2029[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-06-29' and '2029-07-30'";
            arrayOfRequestsSecond_2029[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-07-31' and '2029-08-31'";
            arrayOfRequestsSecond_2029[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-08-29' and '2029-09-28'";
            arrayOfRequestsSecond_2029[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-09-29' and '2029-10-30'";
            arrayOfRequestsSecond_2029[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-10-29' and '2029-11-30'";
            arrayOfRequestsSecond_2029[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-11-29' and '2029-12-30'";
        }

        public void Requests_2030_Second()
        {
            arrayOfRequestsSecond_2030[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2029-12-29' and '2030-01-30'";
            arrayOfRequestsSecond_2030[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2030-02-01' and '2030-02-28'";
            arrayOfRequestsSecond_2030[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2030-02-28' and '2030-03-31'";
            arrayOfRequestsSecond_2030[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2030-04-01' and '2030-04-29'";
            arrayOfRequestsSecond_2030[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2030-04-29' and '2030-05-31'";
            arrayOfRequestsSecond_2030[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2030-05-29' and '2030-06-30'";
            arrayOfRequestsSecond_2030[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2030-06-29' and '2030-07-30'";
            arrayOfRequestsSecond_2030[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2030-07-31' and '2030-08-31'";
            arrayOfRequestsSecond_2030[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2030-08-29' and '2030-09-28'";
            arrayOfRequestsSecond_2030[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2030-09-29' and '2030-10-30'";
            arrayOfRequestsSecond_2030[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2030-10-29' and '2030-11-30'";
            arrayOfRequestsSecond_2030[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Second_Shift WHERE Day_Shift between '2030-11-29' and '2030-12-30'";
        }

        public void Requests_2021_Third()
        {
            arrayOfRequestsThird_2021[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-01-01' and '2021-01-31'";
            arrayOfRequestsThird_2021[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-02-01' and '2021-02-28'";
            arrayOfRequestsThird_2021[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-02-28' and '2021-03-31'";
            arrayOfRequestsThird_2021[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-03-27' and '2021-04-27'";
            arrayOfRequestsThird_2021[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-05-01' and '2021-05-31'";
            arrayOfRequestsThird_2021[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-05-31' and '2021-06-29'";
            arrayOfRequestsThird_2021[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-07-01' and '2021-07-31'";
            arrayOfRequestsThird_2021[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-08-01' and '2021-08-31'";
            arrayOfRequestsThird_2021[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-08-31' and '2021-09-27'";
            arrayOfRequestsThird_2021[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-09-30' and '2021-10-31'";
            arrayOfRequestsThird_2021[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-11-01' and '2021-11-30'";
            arrayOfRequestsThird_2021[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2021-12-01' and '2021-12-31'";
        }

        public void Requests_2022_Third()
        {
            arrayOfRequestsThird_2022[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-01-01' and '2022-02-01'";
            arrayOfRequestsThird_2022[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-02-01' and '2022-02-28'";
            arrayOfRequestsThird_2022[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-03-01' and '2022-03-28'";
            arrayOfRequestsThird_2022[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-03-28' and '2022-04-30'";
            arrayOfRequestsThird_2022[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-05-01' and '2022-06-01'";
            arrayOfRequestsThird_2022[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-05-29' and '2022-06-29'";
            arrayOfRequestsThird_2022[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-06-30' and '2022-07-31'";
            arrayOfRequestsThird_2022[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-08-01' and '2022-09-01'";
            arrayOfRequestsThird_2022[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-08-29' and '2022-09-30'";
            arrayOfRequestsThird_2022[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-09-30' and '2022-10-31'";
            arrayOfRequestsThird_2022[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-10-29' and '2022-11-29'";
            arrayOfRequestsThird_2022[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-11-29' and '2022-12-30'";
        }

        public void Requests_2023_Third()
        {
            arrayOfRequestsThird_2023[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2022-12-29' and '2023-01-31'";
            arrayOfRequestsThird_2023[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-02-01' and '2023-02-28'";
            arrayOfRequestsThird_2023[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-02-28' and '2023-03-31'";
            arrayOfRequestsThird_2023[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-03-29' and '2023-04-29'";
            arrayOfRequestsThird_2023[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-04-29' and '2023-05-31'";
            arrayOfRequestsThird_2023[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-05-31' and '2023-06-29'";
            arrayOfRequestsThird_2023[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-06-29' and '2023-07-30'";
            arrayOfRequestsThird_2023[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-07-31' and '2023-08-31'";
            arrayOfRequestsThird_2023[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-08-31' and '2023-09-28'";
            arrayOfRequestsThird_2023[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-09-29' and '2023-10-30'";
            arrayOfRequestsThird_2023[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-10-29' and '2023-11-30'";
            arrayOfRequestsThird_2023[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-11-29' and '2023-12-30'";
        }

        public void Requests_2024_Third()
        {
            arrayOfRequestsThird_2024[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2023-12-29' and '2024-01-31'";
            arrayOfRequestsThird_2024[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2024-02-01' and '2024-02-29'";
            arrayOfRequestsThird_2024[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2024-02-28' and '2024-03-31'";
            arrayOfRequestsThird_2024[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2024-03-29' and '2024-04-29'";
            arrayOfRequestsThird_2024[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2024-04-29' and '2024-05-30'";
            arrayOfRequestsThird_2024[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2024-05-31' and '2024-06-29'";
            arrayOfRequestsThird_2024[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2024-06-29' and '2024-07-30'";
            arrayOfRequestsThird_2024[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2024-08-01' and '2024-08-30'";
            arrayOfRequestsThird_2024[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2024-08-31' and '2024-09-28'";
            arrayOfRequestsThird_2024[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2024-09-29' and '2024-10-30'";
            arrayOfRequestsThird_2024[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2024-10-29' and '2024-11-30'";
            arrayOfRequestsThird_2024[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2024-11-29' and '2024-12-30'";
        }

        public void Requests_2025_Third()
        {
            arrayOfRequestsThird_2025[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-01-01' and '2025-01-31'";
            arrayOfRequestsThird_2025[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-02-01' and '2025-02-28'";
            arrayOfRequestsThird_2025[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-02-28' and '2025-03-31'";
            arrayOfRequestsThird_2025[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-03-29' and '2025-04-29'";
            arrayOfRequestsThird_2025[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-04-29' and '2025-05-30'";
            arrayOfRequestsThird_2025[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-05-31' and '2025-06-29'";
            arrayOfRequestsThird_2025[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-06-29' and '2025-07-30'";
            arrayOfRequestsThird_2025[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-08-01' and '2025-08-30'";
            arrayOfRequestsThird_2025[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-08-31' and '2025-09-28'";
            arrayOfRequestsThird_2025[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-09-29' and '2025-10-30'";
            arrayOfRequestsThird_2025[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-10-29' and '2025-11-30'";
            arrayOfRequestsThird_2025[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2025-11-29' and '2025-12-30'";
        }

        public void Requests_2026_Third()
        {
            arrayOfRequestsThird_2026[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-01-01' and '2026-01-31'";
            arrayOfRequestsThird_2026[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-02-01' and '2026-02-28'";
            arrayOfRequestsThird_2026[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-02-28' and '2026-03-31'";
            arrayOfRequestsThird_2026[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-03-29' and '2026-04-29'";
            arrayOfRequestsThird_2026[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-04-29' and '2026-05-30'";
            arrayOfRequestsThird_2026[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-05-31' and '2026-06-29'";
            arrayOfRequestsThird_2026[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-06-29' and '2026-07-30'";
            arrayOfRequestsThird_2026[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-08-01' and '2026-08-30'";
            arrayOfRequestsThird_2026[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-08-31' and '2026-09-28'";
            arrayOfRequestsThird_2026[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-09-29' and '2026-10-30'";
            arrayOfRequestsThird_2026[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-10-29' and '2026-11-30'";
            arrayOfRequestsThird_2026[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-11-29' and '2026-12-30'";
        }

        public void Requests_2027_Third()
        {
            arrayOfRequestsThird_2027[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2026-12-29' and '2027-01-31'";
            arrayOfRequestsThird_2027[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-02-01' and '2027-02-28'";
            arrayOfRequestsThird_2027[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-02-28' and '2027-03-31'";
            arrayOfRequestsThird_2027[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-03-29' and '2027-04-29'";
            arrayOfRequestsThird_2027[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-04-29' and '2027-05-30'";
            arrayOfRequestsThird_2027[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-05-31' and '2027-06-29'";
            arrayOfRequestsThird_2027[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-06-29' and '2027-07-30'";
            arrayOfRequestsThird_2027[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-07-29' and '2027-08-30'";
            arrayOfRequestsThird_2027[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-08-31' and '2027-09-28'";
            arrayOfRequestsThird_2027[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-09-29' and '2027-10-30'";
            arrayOfRequestsThird_2027[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-10-29' and '2027-11-28'";
            arrayOfRequestsThird_2027[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-11-29' and '2027-12-30'";
        }

        public void Requests_2028_Third()
        {
            arrayOfRequestsThird_2028[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2027-12-29' and '2028-01-30'";
            arrayOfRequestsThird_2028[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-02-01' and '2028-02-29'";
            arrayOfRequestsThird_2028[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-02-28' and '2028-03-31'";
            arrayOfRequestsThird_2028[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-03-29' and '2028-04-29'";
            arrayOfRequestsThird_2028[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-04-29' and '2028-05-30'";
            arrayOfRequestsThird_2028[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-05-31' and '2028-06-29'";
            arrayOfRequestsThird_2028[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-06-29' and '2028-07-30'";
            arrayOfRequestsThird_2028[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-08-02' and '2028-08-30'";
            arrayOfRequestsThird_2028[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-08-31' and '2028-09-28'";
            arrayOfRequestsThird_2028[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-09-29' and '2028-10-30'";
            arrayOfRequestsThird_2028[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-10-29' and '2028-11-28'";
            arrayOfRequestsThird_2028[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-11-29' and '2028-12-30'";
        }

        public void Requests_2029_Third()
        {
            arrayOfRequestsThird_2029[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2028-12-29' and '2029-01-30'";
            arrayOfRequestsThird_2029[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-02-01' and '2029-02-28'";
            arrayOfRequestsThird_2029[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-02-28' and '2029-03-31'";
            arrayOfRequestsThird_2029[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-03-29' and '2029-04-29'";
            arrayOfRequestsThird_2029[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-04-29' and '2029-05-30'";
            arrayOfRequestsThird_2029[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-05-31' and '2029-06-29'";
            arrayOfRequestsThird_2029[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-06-29' and '2029-07-30'";
            arrayOfRequestsThird_2029[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-08-01' and '2029-08-30'";
            arrayOfRequestsThird_2029[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-08-31' and '2029-09-28'";
            arrayOfRequestsThird_2029[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-09-29' and '2029-10-30'";
            arrayOfRequestsThird_2029[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-10-29' and '2029-11-28'";
            arrayOfRequestsThird_2029[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-12-01' and '2030-01-03'";
        }

        public void Requests_2030_Third()
        {
            arrayOfRequestsThird_2030[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2029-12-29' and '2030-01-30'";
            arrayOfRequestsThird_2030[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2030-02-01' and '2030-02-28'";
            arrayOfRequestsThird_2030[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2030-02-28' and '2030-03-31'";
            arrayOfRequestsThird_2030[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2030-03-29' and '2030-04-29'";
            arrayOfRequestsThird_2030[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2030-04-29' and '2030-05-30'";
            arrayOfRequestsThird_2030[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2030-05-31' and '2030-06-29'";
            arrayOfRequestsThird_2030[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2030-06-29' and '2030-07-30'";
            arrayOfRequestsThird_2030[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2030-07-29' and '2030-08-30'";
            arrayOfRequestsThird_2030[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2030-08-31' and '2030-09-28'";
            arrayOfRequestsThird_2030[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2030-09-29' and '2030-10-30'";
            arrayOfRequestsThird_2030[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2030-10-29' and '2030-11-28'";
            arrayOfRequestsThird_2030[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Third_Shift WHERE Day_Shift between '2030-11-29' and '2030-12-30'";
        }

        public void Requests_2021_Fourth()
        {
            arrayOfRequestsFourth_2021[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-01-01' and '2021-02-01'";///
            arrayOfRequestsFourth_2021[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-02-02' and '2021-03-01'";
            arrayOfRequestsFourth_2021[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-03-01' and '2021-04-02'";
            arrayOfRequestsFourth_2021[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-04-01' and '2021-04-30'";
            arrayOfRequestsFourth_2021[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-05-01' and '2021-06-01'";
            arrayOfRequestsFourth_2021[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-06-01' and '2021-06-30'";
            arrayOfRequestsFourth_2021[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-07-01' and '2021-07-31'";
            arrayOfRequestsFourth_2021[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-08-01' and '2021-09-01'";
            arrayOfRequestsFourth_2021[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-09-01' and '2021-09-30'";
            arrayOfRequestsFourth_2021[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-09-30' and '2021-10-31'";
            arrayOfRequestsFourth_2021[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-11-01' and '2021-12-02'";
            arrayOfRequestsFourth_2021[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2021-12-01' and '2021-12-31'";
        }

        public void Requests_2022_Fourth()
        {
            arrayOfRequestsFourth_2022[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-01-01' and '2022-02-01'";
            arrayOfRequestsFourth_2022[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-02-01' and '2022-02-28'";
            arrayOfRequestsFourth_2022[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-03-01' and '2022-04-01'";
            arrayOfRequestsFourth_2022[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-04-01' and '2022-04-30'";
            arrayOfRequestsFourth_2022[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-05-01' and '2022-06-01'";
            arrayOfRequestsFourth_2022[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-05-28' and '2022-06-30'";
            arrayOfRequestsFourth_2022[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-06-30' and '2022-07-31'";
            arrayOfRequestsFourth_2022[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-08-01' and '2022-09-01'";
            arrayOfRequestsFourth_2022[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-09-01' and '2022-09-30'";
            arrayOfRequestsFourth_2022[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-09-30' and '2022-10-31'";
            arrayOfRequestsFourth_2022[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-11-01' and '2022-12-02'";
            arrayOfRequestsFourth_2022[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2022-12-01' and '2022-12-31'";
        }

        public void Requests_2023_Fourth()
        {
            arrayOfRequestsFourth_2023[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-01-01' and '2023-01-31'";
            arrayOfRequestsFourth_2023[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-02-01' and '2023-02-28'";
            arrayOfRequestsFourth_2023[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-02-28' and '2023-03-31'";
            arrayOfRequestsFourth_2023[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-03-29' and '2023-04-29'";
            arrayOfRequestsFourth_2023[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-04-29' and '2023-05-31'";
            arrayOfRequestsFourth_2023[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-06-01' and '2023-07-01'";
            arrayOfRequestsFourth_2023[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-06-29' and '2023-07-30'";
            arrayOfRequestsFourth_2023[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-07-31' and '2023-08-31'";
            arrayOfRequestsFourth_2023[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-09-01' and '2023-10-01'";
            arrayOfRequestsFourth_2023[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-09-29' and '2023-10-30'";
            arrayOfRequestsFourth_2023[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-11-01' and '2023-11-30'";
            arrayOfRequestsFourth_2023[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2023-11-29' and '2023-12-30'";
        }

        public void Requests_2024_Fourth()
        {
            arrayOfRequestsFourth_2024[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-01-01' and '2024-01-31'";
            arrayOfRequestsFourth_2024[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-02-01' and '2024-03-01'";
            arrayOfRequestsFourth_2024[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-02-28' and '2024-03-31'";
            arrayOfRequestsFourth_2024[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-04-01' and '2024-04-30'";
            arrayOfRequestsFourth_2024[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-05-01' and '2024-06-01'";
            arrayOfRequestsFourth_2024[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-06-01' and '2024-07-01'";
            arrayOfRequestsFourth_2024[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-07-01' and '2024-07-31'";
            arrayOfRequestsFourth_2024[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-08-01' and '2024-09-01'";
            arrayOfRequestsFourth_2024[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-09-02' and '2024-10-01'";
            arrayOfRequestsFourth_2024[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-10-01' and '2024-11-01'";
            arrayOfRequestsFourth_2024[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-11-01' and '2024-12-03'";
            arrayOfRequestsFourth_2024[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2024-11-29' and '2024-12-30'";
        }

        public void Requests_2025_Fourth()
        {
            arrayOfRequestsFourth_2025[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-01-01' and '2025-01-31'";
            arrayOfRequestsFourth_2025[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-02-01' and '2025-03-01'";
            arrayOfRequestsFourth_2025[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-03-01' and '2025-04-01'";
            arrayOfRequestsFourth_2025[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-04-01' and '2025-04-30'";
            arrayOfRequestsFourth_2025[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-05-01' and '2025-06-01'";
            arrayOfRequestsFourth_2025[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-06-01' and '2025-07-01'";
            arrayOfRequestsFourth_2025[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-07-01' and '2025-07-31'";
            arrayOfRequestsFourth_2025[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-08-01' and '2025-09-01'";
            arrayOfRequestsFourth_2025[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-09-02' and '2025-10-01'";
            arrayOfRequestsFourth_2025[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-10-01' and '2025-11-01'";
            arrayOfRequestsFourth_2025[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-11-01' and '2025-12-03'";
            arrayOfRequestsFourth_2025[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2025-12-02' and '2026-01-02'";
        }

        public void Requests_2026_Fourth()
        {
            arrayOfRequestsFourth_2026[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-01-01' and '2026-01-31'";
            arrayOfRequestsFourth_2026[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-02-01' and '2026-02-28'";
            arrayOfRequestsFourth_2026[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-03-01' and '2026-04-01'";
            arrayOfRequestsFourth_2026[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-04-01' and '2026-05-02'";
            arrayOfRequestsFourth_2026[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-05-01' and '2026-06-01'";
            arrayOfRequestsFourth_2026[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-06-01' and '2026-07-01'";
            arrayOfRequestsFourth_2026[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-07-01' and '2026-07-31'";
            arrayOfRequestsFourth_2026[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-08-01' and '2026-09-01'";
            arrayOfRequestsFourth_2026[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-09-02' and '2026-10-01'";
            arrayOfRequestsFourth_2026[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-10-01' and '2026-11-01'";
            arrayOfRequestsFourth_2026[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-11-01' and '2026-12-03'";
            arrayOfRequestsFourth_2026[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2026-12-01' and '2027-01-02'";
        }

        public void Requests_2027_Fourth()
        {
            arrayOfRequestsFourth_2027[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-01-02' and '2027-02-02'";
            arrayOfRequestsFourth_2027[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-02-01' and '2027-03-02'";
            arrayOfRequestsFourth_2027[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-03-01' and '2027-04-01'";
            arrayOfRequestsFourth_2027[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-04-01' and '2027-05-02'";
            arrayOfRequestsFourth_2027[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-05-01' and '2027-06-01'";
            arrayOfRequestsFourth_2027[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-06-01' and '2027-07-01'";
            arrayOfRequestsFourth_2027[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-07-01' and '2027-08-02'";
            arrayOfRequestsFourth_2027[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-08-01' and '2027-09-01'";
            arrayOfRequestsFourth_2027[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-09-02' and '2027-10-01'";
            arrayOfRequestsFourth_2027[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-10-01' and '2027-11-01'";
            arrayOfRequestsFourth_2027[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-11-01' and '2027-12-03'";
            arrayOfRequestsFourth_2027[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2027-12-01' and '2027-12-31'";
        }

        public void Requests_2028_Fourth()
        {
            arrayOfRequestsFourth_2028[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-01-02' and '2028-02-02'";
            arrayOfRequestsFourth_2028[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-02-01' and '2028-03-02'";
            arrayOfRequestsFourth_2028[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-03-01' and '2028-04-01'";
            arrayOfRequestsFourth_2028[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-04-01' and '2028-05-02'";
            arrayOfRequestsFourth_2028[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-05-01' and '2028-06-01'";
            arrayOfRequestsFourth_2028[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-06-01' and '2028-07-03'";
            arrayOfRequestsFourth_2028[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-07-01' and '2028-08-02'";
            arrayOfRequestsFourth_2028[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-08-01' and '2028-09-01'";
            arrayOfRequestsFourth_2028[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-09-02' and '2028-10-03'";
            arrayOfRequestsFourth_2028[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-10-01' and '2028-11-01'";
            arrayOfRequestsFourth_2028[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-11-01' and '2028-12-03'";
            arrayOfRequestsFourth_2028[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2028-12-01' and '2028-12-31'";
        }

        public void Requests_2029_Fourth()
        {
            arrayOfRequestsFourth_2029[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-01-02' and '2029-02-02'";
            arrayOfRequestsFourth_2029[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-02-01' and '2029-02-28'";
            arrayOfRequestsFourth_2029[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-03-01' and '2029-04-01'";
            arrayOfRequestsFourth_2029[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-04-01' and '2029-05-02'";
            arrayOfRequestsFourth_2029[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-05-01' and '2029-06-01'";
            arrayOfRequestsFourth_2029[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-06-01' and '2029-07-03'";
            arrayOfRequestsFourth_2029[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-07-01' and '2029-08-02'";
            arrayOfRequestsFourth_2029[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-08-01' and '2029-09-01'";
            arrayOfRequestsFourth_2029[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-09-02' and '2029-10-03'";
            arrayOfRequestsFourth_2029[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-10-01' and '2029-11-01'";
            arrayOfRequestsFourth_2029[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-11-01' and '2029-12-03'";
            arrayOfRequestsFourth_2029[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-12-01' and '2029-12-31'";
        }

        public void Requests_2030_Fourth()
        {
            arrayOfRequestsFourth_2030[0] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2029-12-29' and '2030-01-30'";
            arrayOfRequestsFourth_2030[1] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2030-02-01' and '2030-03-03'";
            arrayOfRequestsFourth_2030[2] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2030-02-27' and '2030-03-30'";
            arrayOfRequestsFourth_2030[3] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2030-03-29' and '2030-04-28'";
            arrayOfRequestsFourth_2030[4] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2030-04-29' and '2030-05-30'";
            arrayOfRequestsFourth_2030[5] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2030-05-29' and '2030-06-28'";
            arrayOfRequestsFourth_2030[6] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2030-07-01' and '2030-07-31'";
            arrayOfRequestsFourth_2030[7] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2030-07-29' and '2030-08-31'";
            arrayOfRequestsFourth_2030[8] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2030-08-29' and '2030-09-30'";
            arrayOfRequestsFourth_2030[9] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2030-10-01' and '2030-10-30'";
            arrayOfRequestsFourth_2030[10] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2030-10-29' and '2030-11-30'";
            arrayOfRequestsFourth_2030[11] = "SELECT Day_Shift,Night_Shift,Day_Off,End_Day_Off FROM Fourth_Shift WHERE Day_Shift between '2030-11-29' and '2030-12-30'";
        }

        public void ClearData()
        {
            dayShift = "";
            nightShift = "";
            dayOff = "";
            endDayOff = "";

            numberOfDayShift.Clear();
            numberOfNightShift.Clear();
            numberOfDayOff.Clear();
            numberOfEndDayOff.Clear();
        }

        public void ClearData2()
        {
            numberOfDay.Clear();
        }

        public List<int> NumberOfDayShift()
        {
            return numberOfDayShift;
        }

        public List<int> NumberOfNightShift()
        {
            return numberOfNightShift;
        }

        public List<int> NumberOfDayOff()
        {
            return numberOfDayOff;
        }

        public List<int> NumberOfEndDayOff()
        {
            return numberOfEndDayOff;
        }

        public List<int> NumberOfDay()
        {
            return numberOfDay;
        }

        private const string Get_Surname_DepartmentName_PassNumber_NumberOfShift = "SELECT Worker.Worker_Id, Worker.Surname, Department.Name as 'Department name', Pass.Number as 'Pass number', Worker.Number_Of_Shift FROM Worker, Department, Pass WHERE Worker.Department_Id = Department.Department_Id AND Worker.Worker_Id = Pass.Worker_Id";
        public DataTable GetTable1()
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();

                command.CommandText = Get_Surname_DepartmentName_PassNumber_NumberOfShift;

                command.Connection = (SqlConnection)connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        private const string GetChangedInformation_ = "SELECT Number_Of_Day, Condition, Since_What_Time, Till_What_Time, Value FROM Individual_Work_Shedule_ WHERE Year = @year AND Month = @month AND Worker_Id = @workerId";
        public DataTable GetChangedInformation(int year, string month, int workId)
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();

                command.CommandText = GetChangedInformation_;

                command.Connection = (SqlConnection)connection;

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlParameter month_ = new SqlParameter();
                month_.ParameterName = "@month";
                month_.SqlDbType = System.Data.SqlDbType.VarChar;
                month_.Value = month;
                command.Parameters.Add(month_);

                SqlParameter workId_ = new SqlParameter();
                workId_.ParameterName = "@workerId";
                workId_.SqlDbType = System.Data.SqlDbType.Int;
                workId_.Value = workId;
                command.Parameters.Add(workId_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        public List<int> SelectingSaturdayOrSundayForChangeInformation(int years, string months)
        {
            List<int> numOfDay = new List<int>();

            try
            {
                int year = years;

                string month = months;

                int numberOfMonth = 0;

                switch (month)
                {
                    case "January":
                        numberOfMonth = 1;
                        break;
                    case "February":
                        numberOfMonth = 2;
                        break;
                    case "March":
                        numberOfMonth = 3;
                        break;
                    case "April":
                        numberOfMonth = 4;
                        break;
                    case "May":
                        numberOfMonth = 5;
                        break;
                    case "June":
                        numberOfMonth = 6;
                        break;
                    case "July":
                        numberOfMonth = 7;
                        break;
                    case "August":
                        numberOfMonth = 8;
                        break;
                    case "September":
                        numberOfMonth = 9;
                        break;
                    case "October":
                        numberOfMonth = 10;
                        break;
                    case "November":
                        numberOfMonth = 11;
                        break;
                    case "December":
                        numberOfMonth = 12;
                        break;
                    default:
                        break;
                }

                DateTime date = new DateTime(year, numberOfMonth, 1);

                numOfDayOfMonth = DateTime.DaysInMonth(year, numberOfMonth);

                for (int i = 0; i < numOfDayOfMonth; i++)
                {
                    if (date.DayOfWeek.Equals(DayOfWeek.Saturday) || date.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        numOfDay.Add(i + 1);
                    }

                    date = date.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return numOfDay;
        }

        public void ChangingAWorkerSchedule(int shiftNumber, string year, string month)
        {
            try
            {
                SelectingYear(year, true, "");
                SelectingMonth(year, month, true, "", "");

                switch (shiftNumber)
                {
                    case 1:
                        SqlCommand first = new SqlCommand();

                        first.CommandText = firstCommandText;

                        first.Connection = (SqlConnection)connection;

                        SqlDataAdapter adapterFirst = new SqlDataAdapter(first);

                        DataTable tableFirst = new DataTable();

                        adapterFirst.Fill(tableFirst);

                        FillingInWithData(tableFirst, 0, false);

                        break;
                    case 2:
                        SqlCommand second = new SqlCommand();

                        second.CommandText = secondCommandText;

                        second.Connection = (SqlConnection)connection;

                        SqlDataAdapter adapterSecond = new SqlDataAdapter(second);

                        DataTable tableSecond = new DataTable();

                        adapterSecond.Fill(tableSecond);

                        FillingInWithData(tableSecond, 0, false);

                        break;
                    case 3:
                        SqlCommand third = new SqlCommand();

                        third.CommandText = thirdCommandText;

                        third.Connection = (SqlConnection)connection;

                        SqlDataAdapter adapterThird = new SqlDataAdapter(third);

                        DataTable tableThird = new DataTable();

                        adapterThird.Fill(tableThird);

                        FillingInWithData(tableThird, 0, false);

                        break;
                    case 4:
                        SqlCommand fourth = new SqlCommand();

                        fourth.CommandText = fourthCommandText;

                        fourth.Connection = (SqlConnection)connection;

                        SqlDataAdapter adapterFourth = new SqlDataAdapter(fourth);

                        DataTable tableFourth = new DataTable();

                        adapterFourth.Fill(tableFourth);

                        FillingInWithData(tableFourth, 0, false);

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        private const string CheckingForChangedDataInTheDatabase_ = "SELECT * FROM Individual_Work_Shedule_ WHERE Year = @year AND Month = @month AND Worker_Id = @workId";
        public int ChekingForChangedDataInTheDatabase(int year, string month, int workId)
        {
            int id = 0;

            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = CheckingForChangedDataInTheDatabase_;

                IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlParameter month_ = new SqlParameter();
                month_.ParameterName = "@month";
                month_.SqlDbType = System.Data.SqlDbType.VarChar;
                month_.Value = month;
                command.Parameters.Add(month_);

                SqlParameter workId_ = new SqlParameter();
                workId_.ParameterName = "@workId";
                workId_.SqlDbType = System.Data.SqlDbType.Int;
                workId_.Value = workId;
                command.Parameters.Add(workId_);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return id;
        }

        private const string UPDATE_CHANGED_SHEDULE = "UPDATE Individual_Work_Shedule_ SET Condition = @condition, Since_What_Time = @sinceWhatTime, Till_What_Time = @tillWhatTime, Value = @value WHERE Number_Of_Day = @numberOfDay AND Worker_Id = @workerId AND Year = @year AND Month = @month";
        public void UpdateChangedShedule(FieldsForChangingTheWorkShedule fieldsForChangingTheWorkShedule, int id, FieldsForChangingTheWorkShedule[] fields, int year, string month)
        {
            IDbConnection connection = GetConnection();

            command = new SqlCommand();

            command.CommandText = UPDATE_CHANGED_SHEDULE;

            command.Connection = (SqlConnection)connection;

            try
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    fieldsForChangingTheWorkShedule = new FieldsForChangingTheWorkShedule(fields[i].NumberOfDay, fields[i].Condition,
                                                                                          fields[i].SinceWhatTime, fields[i].TillWhatTime,
                                                                                          fields[i].Value);

                    SqlParameter numberOfDay = new SqlParameter();
                    numberOfDay.ParameterName = "@numberOfDay";
                    numberOfDay.SqlDbType = System.Data.SqlDbType.Int;
                    numberOfDay.Value = fieldsForChangingTheWorkShedule.NumberOfDay;
                    command.Parameters.Add(numberOfDay);

                    SqlParameter condition = new SqlParameter();
                    condition.ParameterName = "@condition";
                    condition.SqlDbType = System.Data.SqlDbType.VarChar;
                    if (fieldsForChangingTheWorkShedule.Condition == "")
                    {
                        condition.Value = "Undefined";
                    }
                    else
                    {
                        condition.Value = fieldsForChangingTheWorkShedule.Condition;
                    }
                    command.Parameters.Add(condition);

                    SqlParameter sinceWhatTime = new SqlParameter();
                    sinceWhatTime.ParameterName = "@sinceWhatTime";
                    sinceWhatTime.SqlDbType = System.Data.SqlDbType.Time;
                    if (fieldsForChangingTheWorkShedule.SinceWhatTime == "-" || fieldsForChangingTheWorkShedule.SinceWhatTime == "")
                    {
                        sinceWhatTime.Value = "0:00:00";
                    }
                    else
                    {
                        sinceWhatTime.Value = fieldsForChangingTheWorkShedule.SinceWhatTime;
                    }
                    command.Parameters.Add(sinceWhatTime);

                    SqlParameter tillWhatTime = new SqlParameter();
                    tillWhatTime.ParameterName = "@tillWhatTime";
                    tillWhatTime.SqlDbType = System.Data.SqlDbType.Time;
                    if (fieldsForChangingTheWorkShedule.TillWhatTime == "-" || fieldsForChangingTheWorkShedule.TillWhatTime == "")
                    {
                        tillWhatTime.Value = "0:00:00";
                    }
                    else
                    {
                        tillWhatTime.Value = fieldsForChangingTheWorkShedule.TillWhatTime;
                    }
                    command.Parameters.Add(tillWhatTime);

                    SqlParameter value = new SqlParameter();
                    value.ParameterName = "@value";
                    value.SqlDbType = System.Data.SqlDbType.VarChar;
                    if (fieldsForChangingTheWorkShedule.Value == "")
                    {
                        value.Value = "undefined";
                    }
                    else
                    {
                        value.Value = fieldsForChangingTheWorkShedule.Value;
                    }
                    command.Parameters.Add(value);

                    SqlParameter workerId = new SqlParameter();
                    workerId.ParameterName = "@workerId";
                    workerId.SqlDbType = System.Data.SqlDbType.Int;
                    workerId.Value = id;
                    command.Parameters.Add(workerId);

                    SqlParameter year_ = new SqlParameter();
                    year_.ParameterName = "@year";
                    year_.SqlDbType = System.Data.SqlDbType.Int;
                    year_.Value = year;
                    command.Parameters.Add(year_);

                    SqlParameter month_ = new SqlParameter();
                    month_.ParameterName = "@month";
                    month_.SqlDbType = System.Data.SqlDbType.VarChar;
                    month_.Value = month;
                    command.Parameters.Add(month_);

                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        private const string Add_Changed_Information = "INSERT INTO Individual_Work_Shedule_(Number_Of_Day, Year, Month, Condition, Since_What_Time, Till_What_Time, Value, Worker_Id) VALUES(@numberOfDay, @year, @month, @condition, @sinceWhatTime, @tillWhatTime, @value, @workerId)";
        public void AddFieldsForChanging(FieldsForChangingTheWorkShedule fieldsForChangingTheWorkShedule, int id, List<FieldsForChangingTheWorkShedule> fields, int year, string month)
        {
            IDbConnection connection = GetConnection();

            command = new SqlCommand();

            command.CommandText = Add_Changed_Information;

            command.Connection = (SqlConnection)connection;

            try
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    fieldsForChangingTheWorkShedule = new FieldsForChangingTheWorkShedule(fields[i].NumberOfDay, fields[i].Condition,
                                                                                          fields[i].SinceWhatTime, fields[i].TillWhatTime,
                                                                                          fields[i].Value);

                    SqlParameter numberOfDay = new SqlParameter();
                    numberOfDay.ParameterName = "@numberOfDay";
                    numberOfDay.SqlDbType = System.Data.SqlDbType.Int;
                    numberOfDay.Value = fieldsForChangingTheWorkShedule.NumberOfDay;
                    command.Parameters.Add(numberOfDay);

                    SqlParameter year_ = new SqlParameter();
                    year_.ParameterName = "@year";
                    year_.SqlDbType = System.Data.SqlDbType.Int;
                    year_.Value = year;
                    command.Parameters.Add(year_);

                    SqlParameter month_ = new SqlParameter();
                    month_.ParameterName = "@month";
                    month_.SqlDbType = System.Data.SqlDbType.VarChar;
                    month_.Value = month;
                    command.Parameters.Add(month_);

                    SqlParameter condition = new SqlParameter();
                    condition.ParameterName = "@condition";
                    condition.SqlDbType = System.Data.SqlDbType.VarChar;
                    if (fieldsForChangingTheWorkShedule.Condition == "")
                    {
                        condition.Value = "Undefined";
                    }
                    else
                    {
                        condition.Value = fieldsForChangingTheWorkShedule.Condition;
                    }
                    command.Parameters.Add(condition);

                    SqlParameter sinceWhatTime = new SqlParameter();
                    sinceWhatTime.ParameterName = "@sinceWhatTime";
                    sinceWhatTime.SqlDbType = System.Data.SqlDbType.Time;
                    if (fieldsForChangingTheWorkShedule.SinceWhatTime == "-" || fieldsForChangingTheWorkShedule.SinceWhatTime == "")
                    {
                        sinceWhatTime.Value = "0:00:00";
                    }
                    else
                    {
                        sinceWhatTime.Value = fieldsForChangingTheWorkShedule.SinceWhatTime;
                    }
                    command.Parameters.Add(sinceWhatTime);

                    SqlParameter tillWhatTime = new SqlParameter();
                    tillWhatTime.ParameterName = "@tillWhatTime";
                    tillWhatTime.SqlDbType = System.Data.SqlDbType.Time;
                    if (fieldsForChangingTheWorkShedule.TillWhatTime == "-" || fieldsForChangingTheWorkShedule.TillWhatTime == "")
                    {
                        tillWhatTime.Value = "0:00:00";
                    }
                    else
                    {
                        tillWhatTime.Value = fieldsForChangingTheWorkShedule.TillWhatTime;
                    }
                    command.Parameters.Add(tillWhatTime);

                    SqlParameter value = new SqlParameter();
                    value.ParameterName = "@value";
                    value.SqlDbType = System.Data.SqlDbType.VarChar;
                    if (fieldsForChangingTheWorkShedule.Value == "")
                    {
                        value.Value = "undefined";
                    }
                    else
                    {
                        value.Value = fieldsForChangingTheWorkShedule.Value;
                    }
                    command.Parameters.Add(value);

                    SqlParameter workerId = new SqlParameter();
                    workerId.ParameterName = "@workerId";
                    workerId.SqlDbType = System.Data.SqlDbType.Int;
                    workerId.Value = id;
                    command.Parameters.Add(workerId);

                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        /// <summary>
        /// ////////////////////////////////////////AddWorker
        /// </summary>
        /// <param name="worker"></param>

        private const string INSERT_WORKER = "INSERT INTO Worker(Surname, Name, Patronymic, Date_Of_Birth, Gender, Phone_Number, Department_Id, Profession, Date_Of_Start_To_Work, Number_Of_Shift) VALUES(@surname, @name, @patronymic, @dateOfBirth, @gender, @phoneNumber, @departmentId, @profession, @dateOfStartToWork, @numberOfShift)";
        public void AddWorker(Worker worker)
        {
            try
            {
                command = (SqlCommand)connection.CreateCommand();

                command.CommandText = INSERT_WORKER;

                SqlParameter surname = new SqlParameter();
                surname.ParameterName = "@surname";
                surname.SqlDbType = System.Data.SqlDbType.NVarChar;
                surname.Value = worker.Surname;
                command.Parameters.Add(surname);

                SqlParameter name = new SqlParameter();
                name.ParameterName = "@name";
                name.SqlDbType = System.Data.SqlDbType.NVarChar;
                name.Value = worker.Name;
                command.Parameters.Add(name);

                SqlParameter patronymic = new SqlParameter();
                patronymic.ParameterName = "@patronymic";
                patronymic.SqlDbType = System.Data.SqlDbType.NVarChar;
                patronymic.Value = worker.Patronymic;
                command.Parameters.Add(patronymic);

                SqlParameter dateOfBirth = new SqlParameter();
                dateOfBirth.ParameterName = "@dateOfBirth";
                dateOfBirth.SqlDbType = System.Data.SqlDbType.Date;
                dateOfBirth.Value = worker.DateOfBirth;
                command.Parameters.Add(dateOfBirth);

                SqlParameter gender = new SqlParameter();
                gender.ParameterName = "@gender";
                gender.SqlDbType = System.Data.SqlDbType.Bit;
                gender.Value = worker.Gender;
                command.Parameters.Add(gender);

                SqlParameter phoneNumber = new SqlParameter();
                phoneNumber.ParameterName = "@phoneNumber";
                phoneNumber.SqlDbType = System.Data.SqlDbType.VarChar;
                phoneNumber.Value = worker.PhoneNumber;
                command.Parameters.Add(phoneNumber);

                SqlParameter departmentId = new SqlParameter();
                departmentId.ParameterName = "@departmentId";
                departmentId.SqlDbType = System.Data.SqlDbType.Int;
                departmentId.Value = worker.DepartmentId;
                command.Parameters.Add(departmentId);

                SqlParameter profession = new SqlParameter();
                profession.ParameterName = "@profession";
                profession.SqlDbType = System.Data.SqlDbType.NVarChar;
                profession.Value = worker.Profession;
                command.Parameters.Add(profession);

                SqlParameter dateOfStartToWork = new SqlParameter();
                dateOfStartToWork.ParameterName = "@dateOfStartToWork";
                dateOfStartToWork.SqlDbType = System.Data.SqlDbType.Date;
                dateOfStartToWork.Value = worker.DateOfStartToWork;
                command.Parameters.Add(dateOfStartToWork);

                SqlParameter numberOfShift = new SqlParameter();
                numberOfShift.ParameterName = "@numberOfShift";
                numberOfShift.SqlDbType = System.Data.SqlDbType.Int;
                numberOfShift.Value = worker.NumberOfShift;
                command.Parameters.Add(numberOfShift);

                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        private const string Get_Max_Id = "SELECT MAX(Worker_Id) FROM Worker";
        public int GetMaxId()
        {
            int id = 0;

            try
            {
                IDbConnection connection = GetConnection();

                command = (SqlCommand)connection.CreateCommand();

                command.CommandText = Get_Max_Id;

                command.Connection = (SqlConnection)connection;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = (int)reader[0];
                    }
                }

                ReleaseConnection(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return id;
        }

        private const string INSERT_ADDRESS = "INSERT INTO ADDRESS(Name_Of_The_City, Name_Of_The_Street, House_Number, Worker_Id) VALUES(@nameOfTheCity, @nameOfTheStreet, @houseNumber, @workerId)";
        public void AddAddress(Address address)
        {
            try
            {
                command = (SqlCommand)connection.CreateCommand();

                command.CommandText = INSERT_ADDRESS;

                command.Connection = (SqlConnection)connection;

                SqlParameter nameOfTheCity = new SqlParameter();
                nameOfTheCity.ParameterName = "@nameOfTheCity";
                nameOfTheCity.SqlDbType = System.Data.SqlDbType.NVarChar;
                nameOfTheCity.Value = address.NameOfTheCity;
                command.Parameters.Add(nameOfTheCity);

                SqlParameter nameOfTheStreet = new SqlParameter();
                nameOfTheStreet.ParameterName = "@nameOfTheStreet";
                nameOfTheStreet.SqlDbType = System.Data.SqlDbType.NVarChar;
                nameOfTheStreet.Value = address.NameOfTheStreet;
                command.Parameters.Add(nameOfTheStreet);

                SqlParameter houseNumber = new SqlParameter();
                houseNumber.ParameterName = "@houseNumber";
                houseNumber.SqlDbType = System.Data.SqlDbType.NVarChar;
                houseNumber.Value = address.HouseNumber;
                command.Parameters.Add(houseNumber);

                SqlParameter workerId = new SqlParameter();
                workerId.ParameterName = "@workerId";
                workerId.SqlDbType = System.Data.SqlDbType.Int;
                workerId.Value = address.WorkerId;
                command.Parameters.Add(workerId);

                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        private const string INSERT_PASS = "INSERT INTO Pass(Number, Condition, Worker_Id) VALUES(@number, @condition, @workerId)";
        public void AddPass(Pass pass)
        {
            try
            {
                command = (SqlCommand)connection.CreateCommand();

                command.CommandText = INSERT_PASS;

                command.Connection = (SqlConnection)connection;

                SqlParameter number = new SqlParameter();
                number.ParameterName = "@number";
                number.SqlDbType = System.Data.SqlDbType.VarChar;
                number.Value = pass.Number;
                command.Parameters.Add(number);

                SqlParameter condition = new SqlParameter();
                condition.ParameterName = "@condition";
                condition.SqlDbType = System.Data.SqlDbType.Bit;
                condition.Value = pass.Condition;
                command.Parameters.Add(condition);

                SqlParameter workerId = new SqlParameter();
                workerId.ParameterName = "@workerId";
                workerId.SqlDbType = System.Data.SqlDbType.Int;
                workerId.Value = pass.WorkerId;
                command.Parameters.Add(workerId);

                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        private const string INSERT_PHOTO = "INSERT INTO Photo(Path, Worker_Id) VALUES(@path, @workerId)";
        public void AddPhoto(Photo photo)
        {
            try
            {
                command = (SqlCommand)connection.CreateCommand();

                command.CommandText = INSERT_PHOTO;

                command.Connection = (SqlConnection)connection;

                SqlParameter path = new SqlParameter();
                path.ParameterName = "@path";
                path.SqlDbType = System.Data.SqlDbType.NVarChar;
                path.Value = photo.Path;
                command.Parameters.Add(path);

                SqlParameter workerId = new SqlParameter();
                workerId.ParameterName = "@workerId";
                workerId.SqlDbType = System.Data.SqlDbType.Int;
                workerId.Value = photo.WorkerId;
                command.Parameters.Add(workerId);

                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        /// <summary>
        /// FindWorker//////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public List<Worker> worker;
        public List<Pass> pass_;
        public List<Address> address;
        public List<Photo> photo;
        public List<string> depName;

        private const string FIND_WORKER = "SELECT * FROM Worker, Address, Pass, Department, Photo WHERE Worker.Worker_Id = Pass.Worker_Id AND Worker.Department_Id = Department.Department_Id AND Worker.Worker_Id = Photo.Worker_Id AND Worker.Worker_Id = Address.Worker_Id AND Pass.Number = @passNumber AND Department.Name = @departmentName";
        public void FindByPassNumberAndDepartmentName(string passNumber, string departmentName)
        {
            worker = new List<Worker>();
            address = new List<Address>();
            pass_ = new List<Pass>();
            photo = new List<Photo>();
            depName = new List<string>();

            try
            {
                command = new SqlCommand();
                command.CommandText = FIND_WORKER;

                // IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter passNumber_ = new SqlParameter();
                passNumber_.ParameterName = "@passNumber";
                passNumber_.SqlDbType = System.Data.SqlDbType.VarChar;
                passNumber_.Value = passNumber;
                command.Parameters.Add(passNumber_);

                SqlParameter departmentName_ = new SqlParameter();
                departmentName_.ParameterName = "@departmentName";
                departmentName_.SqlDbType = System.Data.SqlDbType.NVarChar;
                departmentName_.Value = departmentName;
                command.Parameters.Add(departmentName_);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int workerId = (int)reader[0];
                        string surname = (string)reader[1] + "";
                        string name = (string)reader[2] + "";
                        string patronymic = (string)reader[3] + "";
                        string dateOfBirth = reader[4].ToString() + "";
                        dateOfBirth = dateOfBirth.Substring(0, dateOfBirth.Length - 7);
                        bool gender = (bool)reader[5];
                        string phoneNumber = (string)reader[6] + "";
                        int departmentId = (int)reader[7];
                        string profession = (string)reader[8] + "";
                        string dateOfStartToWork = reader[9].ToString() + "";
                        dateOfStartToWork = dateOfStartToWork.Substring(0, dateOfStartToWork.Length - 7);
                        int numberOfShift = (int)reader[10];

                        worker.Add(new Worker(workerId, surname, name, patronymic, dateOfBirth, gender, phoneNumber, departmentId,
                                              profession, dateOfStartToWork, numberOfShift));

                        int addressId = (int)reader[11];
                        string nameOfTheCity = (string)reader[12] + "";
                        string nameOfTheStreet = (string)reader[13] + "";
                        string houseNumber = (string)reader[14] + "";
                        int woId = (int)reader[15];

                        address.Add(new Address(addressId, nameOfTheCity, nameOfTheStreet, houseNumber, woId));

                        int passId = (int)reader[16];
                        string number = (string)reader[17] + "";
                        bool condition = (bool)reader[18];
                        int workId = (int)reader[19];

                        pass_.Add(new Pass(passId, number, condition, workId));

                        int depId = (int)reader[20];
                        depName.Add((string)reader[21] + "");

                        int photoId = (int)reader[22];
                        string path = ((string)reader[23] + "");
                        int worId = (int)reader[24];

                        photo.Add(new Photo(photoId, path, worId));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        private const string Get_Surname_DepartmentName_PassNumber = "SELECT Worker.Surname, Department.Name as 'Department name', Pass.Number as 'Pass number' FROM Worker, Department, Pass WHERE Worker.Department_Id = Department.Department_Id AND Worker.Worker_Id = Pass.Worker_Id";
        public DataTable GetTable()
        {
            DataTable table = new DataTable("Table");
            try
            {
                command = new SqlCommand();

                command.CommandText = Get_Surname_DepartmentName_PassNumber;

                command.Connection = (SqlConnection)connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        public List<Worker> GetListWorker()
        {
            return worker;
        }

        public List<Pass> GetListPass()
        {
            return pass_;
        }

        public List<Address> GetListAddress()
        {
            return address;
        }

        public List<Photo> GetListPhoto()
        {
            return photo;
        }

        public List<string> GetListDepName()
        {
            return depName;
        }

        /// <summary>
        /// GetAll/////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        private const string GetTheNumberOfWorkers = "SELECT COUNT (Worker_Id) FROM Worker";
        public int GetNumberOfWorkers()
        {
            int count = 0;

            try
            {
                IDbConnection connection = GetConnection();

                SqlCommand command = new SqlCommand();

                command.CommandText = GetTheNumberOfWorkers;

                command.Connection = (SqlConnection)connection;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return count;
        }

        private const string FindNumberWorkerInDepartment = "SELECT COUNT(Worker_Id) FROM Worker, Department WHERE Worker.Department_Id = Department.Department_Id AND Department.Name = @name";
        public int FindTheNumberOfEmployeesInDepartment(string nameOfDepartment)
        {
            int quantity = 0;

            try
            {
                IDbConnection connection = GetConnection();

                command = new SqlCommand();

                command.CommandText = FindNumberWorkerInDepartment;

                command.Connection = (SqlConnection)connection;

                SqlParameter name = new SqlParameter();
                name.ParameterName = "@name";
                name.SqlDbType = System.Data.SqlDbType.NVarChar;
                name.Value = nameOfDepartment;
                command.Parameters.Add(name);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        quantity = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return quantity;
        }

        private const string Get_All_Worker = "SELECT * FROM Worker w LEFT JOIN Address a on a.Worker_Id = w.Worker_Id LEFT JOIN Pass p on p.Worker_Id = w.Worker_Id LEFT JOIN Department d on d.Department_Id = w.Department_Id LEFT JOIN Photo ph on ph.Worker_Id = w.Worker_Id";
        public DataTable GetAllWorker()
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();

                command.CommandText = Get_All_Worker;

                command.Connection = (SqlConnection)connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                ReleaseConnection(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return table;
        }

        private const string Get_All_Worker_By_Department = "SELECT * FROM Worker w LEFT JOIN Address a on a.Worker_Id = w.Worker_Id LEFT JOIN Pass p on p.Worker_Id = w.Worker_Id LEFT JOIN Department d on d.Department_Id = w.Department_Id LEFT JOIN Photo ph on ph.Worker_Id = w.Worker_Id WHERE d.Name = @departmentName";
        public DataTable GetAllWorkerByDepartment(string departmentName)
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();

                command.CommandText = Get_All_Worker_By_Department;

                command.Connection = (SqlConnection)connection;

                SqlParameter departmentName_ = new SqlParameter();
                departmentName_.ParameterName = "@departmentName";
                departmentName_.SqlDbType = System.Data.SqlDbType.NVarChar;
                departmentName_.Value = departmentName;
                command.Parameters.Add(departmentName_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                ReleaseConnection(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Remove///////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// 
        private const string Availability_Of_A_Specific_Worker_Id = "DECLARE @s INT EXEC @s = dbo.WorkerId @workerId";
        public int AvailabilityOfASpecificWorkerId(int workerId)
        {
            int id = 0;

            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = Availability_Of_A_Specific_Worker_Id;

                IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return id;
        }

        private const string DELETE_WORKER_BY_ID = "DELETE FROM Worker WHERE Worker_Id = @Id";
        public void Remove(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = DELETE_WORKER_BY_ID;

                IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter id_ = new SqlParameter();
                id_.ParameterName = "@id";
                id_.SqlDbType = System.Data.SqlDbType.Int;
                id_.Value = id;

                command.Parameters.Add(id_);

                command.ExecuteNonQuery();

                ReleaseConnection(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        public DataTable GetAllWorker1()
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();

                command.CommandText = Get_All_Worker;

                command.Connection = (SqlConnection)connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                ReleaseConnection(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return table;
        }

        public DataTable GetAllWorkerByDepartment1(string departmentName)
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();

                command.CommandText = Get_All_Worker_By_Department;

                command.Connection = (SqlConnection)connection;

                SqlParameter departmentName_ = new SqlParameter();
                departmentName_.ParameterName = "@departmentName";
                departmentName_.SqlDbType = System.Data.SqlDbType.NVarChar;
                departmentName_.Value = departmentName;
                command.Parameters.Add(departmentName_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                ReleaseConnection(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Changing worker information
        /// </summary>

        public DataTable GetAllWorker2()
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();

                command.CommandText = Get_All_Worker;

                command.Connection = (SqlConnection)connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                ReleaseConnection(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return table;
        }

        public int AvailabilityOfASpecificWorkerId1(int workerId)
        {
            int id = 0;

            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = Availability_Of_A_Specific_Worker_Id;

                IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return id;
        }

        public DataTable GetAllWorkerByDepartment2(string departmentName)
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();

                command.CommandText = Get_All_Worker_By_Department;

                command.Connection = (SqlConnection)connection;

                SqlParameter departmentName_ = new SqlParameter();
                departmentName_.ParameterName = "@departmentName";
                departmentName_.SqlDbType = System.Data.SqlDbType.NVarChar;
                departmentName_.Value = departmentName;
                command.Parameters.Add(departmentName_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                ReleaseConnection(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return table;
        }

        private const string GET_WORKER_BY_ID = "SELECT * FROM Worker w LEFT JOIN Address a on a.Worker_Id = w.Worker_Id LEFT JOIN Pass p on p.Worker_Id = w.Worker_Id LEFT JOIN Department d on d.Department_Id = w.Department_Id LEFT JOIN Photo ph on ph.Worker_Id = w.Worker_Id WHERE w.Worker_Id = @id";
        public DataTable FindWorkerById1(int id)
        {
            DataTable table = new DataTable("Table");

            try
            {
                IDbConnection connection = GetConnection();

                SqlCommand command = new SqlCommand();

                command.CommandText = GET_WORKER_BY_ID;

                command.Connection = (SqlConnection)connection;

                SqlParameter id_ = new SqlParameter();
                id_.ParameterName = "@id";
                id_.SqlDbType = System.Data.SqlDbType.Int;
                id_.Value = id;
                command.Parameters.Add(id_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                command.ExecuteNonQuery();

                ReleaseConnection(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return table;
        }

        private const string UPDATE = "UPDATE Worker SET Surname = @surname, Name = @name, Patronymic = @patronymic, Date_Of_Birth = @dateOfBirth, Gender = @gender, Phone_Number = @phoneNumber, Department_Id = @departmentId, Profession = @profession, Date_Of_Start_To_Work = @dateOfStartToWork, Number_Of_Shift = @numberOfShift WHERE Worker.Worker_Id = @id";
        public void UpdateWorker(Worker worker, int ID)
        {
            try
            {
                IDbConnection connection = GetConnection();

                SqlCommand command = new SqlCommand();

                command.CommandText = UPDATE;

                command.Connection = (SqlConnection)connection;
                
                SqlParameter surname = new SqlParameter();
                surname.ParameterName = "@surname";
                surname.SqlDbType = System.Data.SqlDbType.NVarChar;
                surname.Value = worker.Surname;
                command.Parameters.Add(surname);
                
                SqlParameter name = new SqlParameter();
                name.ParameterName = "@name";
                name.SqlDbType = System.Data.SqlDbType.NVarChar;
                name.Value = worker.Name;
                command.Parameters.Add(name);

                SqlParameter patronymic = new SqlParameter();
                patronymic.ParameterName = "@patronymic";
                patronymic.SqlDbType = System.Data.SqlDbType.NVarChar;
                patronymic.Value = worker.Patronymic;
                command.Parameters.Add(patronymic);

                SqlParameter dateOfBirth = new SqlParameter();
                dateOfBirth.ParameterName = "@dateOfBirth";
                dateOfBirth.SqlDbType = System.Data.SqlDbType.Date;
                dateOfBirth.Value = worker.DateOfBirth;
                command.Parameters.Add(dateOfBirth);

                SqlParameter gender = new SqlParameter();
                gender.ParameterName = "@gender";
                gender.SqlDbType = System.Data.SqlDbType.Bit;
                gender.Value = worker.Gender;
                command.Parameters.Add(gender);

                SqlParameter phoneNumber = new SqlParameter();
                phoneNumber.ParameterName = "@phoneNumber";
                phoneNumber.SqlDbType = System.Data.SqlDbType.VarChar;
                phoneNumber.Value = worker.PhoneNumber;
                command.Parameters.Add(phoneNumber);

                SqlParameter departmentId = new SqlParameter();
                departmentId.ParameterName = "@departmentId";
                departmentId.SqlDbType = System.Data.SqlDbType.Int;
                departmentId.Value = worker.DepartmentId;
                command.Parameters.Add(departmentId);

                SqlParameter profession = new SqlParameter();
                profession.ParameterName = "@profession";
                profession.SqlDbType = System.Data.SqlDbType.NVarChar;
                profession.Value = worker.Profession;
                command.Parameters.Add(profession);

                SqlParameter dateOfStartToWork = new SqlParameter();
                dateOfStartToWork.ParameterName = "@dateOfStartToWork";
                dateOfStartToWork.SqlDbType = System.Data.SqlDbType.NVarChar;
                dateOfStartToWork.Value = worker.DateOfStartToWork;
                command.Parameters.Add(dateOfStartToWork);

                SqlParameter numberOfShift = new SqlParameter();
                numberOfShift.ParameterName = "@numberOfShift";
                numberOfShift.SqlDbType = System.Data.SqlDbType.Int;
                numberOfShift.Value = worker.NumberOfShift;
                command.Parameters.Add(numberOfShift);

                SqlParameter id = new SqlParameter();
                id.ParameterName = "@id";
                id.SqlDbType = System.Data.SqlDbType.Int;
                id.Value = ID;
                command.Parameters.Add(id);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        private const string UPDATEADDRESS = "UPDATE Address SET Name_Of_The_City = @nameOfTheCity, Name_Of_The_Street = @nameOfTheStreet, House_Number = @houseNumber WHERE Address.Worker_Id = @id";
        public void UpdateAddress(Address address)
        {
            try
            {
                IDbConnection connection = GetConnection();

                SqlCommand command = new SqlCommand();

                command.CommandText = UPDATEADDRESS;

                command.Connection = (SqlConnection)connection;

                SqlParameter nameOfTheCity = new SqlParameter();
                nameOfTheCity.ParameterName = "@nameOfTheCity";
                nameOfTheCity.SqlDbType = System.Data.SqlDbType.NVarChar;
                nameOfTheCity.Value = address.NameOfTheCity;
                command.Parameters.Add(nameOfTheCity);

                SqlParameter nameOfTheStreet = new SqlParameter();
                nameOfTheStreet.ParameterName = "@nameOfTheStreet";
                nameOfTheStreet.SqlDbType = System.Data.SqlDbType.NVarChar;
                nameOfTheStreet.Value = address.NameOfTheStreet;
                command.Parameters.Add(nameOfTheStreet);

                SqlParameter houseNumber = new SqlParameter();
                houseNumber.ParameterName = "@houseNumber";
                houseNumber.SqlDbType = System.Data.SqlDbType.NVarChar;
                houseNumber.Value = address.HouseNumber;
                command.Parameters.Add(houseNumber);

                SqlParameter id = new SqlParameter();
                id.ParameterName = "@id";
                id.SqlDbType = System.Data.SqlDbType.Int;
                id.Value = address.WorkerId;
                command.Parameters.Add(id);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        private const string UPDATEPASS = "UPDATE Pass SET Number = @number, Condition = @condition WHERE Pass.Worker_Id = @id";
        public void UpdatePass(Pass pass)
        {
            try
            {
                IDbConnection connection = GetConnection();

                SqlCommand command = new SqlCommand();

                command.CommandText = UPDATEPASS;

                command.Connection = (SqlConnection)connection;

                SqlParameter number = new SqlParameter();
                number.ParameterName = "@number";
                number.SqlDbType = System.Data.SqlDbType.VarChar;
                number.Value = pass.Number;
                command.Parameters.Add(number);

                SqlParameter condition = new SqlParameter();
                condition.ParameterName = "@condition";
                condition.SqlDbType = System.Data.SqlDbType.Bit;
                condition.Value = pass.Condition;
                command.Parameters.Add(condition);

                SqlParameter id = new SqlParameter();
                id.ParameterName = "@id";
                id.SqlDbType = System.Data.SqlDbType.Int;
                id.Value = pass.WorkerId;
                command.Parameters.Add(id);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        private const string UPDATEPHOTO = "UPDATE Photo SET Path = @path WHERE Photo.Worker_Id = @id";
        public void UpdatePhoto(Photo photo)
        {
            try
            {
                IDbConnection connection = GetConnection();

                SqlCommand command = new SqlCommand();

                command.CommandText = UPDATEPHOTO;

                command.Connection = (SqlConnection)connection;

                SqlParameter path = new SqlParameter();
                path.ParameterName = "@path";
                path.SqlDbType = System.Data.SqlDbType.NVarChar;
                path.Value = photo.Path;
                command.Parameters.Add(path);

                SqlParameter id = new SqlParameter();
                id.ParameterName = "@id";
                id.SqlDbType = System.Data.SqlDbType.Int;
                id.Value = photo.WorkerId;
                command.Parameters.Add(id);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        ///PassageControll//////////////////////////////////////////////////////////
        ///

        private const string Get_WorkerId_Number_Of_Shift = "SELECT Worker_Id, Number_Of_Shift FROM Worker";
        public DataTable GetWorkerIdAndNumberOfShift()
        {
            DataTable table = new DataTable("Table");

            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = Get_WorkerId_Number_Of_Shift;
                IDbConnection connection = GetConnection();
                command.Connection = (SqlConnection)connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        private const string AddDataOfTheUseOfAPassByAWorker = "IF NOT EXISTS (SELECT * FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE Year = @year AND Month = @month AND NumberOfDay = @numberOfDay AND WorkerId = @workerId AND TimeOfUseOfThePass = @timeOfUseOfThePass) INSERT INTO Data_Of_The_Use_Of_A_Pass_By_A_Worker(NumberOfDay, Year, Month, Condition, SinceWhatTime, TillWhatTime, Value, WorkerId, TimeOfUseOfThePass, TheResultOfUsingOfThePass) VALUES (@numberOfDay, @year, @month, @condition, @sinceWhatTime, @tillWhatTime, @value, @workerId, @timeOfUseOfThePass, @theResultOfUsingOfThePass)";
        public void Add_Data_Of_The_Use_Of_A_Pass_By_A_Worker(ControlOfTheUseOfThePass controlOfTheUseOfThePass, List<ControlOfTheUseOfThePass> controls, string timeOfUseOfThePass)
        {
            SqlCommand command = new SqlCommand();

            command.CommandText = AddDataOfTheUseOfAPassByAWorker;

            IDbConnection connection = GetConnection();

            command.Connection = (SqlConnection)connection;

            try
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    controlOfTheUseOfThePass = new ControlOfTheUseOfThePass(controls[i].Year, controls[i].Month, controls[i].WorkerId, timeOfUseOfThePass,
                                                                           controls[i].TheResultOfUsingThePass, controls[i].NumberOfDay, controls[i].Condition,
                                                                           controls[i].SinceWhatTime, controls[i].TillWhatTime, controls[i].Value);

                    SqlParameter year = new SqlParameter();
                    year.ParameterName = "@year";
                    year.SqlDbType = System.Data.SqlDbType.Int;
                    year.Value = controlOfTheUseOfThePass.Year;
                    command.Parameters.Add(year);

                    SqlParameter month = new SqlParameter();
                    month.ParameterName = "@month";
                    month.SqlDbType = System.Data.SqlDbType.VarChar;
                    month.Value = controlOfTheUseOfThePass.Month;
                    command.Parameters.Add(month);

                    SqlParameter workerId = new SqlParameter();
                    workerId.ParameterName = "@workerId";
                    workerId.SqlDbType = System.Data.SqlDbType.Int;
                    workerId.Value = controlOfTheUseOfThePass.WorkerId;
                    command.Parameters.Add(workerId);

                    SqlParameter timeOfUseOfThePass_ = new SqlParameter();
                    timeOfUseOfThePass_.ParameterName = "@timeOfUseOfThePass";
                    timeOfUseOfThePass_.SqlDbType = System.Data.SqlDbType.Time;
                    timeOfUseOfThePass_.Value = timeOfUseOfThePass;
                    command.Parameters.Add(timeOfUseOfThePass_);

                    SqlParameter theResultOfUsingThePass = new SqlParameter();
                    theResultOfUsingThePass.ParameterName = "@theResultOfUsingOfThePass";
                    theResultOfUsingThePass.SqlDbType = System.Data.SqlDbType.VarChar;
                    theResultOfUsingThePass.Value = controlOfTheUseOfThePass.TheResultOfUsingThePass;
                    command.Parameters.Add(theResultOfUsingThePass);

                    SqlParameter numberOfDay = new SqlParameter();
                    numberOfDay.ParameterName = "@numberOfDay";
                    numberOfDay.SqlDbType = System.Data.SqlDbType.Int;
                    numberOfDay.Value = controlOfTheUseOfThePass.NumberOfDay;
                    command.Parameters.Add(numberOfDay);

                    SqlParameter condition = new SqlParameter();
                    condition.ParameterName = "@condition";
                    condition.SqlDbType = System.Data.SqlDbType.VarChar;
                    if (controlOfTheUseOfThePass.Condition == "")
                    {
                        condition.Value = "Undefined";
                    }
                    else
                    {
                        condition.Value = controlOfTheUseOfThePass.Condition;
                    }
                    command.Parameters.Add(condition);

                    SqlParameter sinceWhatTime = new SqlParameter();
                    sinceWhatTime.ParameterName = "@sinceWhatTime";
                    sinceWhatTime.SqlDbType = System.Data.SqlDbType.Time;
                    if (controlOfTheUseOfThePass.SinceWhatTime == "-" || controlOfTheUseOfThePass.SinceWhatTime == "")
                    {
                        sinceWhatTime.Value = "0:00:00";
                    }
                    else
                    {
                        sinceWhatTime.Value = controlOfTheUseOfThePass.SinceWhatTime;
                    }
                    command.Parameters.Add(sinceWhatTime);

                    SqlParameter tillWhatTime = new SqlParameter();
                    tillWhatTime.ParameterName = "@tillWhatTime";
                    tillWhatTime.SqlDbType = System.Data.SqlDbType.Time;
                    if (controlOfTheUseOfThePass.TillWhatTime == "-" || controlOfTheUseOfThePass.TillWhatTime == "")
                    {
                        tillWhatTime.Value = "0:00:00";
                    }
                    else
                    {
                        tillWhatTime.Value = controlOfTheUseOfThePass.TillWhatTime;
                    }
                    command.Parameters.Add(tillWhatTime);

                    SqlParameter value = new SqlParameter();
                    value.ParameterName = "@value";
                    value.SqlDbType = System.Data.SqlDbType.VarChar;
                    if (controlOfTheUseOfThePass.Value == "")
                    {
                        value.Value = "undefined";
                    }
                    else
                    {
                        value.Value = controlOfTheUseOfThePass.Value;
                    }
                    command.Parameters.Add(value);

                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        public void SelectingYear_PS(string numberOfYear, bool changingInformation, string numberOfYearFromForm)
        {
            try
            {
                string year_ = "";

                if (changingInformation == true)
                {
                    year_ = numberOfYear;
                }
                else
                {
                    year_ = numberOfYearFromForm;
                }

                switch (year_)
                {
                    case "2021":
                        Requests_2021_First();
                        Requests_2021_Second();
                        Requests_2021_Third();
                        Requests_2021_Fourth();
                        break;
                    case "2022":
                        Requests_2022_First();
                        Requests_2022_Second();
                        Requests_2022_Third();
                        Requests_2022_Fourth();
                        break;
                    case "2023":
                        Requests_2023_First();
                        Requests_2023_Second();
                        Requests_2023_Third();
                        Requests_2023_Fourth();
                        break;
                    case "2024":
                        Requests_2024_First();
                        Requests_2024_Second();
                        Requests_2024_Third();
                        Requests_2024_Fourth();
                        break;
                    case "2025":
                        Requests_2025_First();
                        Requests_2025_Second();
                        Requests_2025_Third();
                        Requests_2025_Fourth();
                        break;
                    case "2026":
                        Requests_2026_First();
                        Requests_2026_Second();
                        Requests_2026_Third();
                        Requests_2026_Fourth();
                        break;
                    case "2027":
                        Requests_2027_First();
                        Requests_2027_Second();
                        Requests_2027_Third();
                        Requests_2027_Fourth();
                        break;
                    case "2028":
                        Requests_2028_First();
                        Requests_2028_Second();
                        Requests_2028_Third();
                        Requests_2028_Fourth();
                        break;
                    case "2029":
                        Requests_2029_First();
                        Requests_2029_Second();
                        Requests_2029_Third();
                        Requests_2029_Fourth();
                        break;
                    case "2030":
                        Requests_2030_First();
                        Requests_2030_Second();
                        Requests_2030_Third();
                        Requests_2030_Fourth();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SelectingMonth_PS(string nameOfYear, string nameOfMonths, bool changingInformation, string nameOfYearFromForm, string nameOfMonthFromForm)
        {
            try
            {
                string month = "";
                string year_ = "";

                if (changingInformation == true)
                {
                    month = nameOfMonths;
                    year_ = nameOfYear;
                }
                else
                {
                    month = nameOfMonthFromForm;
                    year_ = nameOfYearFromForm;
                }

                switch (month)
                {
                    case "January":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[0];
                                secondCommandText = arrayOfRequestsSecond_2021[0];
                                thirdCommandText = arrayOfRequestsThird_2021[0];
                                fourthCommandText = arrayOfRequestsFourth_2021[0];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[0];
                                secondCommandText = arrayOfRequestsSecond_2022[0];
                                thirdCommandText = arrayOfRequestsThird_2022[0];
                                fourthCommandText = arrayOfRequestsFourth_2022[0];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[0];
                                secondCommandText = arrayOfRequestsSecond_2023[0];
                                thirdCommandText = arrayOfRequestsThird_2023[0];
                                fourthCommandText = arrayOfRequestsFourth_2023[0];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[0];
                                secondCommandText = arrayOfRequestsSecond_2024[0];
                                thirdCommandText = arrayOfRequestsThird_2024[0];
                                fourthCommandText = arrayOfRequestsFourth_2024[0];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[0];
                                secondCommandText = arrayOfRequestsSecond_2025[0];
                                thirdCommandText = arrayOfRequestsThird_2025[0];
                                fourthCommandText = arrayOfRequestsFourth_2025[0];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[0];
                                secondCommandText = arrayOfRequestsSecond_2026[0];
                                thirdCommandText = arrayOfRequestsThird_2026[0];
                                fourthCommandText = arrayOfRequestsFourth_2026[0];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[0];
                                secondCommandText = arrayOfRequestsSecond_2027[0];
                                thirdCommandText = arrayOfRequestsThird_2027[0];
                                fourthCommandText = arrayOfRequestsFourth_2027[0];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[0];
                                secondCommandText = arrayOfRequestsSecond_2028[0];
                                thirdCommandText = arrayOfRequestsThird_2028[0];
                                fourthCommandText = arrayOfRequestsFourth_2028[0];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[0];
                                secondCommandText = arrayOfRequestsSecond_2029[0];
                                thirdCommandText = arrayOfRequestsThird_2029[0];
                                fourthCommandText = arrayOfRequestsFourth_2029[0];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[0];
                                secondCommandText = arrayOfRequestsSecond_2030[0];
                                thirdCommandText = arrayOfRequestsThird_2030[0];
                                fourthCommandText = arrayOfRequestsFourth_2030[0];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "February":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[1];
                                secondCommandText = arrayOfRequestsSecond_2021[1];
                                thirdCommandText = arrayOfRequestsThird_2021[1];
                                fourthCommandText = arrayOfRequestsFourth_2021[1];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[1];
                                secondCommandText = arrayOfRequestsSecond_2022[1];
                                thirdCommandText = arrayOfRequestsThird_2022[1];
                                fourthCommandText = arrayOfRequestsFourth_2022[1];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[1];
                                secondCommandText = arrayOfRequestsSecond_2023[1];
                                thirdCommandText = arrayOfRequestsThird_2023[1];
                                fourthCommandText = arrayOfRequestsFourth_2023[1];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[1];
                                secondCommandText = arrayOfRequestsSecond_2024[1];
                                thirdCommandText = arrayOfRequestsThird_2024[1];
                                fourthCommandText = arrayOfRequestsFourth_2024[1];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[1];
                                secondCommandText = arrayOfRequestsSecond_2025[1];
                                thirdCommandText = arrayOfRequestsThird_2025[1];
                                fourthCommandText = arrayOfRequestsFourth_2025[1];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[1];
                                secondCommandText = arrayOfRequestsSecond_2026[1];
                                thirdCommandText = arrayOfRequestsThird_2026[1];
                                fourthCommandText = arrayOfRequestsFourth_2026[1];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[1];
                                secondCommandText = arrayOfRequestsSecond_2027[1];
                                thirdCommandText = arrayOfRequestsThird_2027[1];
                                fourthCommandText = arrayOfRequestsFourth_2027[1];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[1];
                                secondCommandText = arrayOfRequestsSecond_2028[1];
                                thirdCommandText = arrayOfRequestsThird_2028[1];
                                fourthCommandText = arrayOfRequestsFourth_2028[1];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[1];
                                secondCommandText = arrayOfRequestsSecond_2029[1];
                                thirdCommandText = arrayOfRequestsThird_2029[1];
                                fourthCommandText = arrayOfRequestsFourth_2029[1];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[1];
                                secondCommandText = arrayOfRequestsSecond_2030[1];
                                thirdCommandText = arrayOfRequestsThird_2030[1];
                                fourthCommandText = arrayOfRequestsFourth_2030[1];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "March":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[2];
                                secondCommandText = arrayOfRequestsSecond_2021[2];
                                thirdCommandText = arrayOfRequestsThird_2021[2];
                                fourthCommandText = arrayOfRequestsFourth_2021[2];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[2];
                                secondCommandText = arrayOfRequestsSecond_2022[2];
                                thirdCommandText = arrayOfRequestsThird_2022[2];
                                fourthCommandText = arrayOfRequestsFourth_2022[2];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[2];
                                secondCommandText = arrayOfRequestsSecond_2023[2];
                                thirdCommandText = arrayOfRequestsThird_2023[2];
                                fourthCommandText = arrayOfRequestsFourth_2023[2];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[2];
                                secondCommandText = arrayOfRequestsSecond_2024[2];
                                thirdCommandText = arrayOfRequestsThird_2024[2];
                                fourthCommandText = arrayOfRequestsFourth_2024[2];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[2];
                                secondCommandText = arrayOfRequestsSecond_2025[2];
                                thirdCommandText = arrayOfRequestsThird_2025[2];
                                fourthCommandText = arrayOfRequestsFourth_2025[2];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[2];
                                secondCommandText = arrayOfRequestsSecond_2026[2];
                                thirdCommandText = arrayOfRequestsThird_2026[2];
                                fourthCommandText = arrayOfRequestsFourth_2026[2];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[2];
                                secondCommandText = arrayOfRequestsSecond_2027[2];
                                thirdCommandText = arrayOfRequestsThird_2027[2];
                                fourthCommandText = arrayOfRequestsFourth_2027[2];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[2];
                                secondCommandText = arrayOfRequestsSecond_2028[2];
                                thirdCommandText = arrayOfRequestsThird_2028[2];
                                fourthCommandText = arrayOfRequestsFourth_2028[2];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[2];
                                secondCommandText = arrayOfRequestsSecond_2029[2];
                                thirdCommandText = arrayOfRequestsThird_2029[2];
                                fourthCommandText = arrayOfRequestsFourth_2029[2];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[2];
                                secondCommandText = arrayOfRequestsSecond_2030[2];
                                thirdCommandText = arrayOfRequestsThird_2030[2];
                                fourthCommandText = arrayOfRequestsFourth_2030[2];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "April":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[3];
                                secondCommandText = arrayOfRequestsSecond_2021[3];
                                thirdCommandText = arrayOfRequestsThird_2021[3];
                                fourthCommandText = arrayOfRequestsFourth_2021[3];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[3];
                                secondCommandText = arrayOfRequestsSecond_2022[3];
                                thirdCommandText = arrayOfRequestsThird_2022[3];
                                fourthCommandText = arrayOfRequestsFourth_2022[3];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[3];
                                secondCommandText = arrayOfRequestsSecond_2023[3];
                                thirdCommandText = arrayOfRequestsThird_2023[3];
                                fourthCommandText = arrayOfRequestsFourth_2023[3];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[3];
                                secondCommandText = arrayOfRequestsSecond_2024[3];
                                thirdCommandText = arrayOfRequestsThird_2024[3];
                                fourthCommandText = arrayOfRequestsFourth_2024[3];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[3];
                                secondCommandText = arrayOfRequestsSecond_2025[3];
                                thirdCommandText = arrayOfRequestsThird_2025[3];
                                fourthCommandText = arrayOfRequestsFourth_2025[3];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[3];
                                secondCommandText = arrayOfRequestsSecond_2026[3];
                                thirdCommandText = arrayOfRequestsThird_2026[3];
                                fourthCommandText = arrayOfRequestsFourth_2026[3];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[3];
                                secondCommandText = arrayOfRequestsSecond_2027[3];
                                thirdCommandText = arrayOfRequestsThird_2027[3];
                                fourthCommandText = arrayOfRequestsFourth_2027[3];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[3];
                                secondCommandText = arrayOfRequestsSecond_2028[3];
                                thirdCommandText = arrayOfRequestsThird_2028[3];
                                fourthCommandText = arrayOfRequestsFourth_2028[3];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[3];
                                secondCommandText = arrayOfRequestsSecond_2029[3];
                                thirdCommandText = arrayOfRequestsThird_2029[3];
                                fourthCommandText = arrayOfRequestsFourth_2029[3];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[3];
                                secondCommandText = arrayOfRequestsSecond_2030[3];
                                thirdCommandText = arrayOfRequestsThird_2030[3];
                                fourthCommandText = arrayOfRequestsFourth_2030[3];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "May":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[4];
                                secondCommandText = arrayOfRequestsSecond_2021[4];
                                thirdCommandText = arrayOfRequestsThird_2021[4];
                                fourthCommandText = arrayOfRequestsFourth_2021[4];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[4];
                                secondCommandText = arrayOfRequestsSecond_2022[4];
                                thirdCommandText = arrayOfRequestsThird_2022[4];
                                fourthCommandText = arrayOfRequestsFourth_2022[4];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[4];
                                secondCommandText = arrayOfRequestsSecond_2023[4];
                                thirdCommandText = arrayOfRequestsThird_2023[4];
                                fourthCommandText = arrayOfRequestsFourth_2023[4];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[4];
                                secondCommandText = arrayOfRequestsSecond_2024[4];
                                thirdCommandText = arrayOfRequestsThird_2024[4];
                                fourthCommandText = arrayOfRequestsFourth_2024[4];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[4];
                                secondCommandText = arrayOfRequestsSecond_2025[4];
                                thirdCommandText = arrayOfRequestsThird_2025[4];
                                fourthCommandText = arrayOfRequestsFourth_2025[4];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[4];
                                secondCommandText = arrayOfRequestsSecond_2026[4];
                                thirdCommandText = arrayOfRequestsThird_2026[4];
                                fourthCommandText = arrayOfRequestsFourth_2026[4];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[4];
                                secondCommandText = arrayOfRequestsSecond_2027[4];
                                thirdCommandText = arrayOfRequestsThird_2027[4];
                                fourthCommandText = arrayOfRequestsFourth_2027[4];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[4];
                                secondCommandText = arrayOfRequestsSecond_2028[4];
                                thirdCommandText = arrayOfRequestsThird_2028[4];
                                fourthCommandText = arrayOfRequestsFourth_2028[4];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[4];
                                secondCommandText = arrayOfRequestsSecond_2029[4];
                                thirdCommandText = arrayOfRequestsThird_2029[4];
                                fourthCommandText = arrayOfRequestsFourth_2029[4];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[4];
                                secondCommandText = arrayOfRequestsSecond_2030[4];
                                thirdCommandText = arrayOfRequestsThird_2030[4];
                                fourthCommandText = arrayOfRequestsFourth_2030[4];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "June":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[5];
                                secondCommandText = arrayOfRequestsSecond_2021[5];
                                thirdCommandText = arrayOfRequestsThird_2021[5];
                                fourthCommandText = arrayOfRequestsFourth_2021[5];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[5];
                                secondCommandText = arrayOfRequestsSecond_2022[5];
                                thirdCommandText = arrayOfRequestsThird_2022[5];
                                fourthCommandText = arrayOfRequestsFourth_2022[5];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[5];
                                secondCommandText = arrayOfRequestsSecond_2023[5];
                                thirdCommandText = arrayOfRequestsThird_2023[5];
                                fourthCommandText = arrayOfRequestsFourth_2023[5];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[5];
                                secondCommandText = arrayOfRequestsSecond_2024[5];
                                thirdCommandText = arrayOfRequestsThird_2024[5];
                                fourthCommandText = arrayOfRequestsFourth_2024[5];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[5];
                                secondCommandText = arrayOfRequestsSecond_2025[5];
                                thirdCommandText = arrayOfRequestsThird_2025[5];
                                fourthCommandText = arrayOfRequestsFourth_2025[5];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[5];
                                secondCommandText = arrayOfRequestsSecond_2026[5];
                                thirdCommandText = arrayOfRequestsThird_2026[5];
                                fourthCommandText = arrayOfRequestsFourth_2026[5];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[5];
                                secondCommandText = arrayOfRequestsSecond_2027[5];
                                thirdCommandText = arrayOfRequestsThird_2027[5];
                                fourthCommandText = arrayOfRequestsFourth_2027[5];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[5];
                                secondCommandText = arrayOfRequestsSecond_2028[5];
                                thirdCommandText = arrayOfRequestsThird_2028[5];
                                fourthCommandText = arrayOfRequestsFourth_2028[5];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[5];
                                secondCommandText = arrayOfRequestsSecond_2029[5];
                                thirdCommandText = arrayOfRequestsThird_2029[5];
                                fourthCommandText = arrayOfRequestsFourth_2029[5];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[5];
                                secondCommandText = arrayOfRequestsSecond_2030[5];
                                thirdCommandText = arrayOfRequestsThird_2030[5];
                                fourthCommandText = arrayOfRequestsFourth_2030[5];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "July":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[6];
                                secondCommandText = arrayOfRequestsSecond_2021[6];
                                thirdCommandText = arrayOfRequestsThird_2021[6];
                                fourthCommandText = arrayOfRequestsFourth_2021[6];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[6];
                                secondCommandText = arrayOfRequestsSecond_2022[6];
                                thirdCommandText = arrayOfRequestsThird_2022[6];
                                fourthCommandText = arrayOfRequestsFourth_2022[6];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[6];
                                secondCommandText = arrayOfRequestsSecond_2023[6];
                                thirdCommandText = arrayOfRequestsThird_2023[6];
                                fourthCommandText = arrayOfRequestsFourth_2023[6];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[6];
                                secondCommandText = arrayOfRequestsSecond_2024[6];
                                thirdCommandText = arrayOfRequestsThird_2024[6];
                                fourthCommandText = arrayOfRequestsFourth_2024[6];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[6];
                                secondCommandText = arrayOfRequestsSecond_2025[6];
                                thirdCommandText = arrayOfRequestsThird_2025[6];
                                fourthCommandText = arrayOfRequestsFourth_2025[6];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[6];
                                secondCommandText = arrayOfRequestsSecond_2026[6];
                                thirdCommandText = arrayOfRequestsThird_2026[6];
                                fourthCommandText = arrayOfRequestsFourth_2026[6];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[6];
                                secondCommandText = arrayOfRequestsSecond_2027[6];
                                thirdCommandText = arrayOfRequestsThird_2027[6];
                                fourthCommandText = arrayOfRequestsFourth_2027[6];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[6];
                                secondCommandText = arrayOfRequestsSecond_2028[6];
                                thirdCommandText = arrayOfRequestsThird_2028[6];
                                fourthCommandText = arrayOfRequestsFourth_2028[6];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[6];
                                secondCommandText = arrayOfRequestsSecond_2029[6];
                                thirdCommandText = arrayOfRequestsThird_2029[6];
                                fourthCommandText = arrayOfRequestsFourth_2029[6];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[6];
                                secondCommandText = arrayOfRequestsSecond_2030[6];
                                thirdCommandText = arrayOfRequestsThird_2030[6];
                                fourthCommandText = arrayOfRequestsFourth_2030[6];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "August":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[7];
                                secondCommandText = arrayOfRequestsSecond_2021[7];
                                thirdCommandText = arrayOfRequestsThird_2021[7];
                                fourthCommandText = arrayOfRequestsFourth_2021[7];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[7];
                                secondCommandText = arrayOfRequestsSecond_2022[7];
                                thirdCommandText = arrayOfRequestsThird_2022[7];
                                fourthCommandText = arrayOfRequestsFourth_2022[7];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[7];
                                secondCommandText = arrayOfRequestsSecond_2023[7];
                                thirdCommandText = arrayOfRequestsThird_2023[7];
                                fourthCommandText = arrayOfRequestsFourth_2023[7];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[7];
                                secondCommandText = arrayOfRequestsSecond_2024[7];
                                thirdCommandText = arrayOfRequestsThird_2024[7];
                                fourthCommandText = arrayOfRequestsFourth_2024[7];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[7];
                                secondCommandText = arrayOfRequestsSecond_2025[7];
                                thirdCommandText = arrayOfRequestsThird_2025[7];
                                fourthCommandText = arrayOfRequestsFourth_2025[7];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[7];
                                secondCommandText = arrayOfRequestsSecond_2026[7];
                                thirdCommandText = arrayOfRequestsThird_2026[7];
                                fourthCommandText = arrayOfRequestsFourth_2026[7];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[7];
                                secondCommandText = arrayOfRequestsSecond_2027[7];
                                thirdCommandText = arrayOfRequestsThird_2027[7];
                                fourthCommandText = arrayOfRequestsFourth_2027[7];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[7];
                                secondCommandText = arrayOfRequestsSecond_2028[7];
                                thirdCommandText = arrayOfRequestsThird_2028[7];
                                fourthCommandText = arrayOfRequestsFourth_2028[7];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[7];
                                secondCommandText = arrayOfRequestsSecond_2029[7];
                                thirdCommandText = arrayOfRequestsThird_2029[7];
                                fourthCommandText = arrayOfRequestsFourth_2029[7];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[7];
                                secondCommandText = arrayOfRequestsSecond_2030[7];
                                thirdCommandText = arrayOfRequestsThird_2030[7];
                                fourthCommandText = arrayOfRequestsFourth_2030[7];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "September":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[8];
                                secondCommandText = arrayOfRequestsSecond_2021[8];
                                thirdCommandText = arrayOfRequestsThird_2021[8];
                                fourthCommandText = arrayOfRequestsFourth_2021[8];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[8];
                                secondCommandText = arrayOfRequestsSecond_2022[8];
                                thirdCommandText = arrayOfRequestsThird_2022[8];
                                fourthCommandText = arrayOfRequestsFourth_2022[8];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[8];
                                secondCommandText = arrayOfRequestsSecond_2023[8];
                                thirdCommandText = arrayOfRequestsThird_2023[8];
                                fourthCommandText = arrayOfRequestsFourth_2023[8];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[8];
                                secondCommandText = arrayOfRequestsSecond_2024[8];
                                thirdCommandText = arrayOfRequestsThird_2024[8];
                                fourthCommandText = arrayOfRequestsFourth_2024[8];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[8];
                                secondCommandText = arrayOfRequestsSecond_2025[8];
                                thirdCommandText = arrayOfRequestsThird_2025[8];
                                fourthCommandText = arrayOfRequestsFourth_2025[8];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[8];
                                secondCommandText = arrayOfRequestsSecond_2026[8];
                                thirdCommandText = arrayOfRequestsThird_2026[8];
                                fourthCommandText = arrayOfRequestsFourth_2026[8];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[8];
                                secondCommandText = arrayOfRequestsSecond_2027[8];
                                thirdCommandText = arrayOfRequestsThird_2027[8];
                                fourthCommandText = arrayOfRequestsFourth_2027[8];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[8];
                                secondCommandText = arrayOfRequestsSecond_2028[8];
                                thirdCommandText = arrayOfRequestsThird_2028[8];
                                fourthCommandText = arrayOfRequestsFourth_2028[8];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[8];
                                secondCommandText = arrayOfRequestsSecond_2029[8];
                                thirdCommandText = arrayOfRequestsThird_2029[8];
                                fourthCommandText = arrayOfRequestsFourth_2029[8];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[8];
                                secondCommandText = arrayOfRequestsSecond_2030[8];
                                thirdCommandText = arrayOfRequestsThird_2030[8];
                                fourthCommandText = arrayOfRequestsFourth_2030[8];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "October":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[9];
                                secondCommandText = arrayOfRequestsSecond_2021[9];
                                thirdCommandText = arrayOfRequestsThird_2021[9];
                                fourthCommandText = arrayOfRequestsFourth_2021[9];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[9];
                                secondCommandText = arrayOfRequestsSecond_2022[9];
                                thirdCommandText = arrayOfRequestsThird_2022[9];
                                fourthCommandText = arrayOfRequestsFourth_2022[9];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[9];
                                secondCommandText = arrayOfRequestsSecond_2023[9];
                                thirdCommandText = arrayOfRequestsThird_2023[9];
                                fourthCommandText = arrayOfRequestsFourth_2023[9];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[9];
                                secondCommandText = arrayOfRequestsSecond_2024[9];
                                thirdCommandText = arrayOfRequestsThird_2024[9];
                                fourthCommandText = arrayOfRequestsFourth_2024[9];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[9];
                                secondCommandText = arrayOfRequestsSecond_2025[9];
                                thirdCommandText = arrayOfRequestsThird_2025[9];
                                fourthCommandText = arrayOfRequestsFourth_2025[9];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[9];
                                secondCommandText = arrayOfRequestsSecond_2026[9];
                                thirdCommandText = arrayOfRequestsThird_2026[9];
                                fourthCommandText = arrayOfRequestsFourth_2026[9];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[9];
                                secondCommandText = arrayOfRequestsSecond_2027[9];
                                thirdCommandText = arrayOfRequestsThird_2027[9];
                                fourthCommandText = arrayOfRequestsFourth_2027[9];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[9];
                                secondCommandText = arrayOfRequestsSecond_2028[9];
                                thirdCommandText = arrayOfRequestsThird_2028[9];
                                fourthCommandText = arrayOfRequestsFourth_2028[9];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[9];
                                secondCommandText = arrayOfRequestsSecond_2029[9];
                                thirdCommandText = arrayOfRequestsThird_2029[9];
                                fourthCommandText = arrayOfRequestsFourth_2029[9];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[9];
                                secondCommandText = arrayOfRequestsSecond_2030[9];
                                thirdCommandText = arrayOfRequestsThird_2030[9];
                                fourthCommandText = arrayOfRequestsFourth_2030[9];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "November":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[10];
                                secondCommandText = arrayOfRequestsSecond_2021[10];
                                thirdCommandText = arrayOfRequestsThird_2021[10];
                                fourthCommandText = arrayOfRequestsFourth_2021[10];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[10];
                                secondCommandText = arrayOfRequestsSecond_2022[10];
                                thirdCommandText = arrayOfRequestsThird_2022[10];
                                fourthCommandText = arrayOfRequestsFourth_2022[10];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[10];
                                secondCommandText = arrayOfRequestsSecond_2023[10];
                                thirdCommandText = arrayOfRequestsThird_2023[10];
                                fourthCommandText = arrayOfRequestsFourth_2023[10];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[10];
                                secondCommandText = arrayOfRequestsSecond_2024[10];
                                thirdCommandText = arrayOfRequestsThird_2024[10];
                                fourthCommandText = arrayOfRequestsFourth_2024[10];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[10];
                                secondCommandText = arrayOfRequestsSecond_2025[10];
                                thirdCommandText = arrayOfRequestsThird_2025[10];
                                fourthCommandText = arrayOfRequestsFourth_2025[10];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[10];
                                secondCommandText = arrayOfRequestsSecond_2026[10];
                                thirdCommandText = arrayOfRequestsThird_2026[10];
                                fourthCommandText = arrayOfRequestsFourth_2026[10];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[10];
                                secondCommandText = arrayOfRequestsSecond_2027[10];
                                thirdCommandText = arrayOfRequestsThird_2027[10];
                                fourthCommandText = arrayOfRequestsFourth_2027[10];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[10];
                                secondCommandText = arrayOfRequestsSecond_2028[10];
                                thirdCommandText = arrayOfRequestsThird_2028[10];
                                fourthCommandText = arrayOfRequestsFourth_2028[10];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[10];
                                secondCommandText = arrayOfRequestsSecond_2029[10];
                                thirdCommandText = arrayOfRequestsThird_2029[10];
                                fourthCommandText = arrayOfRequestsFourth_2029[10];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[10];
                                secondCommandText = arrayOfRequestsSecond_2030[10];
                                thirdCommandText = arrayOfRequestsThird_2030[10];
                                fourthCommandText = arrayOfRequestsFourth_2030[10];
                                break;
                            default:
                                break;
                        }
                        break;

                    case "December":

                        switch (year_)
                        {
                            case "2021":
                                firstCommandText = arrayOfRequestsFirst_2021[11];
                                secondCommandText = arrayOfRequestsSecond_2021[11];
                                thirdCommandText = arrayOfRequestsThird_2021[11];
                                fourthCommandText = arrayOfRequestsFourth_2021[11];
                                break;
                            case "2022":
                                firstCommandText = arrayOfRequestsFirst_2022[11];
                                secondCommandText = arrayOfRequestsSecond_2022[11];
                                thirdCommandText = arrayOfRequestsThird_2022[11];
                                fourthCommandText = arrayOfRequestsFourth_2022[11];
                                break;
                            case "2023":
                                firstCommandText = arrayOfRequestsFirst_2023[11];
                                secondCommandText = arrayOfRequestsSecond_2023[11];
                                thirdCommandText = arrayOfRequestsThird_2023[11];
                                fourthCommandText = arrayOfRequestsFourth_2023[11];
                                break;
                            case "2024":
                                firstCommandText = arrayOfRequestsFirst_2024[11];
                                secondCommandText = arrayOfRequestsSecond_2024[11];
                                thirdCommandText = arrayOfRequestsThird_2024[11];
                                fourthCommandText = arrayOfRequestsFourth_2024[11];
                                break;
                            case "2025":
                                firstCommandText = arrayOfRequestsFirst_2025[11];
                                secondCommandText = arrayOfRequestsSecond_2025[11];
                                thirdCommandText = arrayOfRequestsThird_2025[11];
                                fourthCommandText = arrayOfRequestsFourth_2025[11];
                                break;
                            case "2026":
                                firstCommandText = arrayOfRequestsFirst_2026[11];
                                secondCommandText = arrayOfRequestsSecond_2026[11];
                                thirdCommandText = arrayOfRequestsThird_2026[11];
                                fourthCommandText = arrayOfRequestsFourth_2026[11];
                                break;
                            case "2027":
                                firstCommandText = arrayOfRequestsFirst_2027[11];
                                secondCommandText = arrayOfRequestsSecond_2027[11];
                                thirdCommandText = arrayOfRequestsThird_2027[11];
                                fourthCommandText = arrayOfRequestsFourth_2027[11];
                                break;
                            case "2028":
                                firstCommandText = arrayOfRequestsFirst_2028[11];
                                secondCommandText = arrayOfRequestsSecond_2028[11];
                                thirdCommandText = arrayOfRequestsThird_2028[11];
                                fourthCommandText = arrayOfRequestsFourth_2028[11];
                                break;
                            case "2029":
                                firstCommandText = arrayOfRequestsFirst_2029[11];
                                secondCommandText = arrayOfRequestsSecond_2029[11];
                                thirdCommandText = arrayOfRequestsThird_2029[11];
                                fourthCommandText = arrayOfRequestsFourth_2029[11];
                                break;
                            case "2030":
                                firstCommandText = arrayOfRequestsFirst_2030[11];
                                secondCommandText = arrayOfRequestsSecond_2030[11];
                                thirdCommandText = arrayOfRequestsThird_2030[11];
                                fourthCommandText = arrayOfRequestsFourth_2030[11];
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetInformationAboutFirstShift_PS(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm)
        {
            SqlCommand first = new SqlCommand();
            first.CommandText = firstCommandText;
            first.Connection = (SqlConnection)connection;

            SqlDataAdapter adapterFirst = new SqlDataAdapter(first);
            DataTable tableFirst = new DataTable();
            adapterFirst.Fill(tableFirst);

            if (passageControl == true)
            {
                FillingInWithData(tableFirst, numOfDays, true);
            }
            else
            {
                FillingInWithData(tableFirst, 0, false);
            }
        }

        public void GetInformationAboutSecondShift_PS(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm)
        {
            SqlCommand second = new SqlCommand();
            second.CommandText = secondCommandText;
            second.Connection = (SqlConnection)connection;

            SqlDataAdapter adapterSecond = new SqlDataAdapter(second);
            DataTable tableSecond = new DataTable();
            adapterSecond.Fill(tableSecond);

            if (passageControl == true)
            {
                FillingInWithData(tableSecond, numOfDays, true);
            }
            else
            {
                FillingInWithData(tableSecond, 0, false);
            }
        }

        public void GetInformationAboutThirdShift_PS(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm)
        {
            SqlCommand third = new SqlCommand();
            third.CommandText = thirdCommandText;
            third.Connection = (SqlConnection)connection;

            SqlDataAdapter adapterThird = new SqlDataAdapter(third);
            DataTable tableThird = new DataTable();
            adapterThird.Fill(tableThird);

            if (passageControl == true)
            {
                FillingInWithData(tableThird, numOfDays, true);
            }
            else
            {
                FillingInWithData(tableThird, 0, false);
            }
        }

        public void GetInformationAboutFourthShift_PS(bool passageControl, int numOfDays, string nameOfYearFromForm, string nameOfMonthFromForm)
        {
            SqlCommand fourth = new SqlCommand();
            fourth.CommandText = fourthCommandText;
            fourth.Connection = (SqlConnection)connection;

            SqlDataAdapter adapterFourth = new SqlDataAdapter(fourth);
            DataTable tableFourth = new DataTable();
            adapterFourth.Fill(tableFourth);

            if (passageControl == true)
            {
                FillingInWithData(tableFourth, numOfDays, true);
            }
            else
            {
                FillingInWithData(tableFourth, 0, false);
            }

            ReleaseConnection(connection);
        }

        private const string Get_WorkerId_From_Changed_Information = "SELECT DISTINCT Worker_Id FROM Individual_Work_Shedule_ WHERE Year = @year AND Month = @month";
        public DataTable GetWorkerIdFromChangedInformation(int year, string month)
        {
            DataTable table = new DataTable("Table");

            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = Get_WorkerId_From_Changed_Information;
                IDbConnection connection = GetConnection();
                command.Connection = (SqlConnection)connection;

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlParameter month_ = new SqlParameter();
                month_.ParameterName = "@month";
                month_.SqlDbType = System.Data.SqlDbType.VarChar;
                month_.Value = month;
                command.Parameters.Add(month_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        public int AvailabilityOfASpecificWorkerId_PS(int workerId)
        {
            int id = 0;

            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = Availability_Of_A_Specific_Worker_Id;

                IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return id;
        }

        private const string Get_Values_Of_Time = "SELECT Since_What_Time, Till_What_Time FROM Individual_Work_Shedule_ WHERE Number_Of_Day = @numberOfDay AND Year = @year AND Month = @month AND Worker_Id = @workerId";
        public DataTable GetValuesOfTime(int numberOfDay, int year, string month, int workerId)
        {
            DataTable table = new DataTable("Table");

            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = Get_Values_Of_Time;
                IDbConnection connection = GetConnection();
                command.Connection = (SqlConnection)connection;

                SqlParameter numberOfDay_ = new SqlParameter();
                numberOfDay_.ParameterName = "@numberOfDay";
                numberOfDay_.SqlDbType = System.Data.SqlDbType.Int;
                numberOfDay_.Value = numberOfDay;
                command.Parameters.Add(numberOfDay_);

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlParameter month_ = new SqlParameter();
                month_.ParameterName = "@month";
                month_.SqlDbType = System.Data.SqlDbType.VarChar;
                month_.Value = month;
                command.Parameters.Add(month_);

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        public List<int> NumberOfDayShift_PS()
        {
            return numberOfDayShift;
        }

        public List<int> NumberOfNightShift_PS()
        {
            return numberOfNightShift;
        }

        public List<int> NumberOfDayOff_PS()
        {
            return numberOfDayOff;
        }

        public List<int> NumberOfEndDayOff_PS()
        {
            return numberOfEndDayOff;
        }

        public void ClearData_PS()
        {
            numberOfDayShift.Clear();
            numberOfNightShift.Clear();
            numberOfDayOff.Clear();
            numberOfEndDayOff.Clear();
        }

        public DataTable GetChangedInformation_PS(int year, string month, int workId)
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();

                command.CommandText = GetChangedInformation_;

                command.Connection = (SqlConnection)connection;

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlParameter month_ = new SqlParameter();
                month_.ParameterName = "@month";
                month_.SqlDbType = System.Data.SqlDbType.VarChar;
                month_.Value = month;
                command.Parameters.Add(month_);

                SqlParameter workId_ = new SqlParameter();
                workId_.ParameterName = "@workerId";
                workId_.SqlDbType = System.Data.SqlDbType.Int;
                workId_.Value = workId;
                command.Parameters.Add(workId_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        public List<int> SelectingSaturdayOrSundayForChangeInformation_PS(int years, string months)
        {
            List<int> numOfDay = new List<int>();

            try
            {
                int year = years;

                string month = months;

                int numberOfMonth = 0;

                switch (month)
                {
                    case "January":
                        numberOfMonth = 1;
                        break;
                    case "February":
                        numberOfMonth = 2;
                        break;
                    case "March":
                        numberOfMonth = 3;
                        break;
                    case "April":
                        numberOfMonth = 4;
                        break;
                    case "May":
                        numberOfMonth = 5;
                        break;
                    case "June":
                        numberOfMonth = 6;
                        break;
                    case "July":
                        numberOfMonth = 7;
                        break;
                    case "August":
                        numberOfMonth = 8;
                        break;
                    case "September":
                        numberOfMonth = 9;
                        break;
                    case "October":
                        numberOfMonth = 10;
                        break;
                    case "November":
                        numberOfMonth = 11;
                        break;
                    case "December":
                        numberOfMonth = 12;
                        break;
                    default:
                        break;
                }

                DateTime date = new DateTime(year, numberOfMonth, 1);

                numOfDayOfMonth = DateTime.DaysInMonth(year, numberOfMonth);

                for (int i = 0; i < numOfDayOfMonth; i++)
                {
                    if (date.DayOfWeek.Equals(DayOfWeek.Saturday) || date.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        numOfDay.Add(i + 1);
                    }

                    date = date.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return numOfDay;
        }

        public DataTable FindWorkerById_PS(int id)
        {
            DataTable table = new DataTable("Table");

            try
            {
                IDbConnection connection = GetConnection();

                SqlCommand command = new SqlCommand();

                command.CommandText = GET_WORKER_BY_ID;

                command.Connection = (SqlConnection)connection;

                SqlParameter id_ = new SqlParameter();
                id_.ParameterName = "@id";
                id_.SqlDbType = System.Data.SqlDbType.Int;
                id_.Value = id;
                command.Parameters.Add(id_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                command.ExecuteNonQuery();

                ReleaseConnection(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return table;
        }

        //InformationAboutUseThePass///////////////////////////////////////////////////////////////////////

        private const string GetAllInformationAboutUseThePass_ = "SELECT DISTINCT Year, Month, NumberOfDay, Condition, SinceWhatTime,TillWhatTime, Value, WorkerId, TimeOfUseOfThePass, TheResultOfUsingOfThePass FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker";
        public DataTable GetAllInformationAboutUseThePass()
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();
                command.CommandText = GetAllInformationAboutUseThePass_;
                connection = GetConnection();
                command.Connection = (SqlConnection)connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        private const string GetAllInformationAboutUseThePassByWorkerId_ = "SELECT DISTINCT Year, Month, NumberOfDay, Condition, SinceWhatTime,TillWhatTime, Value, WorkerId, TimeOfUseOfThePass, TheResultOfUsingOfThePass FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId";
        public DataTable GetAllInformationAboutUseThePassByWorkerId(int workerId)
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();
                command.CommandText = GetAllInformationAboutUseThePassByWorkerId_;
                connection = GetConnection();
                command.Connection = (SqlConnection)connection;

                SqlParameter workId = new SqlParameter();
                workId.ParameterName = "@workerId";
                workId.SqlDbType = System.Data.SqlDbType.Int;
                workId.Value = workerId;
                command.Parameters.Add(workId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        private const string GetAllInformationAboutUseThePassByWorkerIdYear_ = "SELECT DISTINCT Year, Month, NumberOfDay, Condition, SinceWhatTime,TillWhatTime, Value, WorkerId, TimeOfUseOfThePass, TheResultOfUsingOfThePass FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId AND Year = @year";
        public DataTable GetAllInformationAboutUseThePassByWorkerIdYear(int workerId, int year)
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();
                command.CommandText = GetAllInformationAboutUseThePassByWorkerIdYear_;
                connection = GetConnection();
                command.Connection = (SqlConnection)connection;

                SqlParameter workId = new SqlParameter();
                workId.ParameterName = "@workerId";
                workId.SqlDbType = System.Data.SqlDbType.Int;
                workId.Value = workerId;
                command.Parameters.Add(workId);

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        private const string GetAllInformationAboutUseThePassByWorkerIdYearMonth_ = "SELECT DISTINCT Year, Month, NumberOfDay, Condition, SinceWhatTime,TillWhatTime, Value, WorkerId, TimeOfUseOfThePass, TheResultOfUsingOfThePass FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId AND Year = @year AND Month = @month";
        public DataTable GetAllInformationAboutUseThePassByWorkerIdYearMonth(int workerId, int year, string month)
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();
                command.CommandText = GetAllInformationAboutUseThePassByWorkerIdYearMonth_;
                connection = GetConnection();
                command.Connection = (SqlConnection)connection;

                SqlParameter workId = new SqlParameter();
                workId.ParameterName = "@workerId";
                workId.SqlDbType = System.Data.SqlDbType.Int;
                workId.Value = workerId;
                command.Parameters.Add(workId);

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlParameter month_ = new SqlParameter();
                month_.ParameterName = "@month";
                month_.SqlDbType = System.Data.SqlDbType.VarChar;
                month_.Value = month;
                command.Parameters.Add(month_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        private const string GetAllInformationAboutUseThePassByWorkerIdYearMonthNumberOfDay_ = "SELECT DISTINCT Year, Month, NumberOfDay, Condition, SinceWhatTime,TillWhatTime, Value, WorkerId, TimeOfUseOfThePass, TheResultOfUsingOfThePass FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId AND Year = @year AND Month = @month AND NumberOfDay = @numberOfDay";
        public DataTable GetAllInformationAboutUseThePassByWorkerIdYearMonthNumberOfDay(int workerId, int year, string month, int numberOfDay)
        {
            DataTable table = new DataTable("Table");

            try
            {
                command = new SqlCommand();
                command.CommandText = GetAllInformationAboutUseThePassByWorkerIdYearMonthNumberOfDay_;
                connection = GetConnection();
                command.Connection = (SqlConnection)connection;

                SqlParameter workId = new SqlParameter();
                workId.ParameterName = "@workerId";
                workId.SqlDbType = System.Data.SqlDbType.Int;
                workId.Value = workerId;
                command.Parameters.Add(workId);

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlParameter month_ = new SqlParameter();
                month_.ParameterName = "@month";
                month_.SqlDbType = System.Data.SqlDbType.VarChar;
                month_.Value = month;
                command.Parameters.Add(month_);

                SqlParameter numberOfDay_ = new SqlParameter();
                numberOfDay_.ParameterName = "@numberOfDay";
                numberOfDay_.SqlDbType = System.Data.SqlDbType.Int;
                numberOfDay_.Value = numberOfDay;
                command.Parameters.Add(numberOfDay_);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return table;
        }

        private const string DeleteAllFromInformationAboutUseThePass_ = "DELETE FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker";
        public int DeleteAllFromInformationAboutUseThePass()
        {
            int count = 0;

            try
            {
                command = new SqlCommand();

                command.CommandText = DeleteAllFromInformationAboutUseThePass_;

                connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                count = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return count;
        }

        private const string DeleteAllInformationAboutUseThePassByWorkerId_ = "DELETE FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId";
        public int DeleteAllInformationAboutUseThePassByWorkerId(int workerId)
        {
            int count = 0;

            try
            {
                command = new SqlCommand();

                command.CommandText = DeleteAllInformationAboutUseThePassByWorkerId_;

                connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                count = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return count;
        }

        private const string DeletemoreSpecificInformationAboutUseThePassByWorkerIdYear_ = "DELETE FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId AND Year = @year";
        public int DeletemoreSpecificInformationAboutUseThePassByWorkerIdYear(int workerId, int year)
        {
            int count = 0;

            try
            {
                command = new SqlCommand();

                command.CommandText = DeletemoreSpecificInformationAboutUseThePassByWorkerIdYear_;

                connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                count = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return count;
        }

        private const string DeletemoreSpecificInformationAboutUseThePassByWorkerIdYearMonth_ = "DELETE FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId AND Year = @year AND Month = @month";
        public int DeletemoreSpecificInformationAboutUseThePassByWorkerIdYearMonth(int workerId, int year, string month)
        {
            int count = 0;

            try
            {
                command = new SqlCommand();

                command.CommandText = DeletemoreSpecificInformationAboutUseThePassByWorkerIdYearMonth_;

                connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlParameter month_ = new SqlParameter();
                month_.ParameterName = "@month";
                month_.SqlDbType = System.Data.SqlDbType.VarChar;
                month_.Value = month;
                command.Parameters.Add(month_);

                count = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return count;
        }

        private const string DeletemoreSpecificInformationAboutUseThePassByWorkerIdYearMonthNumberOfDay_ = "DELETE FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId AND Year = @year AND Month = @month AND NumberOfDay = @numberOfDay";
        public int DeletemoreSpecificInformationAboutUseThePassByWorkerIdYearMonthNumberOfDay(int workerId, int year, string month, int numberOfDay)
        {
            int count = 0;

            try
            {
                command = new SqlCommand();

                command.CommandText = DeletemoreSpecificInformationAboutUseThePassByWorkerIdYearMonthNumberOfDay_;

                connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlParameter month_ = new SqlParameter();
                month_.ParameterName = "@month";
                month_.SqlDbType = System.Data.SqlDbType.VarChar;
                month_.Value = month;
                command.Parameters.Add(month_);

                SqlParameter numberOfDay_ = new SqlParameter();
                numberOfDay_.ParameterName = "@numberOfDay";
                numberOfDay_.SqlDbType = System.Data.SqlDbType.Int;
                numberOfDay_.Value = numberOfDay;
                command.Parameters.Add(numberOfDay_);

                count = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return count;
        }

        private const string TotalNumberOfPassesUsed_ = "SELECT COUNT(*) FROM ( SELECT DISTINCT Year, Month, NumberOfDay, Condition, SinceWhatTime,TillWhatTime, Value, WorkerId, TimeOfUseOfThePass, TheResultOfUsingOfThePass FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker) t";
        public int TotalNumberOfPassesUsed()
        {
            int count = 0;

            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = TotalNumberOfPassesUsed_;

                IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return count;
        }

        private const string TotalNumberOfPassesUsedByWorkerId_ = "SELECT COUNT(WorkerId) FROM( SELECT DISTINCT Year, Month, NumberOfDay, Condition, SinceWhatTime,TillWhatTime, Value, WorkerId, TimeOfUseOfThePass, TheResultOfUsingOfThePass FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId) t";
        public int TotalNumberOfPassesUsedByWorkerId(int workerId)
        {
            int count = 0;

            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = TotalNumberOfPassesUsedByWorkerId_;

                IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return count;
        }

        private const string TotalNumberOfPassesUsedByWorkerIdYear_ = "SELECT COUNT(WorkerId) FROM( SELECT DISTINCT Year, Month, NumberOfDay, Condition, SinceWhatTime,TillWhatTime, Value, WorkerId, TimeOfUseOfThePass, TheResultOfUsingOfThePass FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId AND Year = @year) t";
        public int TotalNumberOfPassesUsedByWorkerIdYear(int workerId, int year)
        {
            int count = 0;

            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = TotalNumberOfPassesUsedByWorkerIdYear_;

                IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return count;
        }

        private const string TotalNumberOfPassesUsedByWorkerIdYearMonth_ = "SELECT COUNT(WorkerId) FROM( SELECT DISTINCT Year, Month, NumberOfDay, Condition, SinceWhatTime,TillWhatTime, Value, WorkerId, TimeOfUseOfThePass, TheResultOfUsingOfThePass FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId AND Year = @year AND Month = @month) t";
        public int TotalNumberOfPassesUsedByWorkerIdYearMonth(int workerId, int year, string month)
        {
            int count = 0;

            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = TotalNumberOfPassesUsedByWorkerIdYearMonth_;

                IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlParameter month_ = new SqlParameter();
                month_.ParameterName = "@month";
                month_.SqlDbType = System.Data.SqlDbType.VarChar;
                month_.Value = month;
                command.Parameters.Add(month_);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return count;
        }

        private const string TotalNumberOfPassesUsedByWorkerIdYearMonthNumberOfDay_ = "SELECT COUNT(WorkerId) FROM( SELECT DISTINCT Year, Month, NumberOfDay, Condition, SinceWhatTime,TillWhatTime, Value, WorkerId, TimeOfUseOfThePass, TheResultOfUsingOfThePass FROM Data_Of_The_Use_Of_A_Pass_By_A_Worker WHERE WorkerId = @workerId AND Year = @year AND Month = @month AND NumberOfDay = @numberOfDay) t";
        public int TotalNumberOfPassesUsedByWorkerIdYearMonthNumberOfDay(int workerId, int year, string month, int numberOfDay)
        {
            int count = 0;

            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = TotalNumberOfPassesUsedByWorkerIdYearMonthNumberOfDay_;

                IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlParameter year_ = new SqlParameter();
                year_.ParameterName = "@year";
                year_.SqlDbType = System.Data.SqlDbType.Int;
                year_.Value = year;
                command.Parameters.Add(year_);

                SqlParameter month_ = new SqlParameter();
                month_.ParameterName = "@month";
                month_.SqlDbType = System.Data.SqlDbType.VarChar;
                month_.Value = month;
                command.Parameters.Add(month_);

                SqlParameter numberOfDay_ = new SqlParameter();
                numberOfDay_.ParameterName = "@numberOfDay";
                numberOfDay_.SqlDbType = System.Data.SqlDbType.Int;
                numberOfDay_.Value = numberOfDay;
                command.Parameters.Add(numberOfDay_);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return count;
        }

        public int AvailabilityOfASpecificWorkerId_Pass(int workerId)
        {
            int id = 0;

            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = Availability_Of_A_Specific_Worker_Id;

                IDbConnection connection = GetConnection();

                command.Connection = (SqlConnection)connection;

                SqlParameter workerId_ = new SqlParameter();
                workerId_.ParameterName = "@workerId";
                workerId_.SqlDbType = System.Data.SqlDbType.Int;
                workerId_.Value = workerId;
                command.Parameters.Add(workerId_);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = (int)reader[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ReleaseConnection(connection);
            }

            return id;
        }
    }
}

