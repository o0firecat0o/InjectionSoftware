﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InjectionSoftware.Util
{
    public class WindowConfig
    {

        public static double WindowHeight = 0;
        public static double WindowWidth = 0;
        public static double WindowTop = 0;
        public static double WindowLeft = 0;
        public static WindowState WindowState = WindowState.Normal;

        public static int IsAutoRestart = 0;
        public static int IsServer = 0;

        public static string NetworkFolderDirectory = "";

        public static int IsFileSyncServer = 0;
        //newly added schedular will be added here, then copy to T drive for assess of other computers
        public static string SchedularDirectory = "";

        public static void Init()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullpath = path + @"config.xml";
            if (!File.Exists(fullpath))
            {
                Console.WriteLine("[WindowConfig] creating new window config file and saving default values");
                Save();
            }
            else
            {
                Console.WriteLine("[WindowConfig] loading previously saved window value");
                Load();
            }
        }

        public static void Load()
        {
            string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + "config.xml";
            XElement xElement = XElement.Load(fullpath);

            XNamespace df = xElement.Name.Namespace;

            WindowHeight = double.Parse(xElement.Element(df + "WindowHeight").Value);
            WindowWidth = double.Parse(xElement.Element(df + "WindowWidth").Value);
            WindowTop = double.Parse(xElement.Element(df + "WindowTop").Value);
            WindowLeft = double.Parse(xElement.Element(df + "WindowLeft").Value);
            IsAutoRestart = int.Parse(xElement.Element(df + "IsAutoRestart").Value);
            IsServer = int.Parse(xElement.Element(df + "IsServer").Value);
            if (xElement.Element(df + "WindowState").Value == "Maximized")
            {
                WindowState = WindowState.Maximized;
            }
            if (xElement.Elements(df + "NetworkFolderDirectory").Any())
            {
                NetworkFolderDirectory = xElement.Element(df + "NetworkFolderDirectory").Value;
            }
            else
            {
                Console.WriteLine("[WindowConfig] Could not locate previous network directory, creating a temp one at C:/TempInjectionSoftware");
                Console.WriteLine("[WindowConfig] Please specify a default directory at the config file");
                NetworkFolderDirectory = @"C:\TempInjectionSoftware";
            }


            //File sync server will has the job to copy the newly added Hl7 file from O drive to T drive/schedular
            if (xElement.Elements(df + "IsFileSyncServer").Any())
            {
                IsFileSyncServer = int.Parse(xElement.Element(df + "IsFileSyncServer").Value);
            }
            else
            {
                Console.WriteLine("[WindowConfig] [minor] Could not locate (int) IsFileSyncServer @ windowconfig.xml file");
            }

            if (xElement.Elements(df + "SchedularDirectory").Any())
            {
                SchedularDirectory =xElement.Element(df + "SchedularDirectory").Value;
            }
            else
            {
                Console.WriteLine("[WindowConfig] [minor] Could not locate (bool) SchedularDirectory @ windowconfig.xml file");
            }

        }

        public static void Save()
        {
            Console.WriteLine("[WindowConfig] saving config value");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + "config.xml";


            XElement config = new XElement("config");

            XElement windowHeight = new XElement("WindowHeight", WindowHeight);
            XElement windowWidth = new XElement("WindowWidth", WindowWidth);
            XElement windowTop = new XElement("WindowTop", WindowTop);
            XElement windowLeft = new XElement("WindowLeft", WindowLeft);
            XElement windowState = new XElement("WindowState", WindowState);
            XElement isAutoRestart = new XElement("IsAutoRestart", IsAutoRestart);
            XElement isServer = new XElement("IsServer", IsServer);
            XElement networkFolderDirectory = new XElement("NetworkFolderDirectory", NetworkFolderDirectory);
            XElement isFileSyncServer = new XElement("IsFileSyncServer", IsFileSyncServer);
            XElement schedularDirectory = new XElement("SchedularDirectory", SchedularDirectory);

            config.Add(windowHeight);
            config.Add(windowWidth);
            config.Add(windowTop);
            config.Add(windowLeft);
            config.Add(windowState);
            config.Add(isAutoRestart);
            config.Add(isServer);
            config.Add(networkFolderDirectory);
            config.Add(isFileSyncServer);
            config.Add(schedularDirectory);

            config.Save(fullpath);
        }
    }
}
