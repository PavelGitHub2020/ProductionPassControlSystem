using System;
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
using AddWorker.AddWorkerService;

namespace Menu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Close_Window_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Information_About_Shifts_Click(object sender, RoutedEventArgs e)
        {
            InformationAboutShifts.MainWindow informationAboutShifts = new InformationAboutShifts.MainWindow();
            informationAboutShifts.ShowDialog();
        }

        private void Add_Worker_Click(object sender, RoutedEventArgs e)
        {
            AddWorker.MainWindow addWorker = new AddWorker.MainWindow();
            addWorker.ShowDialog();
        }

        private void Find_Worker_Click(object sender, RoutedEventArgs e)
        {
            FindWorker.MainWindow findWorker = new FindWorker.MainWindow();
            findWorker.ShowDialog();
        }

        private void Get_All_Worker_Click(object sender, RoutedEventArgs e)
        {
            GetAll.MainWindow getAll = new GetAll.MainWindow();
            getAll.ShowDialog();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Remove.MainWindow remove = new Remove.MainWindow();
            remove.ShowDialog();
        }

        private void Changing_Worker_Information_Click(object sender, RoutedEventArgs e)
        {
            ChangingWorkerInformation.MainWindow changingWorkerInformation = new ChangingWorkerInformation.MainWindow();
            changingWorkerInformation.ShowDialog();
        }

        private void Change_The_Work_Shedule_Click(object sender, RoutedEventArgs e)
        {
            ChangeTheWorkShedule.MainWindow changeTheWorkShedule = new ChangeTheWorkShedule.MainWindow();
            changeTheWorkShedule.ShowDialog();
        }

        private void Passage_Control_Click(object sender, RoutedEventArgs e)
        {
            PassageControll.MainWindow passageControll = new PassageControll.MainWindow();
            passageControll.ShowDialog();
        }

        private void Information_About_Use_The_Pass_Click(object sender, RoutedEventArgs e)
        {
            InformationAboutUseThePass.MainWindow informationAboutUseThePass = new InformationAboutUseThePass.MainWindow();
            informationAboutUseThePass.ShowDialog();
        }
    }
}
