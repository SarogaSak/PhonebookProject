using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Phonebook.CollectionModels;
using Phonebook.Models;
using System.Windows.Media;

namespace Phonebook
{
    /// <summary>
    /// Логика взаимодействия для EnterprisesWindow.xaml
    /// </summary>
    public partial class EnterprisesWindow : Window
    {
        private CollectionEnterprises collectionEnterprises;

        public EnterprisesWindow()
        {
            InitializeComponent();

            buttonDelete.MouseLeave += MouseEvents.MouseLeave;
            buttonDelete.MouseMove += MouseEvents.MouseMove;
            buttonSaveAll.MouseLeave += MouseEvents.MouseLeave;
            buttonSaveAll.MouseMove += MouseEvents.MouseMove;

            collectionEnterprises = new CollectionEnterprises();
            dataGridEnterprises.ItemsSource = collectionEnterprises.Enterprises;
        }

        private void buttonSaveAll_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(delegate
            {
                collectionEnterprises.Enterprises = (List<Enterprise>)dataGridEnterprises.Items.SourceCollection;
                var temp = new CollectionEnterprises();
                collectionEnterprises.Update(temp);
                Thread.Sleep(5000);
                collectionEnterprises = new CollectionEnterprises();
                Dispatcher.Invoke(
                    new Action(delegate { dataGridEnterprises.ItemsSource = collectionEnterprises.SortedList(); }));
            }));
            thread.Start();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Внимание";
            string message = "Вы действительно хотите удалить?";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            if (dataGridEnterprises.SelectedIndex != -1)
            {
                var result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var enterprises = dataGridEnterprises.SelectedItems.Cast<Enterprise>();
                    foreach (var enterprise in enterprises)
                    {
                        collectionEnterprises.DeleteById(enterprise.Id);
                    }

                }
            }
            dataGridEnterprises.ItemsSource = collectionEnterprises.SortedList();
        }
    }
}
