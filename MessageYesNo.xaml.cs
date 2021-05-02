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
    /// Interaction logic for MessageYesNo.xaml
    /// </summary>
    public partial class MessageYesNo : Window
    {
        
        int resultType = 0;
        private TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        public MessageYesNo(String msgText)
        {
            InitializeComponent();
            
            this.msgBoxText.Text = msgText;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ///Yes
            
            this.resultType = 1;
            this.Hide();
            tcs.SetResult(true);
        }
 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //No
            this.resultType = 2;
            this.Hide();
            tcs.SetResult(true);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        public async Task<int> showMsg()
        {
            this.ShowDialog();
            await tcs.Task;
            this.Close();
            return resultType;
        }

    }
}
