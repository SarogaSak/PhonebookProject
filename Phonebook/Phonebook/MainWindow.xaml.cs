using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Phonebook.CollectionModels;
using Phonebook.Models;

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
            accessLevel = 0;
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
            comboBoxJob.ItemsSource = jobs.Select(job => job.Name);
        }

        /// <summary>
        /// Заполняет comboBoxEnterprise.
        /// </summary>
        /// <param name="enterprises">Коллекция значений для заполнения</param>
        private void FillComboBoxEnterpise(List<Enterprise> enterprises)
        {
            comboBoxEnterprise.ItemsSource = enterprises.Select(enterprise => enterprise.Name);
        }

        private void comboBoxJob_KeyDown(object sender, KeyEventArgs e)
        {
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
            comboBoxJob.ItemsSource = collectionJobs.FindJobsForMask(comboBoxJob.Text.ToLower().Trim()).Select(job => job.Name);
        }

        private void comboBoxEnterprise_KeyDown(object sender, KeyEventArgs e)
        {
            comboBoxEnterprise.IsDropDownOpen = true;
            switch (e.Key)
            {
                case Key.Down:
                    comboBoxEnterprise.SelectedIndex++;
                    break;
                case Key.Up:
                    if (comboBoxEnterprise.SelectedIndex != -1)
                    {
                        comboBoxEnterprise.SelectedIndex--;
                    }
                    break;
                case Key.Enter:
                    Find();
                    break;
            }
        }
        private void comboBoxEnterprise_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Enter) return;
            comboBoxEnterprise.ItemsSource =
                collectionEnterprises.FindEnterprisesForMask(comboBoxEnterprise.Text.ToLower().Trim())
                    .Select(enterprise => enterprise.Name);
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
                    person.JobName,
                    person.DeptName,
                    person.LandlineNumbers.Replace('*', '\n')
                    );
                items.Add(newItem);
            }
            listViewResult.ItemsSource = items;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewResult.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Enterprise");
            PropertyGroupDescription groupDescription2 = new PropertyGroupDescription("Job");
            view.GroupDescriptions.Add(groupDescription);
            view.GroupDescriptions.Add(groupDescription2);
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
