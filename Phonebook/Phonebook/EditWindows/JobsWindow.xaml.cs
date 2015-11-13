using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using Phonebook.CollectionModels;
using Phonebook.Models;

namespace Phonebook
{
    /// <summary>
    /// Логика взаимодействия для JobsWindow.xaml
    /// </summary>
    public partial class JobsWindow : Window
    {
        private CollectionJobs collectionJobs;

        public JobsWindow()
        {
            InitializeComponent();

            collectionJobs = new CollectionJobs();
            dataGridJob.ItemsSource = collectionJobs.SortedList();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(delegate
            {
                collectionJobs.Jobs = (List<Job>)dataGridJob.Items.SourceCollection;
                var temp = new CollectionJobs();
                collectionJobs.Update(temp);
                Thread.Sleep(5000);
                collectionJobs = new CollectionJobs();
                Dispatcher.Invoke(new Action(delegate { dataGridJob.ItemsSource = collectionJobs.SortedList(); }));
            }));
            thread.Start();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Внимание";
            string message = "Вы действительно хотите удалить?";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            if (dataGridJob.SelectedIndex != -1)
            {
                var result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var jobs = dataGridJob.SelectedItems.Cast<Job>();
                    foreach (var job in jobs)
                    {
                        collectionJobs.DeleteById(job.Id);
                    }
                    
                }
            }
            dataGridJob.ItemsSource = collectionJobs.SortedList();
        }
    }
}
