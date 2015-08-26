using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Phonebook.CollectionModels;
using Phonebook.Helpers;
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

        private CollectionJobs collectionJobs;
        private CollectionEnterprises collectionEnterprises;

        public PersonInfo(int accessLevel, Person person)
        {
            InitializeComponent();
            ImagePath = AppDomain.CurrentDomain.BaseDirectory + @"Image\";

            this.person = person;

            collectionJobs = new CollectionJobs();
            collectionEnterprises = new CollectionEnterprises();

            Height -= 50;
            switch (accessLevel)
            {
                case 0:
                    textBoxFIO.IsReadOnly = false;
                    comboBoxJob.IsEditable = false;
                    comboBoxJob.IsEnabled = true;
                    comboBoxEnterprise.IsEditable = false;
                    comboBoxEnterprise.IsEnabled = true;
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

            foreach (var job in collectionJobs.Jobs)
            {
                comboBoxJob.Items.Add(job.JobName);
            }
            foreach (var enterprise in collectionEnterprises.Enterprises)
            {
                comboBoxEnterprise.Items.Add(enterprise.Name);
            }

            textBoxFIO.Text = (person.Surname + " " + person.Name + " " + person.SecondName).Trim();
            comboBoxJob.Text = person.Job;
            comboBoxEnterprise.Text = person.Entretprise;
            textBoxAddress.Text = collectionEnterprises.GetAddressByName(person.Entretprise);
            textBoxLandline.Text = person.LandlineNumber.Replace('*', '\n');
            textBoxInternal.Text = person.InternalNumber.Replace('*', '\n');
            textBoxMobile.Text = person.CellNumber.Replace('*', '\n');
            textBoxMail.Text = person.Email.Replace('*', '\n');
            image1.Source = person.Photo.Equals("") ? new BitmapImage(new Uri(ImagePath + NullImage)) : new BitmapImage(new Uri(ImagePath + person.Photo));
        }

        private void comboBoxEnterprise_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            textBoxAddress.Text = collectionEnterprises.GetAddressByName(e.AddedItems[0].ToString());
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
            person.Job = comboBoxJob.Text;
            int idJob = collectionJobs.GetIdByName(person.Job);
            if (comboBoxEnterprise.SelectedIndex == -1)
            {
                MessageBox.Show("Не выбрано предприятие!");
                return;
            }
            person.Entretprise = comboBoxEnterprise.Text;
            int idEnterprise = collectionEnterprises.GetIdByName(person.Entretprise);
            person.CellNumber = textBoxMobile.Text.Replace('\n', '*');
            person.LandlineNumber = textBoxLandline.Text.Replace('\n', '*');
            person.InternalNumber = textBoxInternal.Text.Replace('\n', '*');
            person.Email = textBoxMail.Text.Replace('\n', '*');
            if (image1.Source != null)
            {
                person.Photo = ((BitmapImage) image1.Source).UriSource.AbsolutePath.Split('/').Last();
            }
            if (person.Id == 0)
            {
                AccessHelper.InsertPerson(person,idJob,idEnterprise);
                buttonDelete.Visibility = Visibility.Visible;
            }
            else
            {
                AccessHelper.UpdatePerson(person, idJob, idEnterprise);
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
                image1.Source = new BitmapImage(new Uri(ImagePath + fileName));
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Внимание";
            string message = "Вы действительно хотите \nудалить сотрудника из базы?";
            MessageBoxButton buttons = MessageBoxButton.YesNo;

            var result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                AccessHelper.DeletePerson(person.Id);
                Thread.Sleep(200);
                Close();
            }
        }

    }
}
