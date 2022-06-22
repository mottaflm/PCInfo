using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCInfo.Classes
{
    class Computer
    {

        //Classe computador para armazenar os dados.
        public Computer() 
        {
            LocalUsers = new List<string>();
        }

        public Computer(string computerName)
        {
            Name = computerName;
            LocalUsers = new List<string>();
        }

        public bool IsConnected { get; set; }
        public string Name { get; set; }
        public string LoggedUser { get; set; }
        public List<string> LocalUsers { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string ServiceTag { get; set; }
        public string OSName { get; set; }
        public string OSVersion { get; set; }
        public DateTime OSInstallDate { get; set; }
        public DateTime OSBootTime { get; set; }
        public TimeSpan OSUptime { get; set; }
        public double DiskSize { get; set; }
        public double DiskUsedSpace { get; set; }
        public double DiskFreeSpace { get; set; }
        public string CPUName { get; set; }
        public double CPUClock { get; set; }
        public byte CPUPercent { get; set; }
        public double TotalMemory { get; set; }
        public double FreeMemory { get; set; }
        public double UsedMemory { get; set; }
        public double NetworkUsage { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }

    }
}
