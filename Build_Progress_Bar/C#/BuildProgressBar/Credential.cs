using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using System.Windows;
using Google.Apis.Download;

namespace Microsoft.BuildProgressBar
{
    public class Credential
    {
        UserCredential credential;
        private const string g_TagFileName = "_DocumentTag";
        public string tagId;
        public string folderId;
        public string currentFileId;
        DriveService service;
        IList<Google.Apis.Drive.v3.Data.File> files;

        private static Credential instance;
        public static Credential Instance
        {
            get
            {
                return instance;
            }
        }

        public Credential()
        {
            tagId = null;
            folderId = null;
            instance = this;
        }

        public void InitCredential()
        {
            string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };
            IList<Google.Apis.Drive.v3.Data.File> Files = new List<Google.Apis.Drive.v3.Data.File>();

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.ReadWrite))
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
            }
        } 

        public void CreateService()
        {
            string ApplicationName = "Drive API .NET Quickstart";

            // Create Drive API service.
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

        }

        public void LoadFiles()
        {
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name, parents, kind, mimeType)";
            //listRequest.Fields = "nextPageToken, files(*)";

            //IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            files = listRequest.Execute().Files;
        }

        public void LoadFullFiles()
        {
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "*";
            //listRequest.Fields = "nextPageToken, files(*)";

            //IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            files = listRequest.Execute().Files;
        }

        public List<FolderItem> FindFolders()
        {
            List<FolderItem> folderList = new List<FolderItem>();
            /*
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name, parents, kind, mimeType)";
            
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
                */
            Console.WriteLine("Files:");

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    
                    if (file.Name == g_TagFileName)
                    {
                        FolderItem item = new FolderItem();
                        //tagId.Add(file.Id);
                        if (file.Parents != null && file.Parents.Count > 0)
                        {
                            //folderId.Add(file.Parents[0]);
                            item.Id = file.Parents[0];
                            item.Title = service.Files.Get(file.Parents[0]).Execute().Name;
                            //item.Type = file.Kind;
                            if (file.MimeType.Split('.').Length > 1)
                            {
                                item.Type = file.MimeType.Split('.')[1];
                            }
                            else
                            {
                                item.Type = file.MimeType;
                            }
                        }
                        else
                        {
                            //folderId.Add("");
                        }
                        folderList.Add(item);
                    }
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            return folderList;
        }

        public void SetTagId(string parentId)
        {
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Parents != null && file.Parents[0] == parentId)
                    {
                        tagId = file.Id;
                        return;
                    }
                }
            }
            return;

        }

        public List<FolderItem> FindFolders(string id)
        {
            List<FolderItem> folderList = new List<FolderItem>();
            /*
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name, parents, kind, mimeType)";
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
                */
            Console.WriteLine("Files:");

            folderId = id;

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    //tagId.Add(file.Id);
                    if (file.Parents != null && file.Parents[0] == id)
                    {
                        FolderItem item = new FolderItem();
                        item.Id = file.Id;
                        item.Title = file.Name;
                        //item.Type = file.MimeType;
                        if (file.MimeType.Split('.').Length > 2)
                        {
                            item.Type = file.MimeType.Split('.')[2];
                        }
                        else
                        {
                            item.Type = file.MimeType;
                        }
                        folderList.Add(item);
                    }
                }
            }
            return folderList;
        }

        public List<FolderItem> FindTestFolders()
        {
            List<FolderItem> folderList = new List<FolderItem>();
            /*
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name, parents, kind, mimeType)";
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
                */
            Console.WriteLine("Files:");

            string idd ="";

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Name == g_TagFileName)
                    {
                        if (file.Parents != null && file.Parents.Count > 0)
                        {
                            idd = file.Parents[0];
                            break;
                        }
                    }
                }
            }

            folderId = idd;

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    //tagId.Add(file.Id);
                    if (file.Parents != null && file.Parents[0] == idd)
                    {
                        FolderItem item = new FolderItem();
                        item.Id = file.Id;
                        item.Title = file.Name;
                        //item.Type = file.MimeType;
                        if (file.MimeType.Split('.').Length > 1)
                        {
                            item.Type = file.MimeType.Split('.')[1];
                        }
                        else
                        {
                            item.Type = file.MimeType;
                        }
                        folderList.Add(item);
                    }
                }
            }
            return folderList;
        }

        public string GetParentID(string id)
        {
            /*
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name, parents, kind, mimeType)";
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
                */
            /*
        IList<string> temp = service.Files.Get(id).Execute().Parents;

        if (temp != null)
        {
            MessageBox.Show(temp[0]);

            return temp[0];
        }
        return null;
        */

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    //tagId.Add(file.Id);
                    if (file.Id == id)
                    {
                        return file.Parents[0];
                    }
                }
            }
            return null;

        }

        //public string LoadTextFromId(string id)
        //public MemoryStream LoadTextFromId(string id)
        public DocumentNode LoadTextFromId(string id)
        {
            DocumentNode rootNode;
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name, parents, kind, mimeType, webViewLink)";
            string name;

            //IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            IList<Google.Apis.Drive.v3.Data.File> new_files = listRequest.Execute().Files;
            if (new_files != null && new_files.Count > 0)
            {
                foreach (var file in new_files)
                {
                    //tagId.Add(file.Id);
                    if (file.Id == id)
                    {
                        //return file.WebViewLink;
                        name = file.Name;
                    }
                }
            }
            //return null;
            var request = service.Files.Export(id, "text/plain");
            var stream = new System.IO.MemoryStream();
            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged +=
                    (IDownloadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    MessageBox.Show(string.Format("{0}", progress.BytesDownloaded));
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {
                                    //MessageBox.Show("Download complete.");
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {
                                    MessageBox.Show("Download failed.");
                                    break;
                                }
                        }
                    };
            request.Download(stream);
            /*
            byte[] returnByte = new byte[102400];
            stream.Write((byte[])returnByte, 0, (int)stream.Length);
            return System.Text.Encoding.UTF8.GetString(returnByte);
            return null;
            */
            stream.Position = 0;
            //using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            //{
            //    return reader.ReadToEnd();
            //}

            //Stream stream = credential.LoadTextFromId(id);
            string str;
            List<int> numList = new List<int>();
            Boolean fileExist = File.Exists(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\" + id);
            List<string> progressList = new List<string>();
            if(fileExist)
            {
                progressList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\" + id).ToList();
            }
            
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                rootNode = new DocumentNode();
                rootNode.documentId = id;
                DocumentNode currentNode = rootNode;
                while ((str = reader.ReadLine()) != null)
                {
                    numList.Add(1);
                    if (str.Contains(string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray()) + ". "))
                    {
                        DocumentNode tempNode = new DocumentNode(str, "", currentNode, numList, 0, id);
                        currentNode.Childs.Add(tempNode);
                        currentNode = tempNode;
                        if(!fileExist)
                        {
                            progressList.Add(string.Format("{0}\t0", string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray())));
                        }
                        else
                        {
                            foreach(string st in progressList)
                            {
                                if(st.Split('\t')[0] == string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray()))
                                {
                                    tempNode.State = int.Parse(st.Split('\t')[1]);
                                }
                            }
                        }
                        continue;
                    }
                    numList.RemoveAt(numList.Count - 1);
                    if (numList.Count > 0)
                    {
                        numList[numList.Count - 1] += 1;
                        if (str.Contains(string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray()) + ". "))
                        {
                            DocumentNode tempNode = new DocumentNode(str, "", currentNode.Parent, numList, 0, id);
                            currentNode.Parent.Childs.Add(tempNode);
                            currentNode = tempNode;
                            if (!fileExist)
                            {
                                progressList.Add(string.Format("{0}\t0", string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray())));
                            }
                            else
                            {
                                foreach (string st in progressList)
                                {
                                    if (st.Split('\t')[0] == string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray()))
                                    {
                                        tempNode.State = int.Parse(st.Split('\t')[1]);
                                    }
                                }
                            }
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
                            DocumentNode tempNode = new DocumentNode(str, "", currentNode.Parent.Parent, numList, 0, id);
                            currentNode.Parent.Parent.Childs.Add(tempNode);
                            currentNode = tempNode;
                            if (!fileExist)
                            {
                                progressList.Add(string.Format("{0}\t0", string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray())));
                            }
                            else
                            {
                                foreach (string st in progressList)
                                {
                                    if (st.Split('\t')[0] == string.Join(".", numList.ConvertAll(x => x.ToString()).ToArray()))
                                    {
                                        tempNode.State = int.Parse(st.Split('\t')[1]);
                                    }
                                }
                            }
                            continue;
                        }
                        numList[numList.Count - 1] -= 1;
                        numList.Add(n);
                    }
                    currentNode.Content += (str + "\n");
                }
            }
            //return stream;
            if(!fileExist)
            {
                File.WriteAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\" + id, progressList);
            }
            return rootNode;
        }

        public void TestUpdate()
        {
            LoadFullFiles();
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {

                    if (file.Id == tagId)
                    {
                        //MessageBox.Show(file.Name);
                        //FolderItem item = new FolderItem();
                        ////tagId.Add(file.Id);
                        //if (file.Parents != null && file.Parents.Count > 0)
                        //{
                        //    //folderId.Add(file.Parents[0]);
                        //    item.Id = file.Parents[0];
                        //    item.Title = service.Files.Get(file.Parents[0]).Execute().Name;
                        //    //item.Type = file.Kind;
                        //    if (file.MimeType.Split('.').Length > 1)
                        //    {
                        //        item.Type = file.MimeType.Split('.')[1];
                        //    }
                        //    else
                        //    {
                        //        item.Type = file.MimeType;
                        //    }
                        //}
                        //else
                        //{
                        //    //folderId.Add("");
                        //}
                        //folderList.Add(item);
                        Google.Apis.Drive.v3.Data.File f = service.Files.Get(tagId).Execute();
                        Google.Apis.Drive.v3.Data.File f2 = new Google.Apis.Drive.v3.Data.File();
                        byte[] byteArray = System.IO.File.ReadAllBytes("C:\\Users\\Magun\\source\\repos\\GDCS_Programmer\\Build_Progress_Bar\\C#\\BuildProgressBar\\Credential.cs");
                        //var stream = new MemoryStream();
                        var stream = new MemoryStream(byteArray);
                        //var writer = new StreamWriter(stream);
                        //writer.Write("PPAP\n");
                        //writer.Flush();
                        //stream.Position = 0;
                        f2.Parents = new List<string>();
                        f2.Parents.Add(folderId);
                        MessageBox.Show(f2.Parents.Count.ToString());
                        
                        Google.Apis.Drive.v3.FilesResource.UpdateMediaUpload request = service.Files.Update(f2, tagId, stream, f.MimeType);
                        request.SupportsTeamDrives = true;
                        request.Upload();

                        Google.Apis.Drive.v3.Data.File updatedFile = request.ResponseBody;
                        if (updatedFile != null)
                        {
                            MessageBox.Show(updatedFile.ToString());
                        }
                        //Google.Apis.Drive.v3.Data.File f = service.Files.Get(tagId).Execute();
                        ////var per = f.Permissions[1];
                        //var per = file.Permissions[2];
                        
                        //MessageBox.Show(string.Format("{0} {1} {2}", per.EmailAddress, per.Id, credential.UserId));
                        //PermissionsResource.DeleteRequest deleteRequest = service.Permissions.Delete(tagId, per.Id);

                        //deleteRequest.Execute();
                    }
                    //Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }

        }

        public void LoadGoogleDocs()
        {
            /*
            string[] Scopes = { DriveService.Scope.DriveReadonly };
            IList<Google.Apis.Drive.v3.Data.File> Files = new List<Google.Apis.Drive.v3.Data.File>();
            string ApplicationName = "Drive API .NET Quickstart";

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
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            */
            // Define parameters of request.
            /*
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {

                    if (file.Name == g_TagFileName)
                    {
                        tagId = file.Id;
                        if (file.Parents != null && file.Parents.Count > 0)
                        {
                            folderId = file.Parents[0];
                        }
                    }
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            */
            /*
            if (tagId.Count != 0)
            {
                {
                    var request1 = service.About.Get();
                    request1.Fields = "user ";

                    var data1 = request1.Execute();
                    System.Reflection.PropertyInfo[] properties = typeof(Google.Apis.Drive.v3.Data.User).GetProperties();
                    foreach (System.Reflection.PropertyInfo property in properties)
                    {
                        Console.WriteLine("{0} = {1}\n", property.Name, property.GetValue(data1.User, null));
                    }
                }

                Google.Apis.Drive.v3.Data.File file = service.Files.Get(tagId[0]).Execute();

                //returnstring += string.Format("{0} ({1})\n", file.Name, file.Id);

                var request = service.Files.Export(tagId[0], "text/plain");
                var stream = new System.IO.MemoryStream();

                request.MediaDownloader.ProgressChanged +=
                    (Google.Apis.Download.IDownloadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case Google.Apis.Download.DownloadStatus.Downloading:
                                {
                                    //returnstring += progress.BytesDownloaded;
                                    break;
                                }
                            case Google.Apis.Download.DownloadStatus.Completed:
                                {
                                    //returnstring += "Download complete.";
                                    break;
                                }
                            case Google.Apis.Download.DownloadStatus.Failed:
                                {
                                    //returnstring += "Download failed.";
                                    break;
                                }
                        }
                    };
                request.Download(stream);


            }
            else
            {
                Console.WriteLine("No files found.");
            }
            */
        }
    }

    public class FolderItem
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
    }

    public class DocumentNode
    {
        public string Title;
        public string Content;
        public List<DocumentNode> Childs;
        public DocumentNode Parent;
        public List<int> Index;
        public int State;
        public string documentId;

        public DocumentNode(string title = "", string content = "", DocumentNode parent = null, List<int> index = null, int state = 0, string id = "")
        {
            Title = title;
            Content = content;
            Childs = new List<DocumentNode>();
            Parent = parent;
            if(index != null)
            {
                Index = new List<int>(index);
            }
            else
            {
                index = null;
            }
            State = state;
            documentId = id;
        }

        public void ChangeState(int newState)
        {
            State = newState;
            if(newState == 2)
            {
                List<string> progressList = new List<string>();
                progressList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\" + documentId).ToList();
                for (int i = 0; i < progressList.Count; i++)
                {
                    if (Index != null)
                    {
                        if (progressList[i].Split('\t')[0] == string.Join(".", Index))
                        {
                            progressList[i] = string.Format("{0}\t{1}", string.Join(".", Index), newState);
                        }
                    }
                }
                File.WriteAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\" + documentId, progressList);
                List<string> lineList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag").ToList();

                for (int i = 0; i < lineList.Count; i++)
                {
                    string[] elements = lineList[i].Split('\t');
                    if (Index != null)
                    {
                        if (elements[0] == documentId && string.Join(".", Index) == elements[3])
                        {
                            elements[4] = "1";
                            string str = String.Join("\t", elements);
                            lineList[i] = str;
                        }
                    }
                    else
                    {
                        if (elements[0] == documentId && "All" == elements[3])
                        {
                            elements[4] = "1";
                            string str = String.Join("\t", elements);
                            lineList[i] = str;
                        }
                    }
                }
                File.WriteAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\_CodeTag", lineList.ToArray());

                if (Parent != null)
                {
                    if(Parent.State == 0)
                    {
                        Parent.ChangeState(1);
                    }
                }
                foreach(DocumentNode node in Childs)
                {
                    node.ChangeState(2);
                }
            }
            else if(newState == 1 || newState == 0)
            {
                List<string> progressList = new List<string>();
                progressList = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Tags\\" + documentId).ToList();
                for (int i = 0; i < progressList.Count; i++)
                {
                    if (Index != null)
                    {
                        if (progressList[i].Split('\t')[0] == string.Join(".", Index))
                        {
                            progressList[i] = string.Format("{0}\t{1}", string.Join(".", Index), newState);
                        }
                    }
                }
                if (Parent != null)
                {
                    Parent.ChangeState(1);
                }

            }

        }
    }
}
