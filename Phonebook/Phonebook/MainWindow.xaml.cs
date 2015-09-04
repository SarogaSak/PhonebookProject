using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Phonebook.CollectionModels;
using Phonebook.Helpers;
using Phonebook.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace Phonebook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int accessLevel;

        private CollectionPersonnel collectionPersonnel;
        private CollectionJobs collectionJobs;
        private CollectionEnterprises collectionEnterprises;

        private const string FIOText = "ФИО";
        private const string JobText = "Должность";
        private const string EnterpriseText = "Предприятие";
        private const string PhoneText = "Тел : ___-__-__";
        private readonly Brush emptyBrush = new SolidColorBrush(Colors.Gray);
        private readonly Brush filledBrush = new SolidColorBrush(Colors.Black);

        public MainWindow()
        {
            InitializeComponent();

            accessLevel = AccessHelper.GetAccessLevel();
            //получаем уровень доступа
            if (accessLevel != 0)
            {
                menuEdit.Visibility = Visibility.Hidden;
            }
        }

        private void textBoxFIO_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxFIO.Text.Equals(FIOText))
            {
                textBoxFIO.Clear();
                textBoxFIO.Foreground = filledBrush;
            }
        }

        private void textBoxFIO_LostFocus(object sender, RoutedEventArgs e)
        {
            textBoxFIO.Text = textBoxFIO.Text.Trim();
            if (textBoxFIO.Text.Equals(string.Empty))
            {
                textBoxFIO.Text = FIOText;
                textBoxFIO.Foreground = emptyBrush;
            }
        }

        private void comboBoxJob_GotFocus(object sender, RoutedEventArgs e)
        {
            if (comboBoxJob.Text.Equals(JobText))
            {
                comboBoxJob.Text = "";
                comboBoxJob.Foreground = filledBrush;
            }
        }

        private void comboBoxJob_LostFocus(object sender, RoutedEventArgs e)
        {
            comboBoxJob.Text = comboBoxJob.Text.Trim();
            if (comboBoxJob.Text.Equals(string.Empty))
            {
                comboBoxJob.Text = JobText;
                
                comboBoxJob.Foreground = emptyBrush;
            }
        }

        private void comboBoxEnterprise_GotFocus(object sender, RoutedEventArgs e)
        {
            if (comboBoxEnterprise.Text.Equals(EnterpriseText))
            {
                comboBoxEnterprise.Text = "";
                comboBoxEnterprise.Foreground = filledBrush;
            }
        }

        private void comboBoxEnterprise_LostFocus(object sender, RoutedEventArgs e)
        {
            comboBoxEnterprise.Text = comboBoxEnterprise.Text.Trim();
            if (comboBoxEnterprise.Text.Equals(string.Empty))
            {
                comboBoxEnterprise.Text = EnterpriseText;
                comboBoxEnterprise.Foreground = emptyBrush;
            }
        }

        /// <summary>
        /// Заполняет comboBoxJob. 
        /// </summary>
        /// <param name="jobs">Коллекция значений для заполнения</param>
        private void FillComboBoxJob(List<Job> jobs)
        {
            comboBoxJob.Items.Clear();
            foreach (Job job in jobs)
            {
                comboBoxJob.Items.Add(job.JobName);
            }
        }

        /// <summary>
        /// Заполняет comboBoxEnterprise.
        /// </summary>
        /// <param name="enterprises">Коллекция значений для заполнения</param>
        private void FillComboBoxEnterpise(List<Enterprise> enterprises)
        {
            comboBoxEnterprise.Items.Clear();
            foreach (var enterprise in enterprises)
            {
                comboBoxEnterprise.Items.Add(enterprise.Name);
            }
        }

        private void comboBoxJob_KeyDown(object sender, KeyEventArgs e)
        {
            comboBoxJob.Items.Clear();
            comboBoxJob.IsDropDownOpen = true;
            switch (e.Key)
            {
                case Key.Down:
                    comboBoxJob.SelectedIndex++;
                    break;
                case Key.Up:
                    if (comboBoxJob.SelectedIndex != -1)
                    {
                        comboBoxJob.SelectedIndex--;
                    }
                    break;
                case Key.Enter:
                    Find();
                    break;
            }
        }

        private void comboBoxJob_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key ==Key.Up || e.Key==Key.Enter) return;
            FillComboBoxJob(collectionJobs.FindJobsForMask(comboBoxJob.Text.ToLower().Trim()));
        }

        private void comboBoxEnterprise_KeyDown(object sender, KeyEventArgs e)
        {
            comboBoxEnterprise.Items.Clear();
            comboBoxEnterprise.IsDropDownOpen = true;
            if (e.Key == Key.Down)
            {
                comboBoxEnterprise.SelectedIndex++;
            }
            if (e.Key == Key.Up)
            {
                if (comboBoxEnterprise.SelectedIndex != -1)
                {
                    comboBoxEnterprise.SelectedIndex--;
                }
            }
        }
        private void comboBoxEnterprise_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key == Key.Up) return;
            FillComboBoxEnterpise(collectionEnterprises.FindEnterprisesForMask(comboBoxEnterprise.Text.ToLower().Trim()));
        }

        private void listViewResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViewResult.SelectedItem!=null)
            {
                PersonInfo q = new PersonInfo(accessLevel, collectionPersonnel.FindPersonForId(((ListItem)listViewResult.SelectedItem).Id));
                q.Show();
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxFIO.Text = FIOText;
            textBoxFIO.Foreground = emptyBrush;
            comboBoxJob.Text = JobText;
            comboBoxJob.Foreground = emptyBrush;
            comboBoxEnterprise.Text = EnterpriseText;
            comboBoxEnterprise.Foreground = emptyBrush;
            maskedtextBoxPhone.Text = PhoneText;
            Find();
        }

        private void Find()
        {
            //listViewResult.Items.Clear();

            string fio = textBoxFIO.Text.Equals(FIOText) ? "" : textBoxFIO.Text.ToLower();
            string job = comboBoxJob.Text.Equals(JobText) ? "" : comboBoxJob.Text.ToLower();
            string enterprise = comboBoxEnterprise.Text.Equals(EnterpriseText) ? "" : comboBoxEnterprise.Text.ToLower();
            string number = maskedtextBoxPhone.Text.Equals(PhoneText) ? "" : maskedtextBoxPhone.Text.Remove(0,6);

            List<Person> personnel = collectionPersonnel.FindPersonnel(fio, job, enterprise, number);
            List<ListItem> items = new List<ListItem>();
            foreach (var person in personnel)
            {
                ListItem newItem = new ListItem(
                    person.Id,
                    person.Surname + " " + person.Name + " " + person.SecondName,
                    person.Job,
                    person.Entretprise,
                    person.LandlineNumber.Replace('*', '\n')
                    );
                items.Add(newItem);
            }
            listViewResult.ItemsSource = items;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewResult.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Enterprise");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void buttonFind_Click(object sender, RoutedEventArgs e)
        {
            Find();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(delegate
            {
                collectionPersonnel = new CollectionPersonnel();
                collectionJobs = new CollectionJobs();
                collectionEnterprises = new CollectionEnterprises();

                Dispatcher.Invoke(new Action(()=>FillComboBoxJob(collectionJobs.Jobs)));
                Dispatcher.Invoke(new Action(()=>FillComboBoxEnterpise(collectionEnterprises.Enterprises)));
                Dispatcher.Invoke(new Action(Find));
            }));
            thread.Start();
        }

        private void MenuItemJob_Click(object sender, RoutedEventArgs e)
        {
            new HandbookWindow(0).Show();
        }

        private void MenuItemEnterprise_Click(object sender, RoutedEventArgs e)
        {
            new HandbookWindow(1).Show();
        }

        private void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            new PersonInfo(0, new Person()).Show();
        }

        private void MenuItemAddList_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application(); //открыть эксель
            Excel.Workbook xlWorkBook = excelApp.Workbooks.Open("c:\\1.xls");
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Item[1];
            for (int i = 1; i < xlWorkSheet.Rows.Count; i++)
            {
                MessageBox.Show(xlWorkSheet.Range["B"+i, Type.Missing].Value2.ToString());
            }
            
        }
    }

    public class ListItem
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Job { get; set; }
        public string Enterprise { get; set; }
        public string Number { get; set; }

        public ListItem(int id,string fio, string job, string enterprise, string number)
        {
            Id = id;
            FIO = fio;
            Job = job;
            Enterprise = enterprise;
            Number = number;
        }
    }
}
