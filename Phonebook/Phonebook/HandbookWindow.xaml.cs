using System.Collections.Generic;
using System.Windows;
using Phonebook.CollectionModels;
using Phonebook.Models;

namespace Phonebook
{
    /// <summary>
    /// Логика взаимодействия для HandbookWindow.xaml
    /// </summary>
    public partial class HandbookWindow : Window
    {
        private int key;
        private CollectionJobs collectionJobs;
        private CollectionEnterprises collectionEnterprises;

        public HandbookWindow(int key)
        {
            InitializeComponent();
            this.key = key;
            switch (key)
            {
                case 0:
                    dataGridJob.Visibility = Visibility.Visible;
                    collectionJobs = new CollectionJobs();
                    dataGridJob.ItemsSource = collectionJobs.SortedList();
                    break;
                case 1:
                    dataGridEntrprize.Visibility = Visibility.Visible;
                    collectionEnterprises = new CollectionEnterprises();
                    dataGridEntrprize.ItemsSource = collectionEnterprises.SortedList();
                    break;
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            switch (key)
            {
                case 0:
                    collectionJobs.Jobs = (List<Job>) dataGridJob.Items.SourceCollection;
                    collectionJobs.Update();
                    break;
                case 1:
                    collectionEnterprises.Enterprises = (List<Enterprise>) dataGridEntrprize.Items.SourceCollection;
                    collectionEnterprises.Update();
                    break;
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            switch (key)
            {
                case 0:
                    collectionJobs.InsertNew();
                    dataGridJob.Items.Refresh();
                    break;
                case 1:
                    collectionEnterprises.InsertNew();
                    dataGridEntrprize.Items.Refresh();
                    break;
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Внимание";
            string message = "Вы действительно хотите удалить?";
            MessageBoxButton buttons = MessageBoxButton.YesNo;

            if (dataGridJob.SelectedIndex != -1 || dataGridEntrprize.SelectedIndex != -1)
            {
                var result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    switch (key)
                    {
                        case 0:
                            collectionJobs.DeleteById(((Job) dataGridJob.SelectedItem).Id);
                            dataGridJob.Items.Refresh();
                            break;
                        case 1:
                            collectionEnterprises.DeleteById(((Enterprise) dataGridEntrprize.SelectedItem).Id);
                            dataGridEntrprize.Items.Refresh();
                            break;
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            switch (key)
            {
                case 0:
                    Title = "Должности";
                    break;
                case 1:
                    Title = "Предприятия";
                    break;
            }
        }
    }
}
