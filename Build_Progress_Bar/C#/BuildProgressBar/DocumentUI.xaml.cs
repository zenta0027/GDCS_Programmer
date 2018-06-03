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
using System.Linq;
using Microsoft.Internal.VisualStudio.PlatformUI;

namespace Microsoft.BuildProgressBar
{
    /// <summary>
    /// Interaction logic for DocumentUI.xaml.
    /// </summary>
    public partial class DocumentUI : UserControl
    {
        string documentId;
        DocumentNode currentNode;

        //public DocumentUI(string name, string id)
        public DocumentUI(DocumentNode node, string id)
        {
            InitializeComponent();
            currentNode = node;
            documentId = id;
            //Credential credential;
            //DocumentNode rootNode;

            //documentId = id;

            //if (Credential.Instance == null)
            //{
            //    credential = new Credential();
            //    credential.InitCredential();
            //    credential.CreateService();
            //    credential.LoadFiles();
            //}
            //else
            //{
            //    credential = Credential.Instance;
            //}
            //items = credential.FindTestFolders();
            //string str = credential.LoadTextFromId(id);
            //Stream stream = credential.LoadTextFromId(id);
            //string str;
            //List<int> numList = new List<int>();
            //using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            //{
            //    rootNode = new DocumentNode();
            //    DocumentNode currentNode = rootNode;
            //    while ((str = reader.ReadLine()) != null)
            //    {
            //        numList.Add(1);
            //        if (str.Contains(string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray()) + ". "))
            //        {
            //            DocumentNode tempNode = new DocumentNode(str, "", currentNode, numList);
            //            currentNode.Childs.Add(tempNode);
            //            currentNode = tempNode;
            //            continue;
            //        }
            //        numList.RemoveAt(numList.Count - 1);
            //        if (numList.Count > 0)
            //        {
            //            numList[numList.Count - 1] += 1;
            //            if (str.Contains(string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray()) + ". "))
            //            {
            //                DocumentNode tempNode = new DocumentNode(str, "", currentNode.Parent, numList);
            //                currentNode.Parent.Childs.Add(tempNode);
            //                currentNode = tempNode;
            //                continue;
            //            }
            //            numList[numList.Count - 1] -= 1;
            //        }
            //        if (numList.Count > 1)
            //        {
            //            int n = numList[numList.Count - 1];
            //            numList.RemoveAt(numList.Count - 1);
            //            numList[numList.Count - 1] += 1;
            //            if (str.Contains(string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray()) + ". "))
            //            {
            //                DocumentNode tempNode = new DocumentNode(str, "", currentNode.Parent.Parent, numList);
            //                currentNode.Parent.Parent.Childs.Add(tempNode);
            //                currentNode = tempNode;
            //                continue;
            //            }
            //            numList[numList.Count - 1] -= 1;
            //            numList.Add(n);
            //        }
            //        currentNode.Content += (str + "\n");
            //    }
            //}

            //rootNode = credential.LoadTextFromId(id);

            foreach (var n in node.Childs)
            {
                DocumentElement element = new DocumentElement(n, 10);
                element.setCheck(n.State == 2);
                //element.checkBox.IsChecked = (n.State == 2);
                listView.Items.Add(element);
            }

            //textBlock.Text = rootNode.Childs[0].Content;
            documentName.Text = node.Title;
            if (!node.Content.Any(x => char.IsLetterOrDigit(x)))
            {
                scrollView.Visibility = Visibility.Collapsed;
                //textBlock.Visibility = Visibility.Collapsed;
                //textBlockBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                textBlock.Text = node.Content;
            }
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
            string str;
            if (currentNode.Index != null)
            {
                str = String.Join(".", currentNode.Index.ToArray());
            }
            else
            {
                str = "All";
            }

            List<string> lineList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag").ToList();
            for (int i = 0; i < lineList.Count; i++)
            {
                string[] elements = lineList[i].Split('\t');
                if (elements[0] == documentId && elements[3] == str)
                {
                    TagElement tagElement = new TagElement(currentNode, elements[1], elements[2], elements[0], str, elements[4], elements[5]);
                    tagBox.Items.Add(tagElement);
                }
            }
        }

        public void RefreshUI(DocumentNode node)
        {
            currentNode = node;
            listView.Items.Clear();
            
            foreach (var n in node.Childs)
            {
                DocumentElement element = new DocumentElement(n, 10);
                element.setCheck(n.State == 2);
                //element.checkBox.IsChecked = (n.State == 2);
                listView.Items.Add(element);
            }
            documentName.Text = node.Title;
            if (!node.Content.Any(x => char.IsLetterOrDigit(x)))
            {
                //textBlock.Visibility = Visibility.Collapsed;
                //textBlockBorder.Visibility = Visibility.Collapsed;
                scrollView.Visibility = Visibility.Collapsed;

            }
            else
            {
                //textBlock.Visibility = Visibility.Visible;
                //textBlockBorder.Visibility = Visibility.Visible;
                scrollView.Visibility = Visibility.Visible;

                textBlock.Text = node.Content;
            }

            string str;
            if (currentNode.Index != null)
            {
                str = String.Join(".", currentNode.Index.ToArray());
            }
            else
            {
                str = "All";
            }
            tagBox.Items.Clear();
            List<string> lineList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag").ToList();
            for (int i = 0; i < lineList.Count; i++)
            {
                string[] elements = lineList[i].Split('\t');
                if (elements[0] == documentId && elements[3] == str)
                {
                    TagElement tagElement = new TagElement(currentNode, elements[1], elements[2], elements[0], str, elements[4], elements[5]);
                    tagBox.Items.Add(tagElement);
                }
            }

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
            MessageBoxResult messageResult = MessageBox.Show("Do you want to add selected function or class to tag list?\nIf you press [no] or not select anything, it will add current script to tag list", "Choose One", MessageBoxButton.YesNoCancel);
            string str;
            if (currentNode.Index != null)
            {
                str = String.Join(".", currentNode.Index.ToArray());
            }
            else
            {
                str = "All";
            }
            if (messageResult == MessageBoxResult.Yes)
            {
                DTE2 _applicationObject = Package.GetGlobalService(typeof(DTE)) as DTE2;
                //FileCodeModel myFCM = _applicationObject.ActiveDocument.ProjectItem.FileCodeModel;
                //CodeClass myClass1 = (CodeClass2)myFCM.CodeElements.Item("Form1");
                //TextSelection ts = _applicationObject.ActiveWindow.Selection as TextSelection;
                TextSelection ts = null;
                try
                {
                    ts = _applicationObject.ActiveDocument.Selection as TextSelection;
                }
                catch
                {

                    TagElement tagElement = new TagElement(null, _applicationObject.ActiveDocument.Name, documentId, str, _applicationObject.ActiveDocument.FullName.Replace(
                            _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                                _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                            _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\"));
                    List<string> lineList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag").ToList();
                    for (int i = 0; i < lineList.Count; i++)
                    {
                        string[] elements = lineList[i].Split('\t');
                        if (elements[0] == documentId && elements[1] == "_Null" && elements[2] == _applicationObject.ActiveDocument.Name && elements[3] == str)
                        {
                            MessageBox.Show("Same tag already exists!", "Warning");
                            return;
                        }
                    }
                    tagBox.Items.Add(tagElement);

                    MessageBox.Show(string.Format("Adding {0} to working process {1}", _applicationObject.ActiveDocument.FullName.Replace(
                        _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                            _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                        _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\"), currentNode.Title), "Adding Tag Success");


                    FileStream stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag", FileMode.Append);

                    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        writer.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", documentId, "_Null", _applicationObject.ActiveDocument.Name, str, 0, _applicationObject.ActiveDocument.FullName.Replace(
                            _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                                _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                            _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\")));
                    }

                    if(currentNode.State == 0)
                    {
                        currentNode.ChangeState(1);
                        RefreshUI(currentNode);
                    }
                    return;
                }
                if(ts == null)
                {
                    MessageBox.Show("There is no selection!");

                }
                else
                {

                    CodeFunction func = ts.ActivePoint.CodeElement[vsCMElement.vsCMElementFunction] as EnvDTE.CodeFunction;
                    if (func == null)
                    {
                        CodeClass codeClass = ts.ActivePoint.CodeElement[vsCMElement.vsCMElementClass] as EnvDTE.CodeClass;
                        if (codeClass == null)
                        {
                        }
                        else
                        {
                            TagElement tagElement = new TagElement(currentNode, codeClass.FullName, _applicationObject.ActiveDocument.Name, documentId, str, _applicationObject.ActiveDocument.FullName.Replace(
                            _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                                _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                            _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\"));
                            List<string> lineList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag").ToList();
                            for (int i = 0; i < lineList.Count; i++)
                            {
                                string[] elements = lineList[i].Split('\t');
                                if (elements[0] == documentId && elements[1] == codeClass.FullName && elements[2] == _applicationObject.ActiveDocument.Name && elements[3] == str)
                                {
                                    MessageBox.Show("Same tag already exists!", "Warning");
                                    return;
                                }
                            }
                            tagBox.Items.Add(tagElement);
                            MessageBox.Show(string.Format("Adding {0} to working process {1}", _applicationObject.ActiveDocument.FullName.Replace(
                                _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                                    _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                                _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\") + "/" + codeClass.FullName, currentNode.Title), "Adding Tag Success");
                            //MessageBox.Show(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName);
                            FileStream stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag", FileMode.Append);

                            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                            {
                                writer.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", documentId, codeClass.FullName, _applicationObject.ActiveDocument.Name, str, 0, _applicationObject.ActiveDocument.FullName.Replace(
                            _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                                _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                            _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\")));
                            }

                            if (currentNode.State == 0)
                            {
                                currentNode.ChangeState(1);
                                RefreshUI(currentNode);
                            }
                            return;
                        }
                    }
                    else
                    {
                        //MessageBox.Show(_applicationObject.ActiveDocument.ActiveWindow.Project.Name);
                        //MessageBox.Show(_applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName);
                        //MessageBox.Show(_applicationObject.ActiveDocument.ActiveWindow.Project.FullName);
                        //MessageBox.Show(_applicationObject.ActiveDocument.ActiveWindow.Project.FileName);

                        //MessageBox.Show(_applicationObject.ActiveDocument.Name);
                        //MessageBox.Show(_applicationObject.ActiveDocument.FullName);
                        //MessageBox.Show(_applicationObject.ActiveDocument.Path);
                        //MessageBox.Show(func.Name);
                        //MessageBox.Show(func.FullName);

                        //textBlock.Text = func.FullName;
                        //textBlock.Text = _applicationObject.ActiveDocument.Path;
                        //MessageBox.Show(string.Format("Adding {0} {1} {2} to working process [Attack]", _applicationObject.ActiveDocument.Name, _applicationObject.ActiveDocument.FullName.Replace(_applicationObject.ActiveDocument.Path, ""), func.Name), "Adding Tag Success");

                        TagElement tagElement = new TagElement(currentNode, func.FullName + "()", _applicationObject.ActiveDocument.Name, documentId, str, _applicationObject.ActiveDocument.FullName.Replace(
                            _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                                _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                            _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\"));
                        List<string> lineList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag").ToList();
                        for (int i = 0; i < lineList.Count; i++)
                        {
                            string[] elements = lineList[i].Split('\t');
                            if (elements[0] == documentId && elements[1] == func.FullName + "()" && elements[2] == _applicationObject.ActiveDocument.Name && elements[3] == str)
                            {
                                MessageBox.Show("Same tag already exists!", "Warning");
                                return;
                            }
                        }
                        tagBox.Items.Add(tagElement);
                        MessageBox.Show(string.Format("Adding {0} to working process {1}", _applicationObject.ActiveDocument.FullName.Replace(
                            _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                                _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                            _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\") + "/" + func.FullName + "()", currentNode.Title), "Adding Tag Success");
                        //MessageBox.Show(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName);
                        FileStream stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag", FileMode.Append);
                    
                        using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                        {
                            writer.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", documentId, func.FullName + "()", _applicationObject.ActiveDocument.Name, str, 0, _applicationObject.ActiveDocument.FullName.Replace(
                            _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                                _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                            _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\")));
                        }

                        if (currentNode.State == 0)
                        {
                            currentNode.ChangeState(1);
                            RefreshUI(currentNode);
                        }
                        return;
                    }
                }
            }
            else if(messageResult == MessageBoxResult.No)
            {
                DTE2 _applicationObject = Package.GetGlobalService(typeof(DTE)) as DTE2;

                TagElement tagElement = new TagElement(currentNode, "_Null", _applicationObject.ActiveDocument.Name, documentId, str, _applicationObject.ActiveDocument.FullName.Replace(
                            _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                                _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                            _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\"));
                List<string> lineList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag").ToList();
                for (int i = 0; i < lineList.Count; i++)
                {
                    string[] elements = lineList[i].Split('\t');
                    if (elements[0] == documentId && elements[1] == "_Null" && elements[2] == _applicationObject.ActiveDocument.Name && elements[3] == str)
                    {
                        MessageBox.Show("Same tag already exists!", "Warning");
                        return;
                    }
                }
                tagBox.Items.Add(tagElement);

                MessageBox.Show(string.Format("Adding {0} to working process {1}", _applicationObject.ActiveDocument.FullName.Replace(
                    _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                        _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                    _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\"), currentNode.Title), "Adding Tag Success");


                FileStream stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag", FileMode.Append);

                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", documentId, "_Null", _applicationObject.ActiveDocument.Name, str, 0, _applicationObject.ActiveDocument.FullName.Replace(
                            _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                                _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
                            _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\")));
                }

                if (currentNode.State == 0)
                {
                    currentNode.ChangeState(1);
                    RefreshUI(currentNode);
                }
                return;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //buttonGrid.Visibility = Visibility.Collapsed;
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

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if(currentNode.Parent == null)
            {
                BuildProgressToolWindow.Instance.LoadToolBox(Credential.Instance.folderId);
            }
            else
            {
                RefreshUI(currentNode.Parent);
            }
        }
    }
}
