using System.Windows;
using System.Windows.Controls;

namespace UI.classes
{
    internal class Notification
    {
        public StackPanel View { get; set; }
        public int TimeRemaining { get; set; }

        public Notification(string message)
        {
            View = new StackPanel();
            View.Style = Application.Current.FindResource("ErrorMessage") as Style;
            TextBlock textBlock = new TextBlock();
            textBlock.Style = Application.Current.FindResource("ErrorText") as Style;
            textBlock.Text = message;
            View.Children.Add(textBlock);
            TimeRemaining = 5;
        }
    }
}