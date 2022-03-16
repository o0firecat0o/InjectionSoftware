using InjectionSoftware.Class;
using InjectionSoftware.Util;
using InjectionSoftware.Util.Scheduler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.FileSync
{
    public class SchedularSyncManager
    {
        public static void Init()
        {
            string date = DateTime.Now.ToString("ddMMyyyy");
            string fullpath = System.IO.Path.Combine(WindowConfig.NetworkFolderDirectory, "InjectionSoftware", date, "schedular");

            LoadInitial();

            FileSystemWatcher FileWatcher = new FileSystemWatcher();
            try
            {
                //          \\DESKTOP-5KHCAT7\\Temp
                FileWatcher.Path = fullpath;
                FileWatcher.IncludeSubdirectories = false;

                FileWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;


                FileWatcher.Filter = "*.hl7*";

                FileWatcher.Changed += new FileSystemEventHandler(OnChanged);
                FileWatcher.Created += new FileSystemEventHandler(OnChanged);

                FileWatcher.EnableRaisingEvents = true;

                Console.Out.WriteLine("[SchedularSyncManager] Created listerner sucessfuly");
            }
            catch (System.ArgumentException e)
            {
                Console.Out.WriteLine("[SchedularSyncManager] Failed to create listener");
                Console.WriteLine(e);
            }
        }

        public static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed.  
            Console.WriteLine("[SchedularSyncManager]" + "{0}, with path {1} has been {2}", e.Name, e.FullPath, e.ChangeType);

            ReadFileImportPatient(e.FullPath);
        }

        public static void LoadInitial()
        {
            string date = DateTime.Now.ToString("ddMMyyyy");
            string fullpath = System.IO.Path.Combine(WindowConfig.NetworkFolderDirectory, "InjectionSoftware", date, "schedular");

            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }

            var fileNames = Directory.EnumerateFiles(fullpath, "*.hl7", SearchOption.TopDirectoryOnly);
            Console.WriteLine("[SchedularSyncManager/loadInitial()] There are a total of: " + fileNames.Count() + "files to load initially.");

            foreach (string file in fileNames)
            {
                ReadFileImportPatient(file);
            }
        }

        private static int tries = 5;

        public static void ReadFileImportPatient(string file)
        {
            //Please Remeber to put this line back to the init series
            //PatientManager.LoadAllPatient();

            string text;

            try
            {
                text = System.IO.File.ReadAllText(file);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("[SchedularSyncManager/loadInitial()] there is error loading the initial files with file directory: " + file);
                Console.Error.WriteLine(e);

                Console.WriteLine("[SchedularSyncManager/loadInitial()] retrying for " + tries + " times left.");

                if (tries > 0)
                {
                    tries--;
                    ReadFileImportPatient(file);
                }

                return;
            }

            tries = 5;

            Hl7file ff = Hl7file.load(text);

            Patient patient = new Patient(ff);

            MainWindow.window.Dispatcher.Invoke(() =>
            {
                PatientManager.ModPatient(patient);
            });

        }
    }
}
