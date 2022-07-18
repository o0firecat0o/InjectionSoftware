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

            Console.Out.WriteLine("[SchedularCopyManager] Loading initial file @" + WindowConfig.SchedularDirectory);

            loadInitial();

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
                //FileWatcher.Created += new FileSystemEventHandler(OnChanged);

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

            FileValidationAndCopy(e.FullPath);
        }

        public static void loadInitial()
        {
            string fullpath = WindowConfig.SchedularDirectory;

            DateTime today = DateTime.Now.Date;           

            DirectoryInfo DirInfo = new DirectoryInfo(fullpath);

            // LINQ query for all files created before 2009.
            var files = from f in DirInfo.EnumerateFiles("*.hl7",SearchOption.TopDirectoryOnly)
                        where f.CreationTimeUtc > today
                        select f;          

            Console.WriteLine("[SchedularCopyManager/loadInitial()] There are a total of: " + files.Count() + "files to load initially.");

            foreach (var f in files)
            {
                FileValidationAndCopy(fullpath +f.Name);
            }


            //var fileNames = Directory.EnumerateFiles(fullpath, "*.hl7", SearchOption.TopDirectoryOnly);
            //Console.WriteLine("[SchedularCopyManager/loadInitial()] There are a total of: " + fileNames.Count() + "files to load initially.");

            //foreach (string file in fileNames)
            //{
            //   FileValidationAndCopy(file);
            //}
        }



        private static void FileValidationAndCopy(string file)
        {
            string date = DateTime.Now.ToString("ddMMyyyy");
            string destinationPath = System.IO.Path.Combine(WindowConfig.NetworkFolderDirectory, "InjectionSoftware", date, "schedular");

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            //only read files from today
            if (!file.Contains(todayDateString))
            {
                return;
            }

            string text = "";

            try
            {
                text = System.IO.File.ReadAllText(file);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("[SchedularCopyManager/loadInitial()] there is error loading the initial files with file directory: " + file);
                Console.Error.WriteLine(e);
                return;
            }

            //sometime booker willl register patients not from today but with file name of today
            //the following segment will get rid of all those patients

            Hl7file ff = Hl7file.load(text);
            if (!ff.getSegment("ORC").getString(15).Contains(todayDateString))
            {
                return;
            }

            Patient patient = new Patient(ff);

            if (!(patient.ExamCode.Contains("PO") || patient.ExamCode.Contains("NM") || patient.ExamCode.Contains("PI")))
            {
                return;
            }

            if (patient.PatientID == "")
            {
                Console.WriteLine("[SchedularCopyManager/loadInitial()] " + file + " path contain corrupted information, the patient information has failed to load");
                return;
            }

            string filenameWithoutPath = Path.GetFileName(file);

            try
            {
                File.Copy(file, destinationPath + "/" + filenameWithoutPath, true);
            }
            catch (System.IO.IOException e)
            {
                Console.Out.WriteLine("[SchedularCopyManager]" + e);
            }

            Console.Out.WriteLine("[SchedularCopyManager/FileValidationAndCopy()] Loading patient information from HL7 file with patientID:" + patient.PatientID);
            Console.Out.WriteLine("[SchedularCopyManager/FileValidationAndCopy()] The patient referring phys and file directories are: " + patient.ExamCode + " , " + file);
            Console.WriteLine("[SchedularCopyManager/FileValidationAndCopy()]" + " Copying file... from " + file + " to " + destinationPath + "/" + filenameWithoutPath);

            //PatientManager.ModPatient(patient);
        }
    }
}


