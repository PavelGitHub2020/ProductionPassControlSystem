using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using Microsoft.Win32;
using PassageControll.PassageControlService;
using ProductionPassControlSystem.Entity;




namespace PassageControll
{
    /// <summary>
    /// Interaction logic for InfoAboutWorker.xaml
    /// </summary>
    public partial class InfoAboutWorker : Window
    {
        private OpenFileDialog op;
        private DataTable table;

        private Address address;
        private Worker worker;
        private Pass pass;
        private Photo photos;

        private int workerId;
        private int addressId;
        private int passId;
        private int photosId;
        private int departmentId;

        private SoundPlayer player;

        private bool soundState;
        private string langaugeState;

        private int counter = 0;
        public InfoAboutWorker(int id)
        {
            try
            {
                InitializeComponent();

                PassageControlClient client1 = new PassageControlClient("NetTcpBinding_IPassageControl");

                table = client1.FindWorkerById_PS(id);

                client1.Close();

                GetInformationFromTable(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetInformationFromTable(DataTable table)
        {
            try
            {
                foreach (DataRow dataTable in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        workerId = (int)dataTable[0];
                        surnameUp.Text = dataTable[1].ToString();
                        surnameUp.IsEnabled = false;
                        nameUp.Text = dataTable[2].ToString();
                        nameUp.IsEnabled = false;
                        patronymicUp.Text = dataTable[3].ToString();
                        patronymicUp.IsEnabled = false;
                        dateOfBirthUp.Text = dataTable[4].ToString();
                        dateOfBirthUp.IsEnabled = false;
                        genderUp.Text = dataTable[5].ToString();
                        genderUp.IsEnabled = false;
                        phonenumberUp.Text = dataTable[6].ToString();
                        phonenumberUp.IsEnabled = false;
                        professionUp.Text = dataTable[8].ToString();
                        professionUp.IsEnabled = false;
                        dateOfStartToWorkUp.Text = dataTable[9].ToString();
                        dateOfStartToWorkUp.IsEnabled = false;

                        if (dataTable[10].ToString() == "1")
                        {
                            numberOfShiftUp.Text = "First shift";
                        }
                        if (dataTable[10].ToString() == "2")
                        {
                            numberOfShiftUp.Text = "Second shift";
                        }
                        if (dataTable[10].ToString() == "3")
                        {
                            numberOfShiftUp.Text = "Third shift";
                        }
                        if (dataTable[10].ToString() == "4")
                        {
                            numberOfShiftUp.Text = "Fourth shift";
                        }

                        numberOfShiftUp.IsEnabled = false;

                        addressId = (int)dataTable[11];
                        nameOfTheCityUp.Text = dataTable[12].ToString();
                        nameOfTheCityUp.IsEnabled = false;
                        nameOfTheStreetUp.Text = dataTable[13].ToString();
                        nameOfTheStreetUp.IsEnabled = false;
                        houseNumberUp.Text = dataTable[14].ToString();
                        houseNumberUp.IsEnabled = false;

                        passId = (int)dataTable[16];
                        passNumberUp.Text = dataTable[17].ToString();
                        passNumberUp.IsEnabled = false;

                        if ((bool)dataTable[18] == true)
                        {
                            conditionUp.Text = "Unlocked";
                        }
                        else
                        {
                            conditionUp.Text = "Locked";
                        }

                        conditionUp.IsEnabled = false;

                        departmentId = (int)dataTable[20];
                        nameOfDepartmentUp.Text = dataTable[21].ToString();
                        nameOfDepartmentUp.IsEnabled = false;

                        photosId = (int)dataTable[22];
                        image.Source = new BitmapImage(new Uri(dataTable[23].ToString()));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
