using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace GDCS_Programmer
{
    /// <summary>
    /// Interaction logic for ToolboxControl1.xaml.
    /// </summary>
    [ProvideToolboxControl("GDCS_Programmer.ToolboxControl1", true)]
    public partial class ToolboxControl1 : UserControl
    {
        public ToolboxControl1()
        {
            InitializeComponent();
            List<FolderItem> items = new List<FolderItem>();
            for(int i = 0; i < 3; i++)
            {
                items.Add(new FolderItem() { Title = string.Format("index {0}", i), Id = i.ToString() });
            }
            FolderList.ItemsSource = items;
            
        } 

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(string.Format(CultureInfo.CurrentUICulture, "We are inside {0}.Button1_Click()", this.ToString()));
        }
    }

    public class FolderItem
    {
        public string Title;
        public string Id;

    }
}
