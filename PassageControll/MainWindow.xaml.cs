using ProductionPassControlSystem.BLL;
using System;
using System.Media;
using System.Windows;
using PassageControll.PassageControlService;
using System.Text;
using System.Collections.Generic;
using System.Data;
using ProductionPassControlSystem.Entity;
using System.Windows.Controls;
using System.Windows.Data;

namespace PassageControll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAddColumnsInGrid, IStringMessage, ISettingSoundParameters, ISettingLanguageParameters,
                                              ISetOfParameters, ISetOfParameters2, IAudioMessageAboutNotAllParametersAreFilledIn,
                                              IAudioMessageAboutHereIsResult, ISetOfParameters3, ISetOfParameters4,
                                              IAudioMessageAboutEnterWorkerId, IAudioMessageAboutThereIsNoWorkerWithSuchIdentificator,
                                              IAudioMessageAboutTimeIsNotSet, ISetOfParameters5
    {
        private List<string> listOfValuesFromUnChangedInformation;
        private List<string> listOfValuesFromChangedInformation;
        private List<ControlOfTheUseOfThePass> inf;

        private List<int> IdFromChangedInformation;
        private List<int> IdFromUnChangedInformation;

        private DataTable tableFromChangedInformation;
        private DataTable tableFromUnChangedInformation;
        DataTable valuesOfTime;

        private string year_;
        private string month_;
        private int dayNumber_;


        private List<string> sinceWhatTime_P;
        private List<string> tillWhatTime_P;
        private string sinceWhatTime = "";
        private string tillWhatTime = "";
        private string condition = "";
        private string theResultOfUsingThePass = "";

        private int hours;
        private int minutes;
        private int seconds;

        private int indexForP;
        private int indexForP2 = 0;

        private Random random;

        private StringBuilder builder;

        private string timeOfUse = "";

        private System.Media.SoundPlayer player;

        private bool soundState;

        private string langaugeState;
        public MainWindow()
        {
            InitializeComponent();

            PassageControlClient client = new PassageControlClient("NetTcpBinding_IPassageControl");

            sinceWhatTime_P = new List<string>();

            tillWhatTime_P = new List<string>();

            IdFromChangedInformation = new List<int>();

            IdFromUnChangedInformation = new List<int>();

            listOfValuesFromUnChangedInformation = new List<string>();

            listOfValuesFromChangedInformation = new List<string>();

            inf = new List<ControlOfTheUseOfThePass>();

            random = new Random();

            builder = new StringBuilder(8);

        }

        private void TimeCheck(string sinceWhatTime, string tillWhatTime, string value)
        {
            int sinceTime = 0;
            int tillTime = 0;

            try
            {
                if (value == "12")
                {
                    sinceWhatTime = CroppingAString(sinceWhatTime);
                    tillWhatTime = CroppingAString(tillWhatTime);

                    sinceTime = ConvertStringToInt(sinceWhatTime);
                    tillTime = ConvertStringToInt(tillWhatTime);

                    if (hours < sinceTime || hours > tillTime)
                    {
                        condition = "False";
                        theResultOfUsingThePass = "-";
                    }
                    else if (hours >= sinceTime && hours < tillTime && minutes > 0)
                    {
                        condition = "True";
                        theResultOfUsingThePass = "+";
                    }
                    else if (hours == tillTime && minutes == 0)
                    {
                        condition = "True";
                        theResultOfUsingThePass = "+";
                    }
                    else if (hours == tillTime && minutes > 0)
                    {
                        condition = "False";
                        theResultOfUsingThePass = "-";
                    }
                }

                if (value == "4")
                {
                    sinceWhatTime = CroppingAString(sinceWhatTime);
                    tillWhatTime = CroppingAString(tillWhatTime);

                    sinceTime = ConvertStringToInt(sinceWhatTime);
                    tillTime = ConvertStringToInt(tillWhatTime);

                    if (hours < sinceTime && hours > tillTime)
                    {
                        condition = "False";
                        theResultOfUsingThePass = "-";
                    }
                    else if (hours >= sinceTime && hours <= 23 && minutes > 0)
                    {
                        condition = "True";
                        theResultOfUsingThePass = "+";
                    }
                    else if (hours < tillTime)
                    {
                        condition = "True";
                        theResultOfUsingThePass = "+";
                    }
                    else if (hours == tillTime && minutes == 0)
                    {
                        condition = "True";
                        theResultOfUsingThePass = "+";
                    }
                    else if (hours == tillTime && minutes > 0)
                    {
                        condition = "False";
                        theResultOfUsingThePass = "-";
                    }

                }
                if (value == "8" || value == "D" || value == "V" || value == "S")
                {
                    condition = "False";
                    theResultOfUsingThePass = "-";
                }
                if (value == "P")
                {
                    sinceWhatTime = CroppingAString(sinceWhatTime);
                    tillWhatTime = CroppingAString(tillWhatTime);

                    sinceTime = ConvertStringToInt(sinceWhatTime);
                    tillTime = ConvertStringToInt(tillWhatTime);

                    if (sinceTime >= 8 && tillTime <= 20)
                    {
                        if (hours < sinceTime || hours > tillTime)
                        {
                            condition = "False";
                            theResultOfUsingThePass = "-";
                        }
                        else if (hours >= sinceTime && hours < tillTime && minutes > 0)
                        {
                            condition = "True";
                            theResultOfUsingThePass = "+";
                        }
                        else if (hours == tillTime && minutes == 0)
                        {
                            condition = "True";
                            theResultOfUsingThePass = "+";
                        }
                        else if (hours == tillTime && minutes > 0)
                        {
                            condition = "False";
                            theResultOfUsingThePass = "-";
                        }
                    }

                    if (sinceTime >= 20 && tillTime <= 8)
                    {
                        if (hours < sinceTime && hours > tillTime)
                        {
                            condition = "False";
                            theResultOfUsingThePass = "-";
                        }
                        else if (hours >= sinceTime && hours <= 23 && minutes > 0)
                        {
                            condition = "True";
                            theResultOfUsingThePass = "+";
                        }
                        else if (hours < tillTime)
                        {
                            condition = "True";
                            theResultOfUsingThePass = "+";
                        }
                        else if (hours == tillTime && minutes == 0)
                        {
                            condition = "True";
                            theResultOfUsingThePass = "+";
                        }
                        else if (hours == tillTime && minutes > 0)
                        {
                            condition = "False";
                            theResultOfUsingThePass = "-";
                        }
                    }

                    if (sinceTime > 0 && sinceTime < 8 && tillTime <= 8)
                    {
                        if (hours >= sinceTime && sinceTime < 8 && hours < 8 && minutes == 0)
                        {
                            condition = "True";
                            theResultOfUsingThePass = "+";
                        }
                        else if (hours > 8)
                        {
                            condition = "False";
                            theResultOfUsingThePass = "-";
                        }
                        else if (hours == tillTime && minutes == 0)
                        {
                            condition = "True";
                            theResultOfUsingThePass = "+";
                        }
                        else if (hours == tillTime && minutes > 0)
                        {
                            condition = "False";
                            theResultOfUsingThePass = "-";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AssigningValues(string value)
        {
            try
            {
                switch (value)
                {
                    case "12":
                        sinceWhatTime = "8:00:00";
                        tillWhatTime = "20:00:00";
                        TimeCheck(sinceWhatTime, tillWhatTime, value);
                        break;
                    case "4":
                        sinceWhatTime = "20:00:00";
                        tillWhatTime = "8:00:00";
                        TimeCheck(sinceWhatTime, tillWhatTime, value);
                        break;
                    case "8":
                        sinceWhatTime = "-";
                        tillWhatTime = "-";
                        TimeCheck(sinceWhatTime, tillWhatTime, value);
                        break;
                    case "D":
                        sinceWhatTime = "-";
                        tillWhatTime = "-";
                        TimeCheck(sinceWhatTime, tillWhatTime, value);
                        break;
                    case "V":
                        sinceWhatTime = "-";
                        tillWhatTime = "-";
                        TimeCheck(sinceWhatTime, tillWhatTime, value);
                        break;
                    case "S":
                        sinceWhatTime = "-";
                        tillWhatTime = "-";
                        TimeCheck(sinceWhatTime, tillWhatTime, value);
                        break;
                    case "P":
                        if (indexForP2 <= indexForP)
                        {
                            sinceWhatTime = sinceWhatTime_P[indexForP2];
                            tillWhatTime = tillWhatTime_P[indexForP2];
                            indexForP2++;
                        }
                        TimeCheck(sinceWhatTime, tillWhatTime, value);
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

        private void RemoveUnnecessaryWorkerId()
        {
            try
            {
                if (IdFromChangedInformation.Count != 0)
                {
                    for (int i = 0; i < IdFromChangedInformation.Count; i++)
                    {
                        for (int j = 0; j < IdFromUnChangedInformation.Count; j++)
                        {
                            if (IdFromUnChangedInformation[j] == IdFromChangedInformation[i])
                            {
                                IdFromUnChangedInformation.Remove(IdFromChangedInformation[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillingArrayWithWorkesIdFromUnChangedInformation()
        {
            try
            {
                PassageControlClient client = new PassageControlClient("NetTcpBinding_IPassageControl");

                tableFromUnChangedInformation = client.GetWorkerIdAndNumberOfShift();

                client.Close();

                foreach (DataRow row in tableFromUnChangedInformation.Rows)
                {
                    IdFromUnChangedInformation.Add((int)row[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void FillingInListFromUnchangedInformation()
        {
            try
            {
                passageControlGrid.Columns.Clear();
                IdFromUnChangedInformation.Clear();

                string[] arrayOfValues = { "12", "4", "8", "D" };
                int[] arrayOfShifNumbers = { 1, 2, 3, 4 };

                timeOfUse = builder.ToString();

                FillingArrayWithWorkesIdFromUnChangedInformation();

                RemoveUnnecessaryWorkerId();

                int count = 0;

                foreach (DataRow row in tableFromUnChangedInformation.Rows)
                {
                    for (int i = 0; i < IdFromUnChangedInformation.Count; i++)
                    {
                        if (IdFromUnChangedInformation[i] == (int)row[0])
                        {
                            if (count < IdFromUnChangedInformation.Count)
                            {
                                for (int j = 0; j < arrayOfShifNumbers.Length; j++)
                                {
                                    if ((int)row[1] == arrayOfShifNumbers[j])
                                    {
                                        AssigningValues(listOfValuesFromUnChangedInformation[j]);

                                        inf.Add(new ControlOfTheUseOfThePass(year_, month_, (int)row[0], timeOfUse, theResultOfUsingThePass, dayNumber_, condition, sinceWhatTime,
                                                                             tillWhatTime, listOfValuesFromUnChangedInformation[j])
                                        {
                                            Year = year_,
                                            Month = month_,
                                            WorkerId = IdFromUnChangedInformation[count],
                                            TimeOfUseOfThePass = timeOfUse,
                                            TheResultOfUsingThePass = theResultOfUsingThePass
                                        });
                                    }
                                }
                            }
                            count++;
                        }
                    }
                }

                AddColumnsInGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddColumnsInGrid()
        {
            try
            {
                passageControlGrid.ItemsSource = null;

                passageControlGrid.AutoGenerateColumns = false;

                passageControlGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Year",
                    Binding = new Binding("Year")
                });

                passageControlGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Month",
                    Binding = new Binding("Month")
                });

                passageControlGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Number Of day",
                    Binding = new Binding("NumberOfDay")
                });

                passageControlGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Condition",
                    Binding = new Binding("Condition")
                });

                passageControlGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Since what time",
                    Binding = new Binding("SinceWhatTime")
                });

                passageControlGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Till what time",
                    Binding = new Binding("TillWhatTime")
                });

                passageControlGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Value",
                    Binding = new Binding("Value")
                });

                passageControlGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Worker id",
                    Binding = new Binding("WorkerId")
                });

                passageControlGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Time of use of the pass",
                    Binding = new Binding("TimeOfUseOfThePass")
                });

                passageControlGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "The result of using the pass",
                    Binding = new Binding("TheResultOfUsingThePass")
                });

                passageControlGrid.ItemsSource = inf;

                Save_Result();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Save_Result()
        {
            ControlOfTheUseOfThePass controlOfTheUseOfThePass = new ControlOfTheUseOfThePass();

            PassageControlClient client = new PassageControlClient("NetTcpBinding_IPassageControl");

            ControlOfTheUseOfThePass[] controls = new ControlOfTheUseOfThePass[inf.Count];

            for (int i = 0; i < inf.Count; i++)
            {
                controls[i] = inf[i];
            }

            client.Add_Data_Of_The_Use_Of_A_Pass_By_A_Worker(controlOfTheUseOfThePass, controls, time.Text);

            client.Close();
        }

        private int DeterminingTheMonthNumber(string month)
        {
            int numberOfMonth = 0;

            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return numberOfMonth;
        }

        private void ConverInputValue()
        {
            year_ = Year.Text.ToString();
            month_ = nameOfMonth.Text.ToString();
            dayNumber_ = Convert.ToInt32(dayNumber.Text);
        }

        private void GetValueOfShift(PassageControlClient client)
        {
            try
            {
                string day = "12";
                string night = "4";
                string dayOff = "8";
                string endDayOff = "D";

                int[] days =  client.NumberOfDayShift_PS();

                for (int i = 0; i < days.Length; i++)
                {
                    if (days[i].ToString() == dayNumber.Text)
                    {
                        listOfValuesFromUnChangedInformation.Add(day);
                    }
                }

                days = null;

                int[] nights = client.NumberOfNightShift_PS();

                for (int i = 0; i < nights.Length; i++)
                {
                    if (nights[i].ToString() == dayNumber.Text)
                    {
                        listOfValuesFromUnChangedInformation.Add(night);
                    }
                }

                nights = null;

                int[] daysOff = client.NumberOfDayOff_PS();

                for (int i = 0; i < daysOff.Length; i++)
                {
                    if (daysOff[i].ToString() == dayNumber.Text)
                    {
                        listOfValuesFromUnChangedInformation.Add(dayOff);
                    }
                }

                daysOff = null;

                int[] endDaysOff = client.NumberOfEndDayOff_PS();

                for (int i = 0; i < endDaysOff.Length; i++)
                {
                    if (endDaysOff[i].ToString() == dayNumber.Text)
                    {
                        listOfValuesFromUnChangedInformation.Add(endDayOff);
                    }
                }

                endDaysOff = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetTheValuesOfShifts()
        {
            try
            {
                if (listOfValuesFromUnChangedInformation != null)
                listOfValuesFromUnChangedInformation.Clear();

                ConverInputValue();

                ScheduleOfShift scheduleOf = new ScheduleOfShift();

                PassageControlClient client = new PassageControlClient("NetTcpBinding_IPassageControl");

                client.SelectingYear_PS(year_, true, "");

                client.SelectingMonth_PS(year_, month_, true, "", "");

                int numOfMonth = DeterminingTheMonthNumber(month_);

                int numberOfDaysOfMonth = DateTime.DaysInMonth(Convert.ToInt32(year_), numOfMonth);

                client.GetInformationAboutFirstShift_PS(true, numberOfDaysOfMonth, "", "");

                GetValueOfShift(client);

                client.ClearData_PS();

                client.GetInformationAboutSecondShift_PS(true, numberOfDaysOfMonth, "", "");

                GetValueOfShift(client);

                client.ClearData_PS();

                client.GetInformationAboutThirdShift_PS(true, numberOfDaysOfMonth, "", "");

                GetValueOfShift(client);

                client.ClearData_PS();

                client.GetInformationAboutFourthShift_PS(true, numberOfDaysOfMonth, "", "");

                GetValueOfShift(client);

                client.ClearData_PS();

                client.Close();

                FillingInListFromUnchangedInformation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string CroppingAString(string sinceWhatTime)
        {
            return sinceWhatTime.Substring(0, sinceWhatTime.Length - 6);
        }

        private int ConvertStringToInt(string value)
        {
            return Convert.ToInt32(value);
        }

        private StringBuilder RandomTime()
        {
            try
            {
                hours = 0;
                minutes = 0;
                seconds = 0;

                builder.Clear();

                hours = random.Next(1, 24);
                minutes = random.Next(1, 60);
                seconds = random.Next(1, 60);

                if (hours < 10)
                    builder.Append("0");

                builder.Append(hours);
                builder.Append(":");

                if (minutes < 10)
                    builder.Append("0");

                builder.Append(minutes);
                builder.Append(":");

                if (seconds < 10)
                    builder.Append("0");

                builder.Append(seconds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return builder;
        }

        private void Generation_Time_Click(object sender, RoutedEventArgs e)
        {
            time.Text = "";
            StringBuilder stringBuilder = RandomTime();
            time.Text = stringBuilder.ToString();
        }

        private void FillingArrayWithWorkesIdFromChangedInformation()
        {
            try
            {
                foreach (DataRow row in tableFromChangedInformation.Rows)
                {
                    IdFromChangedInformation.Add((int)row[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckingForThePresenceOfP(List<string> values)
        {
            try
            {
                PassageControlClient client = new PassageControlClient("NetTcpBinding_IPassageControl");

                for (int i = 0; i < listOfValuesFromChangedInformation.Count; i++)
                {
                    if (listOfValuesFromChangedInformation[i] == "P")
                    {
                        valuesOfTime = client.GetValuesOfTime(dayNumber_, Convert.ToInt32(year_), month_, IdFromChangedInformation[i]);

                        AssigningValuesForP();
                    }
                }

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AssigningValuesForP()
        {
            try
            {
                foreach (DataRow row in valuesOfTime.Rows)
                {
                    sinceWhatTime_P.Add(row[0].ToString());
                    tillWhatTime_P.Add(row[1].ToString());
                    indexForP++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<string> dayShift_ = new List<string>();
        private List<string> nightShift_ = new List<string>();
        private List<string> dayOff_ = new List<string>();
        private List<string> endDayOff_ = new List<string>();
        private List<string> vacation_ = new List<string>();
        private List<string> sickLeave_ = new List<string>();
        private List<string> prodactionTasks_ = new List<string>();

        private void ClearValues()
        {
            dayShift_.Clear();
            nightShift_.Clear();
            dayOff_.Clear();
            endDayOff_.Clear();
            vacation_.Clear();
            sickLeave_.Clear();
            prodactionTasks_.Clear();
        }

        DataTable table1 = new DataTable();
        private void GetValuesFromChangedInformation(DataTable table, int dayNumber)//доделать, juliy 1 еще раз посмотреть, ошибка выходит
        {
            try
            {
                foreach (DataRow t in table.Rows)
                {
                    var values = t.ItemArray;

                    if (values[4].ToString() == "12" && (int)values[0] == dayNumber)
                    {
                        dayShift_.Add((string)values[4]);
                    }
                    else if (values[4].ToString() == "4" && (int)values[0] == dayNumber)
                    {
                        nightShift_.Add((string)values[4]);
                    }
                    else if (values[4].ToString() == "8" && (int)values[0] == dayNumber)
                    {
                        dayOff_.Add((string)values[4]);
                    }
                    else if (values[4].ToString() == "D" && (int)values[0] == dayNumber)
                    {
                        endDayOff_.Add((string)values[4]);
                    }
                    else if (values[4].ToString() == "V" && (int)values[0] == dayNumber)
                    {
                        vacation_.Add((string)values[4]);
                    }
                    else if (values[4].ToString() == "S" && (int)values[0] == dayNumber)
                    {
                        sickLeave_.Add((string)values[4]);
                    }
                    else if (values[4].ToString() == "P" && (int)values[0] == dayNumber)
                    {
                        prodactionTasks_.Add((string)values[4]);
                    }
                }

                if (dayShift_ != null)
                {
                    foreach (var c in dayShift_)
                        listOfValuesFromChangedInformation.Add(c.ToString());
                }
                if (nightShift_ != null)
                {
                    foreach (var c in nightShift_)
                        listOfValuesFromChangedInformation.Add(c.ToString());
                }
                if (dayOff_ != null)
                {
                    foreach (var c in dayOff_)
                        listOfValuesFromChangedInformation.Add(c.ToString());
                }
                if (endDayOff_ != null)
                {
                    foreach (var c in endDayOff_)
                        listOfValuesFromChangedInformation.Add(c.ToString());
                }
                if (vacation_ != null)
                {
                    foreach (var c in vacation_)
                        listOfValuesFromChangedInformation.Add(c.ToString());
                }
                if (sickLeave_ != null)
                {
                    foreach (var c in sickLeave_)
                        listOfValuesFromChangedInformation.Add(c.ToString());
                }
                if (prodactionTasks_ != null)
                {
                    foreach (var c in prodactionTasks_)
                        listOfValuesFromChangedInformation.Add(c.ToString());
                }

                ClearValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetTheValuesOfShiftsFromChangedInformation()
        {
            try
            {
                IdFromChangedInformation.Clear();
                listOfValuesFromChangedInformation.Clear();

                FillingArrayWithWorkesIdFromChangedInformation();

                PassageControlClient client = new PassageControlClient("NetTcpBinding_IPassageControl");

                for (int i = 0; i < IdFromChangedInformation.Count; i++)
                {
                    table1 = client.GetChangedInformation_PS(Convert.ToInt32(year_), month_, IdFromChangedInformation[i]);

                    GetValuesFromChangedInformation(table1, dayNumber_);
                }

                CheckingForThePresenceOfP(listOfValuesFromChangedInformation);

                FillingInListFromChangedInformation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillingInListFromChangedInformation()
        {
            try
            {
                timeOfUse = builder.ToString();

                int count = 0;

                foreach (DataRow row in tableFromChangedInformation.Rows)
                {
                    if (count < listOfValuesFromChangedInformation.Count)
                    {
                        AssigningValues(listOfValuesFromChangedInformation[count]);

                        inf.Add(new ControlOfTheUseOfThePass(year_, month_, (int)row[0], timeOfUse, theResultOfUsingThePass, dayNumber_, condition, sinceWhatTime,
                                                                                     tillWhatTime, listOfValuesFromChangedInformation[count].ToString())
                        {
                            Year = year_,
                            Month = month_,
                            WorkerId = IdFromChangedInformation[count],
                            TimeOfUseOfThePass = timeOfUse,
                            TheResultOfUsingThePass = theResultOfUsingThePass
                        });
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetOfParameters()
        {
            try
            {
                SettingSoundParameters();
                SettingLanguageParameters();

                SoundMesaageAboutNotAllParametersAreFilledIn(player, soundState, langaugeState);

                if (langaugeState == "eng" && soundState == false)
                {
                    StringMessageInEnglish("Not all parameters are filled in!");
                }
                else if (langaugeState == "ru" && soundState == false)
                {
                    StringMessageInRussian("Не все параметры выставлены!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetOfParameters2()
        {
            try
            {
                SettingSoundParameters();
                SettingLanguageParameters();

                SoundMessageAboutHereIsResult(player, soundState, langaugeState);

                if (langaugeState == "eng" && soundState == false)
                {
                    StringMessageInEnglish("Here is the result!");
                }
                else if (langaugeState == "ru" && soundState == false)
                {
                    StringMessageInRussian("Вот результат!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetOfParameters3()
        {
            try
            {
                SettingSoundParameters();
                SettingLanguageParameters();

                SoundMessageAboutThereIsNoWorkerWithSuchIdentificator(player, soundState, langaugeState);

                if (langaugeState == "eng" && soundState == false)
                {
                    StringMessageInEnglish("There is no worker with such identificator!");
                }
                else if (langaugeState == "ru" && soundState == false)
                {
                    StringMessageInRussian("Нет работника с таким идентификатором!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetOfParameters4()
        {
            try
            {
                SettingSoundParameters();
                SettingLanguageParameters();

                SoundMessageAboutEnterWorkerId(player, soundState, langaugeState);

                if (langaugeState == "eng" && soundState == false)
                {
                    StringMessageInEnglish("Enter the worker ID!");
                }
                else if (langaugeState == "ru" && soundState == false)
                {
                    StringMessageInRussian("Введите идентификатор работника!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetOfParameters5()
        {
            try
            {
                SettingSoundParameters();
                SettingLanguageParameters();

                SoundMessageAboutTimeIsNotSet(player, soundState, langaugeState);

                if (langaugeState == "eng" && soundState == false)
                {
                    StringMessageInEnglish("The time isn't set!");
                }
                else if (langaugeState == "ru" && soundState == false)
                {
                    StringMessageInRussian("Время не установлено!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SettingLanguageParameters()
        {
            try
            {
                if (language.Content == FindResource("eng"))
                {
                    langaugeState = "eng";
                }
                else
                    langaugeState = "ru";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SettingSoundParameters()
        {
            try
            {
                if (sound.Content == FindResource("soundOn"))
                {
                    soundState = true;
                }
                else
                    soundState = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SoundMesaageAboutNotAllParametersAreFilledIn(SoundPlayer player, bool soundState, string languageState)
        {
            if (soundState == true)
            {
                try
                {
                    if (langaugeState == "eng")
                    {
                        player = new SoundPlayer(Properties.Resources.not_all_parameters_are_filled_in);
                        player.Play();
                        player.Dispose();
                    }
                    else
                    {
                        player = new SoundPlayer(Properties.Resources.не_все_параметры_выставлены);
                        player.Play();
                        player.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void SoundMessageAboutEnterWorkerId(SoundPlayer player, bool soundState, string languageState)
        {
            if (soundState == true)
            {
                try
                {
                    if (langaugeState == "eng")
                    {
                        player = new SoundPlayer(Properties.Resources.enter_the_worker_id);
                        player.Play();
                        player.Dispose();
                    }
                    else
                    {
                        player = new SoundPlayer(Properties.Resources.введите_идентификаиор_работника);
                        player.Play();
                        player.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void SoundMessageAboutHereIsResult(SoundPlayer player, bool soundState, string languageState)
        {
            if (soundState == true)
            {
                try
                {
                    if (langaugeState == "eng")
                    {
                        player = new SoundPlayer(Properties.Resources.here_is_the_result);
                        player.Play();
                        player.Dispose();
                    }
                    else
                    {
                        player = new SoundPlayer(Properties.Resources.вот_результат);
                        player.Play();
                        player.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void SoundMessageAboutThereIsNoWorkerWithSuchIdentificator(SoundPlayer player, bool soundState, string languageState)
        {
            if (soundState == true)
            {
                try
                {
                    if (langaugeState == "eng")
                    {
                        player = new SoundPlayer(Properties.Resources.there_is_no_worker);
                        player.Play();
                        player.Dispose();
                    }
                    else
                    {
                        player = new SoundPlayer(Properties.Resources.нет_работника);
                        player.Play();
                        player.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void SoundMessageAboutTimeIsNotSet(SoundPlayer player, bool soundState, string languageState)
        {
            if (soundState == true)
            {
                try
                {
                    if (langaugeState == "eng")
                    {
                        player = new SoundPlayer(Properties.Resources.the_time_is_not_set);
                        player.Play();
                        player.Dispose();
                    }
                    else
                    {
                        player = new SoundPlayer(Properties.Resources.время_не_установлено);
                        player.Play();
                        player.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void StringMessageInEnglish(string message)
        {
            try
            {
                MessageBox.Show(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void StringMessageInRussian(string message)
        {
            try
            {
                MessageBox.Show(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void See_Result_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (time.Text == "Time" || time.Text == "")
                {
                    SetOfParameters5();
                }
                else
                {
                    ConverInputValue();

                    inf.Clear();

                    sinceWhatTime_P.Clear();
                    tillWhatTime_P.Clear();

                    indexForP = 0;
                    indexForP2 = 0;

                    PassageControlClient client = new PassageControlClient("NetTcpBinding_IPassageControl");

                    tableFromChangedInformation = client.GetWorkerIdFromChangedInformation(Convert.ToInt32(year_), month_);

                    client.Close();

                    if (tableFromChangedInformation.Rows.Count != 0)
                    {
                        GetTheValuesOfShiftsFromChangedInformation();
                    }

                    GetTheValuesOfShifts();

                    SetOfParameters2();
                }
            }
            catch
            {
                SetOfParameters();
            }
        }

        private void Turning_The_Sound_On_And_Off_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sound.Content == FindResource("soundOn"))
                {
                    sound.Content = FindResource("soundOf");
                    sound.ToolTip = "Sound of";
                }
                else
                {
                    sound.Content = FindResource("soundOn");
                    sound.ToolTip = "Sound on";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Turning_The_Language_RU_ENG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (language.Content == FindResource("eng"))
                {
                    language.Content = FindResource("ru");
                    language.ToolTip = "Russian for sound";
                }
                else
                {
                    language.Content = FindResource("eng");
                    language.ToolTip = "English for sound";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void See_Details_Click(object sender, RoutedEventArgs e)//доделать
        {
            try
            {
                PassageControlClient client = new PassageControlClient("NetTcpBinding_IPassageControl");

                int count = client.AvailabilityOfASpecificWorkerId_PS(Convert.ToInt32(workerId.Text));

                client.Close();

                if (count > 0)
                {
                    SetOfParameters2();

                    int id = Convert.ToInt32(workerId.Text);

                    InfoAboutWorker infoAboutWorker = new InfoAboutWorker(id);

                    infoAboutWorker.ShowDialog();
                }
                else
                {
                    SetOfParameters3();
                }
            }
            catch
            {
                SetOfParameters4();
            }
        }
    }
}
