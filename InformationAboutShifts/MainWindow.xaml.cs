using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InformationAboutShifts.InformationAboutShiftService;
using ProductionPassControlSystem.BLL;

namespace InformationAboutShifts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IChoosingDayFirstShift, IChoosingDaySecondShift, IChoosingDayThirdShift,
                                              IChoosingDayFourthShift, IChoosingTheNumberOfDay, IAudioMessageAboutNotAllParametersAreFilledIn,
                                              ISettingSoundParameters,ISettingLanguageParameters, IStringMessage, ISetOfParameters,
                                              ISetOfParameters2,IAudioMessageAboutHereIsResult
    {
        private bool soundState;
        private string langaugeState;

        private SoundPlayer player;

        private List<int> numberOfDayShift;
        private List<int> numberOfNightShift;
        private List<int> numberOfDayOff;
        private List<int> numberOfEndDayOff;
        private List<int> numberOfDay;

        public MainWindow()
        {
            InitializeComponent();

            numberOfDayShift = new List<int>();
            numberOfNightShift = new List<int>();
            numberOfDayOff = new List<int>();
            numberOfEndDayOff = new List<int>();
            numberOfDay = new List<int>();
        }

        private void AddNumbersFromIntToList(int[] number, List<int> listOfNumbers)
        {
            for (int i = 0; i < number.Length; i++)
            {
                listOfNumbers.Add(number[i]);
            }
        }

        private void See_Result_Click(object sender, RoutedEventArgs e)//проверить чтобы не было лишних методов, лишних и не нужных DataContract, удалить лишние записи и код закоментированный
        {
            try
            {
                ClearTheDaysOfFirst();
                ClearTheDaysOfSecond();
                ClearTheDaysOfThird();
                ClearTheDaysOfFourth();

                InformationAboutShiftsClient client = new InformationAboutShiftsClient("NetTcpBinding_IInformationAboutShifts");

                client.SelectingYear("", false, year.Text);
                client.SelectingMonth("", "", false, year.Text, nameOfMonth.Text);
                client.SelectingSaturdayOrSunday(year.Text, nameOfMonth.Text);

                ClearNumberOfDay();
                int[] num = client.NumberOfDay();
                AddNumbersFromIntToList(num, numberOfDay);
                ChoosingTheNumberOfDay(numberOfDay);
                numberOfDay.Clear();
                client.ClearData2();
                num = null;

                client.GetInformationAboutFirstShift(false, 0, year.Text, nameOfMonth.Text);

                num = client.NumberOfDayShift();
                AddNumbersFromIntToList(num, numberOfDayShift);
                ChoosingTheDayOffFirst(numberOfDayShift);
                numberOfDayShift.Clear();
                num = null;

                num = client.NumberOfNightShift();
                AddNumbersFromIntToList(num, numberOfNightShift);
                ChoosingTheNightFirst(numberOfNightShift);
                numberOfNightShift.Clear();
                num = null;

                num = client.NumberOfDayOff();
                AddNumbersFromIntToList(num, numberOfDayOff);
                ChoosingTheDayOffFirst(numberOfDayOff);
                numberOfDayOff.Clear();
                num = null;

                num = client.NumberOfEndDayOff();
                AddNumbersFromIntToList(num, numberOfEndDayOff);
                ChoosingTheEndDayOffFirst(numberOfEndDayOff);
                numberOfEndDayOff.Clear();
                num = null;

                client.ClearData();

                client.GetInformationAboutSecondShift(false, 0, year.Text, nameOfMonth.Text);

                num = client.NumberOfDayShift();
                AddNumbersFromIntToList(num, numberOfDayShift);
                ChoosingTheDaySecond(numberOfDayShift);
                numberOfDayShift.Clear();
                num = null;

                num = client.NumberOfNightShift();
                AddNumbersFromIntToList(num, numberOfNightShift);
                ChoosingTheNightSecond(numberOfNightShift);
                numberOfNightShift.Clear();
                num = null;

                num = client.NumberOfDayOff();
                AddNumbersFromIntToList(num, numberOfDayOff);
                ChoosingTheDayOffSecond(numberOfDayOff);
                numberOfDayOff.Clear();
                num = null;

                num = client.NumberOfEndDayOff();
                AddNumbersFromIntToList(num, numberOfEndDayOff);
                ChoosingTheEndDayOffSecond(numberOfEndDayOff);
                numberOfEndDayOff.Clear();
                num = null;

                client.ClearData();

                client.GetInformationAboutThirdShift(false, 0, year.Text, nameOfMonth.Text);

                num = client.NumberOfDayShift();
                AddNumbersFromIntToList(num, numberOfDayShift);
                ChoosingTheDayThird(numberOfDayShift);
                numberOfDayShift.Clear();
                num = null;

                num = client.NumberOfNightShift();
                AddNumbersFromIntToList(num, numberOfNightShift);
                ChoosingTheNightThird(numberOfNightShift);
                numberOfNightShift.Clear();
                num = null;

                num = client.NumberOfDayOff();
                AddNumbersFromIntToList(num, numberOfDayOff);
                ChoosingTheDayOffThird(numberOfDayOff);
                numberOfDayOff.Clear();
                num = null;

                num = client.NumberOfEndDayOff();
                AddNumbersFromIntToList(num, numberOfEndDayOff);
                ChoosingTheEndDayOffThird(numberOfEndDayOff);
                numberOfEndDayOff.Clear();
                num = null;

                client.ClearData();

                client.GetInformationAboutFourthShift(false, 0, year.Text, nameOfMonth.Text);

                num = client.NumberOfDayShift();
                AddNumbersFromIntToList(num, numberOfDayShift);
                ChoosingTheDayFourth(numberOfDayShift);
                numberOfDayShift.Clear();
                num = null;

                num = client.NumberOfNightShift();
                AddNumbersFromIntToList(num, numberOfNightShift);
                ChoosingTheNightFourth(numberOfNightShift);
                numberOfNightShift.Clear();
                num = null;

                num = client.NumberOfDayOff();
                AddNumbersFromIntToList(num, numberOfDayOff);
                ChoosingTheDayOffFourth(numberOfDayOff);
                numberOfDayOff.Clear();
                num = null;

                num = client.NumberOfEndDayOff();
                AddNumbersFromIntToList(num, numberOfEndDayOff);
                ChoosingTheEndDayOffFourth(numberOfEndDayOff);
                numberOfEndDayOff.Clear();
                num = null;

                client.ClearData();

                client.Close();

                SetOfParameters();
            }
            catch
            {
                SetOfParameters2();
                ClearNumberOfDay();
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

        public void ChoosingTheDayFirst(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_F.Content = 12;
                            day_1_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 2:
                            day_2_F.Content = 12;
                            day_2_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 3:
                            day_3_F.Content = 12;
                            day_3_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 4:
                            day_4_F.Content = 12;
                            day_4_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 5:
                            day_5_F.Content = 12;
                            day_5_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 6:
                            day_6_F.Content = 12;
                            day_6_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 7:
                            day_7_F.Content = 12;
                            day_7_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 8:
                            day_8_F.Content = 12;
                            day_8_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 9:
                            day_9_F.Content = 12;
                            day_9_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 10:
                            day_10_F.Content = 12;
                            day_10_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 11:
                            day_11_F.Content = 12;
                            day_11_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 12:
                            day_12_F.Content = 12;
                            day_12_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 13:
                            day_13_F.Content = 12;
                            day_13_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 14:
                            day_14_F.Content = 12;
                            day_14_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 15:
                            day_15_F.Content = 12;
                            day_15_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 16:
                            day_16_F.Content = 12;
                            day_16_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 17:
                            day_17_F.Content = 12;
                            day_17_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 18:
                            day_18_F.Content = 12;
                            day_18_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 19:
                            day_19_F.Content = 12;
                            day_19_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 20:
                            day_20_F.Content = 12;
                            day_20_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 21:
                            day_21_F.Content = 12;
                            day_21_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 22:
                            day_22_F.Content = 12;
                            day_22_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 23:
                            day_23_F.Content = 12;
                            day_23_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 24:
                            day_24_F.Content = 12;
                            day_24_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 25:
                            day_25_F.Content = 12;
                            day_25_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 26:
                            day_26_F.Content = 12;
                            day_26_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 27:
                            day_27_F.Content = 12;
                            day_27_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 28:
                            day_28_F.Content = 12;
                            day_28_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 29:
                            day_29_F.Content = 12;
                            day_29_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 30:
                            day_30_F.Content = 12;
                            day_30_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 31:
                            day_31_F.Content = 12;
                            day_31_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheNightFirst(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_F.Content = 4;
                            day_1_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 2:
                            day_2_F.Content = 4;
                            day_2_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 3:
                            day_3_F.Content = 4;
                            day_3_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 4:
                            day_4_F.Content = 4;
                            day_4_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 5:
                            day_5_F.Content = 4;
                            day_5_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 6:
                            day_6_F.Content = 4;
                            day_6_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 7:
                            day_7_F.Content = 4;
                            day_7_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 8:
                            day_8_F.Content = 4;
                            day_8_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 9:
                            day_9_F.Content = 4;
                            day_9_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 10:
                            day_10_F.Content = 4;
                            day_10_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 11:
                            day_11_F.Content = 4;
                            day_11_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 12:
                            day_12_F.Content = 4;
                            day_12_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 13:
                            day_13_F.Content = 4;
                            day_13_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 14:
                            day_14_F.Content = 4;
                            day_14_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 15:
                            day_15_F.Content = 4;
                            day_15_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 16:
                            day_16_F.Content = 4;
                            day_16_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 17:
                            day_17_F.Content = 4;
                            day_17_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 18:
                            day_18_F.Content = 4;
                            day_18_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 19:
                            day_19_F.Content = 4;
                            day_19_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 20:
                            day_20_F.Content = 4;
                            day_20_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 21:
                            day_21_F.Content = 4;
                            day_21_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 22:
                            day_22_F.Content = 4;
                            day_22_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 23:
                            day_23_F.Content = 4;
                            day_23_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 24:
                            day_24_F.Content = 4;
                            day_24_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 25:
                            day_25_F.Content = 4;
                            day_25_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 26:
                            day_26_F.Content = 4;
                            day_26_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 27:
                            day_27_F.Content = 4;
                            day_27_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 28:
                            day_28_F.Content = 4;
                            day_28_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 29:
                            day_29_F.Content = 4;
                            day_29_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 30:
                            day_30_F.Content = 4;
                            day_30_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 31:
                            day_31_F.Content = 4;
                            day_31_F.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheDayOffFirst(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_F.Content = 8;
                            day_1_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 2:
                            day_2_F.Content = 8;
                            day_2_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 3:
                            day_3_F.Content = 8;
                            day_3_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 4:
                            day_4_F.Content = 8;
                            day_4_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 5:
                            day_5_F.Content = 8;
                            day_5_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 6:
                            day_6_F.Content = 8;
                            day_6_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 7:
                            day_7_F.Content = 8;
                            day_7_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 8:
                            day_8_F.Content = 8;
                            day_8_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 9:
                            day_9_F.Content = 8;
                            day_9_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 10:
                            day_10_F.Content = 8;
                            day_10_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 11:
                            day_11_F.Content = 8;
                            day_11_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 12:
                            day_12_F.Content = 8;
                            day_12_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 13:
                            day_13_F.Content = 8;
                            day_13_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 14:
                            day_14_F.Content = 8;
                            day_14_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 15:
                            day_15_F.Content = 8;
                            day_15_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 16:
                            day_16_F.Content = 8;
                            day_16_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 17:
                            day_17_F.Content = 8;
                            day_17_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 18:
                            day_18_F.Content = 8;
                            day_18_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 19:
                            day_19_F.Content = 8;
                            day_19_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 20:
                            day_20_F.Content = 8;
                            day_20_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 21:
                            day_21_F.Content = 8;
                            day_21_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 22:
                            day_22_F.Content = 8;
                            day_22_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 23:
                            day_23_F.Content = 8;
                            day_23_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 24:
                            day_24_F.Content = 8;
                            day_24_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 25:
                            day_25_F.Content = 8;
                            day_25_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 26:
                            day_26_F.Content = 8;
                            day_26_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 27:
                            day_27_F.Content = 8;
                            day_27_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 28:
                            day_28_F.Content = 8;
                            day_28_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 29:
                            day_29_F.Content = 8;
                            day_29_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 30:
                            day_30_F.Content = 8;
                            day_30_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 31:
                            day_31_F.Content = 8;
                            day_31_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheEndDayOffFirst(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_F.Content = "D";
                            day_1_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 2:
                            day_2_F.Content = "D";
                            day_2_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 3:
                            day_3_F.Content = "D";
                            day_3_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 4:
                            day_4_F.Content = "D";
                            day_4_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 5:
                            day_5_F.Content = "D";
                            day_5_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 6:
                            day_6_F.Content = "D";
                            day_6_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 7:
                            day_7_F.Content = "D";
                            day_7_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 8:
                            day_8_F.Content = "D";
                            day_8_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 9:
                            day_9_F.Content = "D";
                            day_9_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 10:
                            day_10_F.Content = "D";
                            day_10_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 11:
                            day_11_F.Content = "D";
                            day_11_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 12:
                            day_12_F.Content = "D";
                            day_12_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 13:
                            day_13_F.Content = "D";
                            day_13_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 14:
                            day_14_F.Content = "D";
                            day_14_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 15:
                            day_15_F.Content = "D";
                            day_15_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 16:
                            day_16_F.Content = "D";
                            day_16_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 17:
                            day_17_F.Content = "D";
                            day_17_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 18:
                            day_18_F.Content = "D";
                            day_18_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 19:
                            day_19_F.Content = "D";
                            day_19_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 20:
                            day_20_F.Content = "D";
                            day_20_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 21:
                            day_21_F.Content = "D";
                            day_21_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 22:
                            day_22_F.Content = "D";
                            day_22_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 23:
                            day_23_F.Content = "D";
                            day_23_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 24:
                            day_24_F.Content = "D";
                            day_24_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 25:
                            day_25_F.Content = "D";
                            day_25_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 26:
                            day_26_F.Content = "D";
                            day_26_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 27:
                            day_27_F.Content = "D";
                            day_27_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 28:
                            day_28_F.Content = "D";
                            day_28_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 29:
                            day_29_F.Content = "D";
                            day_29_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 30:
                            day_30_F.Content = "D";
                            day_30_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 31:
                            day_31_F.Content = "D";
                            day_31_F.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ClearTheDaysOfFirst()
        {
            day_1_F.Content = "";
            day_1_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_2_F.Content = "";
            day_2_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_3_F.Content = "";
            day_3_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_4_F.Content = "";
            day_4_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_5_F.Content = "";
            day_5_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_6_F.Content = "";
            day_6_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_7_F.Content = "";
            day_7_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_8_F.Content = "";
            day_8_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_9_F.Content = "";
            day_9_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_10_F.Content = "";
            day_10_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_11_F.Content = "";
            day_11_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_12_F.Content = "";
            day_12_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_13_F.Content = "";
            day_13_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_14_F.Content = "";
            day_14_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_15_F.Content = "";
            day_15_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_16_F.Content = "";
            day_16_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_17_F.Content = "";
            day_17_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_18_F.Content = "";
            day_18_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_19_F.Content = "";
            day_19_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_20_F.Content = "";
            day_20_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_21_F.Content = "";
            day_21_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_22_F.Content = "";
            day_22_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_23_F.Content = "";
            day_23_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_24_F.Content = "";
            day_24_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_25_F.Content = "";
            day_25_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_26_F.Content = "";
            day_26_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_27_F.Content = "";
            day_27_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_28_F.Content = "";
            day_28_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_29_F.Content = "";
            day_29_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_30_F.Content = "";
            day_30_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_31_F.Content = "";
            day_31_F.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        public void ChoosingTheDaySecond(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_S.Content = 12;
                            day_1_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 2:
                            day_2_S.Content = 12;
                            day_2_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 3:
                            day_3_S.Content = 12;
                            day_3_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 4:
                            day_4_S.Content = 12;
                            day_4_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 5:
                            day_5_S.Content = 12;
                            day_5_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 6:
                            day_6_S.Content = 12;
                            day_6_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 7:
                            day_7_S.Content = 12;
                            day_7_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 8:
                            day_8_S.Content = 12;
                            day_8_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 9:
                            day_9_S.Content = 12;
                            day_9_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 10:
                            day_10_S.Content = 12;
                            day_10_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 11:
                            day_11_S.Content = 12;
                            day_11_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 12:
                            day_12_S.Content = 12;
                            day_12_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 13:
                            day_13_S.Content = 12;
                            day_13_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 14:
                            day_14_S.Content = 12;
                            day_14_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 15:
                            day_15_S.Content = 12;
                            day_15_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 16:
                            day_16_S.Content = 12;
                            day_16_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 17:
                            day_17_S.Content = 12;
                            day_17_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 18:
                            day_18_S.Content = 12;
                            day_18_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 19:
                            day_19_S.Content = 12;
                            day_19_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 20:
                            day_20_S.Content = 12;
                            day_20_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 21:
                            day_21_S.Content = 12;
                            day_21_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 22:
                            day_22_S.Content = 12;
                            day_22_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 23:
                            day_23_S.Content = 12;
                            day_23_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 24:
                            day_24_S.Content = 12;
                            day_24_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 25:
                            day_25_S.Content = 12;
                            day_25_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 26:
                            day_26_S.Content = 12;
                            day_26_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 27:
                            day_27_S.Content = 12;
                            day_27_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 28:
                            day_28_S.Content = 12;
                            day_28_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 29:
                            day_29_S.Content = 12;
                            day_29_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 30:
                            day_30_S.Content = 12;
                            day_30_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 31:
                            day_31_S.Content = 12;
                            day_31_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheNightSecond(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_S.Content = 4;
                            day_1_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 2:
                            day_2_S.Content = 4;
                            day_2_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 3:
                            day_3_S.Content = 4;
                            day_3_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 4:
                            day_4_S.Content = 4;
                            day_4_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 5:
                            day_5_S.Content = 4;
                            day_5_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 6:
                            day_6_S.Content = 4;
                            day_6_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 7:
                            day_7_S.Content = 4;
                            day_7_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 8:
                            day_8_S.Content = 4;
                            day_8_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 9:
                            day_9_S.Content = 4;
                            day_9_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 10:
                            day_10_S.Content = 4;
                            day_10_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 11:
                            day_11_S.Content = 4;
                            day_11_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 12:
                            day_12_S.Content = 4;
                            day_12_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 13:
                            day_13_S.Content = 4;
                            day_13_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 14:
                            day_14_S.Content = 4;
                            day_14_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 15:
                            day_15_S.Content = 4;
                            day_15_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 16:
                            day_16_S.Content = 4;
                            day_16_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 17:
                            day_17_S.Content = 4;
                            day_17_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 18:
                            day_18_S.Content = 4;
                            day_18_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 19:
                            day_19_S.Content = 4;
                            day_19_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 20:
                            day_20_S.Content = 4;
                            day_20_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 21:
                            day_21_S.Content = 4;
                            day_21_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 22:
                            day_22_S.Content = 4;
                            day_22_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 23:
                            day_23_S.Content = 4;
                            day_23_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 24:
                            day_24_S.Content = 4;
                            day_24_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 25:
                            day_25_S.Content = 4;
                            day_25_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 26:
                            day_26_S.Content = 4;
                            day_26_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 27:
                            day_27_S.Content = 4;
                            day_27_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 28:
                            day_28_S.Content = 4;
                            day_28_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 29:
                            day_29_S.Content = 4;
                            day_29_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 30:
                            day_30_S.Content = 4;
                            day_30_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 31:
                            day_31_S.Content = 4;
                            day_31_S.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheDayOffSecond(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_S.Content = 8;
                            day_1_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 2:
                            day_2_S.Content = 8;
                            day_2_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 3:
                            day_3_S.Content = 8;
                            day_3_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 4:
                            day_4_S.Content = 8;
                            day_4_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 5:
                            day_5_S.Content = 8;
                            day_5_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 6:
                            day_6_S.Content = 8;
                            day_6_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 7:
                            day_7_S.Content = 8;
                            day_7_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 8:
                            day_8_S.Content = 8;
                            day_8_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 9:
                            day_9_S.Content = 8;
                            day_9_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 10:
                            day_10_S.Content = 8;
                            day_10_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 11:
                            day_11_S.Content = 8;
                            day_11_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 12:
                            day_12_S.Content = 8;
                            day_12_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 13:
                            day_13_S.Content = 8;
                            day_13_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 14:
                            day_14_S.Content = 8;
                            day_14_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 15:
                            day_15_S.Content = 8;
                            day_15_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 16:
                            day_16_S.Content = 8;
                            day_16_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 17:
                            day_17_S.Content = 8;
                            day_17_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 18:
                            day_18_S.Content = 8;
                            day_18_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 19:
                            day_19_S.Content = 8;
                            day_19_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 20:
                            day_20_S.Content = 8;
                            day_20_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 21:
                            day_21_S.Content = 8;
                            day_21_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 22:
                            day_22_S.Content = 8;
                            day_22_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 23:
                            day_23_S.Content = 8;
                            day_23_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 24:
                            day_24_S.Content = 8;
                            day_24_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 25:
                            day_25_S.Content = 8;
                            day_25_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 26:
                            day_26_S.Content = 8;
                            day_26_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 27:
                            day_27_S.Content = 8;
                            day_27_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 28:
                            day_28_S.Content = 8;
                            day_28_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 29:
                            day_29_S.Content = 8;
                            day_29_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 30:
                            day_30_S.Content = 8;
                            day_30_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 31:
                            day_31_S.Content = 8;
                            day_31_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheEndDayOffSecond(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_S.Content = "D";
                            day_1_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 2:
                            day_2_S.Content = "D";
                            day_2_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 3:
                            day_3_S.Content = "D";
                            day_3_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 4:
                            day_4_S.Content = "D";
                            day_4_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 5:
                            day_5_S.Content = "D";
                            day_5_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 6:
                            day_6_S.Content = "D";
                            day_6_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 7:
                            day_7_S.Content = "D";
                            day_7_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 8:
                            day_8_S.Content = "D";
                            day_8_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 9:
                            day_9_S.Content = "D";
                            day_9_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 10:
                            day_10_S.Content = "D";
                            day_10_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 11:
                            day_11_S.Content = "D";
                            day_11_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 12:
                            day_12_S.Content = "D";
                            day_12_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 13:
                            day_13_S.Content = "D";
                            day_13_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 14:
                            day_14_S.Content = "D";
                            day_14_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 15:
                            day_15_S.Content = "D";
                            day_15_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 16:
                            day_16_S.Content = "D";
                            day_16_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 17:
                            day_17_S.Content = "D";
                            day_17_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 18:
                            day_18_S.Content = "D";
                            day_18_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 19:
                            day_19_S.Content = "D";
                            day_19_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 20:
                            day_20_S.Content = "D";
                            day_20_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 21:
                            day_21_S.Content = "D";
                            day_21_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 22:
                            day_22_S.Content = "D";
                            day_22_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 23:
                            day_23_S.Content = "D";
                            day_23_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 24:
                            day_24_S.Content = "D";
                            day_24_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 25:
                            day_25_S.Content = "D";
                            day_25_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 26:
                            day_26_S.Content = "D";
                            day_26_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 27:
                            day_27_S.Content = "D";
                            day_27_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 28:
                            day_28_S.Content = "D";
                            day_28_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 29:
                            day_29_S.Content = "D";
                            day_29_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 30:
                            day_30_S.Content = "D";
                            day_30_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 31:
                            day_31_S.Content = "D";
                            day_31_S.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ClearTheDaysOfSecond()
        {
            day_1_S.Content = "";
            day_1_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_2_S.Content = "";
            day_2_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_3_S.Content = "";
            day_3_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_4_S.Content = "";
            day_4_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_5_S.Content = "";
            day_5_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_6_S.Content = "";
            day_6_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_7_S.Content = "";
            day_7_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_8_S.Content = "";
            day_8_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_9_S.Content = "";
            day_9_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_10_S.Content = "";
            day_10_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_11_S.Content = "";
            day_11_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_12_S.Content = "";
            day_12_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_13_S.Content = "";
            day_13_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_14_S.Content = "";
            day_14_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_15_S.Content = "";
            day_15_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_16_S.Content = "";
            day_16_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_17_S.Content = "";
            day_17_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_18_S.Content = "";
            day_18_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_19_S.Content = "";
            day_19_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_20_S.Content = "";
            day_20_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_21_S.Content = "";
            day_21_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_22_S.Content = "";
            day_22_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_23_S.Content = "";
            day_23_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_24_S.Content = "";
            day_24_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_25_S.Content = "";
            day_25_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_26_S.Content = "";
            day_26_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_27_S.Content = "";
            day_27_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_28_S.Content = "";
            day_28_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_29_S.Content = "";
            day_29_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_30_S.Content = "";
            day_30_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_31_S.Content = "";
            day_31_S.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        public void ChoosingTheDayThird(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_T.Content = 12;
                            day_1_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 2:
                            day_2_T.Content = 12;
                            day_2_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 3:
                            day_3_T.Content = 12;
                            day_3_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 4:
                            day_4_T.Content = 12;
                            day_4_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 5:
                            day_5_T.Content = 12;
                            day_5_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 6:
                            day_6_T.Content = 12;
                            day_6_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 7:
                            day_7_T.Content = 12;
                            day_7_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 8:
                            day_8_T.Content = 12;
                            day_8_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 9:
                            day_9_T.Content = 12;
                            day_9_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 10:
                            day_10_T.Content = 12;
                            day_10_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 11:
                            day_11_T.Content = 12;
                            day_11_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 12:
                            day_12_T.Content = 12;
                            day_12_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 13:
                            day_13_T.Content = 12;
                            day_13_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 14:
                            day_14_T.Content = 12;
                            day_14_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 15:
                            day_15_T.Content = 12;
                            day_15_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 16:
                            day_16_T.Content = 12;
                            day_16_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 17:
                            day_17_T.Content = 12;
                            day_17_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 18:
                            day_18_T.Content = 12;
                            day_18_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 19:
                            day_19_T.Content = 12;
                            day_19_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 20:
                            day_20_T.Content = 12;
                            day_20_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 21:
                            day_21_T.Content = 12;
                            day_21_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 22:
                            day_22_T.Content = 12;
                            day_22_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 23:
                            day_23_T.Content = 12;
                            day_23_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 24:
                            day_24_T.Content = 12;
                            day_24_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 25:
                            day_25_T.Content = 12;
                            day_25_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 26:
                            day_26_T.Content = 12;
                            day_26_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 27:
                            day_27_T.Content = 12;
                            day_27_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 28:
                            day_28_T.Content = 12;
                            day_28_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 29:
                            day_29_T.Content = 12;
                            day_29_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 30:
                            day_30_T.Content = 12;
                            day_30_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 31:
                            day_31_T.Content = 12;
                            day_31_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheNightThird(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_T.Content = 4;
                            day_1_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 2:
                            day_2_T.Content = 4;
                            day_2_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 3:
                            day_3_T.Content = 4;
                            day_3_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 4:
                            day_4_T.Content = 4;
                            day_4_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 5:
                            day_5_T.Content = 4;
                            day_5_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 6:
                            day_6_T.Content = 4;
                            day_6_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 7:
                            day_7_T.Content = 4;
                            day_7_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 8:
                            day_8_T.Content = 4;
                            day_8_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 9:
                            day_9_T.Content = 4;
                            day_9_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 10:
                            day_10_T.Content = 4;
                            day_10_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 11:
                            day_11_T.Content = 4;
                            day_11_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 12:
                            day_12_T.Content = 4;
                            day_12_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 13:
                            day_13_T.Content = 4;
                            day_13_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 14:
                            day_14_T.Content = 4;
                            day_14_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 15:
                            day_15_T.Content = 4;
                            day_15_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 16:
                            day_16_T.Content = 4;
                            day_16_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 17:
                            day_17_T.Content = 4;
                            day_17_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 18:
                            day_18_T.Content = 4;
                            day_18_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 19:
                            day_19_T.Content = 4;
                            day_19_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 20:
                            day_20_T.Content = 4;
                            day_20_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 21:
                            day_21_T.Content = 4;
                            day_21_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 22:
                            day_22_T.Content = 4;
                            day_22_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 23:
                            day_23_T.Content = 4;
                            day_23_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 24:
                            day_24_T.Content = 4;
                            day_24_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 25:
                            day_25_T.Content = 4;
                            day_25_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 26:
                            day_26_T.Content = 4;
                            day_26_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 27:
                            day_27_T.Content = 4;
                            day_27_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 28:
                            day_28_T.Content = 4;
                            day_28_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 29:
                            day_29_T.Content = 4;
                            day_29_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 30:
                            day_30_T.Content = 4;
                            day_30_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 31:
                            day_31_T.Content = 4;
                            day_31_T.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheDayOffThird(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_T.Content = 8;
                            day_1_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 2:
                            day_2_T.Content = 8;
                            day_2_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 3:
                            day_3_T.Content = 8;
                            day_3_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 4:
                            day_4_T.Content = 8;
                            day_4_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 5:
                            day_5_T.Content = 8;
                            day_5_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 6:
                            day_6_T.Content = 8;
                            day_6_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 7:
                            day_7_T.Content = 8;
                            day_7_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 8:
                            day_8_T.Content = 8;
                            day_8_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 9:
                            day_9_T.Content = 8;
                            day_9_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 10:
                            day_10_T.Content = 8;
                            day_10_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 11:
                            day_11_T.Content = 8;
                            day_11_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 12:
                            day_12_T.Content = 8;
                            day_12_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 13:
                            day_13_T.Content = 8;
                            day_13_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 14:
                            day_14_T.Content = 8;
                            day_14_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 15:
                            day_15_T.Content = 8;
                            day_15_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 16:
                            day_16_T.Content = 8;
                            day_16_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 17:
                            day_17_T.Content = 8;
                            day_17_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 18:
                            day_18_T.Content = 8;
                            day_18_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 19:
                            day_19_T.Content = 8;
                            day_19_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 20:
                            day_20_T.Content = 8;
                            day_20_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 21:
                            day_21_T.Content = 8;
                            day_21_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 22:
                            day_22_T.Content = 8;
                            day_22_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 23:
                            day_23_T.Content = 8;
                            day_23_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 24:
                            day_24_T.Content = 8;
                            day_24_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 25:
                            day_25_T.Content = 8;
                            day_25_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 26:
                            day_26_T.Content = 8;
                            day_26_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 27:
                            day_27_T.Content = 8;
                            day_27_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 28:
                            day_28_T.Content = 8;
                            day_28_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 29:
                            day_29_T.Content = 8;
                            day_29_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 30:
                            day_30_T.Content = 8;
                            day_30_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 31:
                            day_31_T.Content = 8;
                            day_31_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheEndDayOffThird(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_T.Content = "D";
                            day_1_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 2:
                            day_2_T.Content = "D";
                            day_2_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 3:
                            day_3_T.Content = "D";
                            day_3_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 4:
                            day_4_T.Content = "D";
                            day_4_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 5:
                            day_5_T.Content = "D";
                            day_5_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 6:
                            day_6_T.Content = "D";
                            day_6_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 7:
                            day_7_T.Content = "D";
                            day_7_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 8:
                            day_8_T.Content = "D";
                            day_8_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 9:
                            day_9_T.Content = "D";
                            day_9_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 10:
                            day_10_T.Content = "D";
                            day_10_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 11:
                            day_11_T.Content = "D";
                            day_11_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 12:
                            day_12_T.Content = "D";
                            day_12_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 13:
                            day_13_T.Content = "D";
                            day_13_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 14:
                            day_14_T.Content = "D";
                            day_14_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 15:
                            day_15_T.Content = "D";
                            day_15_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 16:
                            day_16_T.Content = "D";
                            day_16_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 17:
                            day_17_T.Content = "D";
                            day_17_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 18:
                            day_18_T.Content = "D";
                            day_18_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 19:
                            day_19_T.Content = "D";
                            day_19_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 20:
                            day_20_T.Content = "D";
                            day_20_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 21:
                            day_21_T.Content = "D";
                            day_21_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 22:
                            day_22_T.Content = "D";
                            day_22_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 23:
                            day_23_T.Content = "D";
                            day_23_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 24:
                            day_24_T.Content = "D";
                            day_24_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 25:
                            day_25_T.Content = "D";
                            day_25_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 26:
                            day_26_T.Content = "D";
                            day_26_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 27:
                            day_27_T.Content = "D";
                            day_27_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 28:
                            day_28_T.Content = "D";
                            day_28_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 29:
                            day_29_T.Content = "D";
                            day_29_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 30:
                            day_30_T.Content = "D";
                            day_30_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 31:
                            day_31_T.Content = "D";
                            day_31_T.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ClearTheDaysOfThird()
        {
            day_1_T.Content = "";
            day_1_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_2_T.Content = "";
            day_2_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_3_T.Content = "";
            day_3_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_4_T.Content = "";
            day_4_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_5_T.Content = "";
            day_5_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_6_T.Content = "";
            day_6_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_7_T.Content = "";
            day_7_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_8_T.Content = "";
            day_8_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_9_T.Content = "";
            day_9_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_10_T.Content = "";
            day_10_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_11_T.Content = "";
            day_11_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_12_T.Content = "";
            day_12_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_13_T.Content = "";
            day_13_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_14_T.Content = "";
            day_14_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_15_T.Content = "";
            day_15_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_16_T.Content = "";
            day_16_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_17_T.Content = "";
            day_17_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_18_T.Content = "";
            day_18_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_19_T.Content = "";
            day_19_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_20_T.Content = "";
            day_20_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_21_T.Content = "";
            day_21_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_22_T.Content = "";
            day_22_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_23_T.Content = "";
            day_23_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_24_T.Content = "";
            day_24_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_25_T.Content = "";
            day_25_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_26_T.Content = "";
            day_26_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_27_T.Content = "";
            day_27_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_28_T.Content = "";
            day_28_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_29_T.Content = "";
            day_29_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_30_T.Content = "";
            day_30_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_31_T.Content = "";
            day_31_T.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        public void ChoosingTheDayFourth(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_Fo.Content = 12;
                            day_1_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 2:
                            day_2_Fo.Content = 12;
                            day_2_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 3:
                            day_3_Fo.Content = 12;
                            day_3_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 4:
                            day_4_Fo.Content = 12;
                            day_4_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 5:
                            day_5_Fo.Content = 12;
                            day_5_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 6:
                            day_6_Fo.Content = 12;
                            day_6_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 7:
                            day_7_Fo.Content = 12;
                            day_7_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 8:
                            day_8_Fo.Content = 12;
                            day_8_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 9:
                            day_9_Fo.Content = 12;
                            day_9_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 10:
                            day_10_Fo.Content = 12;
                            day_10_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 11:
                            day_11_Fo.Content = 12;
                            day_11_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 12:
                            day_12_Fo.Content = 12;
                            day_12_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 13:
                            day_13_Fo.Content = 12;
                            day_13_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 14:
                            day_14_Fo.Content = 12;
                            day_14_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 15:
                            day_15_Fo.Content = 12;
                            day_15_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 16:
                            day_16_Fo.Content = 12;
                            day_16_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 17:
                            day_17_Fo.Content = 12;
                            day_17_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 18:
                            day_18_Fo.Content = 12;
                            day_18_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 19:
                            day_19_Fo.Content = 12;
                            day_19_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 20:
                            day_20_Fo.Content = 12;
                            day_20_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 21:
                            day_21_Fo.Content = 12;
                            day_21_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 22:
                            day_22_Fo.Content = 12;
                            day_22_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 23:
                            day_23_Fo.Content = 12;
                            day_23_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 24:
                            day_24_Fo.Content = 12;
                            day_24_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 25:
                            day_25_Fo.Content = 12;
                            day_25_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 26:
                            day_26_Fo.Content = 12;
                            day_26_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 27:
                            day_27_Fo.Content = 12;
                            day_27_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 28:
                            day_28_Fo.Content = 12;
                            day_28_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 29:
                            day_29_Fo.Content = 12;
                            day_29_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 30:
                            day_30_Fo.Content = 12;
                            day_30_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 31:
                            day_31_Fo.Content = 12;
                            day_31_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheNightFourth(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_Fo.Content = 4;
                            day_1_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 2:
                            day_2_Fo.Content = 4;
                            day_2_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 3:
                            day_3_Fo.Content = 4;
                            day_3_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 4:
                            day_4_Fo.Content = 4;
                            day_4_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 5:
                            day_5_Fo.Content = 4;
                            day_5_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 6:
                            day_6_Fo.Content = 4;
                            day_6_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 7:
                            day_7_Fo.Content = 4;
                            day_7_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 8:
                            day_8_Fo.Content = 4;
                            day_8_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 9:
                            day_9_Fo.Content = 4;
                            day_9_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 10:
                            day_10_Fo.Content = 4;
                            day_10_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 11:
                            day_11_Fo.Content = 4;
                            day_11_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 12:
                            day_12_Fo.Content = 4;
                            day_12_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 13:
                            day_13_Fo.Content = 4;
                            day_13_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 14:
                            day_14_Fo.Content = 4;
                            day_14_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 15:
                            day_15_Fo.Content = 4;
                            day_15_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 16:
                            day_16_Fo.Content = 4;
                            day_16_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 17:
                            day_17_Fo.Content = 4;
                            day_17_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 18:
                            day_18_Fo.Content = 4;
                            day_18_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 19:
                            day_19_Fo.Content = 4;
                            day_19_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 20:
                            day_20_Fo.Content = 4;
                            day_20_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 21:
                            day_21_Fo.Content = 4;
                            day_21_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 22:
                            day_22_Fo.Content = 4;
                            day_22_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 23:
                            day_23_Fo.Content = 4;
                            day_23_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 24:
                            day_24_Fo.Content = 4;
                            day_24_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 25:
                            day_25_Fo.Content = 4;
                            day_25_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 26:
                            day_26_Fo.Content = 4;
                            day_26_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 27:
                            day_27_Fo.Content = 4;
                            day_27_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 28:
                            day_28_Fo.Content = 4;
                            day_28_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 29:
                            day_29_Fo.Content = 4;
                            day_29_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 30:
                            day_30_Fo.Content = 4;
                            day_30_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        case 31:
                            day_31_Fo.Content = 4;
                            day_31_Fo.Background = new SolidColorBrush(Color.FromRgb(255, 0, 7));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheDayOffFourth(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_Fo.Content = 8;
                            day_1_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 2:
                            day_2_Fo.Content = 8;
                            day_2_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 3:
                            day_3_Fo.Content = 8;
                            day_3_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 4:
                            day_4_Fo.Content = 8;
                            day_4_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 5:
                            day_5_Fo.Content = 8;
                            day_5_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 6:
                            day_6_Fo.Content = 8;
                            day_6_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 7:
                            day_7_Fo.Content = 8;
                            day_7_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 8:
                            day_8_Fo.Content = 8;
                            day_8_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 9:
                            day_9_Fo.Content = 8;
                            day_9_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 10:
                            day_10_Fo.Content = 8;
                            day_10_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 11:
                            day_11_Fo.Content = 8;
                            day_11_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 12:
                            day_12_Fo.Content = 8;
                            day_12_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 13:
                            day_13_Fo.Content = 8;
                            day_13_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 14:
                            day_14_Fo.Content = 8;
                            day_14_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 15:
                            day_15_Fo.Content = 8;
                            day_15_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 16:
                            day_16_Fo.Content = 8;
                            day_16_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 17:
                            day_17_Fo.Content = 8;
                            day_17_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 18:
                            day_18_Fo.Content = 8;
                            day_18_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 19:
                            day_19_Fo.Content = 8;
                            day_19_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 20:
                            day_20_Fo.Content = 8;
                            day_20_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 21:
                            day_21_Fo.Content = 8;
                            day_21_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 22:
                            day_22_Fo.Content = 8;
                            day_22_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 23:
                            day_23_Fo.Content = 8;
                            day_23_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 24:
                            day_24_Fo.Content = 8;
                            day_24_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 25:
                            day_25_Fo.Content = 8;
                            day_25_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 26:
                            day_26_Fo.Content = 8;
                            day_26_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 27:
                            day_27_Fo.Content = 8;
                            day_27_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 28:
                            day_28_Fo.Content = 8;
                            day_28_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 29:
                            day_29_Fo.Content = 8;
                            day_29_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 30:
                            day_30_Fo.Content = 8;
                            day_30_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 31:
                            day_31_Fo.Content = 8;
                            day_31_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChoosingTheEndDayOffFourth(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            day_1_Fo.Content = "D";
                            day_1_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 2:
                            day_2_Fo.Content = "D";
                            day_2_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 3:
                            day_3_Fo.Content = "D";
                            day_3_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 4:
                            day_4_Fo.Content = "D";
                            day_4_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 5:
                            day_5_Fo.Content = "D";
                            day_5_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 6:
                            day_6_Fo.Content = "D";
                            day_6_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 7:
                            day_7_Fo.Content = "D";
                            day_7_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 8:
                            day_8_Fo.Content = "D";
                            day_8_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 9:
                            day_9_Fo.Content = "D";
                            day_9_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 10:
                            day_10_Fo.Content = "D";
                            day_10_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 11:
                            day_11_Fo.Content = "D";
                            day_11_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 12:
                            day_12_Fo.Content = "D";
                            day_12_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 13:
                            day_13_Fo.Content = "D";
                            day_13_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 14:
                            day_14_Fo.Content = "D";
                            day_14_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 15:
                            day_15_Fo.Content = "D";
                            day_15_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 16:
                            day_16_Fo.Content = "D";
                            day_16_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 17:
                            day_17_Fo.Content = "D";
                            day_17_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 18:
                            day_18_Fo.Content = "D";
                            day_18_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 19:
                            day_19_Fo.Content = "D";
                            day_19_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 20:
                            day_20_Fo.Content = "D";
                            day_20_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 21:
                            day_21_Fo.Content = "D";
                            day_21_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 22:
                            day_22_Fo.Content = "D";
                            day_22_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 23:
                            day_23_Fo.Content = "D";
                            day_23_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 24:
                            day_24_Fo.Content = "D";
                            day_24_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 25:
                            day_25_Fo.Content = "D";
                            day_25_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 26:
                            day_26_Fo.Content = "D";
                            day_26_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 27:
                            day_27_Fo.Content = "D";
                            day_27_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 28:
                            day_28_Fo.Content = "D";
                            day_28_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 29:
                            day_29_Fo.Content = "D";
                            day_29_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 30:
                            day_30_Fo.Content = "D";
                            day_30_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        case 31:
                            day_31_Fo.Content = "D";
                            day_31_Fo.Background = new SolidColorBrush(Color.FromRgb(139, 255, 1));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ClearTheDaysOfFourth()
        {
            day_1_Fo.Content = "";
            day_1_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_2_Fo.Content = "";
            day_2_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_3_Fo.Content = "";
            day_3_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_4_Fo.Content = "";
            day_4_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_5_Fo.Content = "";
            day_5_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_6_Fo.Content = "";
            day_6_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_7_Fo.Content = "";
            day_7_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_8_Fo.Content = "";
            day_8_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_9_Fo.Content = "";
            day_9_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_10_Fo.Content = "";
            day_10_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_11_Fo.Content = "";
            day_11_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_12_Fo.Content = "";
            day_12_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_13_Fo.Content = "";
            day_13_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_14_Fo.Content = "";
            day_14_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_15_Fo.Content = "";
            day_15_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_16_Fo.Content = "";
            day_16_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_17_Fo.Content = "";
            day_17_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_18_Fo.Content = "";
            day_18_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_19_Fo.Content = "";
            day_19_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_20_Fo.Content = "";
            day_20_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_21_Fo.Content = "";
            day_21_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_22_Fo.Content = "";
            day_22_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_23_Fo.Content = "";
            day_23_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_24_Fo.Content = "";
            day_24_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_25_Fo.Content = "";
            day_25_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_26_Fo.Content = "";
            day_26_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_27_Fo.Content = "";
            day_27_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_28_Fo.Content = "";
            day_28_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_29_Fo.Content = "";
            day_29_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_30_Fo.Content = "";
            day_30_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            day_31_Fo.Content = "";
            day_31_Fo.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        public void ChoosingTheNumberOfDay(List<int> number)
        {
            try
            {
                int[] num = number.ToArray();

                for (int i = 0; i < num.Length; i++)
                {
                    switch (num[i])
                    {
                        case 1:
                            num_1.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 2:
                            num_2.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 3:
                            num_3.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 4:
                            num_4.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 5:
                            num_5.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 6:
                            num_6.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 7:
                            num_7.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 8:
                            num_8.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 9:
                            num_9.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 10:
                            num_10.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 11:
                            num_11.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 12:
                            num_12.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 13:
                            num_13.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 14:
                            num_14.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 15:
                            num_15.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 16:
                            num_16.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 17:
                            num_17.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 18:
                            num_18.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 19:
                            num_19.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 20:
                            num_20.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 21:
                            num_21.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 22:
                            num_22.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 23:
                            num_23.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 24:
                            num_24.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 25:
                            num_25.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 26:
                            num_26.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 27:
                            num_27.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 28:
                            num_28.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 29:
                            num_29.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 30:
                            num_30.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        case 31:
                            num_31.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ClearNumberOfDay()
        {
            num_1.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_2.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_3.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_4.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_5.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_6.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_7.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_8.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_9.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_10.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_11.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_12.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_13.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_14.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_15.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_16.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_17.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_18.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_19.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_20.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_21.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_22.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_23.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_24.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_25.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_26.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_27.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_28.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_29.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_30.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            num_31.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        public void SoundMesaageAboutNotAllParametersAreFilledIn(SoundPlayer player, bool soundState, string languageState)
        {
            if (soundState == true)
            {
                try
                {
                    if (year.Text == "Year" || nameOfMonth.Text == "Name of month")
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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

        public void SetOfParameters()
        {
            try
            {
                SettingSoundParameters();
                SettingLanguageParameters();

                SoundMessageAboutHereIsResult(player, soundState, langaugeState);
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
    }
}
