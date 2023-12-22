using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmDemo.Models;

namespace MvvmDemo.ViewModels
{
    public class EmployeeViewMoreModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
 
    
        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged!=null)
                 PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        MsSqlEmployeeService ObjEmployeeService;

        public EmployeeViewMoreModel()
        {
            ObjEmployeeService = new MsSqlEmployeeService();
            CurrentEmployee = new Employee();


        }

        private Employee currentEmployee;


        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set
            {
                currentEmployee = value; OnPropertyChanged("CurrentEmployee");


            }
        }

    }
}
