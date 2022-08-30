﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using ProductionPassControlSystem.BLL;
using ProductionPassControlSystem.Entity;
using FindWorker.FindWorkerService;
using System.Media;

namespace FindWorker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAudioMessageAboutNotAllParametersAreFilledIn, IStringMessage,
                                              ISettingSoundParameters, ISettingLanguageParameters, IAudioMessageAboutNothingWasFound,
                                              ISetOfParameters, ISetOfParameters2
    {
        private SoundPlayer player;

        private bool soundState;

        private string langaugeState;

        public MainWindow()
        {
            InitializeComponent();

            Get_Surname_DepartmentName_PassNumber();
        }

        private void Get_Surname_DepartmentName_PassNumber()
        {
            try
            {
                FindWorkerClient client = new FindWorkerClient("NetTcpBinding_IFindWorker");

                dataGrid.ItemsSource = client.GetTable().DefaultView;

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearParameters()
        {
            surname.Text = "";
            name.Text = "";
            patronymic.Text = "";
            dateOfBirth.Text = "";
            gender.Text = "";
            phoneNumber.Text = "";
            profession.Text = "";
            dateOfStartToWork.Text = "";
            numberOfShift.Text = "";
            nameOfTheCity.Text = "";
            nameOfTheStreet.Text = "";
            houseNumber.Text = "";
            nameOfTheCity.Text = "";
            nameOfTheStreet.Text = "";
            houseNumber.Text = "";
            image.Source = null;
            nameOfDepartment.Text = "";
            passNumber.Text = "";
            passCondition.Text = "";
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

                SoundMessageAboutNothingWasFound(player, soundState, langaugeState);

                if (langaugeState == "eng" && soundState == false)
                {
                    StringMessageInEnglish("Nothing was found, check the set parameters!");
                }
                else if (langaugeState == "ru" && soundState == false)
                {
                    StringMessageInRussian("Ничего не найдено, проверьте установленные параметры!");
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

        public void SoundMessageAboutNothingWasFound(SoundPlayer player, bool soundState, string langugeState)
        {
            if (soundState == true)
            {
                try
                {
                    if (langaugeState == "eng")
                    {
                        player = new SoundPlayer(Properties.Resources.nothing_was_found);
                        player.Play();
                        player.Dispose();
                    }
                    else
                    {
                        player = new SoundPlayer(Properties.Resources.ничего_не_найдено);
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

        private void Find_Worker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passNumberBox.Text == "" || departmentNameBox.Text == "")
                {
                    SetOfParameters();
                }
                else
                {
                    ClearParameters();

                    FindWorkerClient client = new FindWorkerClient("NetTcpBinding_IFindWorker");

                    client.FindByPassNumberAndDepartmentName(passNumberBox.Text, departmentNameBox.Text);

                    foreach (Worker worker in client.GetListWorker())
                    {
                        surname.Text = worker.Surname;
                        name.Text = worker.Name;
                        patronymic.Text = worker.Patronymic;
                        dateOfBirth.Text = worker.DateOfBirth;
                        gender.Text = worker.Gender.ToString();
                        phoneNumber.Text = worker.PhoneNumber;
                        profession.Text = worker.Profession;
                        dateOfStartToWork.Text = worker.DateOfStartToWork;
                        numberOfShift.Text = worker.NumberOfShift.ToString();
                    }

                    foreach (Address address in client.GetListAddress())
                    {
                        nameOfTheCity.Text = address.NameOfTheCity;
                        nameOfTheStreet.Text = address.NameOfTheStreet;
                        houseNumber.Text = address.HouseNumber;
                    }

                    foreach (Pass pass in client.GetListPass())
                    {
                        passNumber.Text = pass.Number;
                        passCondition.Text = pass.Condition.ToString();
                    }

                    List<string> depName = new List<string>();

                    foreach (var name in client.GetListDepName())
                    {
                        depName.Add(name);
                    }

                    nameOfDepartment.Text = depName[0];

                    foreach (Photo photo in client.GetListPhoto())
                    {
                        image.Source = new BitmapImage(new Uri(photo.Path));
                    }

                    client.Close();
                }
            }
            catch
            {
                SetOfParameters2();
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
    }
}
