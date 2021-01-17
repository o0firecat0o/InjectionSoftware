using System;
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

        public double WindowHeight = 0;
        public double WindowWidth = 0;
        public double WindowTop = 0;
        public double WindowLeft = 0;
        public WindowState WindowState = WindowState.Normal;

        public WindowConfig()
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

        public void Load()
        {
            string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + "config.xml";
            XElement xElement = XElement.Load(fullpath);

            XNamespace df = xElement.Name.Namespace;

            WindowHeight = double.Parse(xElement.Element(df + "WindowHeight").Value);
            WindowWidth = double.Parse(xElement.Element(df + "WindowWidth").Value);
            WindowTop = double.Parse(xElement.Element(df + "WindowTop").Value);
            WindowLeft = double.Parse(xElement.Element(df + "WindowLeft").Value);
            if (xElement.Element(df + "WindowState").Value == "Maximized")
            {
                WindowState = WindowState.Maximized;
            }
        }

        public void Save()
        {
            Console.WriteLine("[WindowConfig] saving config value");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\"  + "config.xml";


            XElement config = new XElement("config");

            XElement windowHeight = new XElement("WindowHeight", WindowHeight);
            XElement windowWidth = new XElement("WindowWidth", WindowWidth);
            XElement windowTop = new XElement("WindowTop", WindowTop);
            XElement windowLeft = new XElement("WindowLeft", WindowLeft);
            XElement windowState = new XElement("WindowState", this.WindowState);

            config.Add(windowHeight);
            config.Add(windowWidth);
            config.Add(windowTop);
            config.Add(windowLeft);
            config.Add(windowState);


            config.Save(fullpath);
        }
    }
}
