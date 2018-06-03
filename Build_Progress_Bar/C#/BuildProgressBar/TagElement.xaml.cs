using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// TagElement.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TagElement : UserControl
    {
        public string functionName;
        string scriptName;
        string tagId;
        string indexString;
        string realPath;
        DocumentNode node;

        public TagElement(DocumentNode n, string fName, string sName, string id, string index, string isChecked = "0", string path = "")
        {
            InitializeComponent();

            node = n;
            indexString = index;
            functionName = fName;
            realPath = path;
            if (fName != "_Null")
            {
                tagButton.Content = sName + "/" + fName;
            }
            else
            {
                tagButton.Content = sName;
            }
            setCheck(isChecked == "1");
            //checkBox.IsChecked = (isChecked == "1");
            if(isChecked == "1")
            {
                tagButton.Background = Brushes.LightGreen;
            }
            else
            {
                tagButton.Background = Brushes.Orange;
            }

            scriptName = sName;
            tagId = id;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            tagButton.Background = Brushes.LightGreen;
            List<string> lineList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag").ToList();
            for(int i = 0; i < lineList.Count; i++)
            {
                string[] elements = lineList[i].Split('\t');
                if(elements[0] == tagId && elements[1] == functionName && elements[2] == scriptName && indexString == elements[3])
                {
                    if (node.State == 0)
                    {
                        node.ChangeState(1);
                    }
                    elements[4] = "1";
                    string str = String.Join("\t", elements);
                    lineList[i] = str;
                    File.WriteAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag", lineList.ToArray());
                    BuildProgressToolWindow.Instance.RefreshUI(node);
                    return;
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> lineList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag").ToList();
            for (int i = 0; i < lineList.Count; i++)
            {
                string[] elements = lineList[i].Split('\t');
                if (elements[0] == tagId && elements[1] == functionName && elements[2] == scriptName && indexString == elements[3])
                {
                    if(node.State == 0)
                    {
                        node.ChangeState(1);
                    }
                    lineList.RemoveAt(i);
                    File.WriteAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag", lineList.ToArray());
                    ((ListBox)this.Parent).Items.Remove(this);
                    BuildProgressToolWindow.Instance.RefreshUI(node);
                    return;
                }
            }
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
            ServiceProvider sp = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)dte);

            DTE2 _applicationObject = Package.GetGlobalService(typeof(DTE)) as DTE2;
            //MessageBox.Show(realPath);
            //MessageBox.Show(_applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
            //    _applicationObject.ActiveDocument.ActiveWindow.Project.Name+"\\"+_applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName,
            //    realPath));
            //_applicationObject.ActiveDocument.FullName.Replace(
            //                 _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
            //                     _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName, ""),
            //                 _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\")
            //_applicationObject.ExecuteCommand("File.OpenFile", string.Format(@"{0}\{1}", _applicationObject.ActiveDocument.Path, scriptName));
            _applicationObject.ExecuteCommand("File.OpenFile", _applicationObject.ActiveDocument.ActiveWindow.Project.FullName.Replace(
                _applicationObject.ActiveDocument.ActiveWindow.Project.Name + "\\" + _applicationObject.ActiveDocument.ActiveWindow.Project.UniqueName,
                realPath));

            //TextDocument document = (TextDocument)(_applicationObject.ActiveDocument.Object("TextDocument"));
            //if (document != null)
            //{
            //    var p = document.StartPoint.CreateEditPoint();
            //    string s = p.GetText(document.EndPoint);
            //    //textBlock.Text = _applicationObject.ActiveDocument.FullName;
            //}

        }

        public void setCheck(bool b)
        {
            checkBox.Checked -= CheckBox_Checked;
            checkBox.Unchecked -= CheckBox_Unchecked;
            checkBox.IsChecked = b;
            checkBox.Checked += CheckBox_Checked;
            checkBox.Unchecked += CheckBox_Unchecked;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            tagButton.Background = Brushes.Orange;
            List<string> lineList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag").ToList();
            for (int i = 0; i < lineList.Count; i++)
            {
                string[] elements = lineList[i].Split('\t');
                if (elements[0] == tagId && elements[1] == functionName && elements[2] == scriptName && indexString == elements[3])
                {
                    if(node.State == 2)
                    {
                        node.ChangeState(1);
                    }
                    elements[4] = "0";
                    string str = String.Join("\t", elements);
                    lineList[i] = str;
                    File.WriteAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag", lineList.ToArray());
                    BuildProgressToolWindow.Instance.RefreshUI(node);

                    return;
                }
            }

        }
    }
}
