using System;
using System.Windows;
using UI.pages;


namespace UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            frContent.Content = new GreetingPage();
        }
    }
}
