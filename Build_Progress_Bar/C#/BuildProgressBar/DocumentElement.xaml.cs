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

namespace Microsoft.BuildProgressBar
{
    /// <summary>
    /// DocumentElement.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DocumentElement : UserControl
    {
        bool isHiding;
        DocumentNode documentNode;
        int state;
        public DocumentElement(DocumentNode node, float widthDelta)
        {
            InitializeComponent();
            documentNode = node;
            testButton.Content = node.Title;
            //textBlock.Text = node.Content;
            //if(node.Childs.Count > 0)
            //{
            //    foreach(var child in node.Childs)
            //    {
            //        DocumentElement element = new DocumentElement(child, widthDelta + 10);
            //        listView.Items.Add(element);
            //        //stackPanel.Children.Add(element);
            //    }
            //}
            isHiding = true;
            //if(!node.Content.Any(x => char.IsLetterOrDigit(x)))
            //{
            //    textBlock.Visibility = Visibility.Collapsed;
            //    textBlockBorder.Visibility = Visibility.Collapsed;
            //}
            //listView.Visibility = Visibility.Collapsed;
            //textBlock.Visibility = Visibility.Collapsed;
            //stackPanel.Visibility = Visibility.Collapsed;
            if (node.State == 0)
            {
                testButton.Background = Brushes.OrangeRed;
            }
            else if (node.State == 1)
            {
                testButton.Background = Brushes.Orange;

            }
            else
            {
                testButton.Background = Brushes.LightGreen;
            }


        }

        public void setState(int newState)
        {
            state = newState;
            if(state == 0)
            {
                testButton.Background = Brushes.OrangeRed;
            }
            else if(state == 1)
            {
                testButton.Background = Brushes.Orange;

            }
            else
            {
                testButton.Background = Brushes.LightGreen;
            }
        }

        public void setCheck(bool b)
        {
            checkBox.Checked -= checkBox_Checked;
            checkBox.Unchecked -= checkBox_Unchecked;
            checkBox.IsChecked = b;
            checkBox.Checked += checkBox_Checked;
            checkBox.Unchecked += checkBox_Unchecked;
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            //isHiding = !isHiding;
            //if(isHiding)
            //{
            //    //listView.Visibility = Visibility.Collapsed;
            //    //textBlock.Visibility = Visibility.Collapsed;
            //    stackPanel.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    //listView.Visibility = Visibility.Visible;
            //    //textBlock.Visibility = Visibility.Visible;
            //    stackPanel.Visibility = Visibility.Visible;
            //}
            BuildProgressToolWindow.Instance.RefreshUI(documentNode);
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            if(documentNode.State == 0 || documentNode.State == 1)
            {
                documentNode.ChangeState(2);
            }
            BuildProgressToolWindow.Instance.RefreshUI(documentNode.Parent);
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (documentNode.State == 2)
            {
                documentNode.ChangeState(1);
            }
            BuildProgressToolWindow.Instance.RefreshUI(documentNode.Parent);

        }
    }
}
