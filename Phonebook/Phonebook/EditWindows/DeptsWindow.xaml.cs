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
    /// Логика взаимодействия для DeptsWindow.xaml
    /// </summary>
    public partial class DeptsWindow : Window
    {
        private CollectionDepts collectionDepts;

        public DeptsWindow()
        {
            InitializeComponent();

            collectionDepts = new CollectionDepts();
            dataGridDepts.ItemsSource = collectionDepts.GetDepts();
        }

        private void buttonSaveAll_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(delegate
            {
                collectionDepts.Depts = (List<Dept>)dataGridDepts.Items.SourceCollection;
                var temp = new CollectionDepts();
                collectionDepts.Update(temp);
                Thread.Sleep(5000);
                collectionDepts = new CollectionDepts();
                Dispatcher.Invoke(new Action(delegate { dataGridDepts.ItemsSource = collectionDepts.GetDepts(); }));
            }));
            thread.Start();
        }

        private void buttonDeleteDept_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Внимание";
            string message = "Вы действительно хотите удалить?";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            if (dataGridDepts.SelectedIndex != -1)
            {
                var result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var depts = dataGridDepts.SelectedItems.Cast<Dept>();
                    foreach (var dept in depts)
                    {
                        collectionDepts.DeleteById(dept.Id);
                    }

                }
            }
            dataGridDepts.ItemsSource = collectionDepts.GetDepts();
        }
    }
}
