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
    /// Логика взаимодействия для CuratorsWindow.xaml
    /// </summary>
    public partial class CuratorsWindow : Window
    {
        private CollectionCurators collectionCurators;

        public CuratorsWindow()
        {
            InitializeComponent();

            buttonDelete.MouseLeave += MouseEvents.MouseLeave;
            buttonDelete.MouseMove += MouseEvents.MouseMove;
            buttonSaveAll.MouseLeave += MouseEvents.MouseLeave;
            buttonSaveAll.MouseMove += MouseEvents.MouseMove;

            collectionCurators = new CollectionCurators();
            dataGridCurator.ItemsSource = collectionCurators.Curators;
        }

        private void buttonSaveAll_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(delegate
            {
                collectionCurators.Curators = (List<Curator>)dataGridCurator.Items.SourceCollection;
                var temp = new CollectionCurators();
                collectionCurators.Update(temp);
                Thread.Sleep(5000);
                collectionCurators = new CollectionCurators();
                Dispatcher.Invoke(new Action(delegate { dataGridCurator.ItemsSource = collectionCurators.SortedList(); }));
            }));
            thread.Start();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Внимание";
            string message = "Вы действительно хотите удалить?";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            if (dataGridCurator.SelectedIndex != -1)
            {
                var result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var curators = dataGridCurator.SelectedItems.Cast<Curator>();
                    foreach (var curator in curators)
                    {
                        collectionCurators.DeleteById(curator.Id);
                    }

                }
            }
            dataGridCurator.ItemsSource = collectionCurators.SortedList();
        }
    }
}
