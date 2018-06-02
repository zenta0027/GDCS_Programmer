using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System;
using System.IO;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using EnvDTE80;
using System.Text;
using System.Collections.Generic;

namespace Microsoft.BuildProgressBar
{
    /// <summary>
    /// Interaction logic for DocumentUI.xaml.
    /// </summary>
    public partial class DocumentUI : UserControl
    {
        public DocumentUI(string name, string id)
        {
            InitializeComponent();

            Credential credential;
            DocumentNode rootNode;

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
            //string str = credential.LoadTextFromId(id);
            Stream stream = credential.LoadTextFromId(id);
            string str;
            List<int> numList = new List<int>();
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                rootNode = new DocumentNode();
                DocumentNode currentNode = rootNode;
                while ((str = reader.ReadLine()) != null)
                {
                    numList.Add(1);
                    if (str.Contains(string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray()) + ". "))
                    {
                        DocumentNode tempNode = new DocumentNode(str, "", currentNode);
                        currentNode.Childs.Add(tempNode);
                        currentNode = tempNode;
                        continue;
                    }
                    numList.RemoveAt(numList.Count - 1);
                    if (numList.Count > 0)
                    {
                        numList[numList.Count - 1] += 1;
                        if (str.Contains(string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray()) + ". "))
                        {
                            DocumentNode tempNode = new DocumentNode(str, "", currentNode.Parent);
                            currentNode.Parent.Childs.Add(tempNode);
                            currentNode = tempNode;
                            continue;
                        }
                        numList[numList.Count - 1] -= 1;
                    }
                    if (numList.Count > 1)
                    {
                        int n = numList[numList.Count - 1];
                        numList.RemoveAt(numList.Count - 1);
                        numList[numList.Count - 1] += 1;
                        if (str.Contains(string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray()) + ". "))
                        {
                            DocumentNode tempNode = new DocumentNode(str, "", currentNode.Parent.Parent);
                            currentNode.Parent.Parent.Childs.Add(tempNode);
                            currentNode = tempNode;
                            continue;
                        }
                        numList[numList.Count - 1] -= 1;
                        numList.Add(n);
                    }
                    currentNode.Content += (str + "\n");
                }
            }

            foreach (var node in rootNode.Childs)
            {
                DocumentElement element = new DocumentElement();
                element.testButton.Content = node.Title;
                listView.Items.Add(element);
            }

            textBlock.Text = rootNode.Childs[0].Content;
            documentName.Text = name;

            //DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
            //ServiceProvider sp = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)dte);

            //DTE2 _applicationObject = Package.GetGlobalService(typeof(DTE)) as DTE2;
            //_applicationObject.ExecuteCommand("File.OpenFile", string.Format(@"{0}\Command.cs", _applicationObject.ActiveDocument.Path));
            //TextDocument document = (TextDocument)(_applicationObject.ActiveDocument.Object("TextDocument"));
            //if (document != null)
            //{
            //    var p = document.StartPoint.CreateEditPoint();
            //    string s = p.GetText(document.EndPoint);
            //    textBlock.Text = s;
            //    //textBlock.Text = _applicationObject.ActiveDocument.FullName;
            //}

            //Document document = _applicationObject.ActiveDocument;
            //if(document != null)
            //{
            //    textBlock.Text = document.Path;
            //}

            //UIHierarchy uih = _applicationObject.ToolWindows.SolutionExplorer;
            //Array selectedItems = (Array)uih.SelectedItems;
            //if(selectedItems != null)
            //{
            //    foreach(UIHierarchyItem selItem in selectedItems)
            //    {
            //        ProjectItem prjItem = selItem.Object as ProjectItem;
            //        string filePath = prjItem.Properties.Item("FullPath").Value.ToString();
            //        textBlock.Text = filePath;
            //    }
            //}
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(string.Format(CultureInfo.CurrentUICulture, "We are inside {0}.Button1_Click()", this.ToString()));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BuildProgressToolWindow.Instance.LoadToolBox(Credential.Instance.folderId);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DTE2 _applicationObject = Package.GetGlobalService(typeof(DTE)) as DTE2;
            //FileCodeModel myFCM = _applicationObject.ActiveDocument.ProjectItem.FileCodeModel;
            //CodeClass myClass1 = (CodeClass2)myFCM.CodeElements.Item("Form1");
            //TextSelection ts = _applicationObject.ActiveWindow.Selection as TextSelection;
            TextSelection ts = _applicationObject.ActiveDocument.Selection as TextSelection;
            if(ts == null)
            {
                MessageBox.Show("Sulho!");

            }
            else
            {
                CodeFunction func = ts.ActivePoint.CodeElement[vsCMElement.vsCMElementFunction] as EnvDTE.CodeFunction;
                if (func == null)
                {
                    MessageBox.Show("Not function!");
                }
                else
                {
                    textBlock.Text = func.FullName;
                    //textBlock.Text = _applicationObject.ActiveDocument.Path;
                    //MessageBox.Show(string.Format("Adding {0} {1} {2} to working process [Attack]", _applicationObject.ActiveDocument.Name, _applicationObject.ActiveDocument.FullName.Replace(_applicationObject.ActiveDocument.Path, ""), func.Name), "Adding Tag Success");
                    MessageBox.Show(string.Format("Adding {0} to working process [Attack]", _applicationObject.ActiveDocument.FullName.Replace(
                        _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                            _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                        _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\") + "/" + func.FullName), "Adding Tag Success");

                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            buttonGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
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
            credential.TestUpdate();

        }
    }
}
