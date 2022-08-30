using ProductionPassControlSystem.BLL;
using System;
using System.Data;
using System.Media;
using System.Windows;
using InformationAboutUseThePass.InformationAboutUseThePassService;
using System.Windows.Data;
using System.Windows.Controls;

namespace InformationAboutUseThePass
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAddColumnsInGrid, IAudioMessageAboutDelete, IStringMessage,
                                              ISettingSoundParameters, ISettingLanguageParameters, ISetOfParameters,
                                              ISetOfParameters2, ISetOfParameters3, IAudioMessageAboutHereIsResult,
                                              IAudioMessageAboutResetInputParameters, IAudioMessageAboutEnterWorkerId,
                                              ISetOfParameters4, IAudioMessageAboutThereIsNoWorkerWithSuchIdentificator,
                                              ISetOfParameters5, ISetOfParameters6, IAudioMessageAboutCheckWorkerIdentificator
    {
        private DataTable informationAboutUseThePass;

        private int workId_ = 0;
        private int year_ = 0;
        private string month_ = "";
        private int numberOfDay_ = 0;

        private SoundPlayer player;

        private bool soundState;
        private string langaugeState;
        public MainWindow()
        {
            InitializeComponent();

            informationAboutUseThePass = new DataTable();

            AddColumnsInGrid();

            FillingTheGridWithAllInformationAboutUseThePass();

            InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");
        }

        private void ConvertInputData()
        {
            try
            {
                if (workerId.Text != "Worker id" && year.Text != "Year" && nameOfMonth.Text != "Month" && numberOfDay.Text != "Day")
                {
                    workId_ = Convert.ToInt32(workerId.Text);
                    year_ = Convert.ToInt32(year.Text);
                    month_ = nameOfMonth.Text;
                    numberOfDay_ = Convert.ToInt32(numberOfDay.Text);
                }
                else if (workerId.Text != "Worker id" && year.Text != "Year" && nameOfMonth.Text != "Month")
                {
                    workId_ = Convert.ToInt32(workerId.Text);
                    year_ = Convert.ToInt32(year.Text);
                    month_ = nameOfMonth.Text;
                }
                else if (workerId.Text != "Worker id" && year.Text != "Year")
                {
                    workId_ = Convert.ToInt32(workerId.Text);
                    year_ = Convert.ToInt32(year.Text);
                }
                else if (workerId.Text != "Worker id")
                {
                    workId_ = Convert.ToInt32(workerId.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillingTheGridWithAllInformationAboutUseThePass()
        {
            ConvertInputData();

            totalNumber.Content = "";

            try
            {
                if (workerId.Text == "Worker id" && year.Text == "Year" && nameOfMonth.Text == "Month" && numberOfDay.Text == "Day")
                {
                    InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");
                    informationAboutUseThePass = client.GetAllInformationAboutUseThePass();
                    totalNumber.Content = client.TotalNumberOfPassesUsed();
                    client.Close();
                }
                else if (workerId.Text != "Worker id" && year.Text == "Year" && nameOfMonth.Text == "Month" && numberOfDay.Text == "Day")
                {
                    InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");
                    informationAboutUseThePass = client.GetAllInformationAboutUseThePassByWorkerId(workId_);
                    totalNumber.Content = client.TotalNumberOfPassesUsedByWorkerId(workId_);
                    client.Close();
                }
                else if (workerId.Text != "Worker id" && year.Text != "Year" && nameOfMonth.Text == "Month" && numberOfDay.Text == "Day")
                {
                    InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");
                    informationAboutUseThePass = client.GetAllInformationAboutUseThePassByWorkerIdYear(workId_, year_);
                    totalNumber.Content = client.TotalNumberOfPassesUsedByWorkerIdYear(workId_, year_);
                    client.Close();
                }
                else if (workerId.Text != "Worker id" && year.Text != "Year" && nameOfMonth.Text != "Month" && numberOfDay.Text == "Day")
                {
                    InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");
                    informationAboutUseThePass = client.GetAllInformationAboutUseThePassByWorkerIdYearMonth(workId_, year_, month_);
                    totalNumber.Content = client.TotalNumberOfPassesUsedByWorkerIdYearMonth(workId_, year_, month_);
                    client.Close();
                }
                else if (workerId.Text != "Worker id" && year.Text != "Year" && nameOfMonth.Text != "Month" && numberOfDay.Text != "Day")
                {
                    InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");
                    informationAboutUseThePass = client.GetAllInformationAboutUseThePassByWorkerIdYearMonthNumberOfDay(workId_, year_, month_, numberOfDay_);
                    totalNumber.Content = client.TotalNumberOfPassesUsedByWorkerIdYearMonthNumberOfDay(workId_, year_, month_, numberOfDay_);
                    client.Close();
                }

                informationAboutUseThePassGrid.ItemsSource = informationAboutUseThePass.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void See_Result_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (workerId.Text == "Worker id" || workerId.Text == "")
                {
                    SetOfParameters4();
                }
                else
                {
                    numberOfDeletions.Content = "0";

                    InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");

                    int count = client.AvailabilityOfASpecificWorkerId_Pass(Convert.ToInt32(workerId.Text));

                    if (count > 0)
                    {
                        FillingTheGridWithAllInformationAboutUseThePass();

                        SetOfParameters2();
                    }
                    else
                    {
                        SetOfParameters5();
                    }
                }
            }
            catch
            {
                SetOfParameters6();
            }
        }

        private void Reset_Input_Parameters_Click(object sender, RoutedEventArgs e)
        {
            workerId.Text = "Worker id";
            year.Text = "Year";
            nameOfMonth.Text = "Month";
            numberOfDay.Text = "Day";
            numberOfDeletions.Content = "0";

            FillingTheGridWithAllInformationAboutUseThePass();

            SetOfParameters3();
        }

        private void DeletingInformationAboutTheUseOfPasses()
        {
            totalNumber.Content = "";
            numberOfDeletions.Content = "";

            int deletingInformationAboutTheUseOfPasses = 0;

            try
            {
                if (workerId.Text == "Worker id" && year.Text == "Year" && nameOfMonth.Text == "Month" && numberOfDay.Text == "Day")
                {
                    InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");
                    deletingInformationAboutTheUseOfPasses = client.DeleteAllFromInformationAboutUseThePass();
                    numberOfDeletions.Content = deletingInformationAboutTheUseOfPasses;
                    totalNumber.Content = client.TotalNumberOfPassesUsed();
                    client.Close();

                }
                else if (workerId.Text != "Worker id" && year.Text == "Year" && nameOfMonth.Text == "Month" && numberOfDay.Text == "Day")
                {
                    InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");
                    deletingInformationAboutTheUseOfPasses = client.DeleteAllInformationAboutUseThePassByWorkerId(workId_);
                    numberOfDeletions.Content = deletingInformationAboutTheUseOfPasses;
                    totalNumber.Content = client.TotalNumberOfPassesUsed();
                    client.Close();
                }
                else if (workerId.Text != "Worker id" && year.Text != "Year" && nameOfMonth.Text == "Month" && numberOfDay.Text == "Day")
                {
                    InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");
                    deletingInformationAboutTheUseOfPasses = client.DeletemoreSpecificInformationAboutUseThePassByWorkerIdYear(workId_,
                                                                                                                                  year_);
                    numberOfDeletions.Content = deletingInformationAboutTheUseOfPasses;
                    totalNumber.Content = client.TotalNumberOfPassesUsed();
                    client.Close();
                }
                else if (workerId.Text != "Worker id" && year.Text != "Year" && nameOfMonth.Text != "Month" && numberOfDay.Text == "Day")
                {
                    InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");
                    deletingInformationAboutTheUseOfPasses = client.DeletemoreSpecificInformationAboutUseThePassByWorkerIdYearMonth(workId_,
                                                                                                                                       year_,
                                                                                                                                       month_);
                    numberOfDeletions.Content = deletingInformationAboutTheUseOfPasses;
                    totalNumber.Content = client.TotalNumberOfPassesUsed();
                    client.Close();
                }
                else if (workerId.Text != "Worker id" && year.Text != "Year" && nameOfMonth.Text != "Month" && numberOfDay.Text != "Day")
                {
                    InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");
                    deletingInformationAboutTheUseOfPasses = client.DeletemoreSpecificInformationAboutUseThePassByWorkerIdYearMonthNumberOfDay(workId_,
                                                                                                                                                  year_,
                                                                                                                                                  month_,
                                                                                                                                                  numberOfDay_);
                    numberOfDeletions.Content = deletingInformationAboutTheUseOfPasses;
                    totalNumber.Content = client.TotalNumberOfPassesUsed();
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InformationAboutUseThePassClient client = new InformationAboutUseThePassClient("NetTcpBinding_IInformationAboutUseThePass");

                int count = client.AvailabilityOfASpecificWorkerId_Pass(Convert.ToInt32(workerId.Text));

                if (count > 0)
                {
                    DeletingInformationAboutTheUseOfPasses();

                    informationAboutUseThePass = client.GetAllInformationAboutUseThePass();
                    totalNumber.Content = client.TotalNumberOfPassesUsed();
                    client.Close();

                    informationAboutUseThePassGrid.ItemsSource = informationAboutUseThePass.DefaultView;

                    SetOfParameters();
                }
                else
                {
                    SetOfParameters5();
                }
            }
            catch
            {
                SetOfParameters6();
            }
        }

        public void AddColumnsInGrid()
        {
            try
            {
                informationAboutUseThePassGrid.ItemsSource = null;

                informationAboutUseThePassGrid.AutoGenerateColumns = false;

                informationAboutUseThePassGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Year",
                    Binding = new Binding("Year")
                });

                informationAboutUseThePassGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Month",
                    Binding = new Binding("Month")
                });

                informationAboutUseThePassGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "NumberOfDay",
                    Binding = new Binding("NumberOfDay")
                });

                informationAboutUseThePassGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Condition",
                    Binding = new Binding("Condition")
                });

                informationAboutUseThePassGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "SinceWhatTime",
                    Binding = new Binding("SinceWhatTime")
                });

                informationAboutUseThePassGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "TillWhatTime",
                    Binding = new Binding("TillWhatTime")
                });

                informationAboutUseThePassGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Value",
                    Binding = new Binding("Value")
                });

                informationAboutUseThePassGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "WorkerId",
                    Binding = new Binding("WorkerId")
                });

                informationAboutUseThePassGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "TimeOfUseOfThePass",
                    Binding = new Binding("TimeOfUseOfThePass")
                });

                informationAboutUseThePassGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "TheResultOfUsingOfThePass",
                    Binding = new Binding("TheResultOfUsingOfThePass")
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SoundMessageAboutDelete(SoundPlayer player, bool soundState, string languageState)
        {
            if (soundState == true)
            {
                try
                {
                    if (langaugeState == "eng")
                    {
                        player = new SoundPlayer(Properties.Resources.deleted);
                        player.Play();
                        player.Dispose();
                    }
                    else
                    {
                        player = new SoundPlayer(Properties.Resources.удалено);
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

        public void SetOfParameters()
        {
            try
            {
                SettingSoundParameters();
                SettingLanguageParameters();

                SoundMessageAboutDelete(player, soundState, langaugeState);

                if (langaugeState == "eng" && soundState == false)
                {
                    StringMessageInEnglish("Deleted!");
                }
                else if (langaugeState == "ru" && soundState == false)
                {
                    StringMessageInRussian("Удалено!");
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

                SoundMessageAboutResetInputParameters(player, soundState, langaugeState);

                if (langaugeState == "eng" && soundState == false)
                {
                    StringMessageInEnglish("Parameters reseted!");
                }
                else if (langaugeState == "ru" && soundState == false)
                {
                    StringMessageInRussian("Параметры сброшены!");
                }
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

        public void SoundMessageAboutResetInputParameters(SoundPlayer player, bool soundState, string languageState)
        {
            if (soundState == true)
            {
                try
                {
                    if (langaugeState == "eng")
                    {
                        player = new SoundPlayer(Properties.Resources.parameters_reseted);
                        player.Play();
                        player.Dispose();
                    }
                    else
                    {
                        player = new SoundPlayer(Properties.Resources.параметры_сброшены);
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

        public void SetOfParameters5()
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

        public void SetOfParameters6()
        {
            try
            {
                SettingSoundParameters();
                SettingLanguageParameters();

                SoundMessageAboutCheckWorkerIdentificator(player, soundState, langaugeState);

                if (langaugeState == "eng" && soundState == false)
                {
                    StringMessageInEnglish("Please, check the worker identificator, you may have made a mistake there!");
                }
                else if (langaugeState == "ru" && soundState == false)
                {
                    StringMessageInRussian("Пожалуйста, проверьте идентификатор работника, возможно, вы там ошиблись!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SoundMessageAboutCheckWorkerIdentificator(SoundPlayer player, bool soundState, string languageState)
        {
            if (soundState == true)
            {
                try
                {
                    if (langaugeState == "eng")
                    {
                        player = new SoundPlayer(Properties.Resources.check_worker_id);
                        player.Play();
                        player.Dispose();
                    }
                    else
                    {
                        player = new SoundPlayer(Properties.Resources.проверить_идентификатор_работника);
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
