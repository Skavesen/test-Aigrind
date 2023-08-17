using FileManager.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Directory = FileManager.Models.Directory;

namespace FileManager.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public ObservableCollection<Directory> Directories { get; } = new ObservableCollection<Directory>();

        private Directory selectedDirectory;

        public Directory SelectedDirectory
        {
            get { return selectedDirectory; }
            set
            {
                SetProperty(ref selectedDirectory, value);
                try
                {
                    SelectedDirectory?.LoadData();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                RaisePropertyChanged(nameof(FileCount));
                RaisePropertyChanged(nameof(AllFilesAndDirs));
                RaisePropertyChanged(nameof(FilesSize));

                RaisePropertyChanged(nameof(CurrentDisc));
                RaisePropertyChanged(nameof(DiskFilling));
                RaisePropertyChanged(nameof(FreeSpaceOnDisc));

                RaisePropertyChanged(nameof(ExtensionFilters));
                ExtensionFilter = "All Files";
            }
        }

        private IFileable _selectedItem;
        public IFileable SelectedItem { get => _selectedItem; set 
            {
                SetProperty(ref _selectedItem, value);   
            } }

        public string ExtensionFilter { get => SelectedDirectory.ShowFilter; set
            {
                if (SelectedDirectory != null)
                {
                    SelectedDirectory.ShowFilter = value;
                    RaisePropertyChanged(nameof(AllFilesAndDirs));
                }
            } }

        public List<string> ExtensionFilters { get
            {
                if (SelectedDirectory != null)
                    return SelectedDirectory.GetFileExtensions();
                else
                    return new List<string> { "All Files" };
            } }

        public List<IFileable> AllFilesAndDirs { get {
                if (SelectedDirectory != null)
                    return SelectedDirectory.AllFilesAndDirs.ToList();
                else
                    return new List<IFileable>();
            } }

        public List<IFileable> AllFiles { get {
                if (SelectedDirectory != null)
                    return SelectedDirectory.Files.ToList();
                else
                    return new List<IFileable>();
            } }

        private List<DriveM> driveMs = DriveInfo.GetDrives().Select(x=>new DriveM(x)).ToList();

        public DriveM CurrentDisc { get {
                if (SelectedDirectory != null)
                    return driveMs.Find(x=>x.DiscName == Path.GetPathRoot(SelectedDirectory.FullName)) ?? driveMs[0];
                else
                    return driveMs[0];
            }
        }
        public string FreeSpaceOnDisc { get {

                return $"{CurrentDisc.AvailableFreeSpace / 1073741824} Gb свободно из {CurrentDisc.TotalSpace / 1073741824} Gb";

        } }

        public double DiskFilling { get => 100d - (CurrentDisc.AvailableFreeSpace * 1d / CurrentDisc.TotalSpace) * 100; }

        public int FileCount { get => AllFiles.Count(); }
        public long FilesSize { get => AllFiles.Select((x) => x.Size).Sum(); }

        public DelegateCommand<Directory> SelectedDirectoryCommand { get;}
        public DelegateCommand<IFileable> OpenDirectoryCommand { get;}
        public DelegateCommand<string> RenameFileCommand { get; }
        public DelegateCommand DeleteFileCommand { get; }

        public MainViewModel()
        {
            foreach (var item in Directory.GetDirectoryDrives())
            {
                Directories.Add(item);
            }
            SelectedDirectory = Directories[0];

            SelectedDirectoryCommand = new DelegateCommand<Directory>((d) => { 
                SelectedDirectory = d;
                SelectedItem = SelectedDirectory;
            });

            OpenDirectoryCommand = new DelegateCommand<IFileable>((d) => {
                if (d == null) return;
                if(d is Directory dir)
                {
                    SelectedDirectory.IsExpanded = true;
                    SelectedDirectory = dir;
                    SelectedDirectory.IsExpanded = true;
                }
                if (d is Models.File fil)
                {
                    var psi = new ProcessStartInfo(fil.FullName) { UseShellExecute = true };
                    Process.Start(psi);
                }
            });

            RenameFileCommand = new DelegateCommand<string>(RenameSelectedFileable);
            DeleteFileCommand = new DelegateCommand(DeleteSelectedFileable);
        }

        private void DeleteSelectedFileable()
        {
            try
            {
                SelectedItem.Delete();
                RaisePropertyChanged(nameof(AllFilesAndDirs));
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка при удалении", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void RenameSelectedFileable(string n)
        {
            if (n == null || n == String.Empty || SelectedItem == null) return;

            if (Regex.IsMatch(n, @"(\\|/|<|>|\?|:|\||\*)"))
            {
                MessageBox.Show("Имя не должно содержать символы : /,\\,|,<,>,*,:", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                SelectedItem.Rename(n);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка при переименовании", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
