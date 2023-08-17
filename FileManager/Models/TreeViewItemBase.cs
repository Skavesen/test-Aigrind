using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.Models
{
    public abstract class TreeViewItemBase : BindableBase
    {

        private bool isSelected;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;


                RaisePropertyChanged("IsSelected");
            }
        }

        private bool isExpanded;

        public bool IsExpanded
        {
            get { return this.isExpanded; }
            set
            {
                if (this.isExpanded == value)
                    return;

                    this.isExpanded = value;
                    if (IsExpanded)
                    {
                        try
                        {
                            LoadData();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {

                        ClearData();  
                    }

                    RaisePropertyChanged("IsExpanded");      
            }
        }
        public abstract void LoadData();
        public abstract void ClearData();

    }
}
