using System.Windows;
using System.Windows.Controls;

namespace UI.classes
{
    internal class ErrorMessage
    {
        public StackPanel View { get; set; }
        public int TimeRemaining { get; set; }

        public static StackPanel CreateView(string text)
        {
            StackPanel panel = new StackPanel();
            panel.Style = Application.Current.FindResource("ErrorMessage") as Style;
            TextBlock textBlock = new TextBlock();
            textBlock.Style = Application.Current.FindResource("ErrorText") as Style;
            textBlock.Text = text;
            panel.Children.Add(textBlock);

            return panel;
        }
    }
}