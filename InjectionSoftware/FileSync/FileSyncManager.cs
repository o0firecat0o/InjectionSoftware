using InjectionSoftware.Class;
using InjectionSoftware.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



            FileSystemWatcher watcher = new FileSystemWatcher();
            try
            {
                //          \\DESKTOP-5KHCAT7\\Temp
                watcher.Path = WindowConfig.NetworkFolderDirectory;
                watcher.IncludeSubdirectories = true;

                watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;


                watcher.Filter = "*.*";

                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.Deleted += new FileSystemEventHandler(OnChanged);

                watcher.EnableRaisingEvents = true;
            }
            catch (System.ArgumentException e)
            {
                Console.WriteLine(e);
            }
            
        }

        public static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed.  
            Console.WriteLine("{0}, with path {1} has been {2}", e.Name, e.FullPath, e.ChangeType);
        }
    }

}
