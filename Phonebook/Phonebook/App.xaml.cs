using System;
using System.Windows;

namespace Phonebook
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        SplashScreen splash = new SplashScreen("Minsk_Shadow.png");

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            splash.Show(true, true);
            splash.Close(TimeSpan.FromSeconds(1));
        }
    }
}
