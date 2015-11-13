using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Phonebook.BusinessLogic;
using Phonebook.CollectionModels;
using Phonebook.Models;

namespace Phonebook
{
    /// <summary>
    /// Логика взаимодействия для PersonInfo.xaml
    /// </summary>
    public partial class PersonInfo
    {
        private readonly string ImagePath;
        private const string NullImage = "NullImage.png";

        private Person person;
        private readonly int accessLevel;

        private readonly BLPerson blPerson = new BLPerson();

        private CollectionJobs collectionJobs;
        private CollectionEnterprises collectionEnterprises;
        private CollectionDepts collectionDepts;

        public PersonInfo(int accessLevel, Person person, CollectionEnterprises enterprises, CollectionDepts depts,CollectionJobs jobs)
        {
            InitializeComponent();
            ImagePath = AppDomain.CurrentDomain.BaseDirectory + @"Images\";

            this.person = person;
            this.accessLevel = accessLevel;

            collectionJobs = jobs;
            collectionEnterprises = enterprises;
            collectionDepts = depts;

            Height -= 50;
            switch (accessLevel)
            {
                case 0:
                    Activated += Window_Activated;

                    textBoxFIO.IsReadOnly = false;
                    comboBoxJob.IsEditable = false;
                    comboBoxJob.IsEnabled = true;
                    comboBoxEnterprise.IsEditable = false;
                    comboBoxEnterprise.IsEnabled = true;
                    comboBoxDept.IsEditable = false;
                    comboBoxDept.IsEnabled = true;
                    textBoxLandline.IsReadOnly = false;
                    textBoxMobile.IsReadOnly = false;
                    textBoxInternal.IsReadOnly = false;
                    textBoxMail.IsReadOnly = false;
                    image1.MouseDown += image1_MouseDown;
                    buttonSave.Visibility = Visibility.Visible;
                    if (person.Id != 0) buttonDelete.Visibility = Visibility.Visible;

                    Height += 50;
                    goto case 1;
                case 1:
                    groupBoxInternal.Visibility = Visibility.Visible;
                    goto case 2;
                case 2:
                    groupBoxMobile.Visibility = Visibility.Visible;
                    goto case 3;
                case 3:
                    
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (accessLevel == 0)
            {
                comboBoxJob.ItemsSource = collectionJobs.Jobs.OrderBy(job => job.Name).Select(job => job.Name);
                comboBoxEnterprise.ItemsSource = collectionEnterprises.Enterprises.OrderBy(enterprise => enterprise.Name).Select(enterprise => enterprise.Name);
                FillComboBoxDept(collectionEnterprises.GetIdByName(comboBoxEnterprise.Text));
            }
            FillData();
        }

        /// <summary>
        /// Заполняет comboBoxDept. 
        /// </summary>
        /// <param name="enterpriseId">ID родительского предприятия.</param>
        private void FillComboBoxDept(int enterpriseId)
        {
            comboBoxDept.ItemsSource =
                collectionDepts.GetDeptsByEnterprise(enterpriseId).OrderBy(dept => dept.Name).Select(dept => dept.Name);
        }

        private void FillData()
        {
            textBoxFIO.Text = (person.Surname + " " + person.Name + " " + person.SecondName).Trim();
            comboBoxJob.Text = person.JobName;
            comboBoxEnterprise.Text = person.EnterpriseName;
            comboBoxDept.Text = person.DeptName;
            if (!person.DeptName.Equals(""))
            {
                textBoxAddress.Text = collectionDepts.GetAddressByName(person.DeptName);
            }
            textBoxLandline.Text = person.LandlineNumbers.Replace('*', '\n');
            textBoxInternal.Text = person.InternalNumbers.Replace('*', '\n');
            textBoxMobile.Text = person.CellNumbers.Replace('*', '\n');
            textBoxMail.Text = person.Email.Replace('*', '\n');
            string imageName = person.Photo.Equals("") ? NullImage : person.Photo;
            LoadImage(imageName);
        }

        private void comboBoxEnterprise_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            textBoxAddress.Text = collectionEnterprises.GetAddressByName(e.AddedItems[0].ToString());
            FillComboBoxDept(collectionEnterprises.GetIdByName(comboBoxEnterprise.SelectedValue.ToString()));
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            string fiotext = textBoxFIO.Text.Trim();
            if (fiotext.Equals(""))
            {
                MessageBox.Show("Не заполнено поле ФИО!");
                return;
            }
            string[] fio = fiotext.Split(' ');
            if (fio.Count() < 3)
            {
                MessageBox.Show("Поле ФИО заполнено некорректно!");
                return;
            }
            person.Surname = fio[0];
            person.Name = fio[1];
            person.SecondName = fio[2];
            if (comboBoxJob.SelectedIndex == -1)
            {
                MessageBox.Show("Не выбрана должность!");
                return;
            }
            person.JobName = comboBoxJob.Text;
            person.IdJob = collectionJobs.GetIdByName(person.JobName);
            if (comboBoxEnterprise.SelectedIndex == -1)
            {
                MessageBox.Show("Не выбрано предприятие!");
                return;
            }
            person.EnterpriseName = comboBoxEnterprise.Text;
            person.DeptName = comboBoxDept.Text;
            person.IdDept = collectionDepts.GetIdByName(person.DeptName, person.EnterpriseName);
            person.CellNumbers = textBoxMobile.Text.Replace('\n', '*');
            person.LandlineNumbers = textBoxLandline.Text.Replace('\n', '*');
            person.InternalNumbers = textBoxInternal.Text.Replace('\n', '*');
            person.Email = textBoxMail.Text.Replace('\n', '*');
            if (image1.Source != null)
            {
                string oldImagePath = person.Photo;
                person.Photo = ((BitmapImage) image1.Source).UriSource.AbsolutePath.Split('/').Last();
                DeleteOldImage(oldImagePath);
            }
            
            if (person.Id == 0)
            {
                blPerson.InsertData(person);
                //buttonDelete.Visibility = Visibility.Visible;
                ClearControl();
            }
            else
            {
                blPerson.UpdateData(person);
            }
        }

        private void image1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png) | *.jpg; *.jpeg; *.jpe; *.png";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string fileName = Guid.NewGuid().ToString();
                File.Copy(dlg.FileName,ImagePath+fileName);
                LoadImage(fileName);              
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            const string caption = "Внимание";
            const string message = "Вы действительно хотите \nудалить сотрудника из базы?";
            MessageBoxButton buttons = MessageBoxButton.YesNo;

            var result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                blPerson.DeleteData(person.Id);
                Thread.Sleep(200);
                Close();
            }
        }

        private void ClearControl()
        {
            person = new Person();
            LoadImage(NullImage);
            textBoxFIO.Text = "";
            textBoxLandline.Text = "";
            textBoxInternal.Text = "";
            textBoxMobile.Text = "";
            textBoxMail.Text = "";
        }

        private void DeleteOldImage(string imagePath)
        {
            string imageName = imagePath.Split('/').Last();
            if (!imageName.Equals(NullImage)&&!imageName.Equals(""))
            {
                File.Delete(ImagePath+imageName);
            }
        }

        private void LoadImage(string imageName)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(ImagePath + imageName);
            image.EndInit();
            image1.Source = image;
        }

        private void buttonOpenJobsWindow_Click(object sender, RoutedEventArgs e)
        {
            new JobsWindow().Show();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            collectionJobs=new CollectionJobs();
            comboBoxJob.ItemsSource = collectionJobs.Jobs.OrderBy(job => job.Name).Select(job => job.Name);
            Topmost = true;
            Topmost = false;
            Focus();
        }
    }
}
