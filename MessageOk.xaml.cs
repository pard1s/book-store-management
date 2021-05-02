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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MessageOk.xaml
    /// </summary>
    public partial class MessageOk : Window
    {
        public MessageOk(String msgText, int icon)
        {
            InitializeComponent();
            this.msgBoxText.Text = msgText;
            if (icon == 1)
            {
                iconType.Source = new BitmapImage(new Uri(@"/img/tick.png", UriKind.Relative));
            }
            else if (icon == 2)
            {
                iconType.Source = new BitmapImage(new Uri(@"/img/error.png", UriKind.Relative));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
