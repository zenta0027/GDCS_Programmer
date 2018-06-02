using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Microsoft.BuildProgressBar
{
    /// <summary>
    /// Interaction logic for ToolboxControl1.xaml.
    /// </summary>
    //[ProvideToolboxControl("Microsoft.BuildProgressBar.ToolboxControl1", true)]
    public partial class ToolboxControl1 : UserControl
    {
        private string baseId;
        public ToolboxControl1(string id)
        //public ToolboxControl1()
        {
            InitializeComponent();

            List<FolderItem> items = new List<FolderItem>();

            Credential credential;
            if (Credential.Instance == null)
            {
                credential = new Credential();
                credential.InitCredential();
                credential.CreateService();
                credential.LoadFiles();
            }
            else
            {
                credential = Credential.Instance;
            }
            //items = credential.FindTestFolders();
            items = credential.FindFolders(id);
            baseId = credential.folderId;

            FolderList.ItemsSource = items;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(string.Format(CultureInfo.CurrentUICulture, "We are inside {0}.Button1_Click()", this.ToString()));
        }

        private void GameDocument_Selected(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(e.ToString(), "Title");
            
        }

        private void TextBlock_Selected(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(e.ToString(), "Title");
            MessageBox.Show(((FolderItem)FolderList.SelectedItem).Title, "Title");


        }

        private void FolderList_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //((FolderItem)FolderList.SelectedItem).Id
            if (((FolderItem)FolderList.SelectedItem).Type.Contains("folder"))
            {
                List<FolderItem> items = new List<FolderItem>();
                Credential credential;
                if (Credential.Instance == null)
                {
                    credential = new Credential();
                    credential.InitCredential();
                    credential.CreateService();
                    credential.LoadFiles();
                }
                else
                {
                    credential = Credential.Instance;
                }
                items = credential.FindFolders(((FolderItem)FolderList.SelectedItem).Id);
                FolderItem item = new FolderItem { Id = "", Title = "...", Type = "←" };
                items.Add(item);
                FolderList.ItemsSource = items;
            }
            else if(((FolderItem)FolderList.SelectedItem).Type == "←")
            {
                List<FolderItem> items = new List<FolderItem>();
                Credential credential;
                if (Credential.Instance == null)
                {
                    credential = new Credential();
                    credential.InitCredential();
                    credential.CreateService();
                    credential.LoadFiles();
                }
                else
                {
                    credential = Credential.Instance;
                }

                string parentId = credential.GetParentID(credential.folderId);
                items = credential.FindFolders(parentId);
                if (credential.folderId != baseId)
                {
                    FolderItem item = new FolderItem { Id = "", Title = "...", Type = "←" };
                    items.Add(item);
                }
                FolderList.ItemsSource = items;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            List<FolderItem> items = new List<FolderItem>();
            Credential credential;
            if (Credential.Instance == null)
            {
                credential = new Credential();
                credential.InitCredential();
                credential.CreateService();
                credential.LoadFiles();
            }
            else
            {
                credential = Credential.Instance;
            }

            if(credential.folderId == baseId)
            {
                MessageBox.Show("It is a base folder!", "Warning");
            }
            else
            {
                string parentId = credential.GetParentID(credential.folderId);
                items = credential.FindFolders(parentId);
                FolderList.ItemsSource = items;
            }
            */
            Credential.Instance.folderId = baseId;
            BuildProgressToolWindow.Instance.LoadProgressBar();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if(FolderList.SelectedItem == null)
            {
                MessageBox.Show("Please select document to open!", "Warning");
                return;
            }

            if(!((FolderItem)FolderList.SelectedItem).Type.Contains("document"))
            {
                MessageBox.Show("It is not a document file!", "Warning");
                return;
            }

            Credential.Instance.SetTagId(((FolderItem)FolderList.SelectedItem).Id);
            BuildProgressToolWindow.Instance.LoadDocumentUI(((FolderItem)FolderList.SelectedItem).Title, ((FolderItem)FolderList.SelectedItem).Id);
        }
    }
}
