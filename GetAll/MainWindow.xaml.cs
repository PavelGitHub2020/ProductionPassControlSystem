﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using ProductionPassControlSystem.Entity;
using GetAll.GetAllService;
using System.Media;

namespace GetAll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IStringMessage, IAudioMessageAboutWhatManagedToFind, ISettingSoundParameters,
                                              ISettingLanguageParameters, ISetOfParameters
    {
        private SoundPlayer player;

        private bool soundState;
        private string langaugeState;

        private int counter = 0;
        public MainWindow()
        {
            InitializeComponent();

            SetOfParameters();

            Get_All_Worker();
        }

        private void Get_All_Worker()
        {
            try
            {
                getAllWorkerGrid.Columns.Clear();

                GetAllClient client = new GetAllClient("NetTcpBinding_IGetAll");

                int holeList_ = client.GetNumberOfWorkers();
                int serviceM = client.FindTheNumberOfEmployeesInDepartment("Service M");
                int serviceH = client.FindTheNumberOfEmployeesInDepartment("Service H");
                int trafficService = client.FindTheNumberOfEmployeesInDepartment("Traffic service");
                int electroMechanicalService = client.FindTheNumberOfEmployeesInDepartment("Electro-mechanical service");
                int securityService = client.FindTheNumberOfEmployeesInDepartment("Security service");
                int economicDepartment = client.FindTheNumberOfEmployeesInDepartment("Economic department");
                int computerDepartment = client.FindTheNumberOfEmployeesInDepartment("Computer department");

                getAllWorkerGrid.ItemsSource = client.GetAllWorker().DefaultView;

                client.Close();

                wholeList.Text = holeList_.ToString();
                numOfServiceM.Text = serviceM.ToString();
                numOfServiceH.Text = serviceH.ToString();
                numOfTrafficService.Text = trafficService.ToString();
                numOfElectroMechanicalService.Text = electroMechanicalService.ToString();
                numOfSecurityService.Text = securityService.ToString();
                numOfEconomicDepartment.Text = economicDepartment.ToString();
                numOfComputerDepartment.Text = computerDepartment.ToString();
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
                SoundMessageAboutWhatManagedToFind(player, soundState, langaugeState);

                if (langaugeState == "eng" && soundState == false)
                {
                    StringMessageInEnglish("Here is what managed to find!");
                }
                else if (langaugeState == "ru" && soundState == false)
                {
                    StringMessageInRussian("Вот, что удалось найти!");
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

        public void SoundMessageAboutWhatManagedToFind(SoundPlayer player, bool soundState, string languageState)
        {
            if (soundState == true)
            {
                try
                {
                    if (langaugeState == "eng")
                    {
                        player = new SoundPlayer(Properties.Resources.here_Is_what_managed_to_find);
                        player.Play();
                        player.Dispose();
                    }
                    else
                    {
                        player = new SoundPlayer(Properties.Resources.вот_что_удалось_найти);
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

        private void Turning_The_Language_RU_ENG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (language.Content == FindResource("eng"))
                {
                    language.Content = FindResource("ru");
                    language.ToolTip = "Russian for sound";

                    if (counter == 0)
                    {
                        SetOfParameters();
                        counter++;
                    }
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
            try
            {
                GetAllClient client = new GetAllClient("NetTcpBinding_IGetAll");

                getAllWorkerGrid.ItemsSource = client.GetAllWorker().DefaultView;

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Service_M_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetAllClient client = new GetAllClient("NetTcpBinding_IGetAll");

                getAllWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment(service_M.Text).DefaultView;

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Service_H_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetAllClient client = new GetAllClient("NetTcpBinding_IGetAll");

                getAllWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment(service_H.Text).DefaultView;

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Traffic_Service_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetAllClient client = new GetAllClient("NetTcpBinding_IGetAll");

                getAllWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment(traffic_Service.Text).DefaultView;

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Eletro_Mechanical_Service_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetAllClient client = new GetAllClient("NetTcpBinding_IGetAll");

                getAllWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment(electro_Mechanical_Service.Text).DefaultView;

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Security_Service_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetAllClient client = new GetAllClient("NetTcpBinding_IGetAll");

                getAllWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment(security_Service.Text).DefaultView;

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Economic_Department_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetAllClient client = new GetAllClient("NetTcpBinding_IGetAll");

                getAllWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment(economic_Department.Text).DefaultView;

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Computer_Department_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetAllClient client = new GetAllClient("NetTcpBinding_IGetAll");

                getAllWorkerGrid.ItemsSource = client.GetAllWorkerByDepartment(computer_Department.Text).DefaultView;

                client.Close();
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
    }
}
