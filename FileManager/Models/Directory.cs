using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace FileManager.Models
{
    public class Directory : TreeViewItemBase, IFileable
    {
        private string _name;
        private string _fullName;
        private DirectoryInfo directoryInfo;

        public Directory? Parent { get;}

        public ObservableCollection<Directory> Children { get; } = new ObservableCollection<Directory>();

        public ObservableCollection<IFileable> Files { get; } = new ObservableCollection<IFileable>();

        private string _showFilter = "All Files";
        public string ShowFilter { get => _showFilter; set { SetProperty(ref _showFilter, value); } }

        public ObservableCollection<IFileable> AllFilesAndDirs { get => GetAllFilesAndDirectories(ShowFilter); }

        public Directory(string fullName , Directory parent)
        {
            this.Parent = parent;
            FullName = fullName;
            
            if (FullName == "*")
                return;
            if(Children.Count == 0)
            Children.Add(new Directory("*", this));

            directoryInfo = new DirectoryInfo(FullName);

            Name = directoryInfo.Name;

        }


        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public DateTime LastWriteTime { get {
                if (directoryInfo != null)
                    return directoryInfo.LastWriteTime;
                else 
                    return new DateTime(1999, 1, 1); }}

        public string FullName
        {
            get { return _fullName; }
            private set
            {

                _fullName = value;
                RaisePropertyChanged("FullName");
            }
        }

        public long Size { get => 0; }

        public bool IsDirectory => true;

        public static ObservableCollection<Directory> GetDirectoryDrives()
        {
            ObservableCollection<Directory> directories = new ObservableCollection<Directory>();
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            foreach (DriveInfo driveInfo in driveInfos)
            {
                directories.Add(new Directory(driveInfo.Name, null));
            }
            return directories;
        }
        public override void ClearData()
        {
            if (IsSelected)
                return;

            Children.Clear();
            Files.Clear();
            Children.Add(new Directory("*", this));

        }
        public override void LoadData()
        {
            Children.Clear();
            
            Files.Clear();
    
                foreach (DirectoryInfo subDir in directoryInfo.GetDirectories())
                {
                    Directory directory = new Directory(subDir.FullName, this);
                    Children.Add(directory);
                }
                Files.AddRange(GetAllFiles());
     
        }

        public ObservableCollection<IFileable> GetAllFiles()
        {
            ObservableCollection<IFileable> files = new ObservableCollection<IFileable>();

            
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                    IFileable f = new File(file.FullName, this);
                    files.Add(f);
            }
            
            return files;
        }

        public ObservableCollection<IFileable> GetAllFilesAndDirectories(string filter = "")
        {
            ObservableCollection<IFileable> fils = new ObservableCollection<IFileable>();


            if(filter == "All Files")
            fils.AddRange(Children);

            if (filter != "All Files")
            {
                fils.AddRange(Files.ToList().Where((x) =>
                {
                    return x.FileExtention == filter;
                }));
            }
            else
                fils.AddRange(Files);

            return fils;
        }

        public List<string> GetFileExtensions()
        {
            List<string> extensions = new List<string>() {"All Files"};

            Files.ToList().ForEach((x) =>
            {
                string ext = ((File)x).FileExtention;
                if (ext != String.Empty && !extensions.Contains(ext))
                {
                    extensions.Add(ext);
                }
            });
            return extensions;
        }

        public void Rename(string newName)
        {
            DirectoryInfo? parent = directoryInfo.Parent;
            if (parent == null)
                throw new DirectoryNotFoundException($"Parent of directory {Name} not found");

            directoryInfo.MoveTo(Path.Combine(parent.FullName, newName));
            UpdateNames();
        }

        public void Delete()
        {
            directoryInfo.Delete();
            Parent?.Children.Remove(this);
        }

        void UpdateNames()
        {
            FullName = directoryInfo.FullName;
            Name = directoryInfo.Name;
        }
    }
}
