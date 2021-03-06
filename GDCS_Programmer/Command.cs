﻿using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace GDCS_Programmer
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Command
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        private const string g_TagFileName = "_DocumentTag";

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("bfc7e37a-c821-49d0-98cd-85130906046c");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private Command(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        private static string LoadGoogleDocs()
        {
            string tagId = "";
            string folderId;
            IList<Google.Apis.Drive.v3.Data.File> Files = new List<Google.Apis.Drive.v3.Data.File>();
            string[] Scopes = { DriveService.Scope.DriveReadonly };
            string ApplicationName = "Drive API .NET Quickstart";
            string returnstring = "";


            UserCredential credential;

            Console.WriteLine("Hello!");
            returnstring = returnstring + "Hello! ";
            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
                returnstring = returnstring + "Credential file saved to: " + credPath;
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");
            returnstring += "Files:";
            returnstring += files.Count;

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    
                    if (file.Name == g_TagFileName)
                    {
                        returnstring += string.Format("{0} ({1})\n", file.Name, file.Id);
                        tagId = file.Id;
                        if (file.Parents != null && file.Parents.Count > 0)
                        {
                            folderId = file.Parents[0];
                        }
                    }
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            if(tagId != "")
            {
                {
                    var request1 = service.About.Get();
                    request1.Fields = "user ";

                    var data1 = request1.Execute();
                    System.Reflection.PropertyInfo[] properties = typeof(Google.Apis.Drive.v3.Data.User).GetProperties();
                    returnstring += properties.Length;
                    foreach (System.Reflection.PropertyInfo property in properties)
                    {
                        Console.WriteLine("{0} = {1}\n", property.Name, property.GetValue(data1.User, null));
                        returnstring += string.Format("{0} = {1}\n", property.Name, property.GetValue(data1.User, null));
                    }
                }

                Google.Apis.Drive.v3.Data.File file = service.Files.Get(tagId).Execute();

                returnstring += string.Format("{0} ({1})\n", file.Name, file.Id);

                var request = service.Files.Export(tagId, "text/plain");
                var stream = new System.IO.MemoryStream();

                request.MediaDownloader.ProgressChanged +=
                    (Google.Apis.Download.IDownloadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case Google.Apis.Download.DownloadStatus.Downloading:
                                {
                                    returnstring += progress.BytesDownloaded;
                                    break;
                                }
                            case Google.Apis.Download.DownloadStatus.Completed:
                                {
                                    returnstring += "Download complete.";
                                    break;
                                }
                            case Google.Apis.Download.DownloadStatus.Failed:
                                {
                                    returnstring += "Download failed.";
                                    break;
                                }
                        }
                    };
                request.Download(stream);
                
                
            }
            else
            {
                Console.WriteLine("No files found.");
                returnstring += "No files found.";
            }
            Console.Read();
            return returnstring;

        }

        private static string LoadTagFile()
        {
            string tagId;
            string folderId;
            IList<Google.Apis.Drive.v3.Data.File> Files = new List<Google.Apis.Drive.v3.Data.File>();
            string[] Scopes = { DriveService.Scope.DriveReadonly };
            string ApplicationName = "Drive API .NET Quickstart";
            string returnstring = "";

            UserCredential credential;

            Console.WriteLine("Hello!");
            returnstring = returnstring + "Hello! ";

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
                returnstring = returnstring + "Credential file saved to: " + credPath;
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");
            returnstring += "Files:";
            returnstring += files.Count;

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Name == g_TagFileName)
                    {
                        returnstring += string.Format("{0} ({1})\n", file.Name, file.Id);
                        tagId = file.Id;
                        folderId = file.Parents[0];
                        break;
                    }
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
                returnstring += "No files found.";
            }
            Console.Read();
            return returnstring;

        }


        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static Command Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new Command(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            Console.WriteLine(sender);
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            string title = "Command";

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            message = LoadGoogleDocs();
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Show a message box to prove we were here
            VsShellUtilities.ShowMessageBox(
                this.ServiceProvider,
                message,
                title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}
