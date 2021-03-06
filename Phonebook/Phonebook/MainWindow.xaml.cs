﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Phonebook.BusinessLogic;
using Phonebook.CollectionModels;
using Phonebook.EditWindows;
using Phonebook.Models;
using ListItem = Phonebook.CollectionModels.ListItem;

namespace Phonebook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly int accessLevel;
        readonly string userName = Environment.UserDomainName + "/" +Environment.UserName;

        private CollectionPersonnel _collectionPersonnel;
        private CollectionJobs _collectionJobs;
        private CollectionDepts _collectionDepts;
        private CollectionEnterprises _collectionEnterprises;
        private CollectionCurators _collectionCurators;

        private const string FIOText = "ФИО";
        private const string JobText = "Должность";
        private const string CuratorText = "---Не выбрано---";
        private const string DeptText = "Управление/Отдел";
        private const string EnterpriseText = "Предприятие";
        private const string PhoneText = "Тел : ___-__-__";
        private readonly Brush emptyBrush = new SolidColorBrush(Colors.Gray);
        private readonly Brush filledBrush = new SolidColorBrush(Colors.Black);

        public MainWindow()
        {
            InitializeComponent();

            buttonUpdate.MouseLeave += MouseEvents.MouseLeave;
            buttonUpdate.MouseMove += MouseEvents.MouseMove;
            buttonClear.MouseLeave += MouseEvents.MouseLeave;
            buttonClear.MouseMove += MouseEvents.MouseMove;
            buttonFind.MouseLeave += MouseEvents.MouseLeave;
            buttonFind.MouseMove += MouseEvents.MouseMove;
            buttonExport.MouseLeave += MouseEvents.MouseLeave;
            buttonExport.MouseMove += MouseEvents.MouseMove;

            UpdateAllData();

            Title += " (Пользователь: " + userName + ")";
            accessLevel = Authentication.GetAccessLevel(userName);
            //получаем уровень доступа
            if (accessLevel != 0)
            {
                menuEdit.Visibility = Visibility.Hidden;
                buttonUpdate.Visibility = Visibility.Hidden;
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

        private void comboBoxDept_GotFocus(object sender, RoutedEventArgs e)
        {
            if (comboBoxDept.Text.Equals(DeptText))
            {
                comboBoxDept.Text = "";
                comboBoxDept.Foreground = filledBrush;
            }
        }

        private void comboBoxDept_LostFocus(object sender, RoutedEventArgs e)
        {
            comboBoxDept.Text = comboBoxDept.Text.Trim();
            if (comboBoxDept.Text.Equals(string.Empty))
            {
                comboBoxDept.Text = DeptText;
                comboBoxDept.Foreground = emptyBrush;
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
            string enterpriseName = comboBoxEnterprise.Text.Trim();
            comboBoxEnterprise.Text = enterpriseName;
            if (enterpriseName.Equals(string.Empty))
            {
                comboBoxEnterprise.Text = EnterpriseText;
                comboBoxEnterprise.Foreground = emptyBrush;
            }
        }

        /// <summary>
        /// Заполняет comboBoxJob. 
        /// </summary>
        private void FillComboBoxJob()
        {
            comboBoxJob.ItemsSource = _collectionJobs.Jobs.OrderBy(job => job.Name).Select(job => job.Name);
        }

        /// <summary>
        /// Заполняет comboBoxDept. 
        /// </summary>
        /// <param name="enterpriseId">ID родительского предприятия.</param>
        private void FillComboBoxDept(int enterpriseId)
        {
            comboBoxDept.ItemsSource =
                _collectionDepts.GetDeptsByEnterprise(enterpriseId)
                    .Where(dept => !dept.Name.Equals("Отдел"))
                    .OrderBy(dept => dept.Name)
                    .Select(dept => dept.Name);
            comboBoxDept.IsEnabled = comboBoxDept.Items.Count != 0;
        }

        /// <summary>
        /// Заполняет comboBoxEnterprise.
        /// </summary>
        private void FillComboBoxEnterpise(List<Enterprise> enterprisesList)
        {
            comboBoxEnterprise.ItemsSource = enterprisesList.Select(enterprise => enterprise.Name);
        }

        /// <summary>
        /// Заполняет сomboBoxCurator.
        /// </summary>
        private void FillComboBoxCurator()
        {
            var items = _collectionCurators.Curators.Select(curator => curator.FIO).ToList();
            items.Insert(0,CuratorText);
            comboBoxCurator.ItemsSource = items;
            comboBoxCurator.SelectedIndex = 0;
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
            comboBoxJob.ItemsSource = _collectionJobs.FindJobsForMask(comboBoxJob.Text.ToLower().Trim()).Select(job => job.Name);
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
            if (comboBoxEnterprise.SelectedValue != null && e.Key ==Key.Back)
            {
                comboBoxEnterprise.SelectedItem = null;
            }
            if (e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Enter) return;
            comboBoxEnterprise.ItemsSource =
                _collectionEnterprises.FindEnterprisesForMask(comboBoxEnterprise.Text.ToLower().Trim())
                    .Select(enterprise => enterprise.Name);
        }

        private void listViewResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViewResult.SelectedItem!=null)
            {
                Person person = _collectionPersonnel.FindPersonForId(((ListItem) listViewResult.SelectedItem).Id);
                PersonInfo q = new PersonInfo(accessLevel, person, _collectionEnterprises,_collectionDepts,_collectionJobs);
                q.Show();
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            textBoxFIO.Text = FIOText;
            textBoxFIO.Foreground = emptyBrush;
            comboBoxJob.Text = JobText;
            comboBoxJob.Foreground = emptyBrush;
            comboBoxEnterprise.Text = EnterpriseText;
            comboBoxEnterprise.Foreground = emptyBrush;
            comboBoxDept.Text = DeptText;
            comboBoxDept.Foreground = emptyBrush;
            maskedtextBoxPhone.Text = PhoneText;

            if (comboBoxCurator.SelectedIndex == 0)
            {
                Find();
            }
            else
            {
                comboBoxCurator.SelectedIndex = 0;
            }
            
        }

        private void Find()
        {
            string fio = textBoxFIO.Text.Equals(FIOText) ? "" : textBoxFIO.Text.ToLower();
            string job = comboBoxJob.Text.Equals(JobText) ? "" : comboBoxJob.Text.ToLower();
            string enterprise = comboBoxEnterprise.Text.Equals(EnterpriseText) ? "" : comboBoxEnterprise.Text.ToLower();
            string dept = comboBoxDept.Text.Equals(DeptText) ? "" : comboBoxDept.Text.ToLower();
            string number = maskedtextBoxPhone.Text.Equals(PhoneText) ? "" : maskedtextBoxPhone.Text.Remove(0,6);

            CollectionPersonnel personnel = new CollectionPersonnel(_collectionPersonnel.FindPersonnel(fio, job, enterprise, dept, number));

            listViewResult.ItemsSource = personnel.ConvertToListItems();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewResult.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("EnterpriseName");
            PropertyGroupDescription groupDescription2 = new PropertyGroupDescription("DeptName");
            view.GroupDescriptions.Add(groupDescription);
            view.GroupDescriptions.Add(groupDescription2);
        }

        private void UpdateAllData()
        {
            Thread thread = new Thread(new ThreadStart(delegate
            {
                _collectionPersonnel = new CollectionPersonnel();
                _collectionJobs = new CollectionJobs();
                _collectionDepts = new CollectionDepts();
                _collectionEnterprises = new CollectionEnterprises();
                _collectionCurators = new CollectionCurators();

                Dispatcher.Invoke(new Action(FillComboBoxJob));
                Dispatcher.Invoke(new Action(()=>FillComboBoxEnterpise(_collectionEnterprises.Enterprises)));
                Dispatcher.Invoke(new Action(FillComboBoxCurator));
                Dispatcher.Invoke(new Action(Clear));
            }));
            thread.Start();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void buttonFind_Click(object sender, RoutedEventArgs e)
        {
            Find();
        }

        private void MenuItemJob_Click(object sender, RoutedEventArgs e)
        {
            new JobsWindow().Show();
        }

        private void MenuItemEnterprise_Click(object sender, RoutedEventArgs e)
        {
            new EnterprisesWindow().Show();
        }

        private void MenuItemDept_Click(object sender, RoutedEventArgs e)
        {
            new DeptsWindow().Show();
        }

        private void MenuItemCurator_Click(object sender, RoutedEventArgs e)
        {
            new CuratorsWindow().Show();
        }

        private void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            new PersonInfo(0, new Person(),_collectionEnterprises,_collectionDepts,_collectionJobs).Show();
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllData();
        }

        private void comboBoxCurator_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (comboBoxCurator.SelectedIndex != 0)
            {
                string fioCurator = comboBoxCurator.SelectedValue.ToString();
                List<Dept> depts = _collectionDepts.GetDeptByCurator(fioCurator);

                CollectionPersonnel personnel = new CollectionPersonnel(_collectionPersonnel.FindPersonnel(depts));

                listViewResult.ItemsSource = personnel.ConvertToListItems();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewResult.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("EnterpriseName");
                PropertyGroupDescription groupDescription2 = new PropertyGroupDescription("DeptName");
                view.GroupDescriptions.Add(groupDescription);
                view.GroupDescriptions.Add(groupDescription2);

                comboBoxEnterprise.Visibility = Visibility.Hidden;
                comboBoxDept.Visibility = Visibility.Hidden;
            }
            else
            {
                comboBoxEnterprise.Visibility = Visibility.Visible;
                comboBoxDept.Visibility = Visibility.Visible;
                Find();
            }
        }

        private void comboBoxEnterprise_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FillComboBoxDept(_collectionEnterprises.GetIdByName(comboBoxEnterprise.SelectedValue.ToString()));
        }

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            WordHelper.CreateDocument(_collectionPersonnel.Personnel);
        }

        private void MenuItemUser_Click(object sender, RoutedEventArgs e)
        {
            new UsersWindow().Show();
        }
    }
}
