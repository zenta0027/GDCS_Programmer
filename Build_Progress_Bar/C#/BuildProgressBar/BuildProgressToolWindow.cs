/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.BuildProgressBar
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsUIElementPane interface.
    /// </summary>
    [Guid("1bcb49dc-47f9-4eba-8d7d-b2baefe89076")]
    public class BuildProgressToolWindow : ToolWindowPane
    {
        private ProgressBarControl progressBar;
        private ToolboxControl1 toolbox;
        public DocumentUI documentUI;
        private MainUI mainUI;
        private bool enableEffects = false;

        private static BuildProgressToolWindow instance;
        public static BuildProgressToolWindow Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public BuildProgressToolWindow() :
            base(null)
        {
            // Set the window title reading it from the resources.
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
            Caption = Resources.ToolWindowTitle;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.

            progressBar = new ProgressBarControl();
            //toolbox = new ToolboxControl1();
            mainUI = new MainUI();

            //base.Content = progressBar;

            //base.Content = toolbox;
            base.Content = mainUI;
            mainUI.grid.Children.Add(progressBar);
            instance = this;
        }

        public void LoadToolBox(string id)
        {

            //Caption = Resources.ToolWindowTitle;
            //toolbox = new ToolboxControl1(id);
            //base.Content = toolbox;
            //IVsWindowFrame windowFrame = (IVsWindowFrame)this.Frame;
            //windowFrame.CloseFrame(0);
            //windowFrame.Show();
            toolbox = new ToolboxControl1(id);
            mainUI.grid.Children.Clear();
            mainUI.grid.Children.Add(toolbox);
            
        }

        public void LoadProgressBar()
        {
            progressBar = new ProgressBarControl();
            mainUI.grid.Children.Clear();
            mainUI.grid.Children.Add(progressBar);

        }

        public void LoadDocumentUI(string name, string fileId)
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
            DocumentNode rootNode = credential.LoadTextFromId(fileId);
            rootNode.Title = name;
            documentUI = new DocumentUI(rootNode, fileId);
            mainUI.grid.Children.Clear();
            mainUI.grid.Children.Add(documentUI);
        }

        public void RefreshUI(DocumentNode node)
        {
            documentUI.RefreshUI(node);
        }

        /// <summary>
        /// Enable/disable animation effects on the progress bar
        /// </summary>
        public bool EffectsEnabled
        {
            get
            {
                return enableEffects;
            }
            set
            {
                enableEffects = value;
                //progressBar.AnimateColor = enableEffects;
            }
        }
        
        // Set the progress bar value
        
        public double Progress
        {
            /*
            get
            {
                return progressBar.Value;
            }
            set
            {
                progressBar.Value = value;
            }
            */
            get; set;
        }
        
        // Set the progress bar text
        public string BarText
        {
            /*
            get
            {
                return progressBar.Text;
            }
            set
            {
                progressBar.Text = value;
            }
            */
            get; set;
        }
        
        
    }
}
