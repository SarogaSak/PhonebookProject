using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using Phonebook.CollectionModels;
using Phonebook.Models;

namespace Phonebook.EditWindows
{
    /// <summary>
    /// Логика взаимодействия для UsersWindow.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        private CollectionUsers collectionUsers;

        public UsersWindow()
        {
            InitializeComponent();

            buttonDelete.MouseLeave += MouseEvents.MouseLeave;
            buttonDelete.MouseMove += MouseEvents.MouseMove;
            buttonSave.MouseLeave += MouseEvents.MouseLeave;
            buttonSave.MouseMove += MouseEvents.MouseMove;

            collectionUsers = new CollectionUsers();
            dataGridUsers.ItemsSource = collectionUsers.Users;
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Внимание";
            string message = "Вы действительно хотите удалить?";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            if (dataGridUsers.SelectedIndex != -1)
            {
                var result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var users = dataGridUsers.SelectedItems.Cast<User>();
                    foreach (var user in users)
                    {
                        collectionUsers.DeleteById(user.Id);
                    }

                }
            }
            dataGridUsers.ItemsSource = collectionUsers.Users;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(delegate
            {
                collectionUsers.Users = (List<User>)dataGridUsers.Items.SourceCollection;
                var temp = new CollectionUsers();
                collectionUsers.Update(temp);
                Thread.Sleep(5000);
                collectionUsers = new CollectionUsers();
                Dispatcher.Invoke(new Action(delegate { dataGridUsers.ItemsSource = collectionUsers.Users; }));
            }));
            thread.Start();
        }
    }
}
