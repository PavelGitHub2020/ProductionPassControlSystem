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
using ProductionPassControlSystem.BLL;
using Remove.RemoveService;

namespace Remove
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAudioMessageAboutDelete, IAudioMessageAboutEnterWorkerId, IStringMessage, ISettingSoundParameters,
                                              ISettingLanguageParameters, ISetOfParameters, ISetOfParameters2, ISetOfParameters3,
                                              IAudioMessageAboutThereIsNoWorkerWithSuchIdentificator, ISetOfParameters4,
                                              IAudioMessageAboutCheckWorkerIdentificator
    {
        private SoundPlayer player;

        private bool soundState;

        private string langaugeState;
        public MainWindow()
        {
            InitializeComponent();

            RemoveClient client = new RemoveClient("NetTcpBinding_IRemove");

            removeWorkerGrid.ItemsSource = client.GetAllWorker1().DefaultView;

            client.Close();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textBoxId.Text == "")
                {
                    SetOfParameters2();
                }
                else
                {
                    RemoveClient client = new RemoveClient("NetTcpBinding_IRemove");

                    int count = client.AvailabilityOfASpecificWorkerId(Convert.ToInt32(textBoxId.Text));

                    if (count > 0)
                    {
                        int id = Convert.ToInt32(textBoxId.Text);

                        client.Remove(id);

                        removeWorkerGrid.ItemsSource = client.GetAllWorker1().DefaultView;

                        textBoxId.Text = "";

                        SetOfParameters();
                    }
                    else
                    {
                        SetOfParameters3();
                    }
                }
            }
            catch
            {
                SetOfParameters4();
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

        private void The_Whole_List_Click(object sender, RoutedEventArgs e)
        {
            RemoveClient client = new RemoveClient("NetTcpBinding_IRemove");

            removeWorkerGrid.ItemsSource = client.GetAllWorker1().DefaultView;

            client.Close();
        }

        private void Service_M_Click(object sender, RoutedEventArgs e)
        {
            RemoveClient client = new RemoveClient("NetTcpBinding_IRemove");

            removeWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment1(service_M.Text).DefaultView;

            client.Close();
        }

        private void Service_H_Click(object sender, RoutedEventArgs e)
        {
            RemoveClient client = new RemoveClient("NetTcpBinding_IRemove");

            removeWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment1(service_H.Text).DefaultView;

            client.Close();
        }

        private void Traffic_Service_Click(object sender, RoutedEventArgs e)
        {
            RemoveClient client = new RemoveClient("NetTcpBinding_IRemove");

            removeWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment1(traffic_Service.Text).DefaultView;

            client.Close();
        }

        private void Eletro_Mechanical_Service_Click(object sender, RoutedEventArgs e)
        {
            RemoveClient client = new RemoveClient("NetTcpBinding_IRemove");

            removeWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment1(electro_Mechanical_Service.Text).DefaultView;

            client.Close();
        }

        private void Security_Service_Click(object sender, RoutedEventArgs e)
        {
            RemoveClient client = new RemoveClient("NetTcpBinding_IRemove");

            removeWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment1(security_Service.Text).DefaultView;

            client.Close();
        }

        private void Economic_Department_Click(object sender, RoutedEventArgs e)
        {
            RemoveClient client = new RemoveClient("NetTcpBinding_IRemove");

            removeWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment1(economic_Department.Text).DefaultView;

            client.Close();
        }

        private void Computer_Department_Click(object sender, RoutedEventArgs e)
        {
            RemoveClient client = new RemoveClient("NetTcpBinding_IRemove");

            removeWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment1(computer_Department.Text).DefaultView;

            client.Close();
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

        public void SetOfParameters4()
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
