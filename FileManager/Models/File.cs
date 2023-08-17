using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public class File : BindableBase, IFileable
    {

        private FileInfo fileInfo;
        private string _name;
        private string _fullName;


        public File( string fullname, Directory parent)
        {   
            FullName = fullname;
            fileInfo = new FileInfo(FullName);
            Name = fileInfo.Name;
            Parent = parent;
        }
        
        public string Name { get => _name; private set
        {
                _name = value;
                RaisePropertyChanged("Name");
        } }

        public string FullName { get => _fullName; private set
            {
                _fullName = value;
                RaisePropertyChanged("FullName");
            } }

        public long Size { get => fileInfo.Length; }

        public DateTime LastWriteTime { get => fileInfo.LastWriteTime; }

        public string FileExtention { get => fileInfo.Extension; }

        public bool IsDirectory => false;

        public Directory? Parent { get; }

        public void Delete()
        {
            fileInfo.Delete();
            Parent?.Files.Remove(this);
        }

        public void Rename(string newName)
        {
            fileInfo.MoveTo(Path.Combine(fileInfo.DirectoryName, newName));
            UpdateNames();
        }
        void UpdateNames()
        {
            FullName = fileInfo.FullName;
            Name = fileInfo.Name;
        }
    }
}
