using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.BuildProgressBar
{
    /// <summary>
    /// Interaction logic for MainUI.xaml.
    /// </summary>
    public partial class MainUI : UserControl
    {
        public MainUI()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(string.Format(CultureInfo.CurrentUICulture, "We are inside {0}.Button1_Click()", this.ToString()));
        }
    }
}
