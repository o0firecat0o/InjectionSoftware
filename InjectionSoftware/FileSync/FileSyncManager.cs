using InjectionSoftware.Class;
using InjectionSoftware.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace InjectionSoftware.FileSync
{
    public class FileSyncManager
    {
        public static void Init()
        {
            PatientManager.LoadAllPatientFromSchedular();
            PatientManager.LoadAllPatient();

            string date = DateTime.Now.ToString("ddMMyyyy");
            string fullpath = WindowConfig.NetworkFolderDirectory + @"\InjectionSoftware\" + date + @"\injection\";

            //load all the injection after starting server, the client will load the injection via contacting with server
            InjectionsManager.loadAllInjections(fullpath);



            FileSystemWatcher InjectionWatcher = new FileSystemWatcher();
            try
            {
                //          \\DESKTOP-5KHCAT7\\Temp
                InjectionWatcher.Path = WindowConfig.NetworkFolderDirectory + @"\InjectionSoftware\" + date + @"\injection\";
                InjectionWatcher.IncludeSubdirectories = true;

                InjectionWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;


                InjectionWatcher.Filter = "*.*";

                InjectionWatcher.Changed += new FileSystemEventHandler(OnChanged);
                InjectionWatcher.Created += new FileSystemEventHandler(OnChanged);
                InjectionWatcher.Deleted += new FileSystemEventHandler(OnChanged);

                InjectionWatcher.EnableRaisingEvents = true;
            }
            catch (System.ArgumentException e)
            {
                Console.WriteLine(e);
            }
            
        }

        public static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed.  
            Console.WriteLine("[FileSyncManager]"+"{0}, with path {1} has been {2}", e.Name, e.FullPath, e.ChangeType);


            try
            {
                using (var textReader = new XmlTextReader(e.FullPath))
                {
                    XElement xElement = XElement.Load(textReader);
                    MainWindow.window.Dispatcher.Invoke((Action)(() => InjectionsManager.modInjection(xElement)));
                }
            }
            catch (System.Exception)
            {
                Thread.Sleep(200);
                OnChanged(source, e);
            }
            finally
            {
            }
        }

        public static void OnDeleted(object source, FileSystemEventArgs e)
        {
            
        }
    }

}
