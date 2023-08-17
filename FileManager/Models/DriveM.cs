using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public class DriveM : BindableBase
    {
        private DriveInfo drive;

        public DriveM(string driveName)
        {
            this.drive = DriveInfo.GetDrives().Where(x => x.Name == driveName).First();
        }
        public DriveM(DriveInfo drive)
        {
            this.drive = drive;
        }

        public string DiscName=> drive.Name;
        public long TotalFreeSpace => drive.TotalFreeSpace;
        public long TotalSpace => drive.TotalSize;
        public long AvailableFreeSpace => drive.AvailableFreeSpace;

        public override string ToString()
        {
            return DiscName;
        }
    }
}
