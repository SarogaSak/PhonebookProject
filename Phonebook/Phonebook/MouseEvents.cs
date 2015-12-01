using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Phonebook
{
    public static class MouseEvents
    {
        public static void MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = Brushes.White;
            ((Button)sender).Background = new SolidColorBrush(Colors.Transparent);
        }

        public static void MouseMove(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = Brushes.DarkSlateBlue;
            ((Button)sender).Background = Brushes.White;
        }
    }
}
