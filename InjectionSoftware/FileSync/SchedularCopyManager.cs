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

    /// <summary>
    /// Used to copy the newly registered hl7 file to the Temp drive for all devices to access the hl7 file through T drive individually
    /// Done by the server only
    /// </summary>
    public class SchedularCopyManager
    {
        //yymmdd
        static string todayDateString = "200730";

        public static void Init()
        {
            //Calculate todayDate, to prevent loading stuff from yesterday
            todayDateString = System.DateTime.Now.ToString("yyMMdd");

            Console.Out.WriteLine("[SchedularCopyManager] Creating listerner @" + WindowConfig.SchedularDirectory);

            FileSystemWatcher FileWatcher = new FileSystemWatcher();
            try
            {
                //          \\DESKTOP-5KHCAT7\\Temp
                FileWatcher.Path = WindowConfig.SchedularDirectory;
                FileWatcher.IncludeSubdirectories = false;

                FileWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;


                FileWatcher.Filter = "*.hl7*";

                FileWatcher.Changed += new FileSystemEventHandler(OnChanged);
                FileWatcher.Created += new FileSystemEventHandler(OnChanged);

                FileWatcher.EnableRaisingEvents = true;

                Console.Out.WriteLine("[SchedularCopyManager] Created listerner sucessfuly");
            }
            catch (System.ArgumentException e)
            {
                Console.Out.WriteLine("[SchedularCopyManager] Failed to create listener");
                Console.WriteLine(e);
            }
        }

        public static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed.  
            Console.WriteLine("[SchedularCopyManager]" + "{0}, with path {1} has been {2}", e.Name, e.FullPath, e.ChangeType);

            //TODO: filter out non-Pet patient and do not copy them
            //TODO: copy all initial files, ?but do not replace them if they are already present @ Temp Drive/ Schedular

            if (e.Name.Contains(todayDateString))
            {
                Console.WriteLine("[SchedularCopyManager]" + e.Name +" is considered added today and modified recently, proceed to copy it to Temp Drive.");

                MainWindow.window.Dispatcher.Invoke(() => {
                    string text = System.IO.File.ReadAllText(e.FullPath);
                    Hl7file ff = Hl7file.load(text);
                    Patient patient = new Patient(ff);

                    PatientManager.ModPatient(patient);
                });                
            }

            //Please Remeber to put this line back to the init series
            //PatientManager.LoadAllPatientFromSchedular();
            //PatientManager.LoadAllPatient();
        }
    }
}
