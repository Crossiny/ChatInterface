using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatInterface
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Client client;
        public MainWindow()
        {
            InitializeComponent();
            client = new Client();
        }

        private void buttonToggleConnect_Click(object sender, RoutedEventArgs e)
        {
            if (buttonToggleConnect.IsChecked == true)
            {
                if (client.Connect(textBoxUserName.Text, textBoxPassword.Text))
                {
                    buttonToggleConnect.Content = "Connected";
                }
                else
                {
                    buttonToggleConnect.IsChecked = false;
                }
            }
            else
            {
                buttonToggleConnect.Content = "Connect";
                client.Disconnect();
            }
        }

        private void buttonStartServer_Click(object sender, RoutedEventArgs e)
        {
            Task t = new Task(Server.startServer);
            t.Start();
            Console.WriteLine(t.Id);
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            client.SendMessage(textBoxSend.Text);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Server.Stop();
        }
    }
}
