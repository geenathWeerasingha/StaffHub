using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmDemo.Models;
using MvvmDemo.Commands;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows;
using Microsoft.Win32;
using System.IO;

namespace MvvmDemo.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged!=null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        
        private IEmployeeService ObjEmployeeService;

        public EmployeeViewModel()
        {
        }

        public EmployeeViewModel(IEmployeeService employeeService)
        {
            ObjEmployeeService = employeeService;
            LoadData();
            CurrentEmployee=new Employee();
            SelectedEmployee=new Employee();
            saveCommand = new RelayCommand(Save);
            searchCommand=new RelayCommand(Search);
            updateCommand=new RelayCommand(Update);
            deleteCommand = new RelayCommand(Delete);
            viewCommand = new RelayCommand(View);
            uploadImageCommand = new RelayCommand(Upload);
        }

        private ObservableCollection<Employee> employeesList;

        public ObservableCollection<Employee> EmployeesList
        {
            get { return employeesList; }
            set { employeesList = value; OnPropertyChanged("EmployeesList"); }
        }

        private void LoadData()
        {
          
            EmployeesList =new ObservableCollection<Employee>(ObjEmployeeService.GetAll());
            System.Diagnostics.Debug.WriteLine("QWERTY", EmployeesList);
        }



        private Employee currentEmployee;


        public Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value;

               
                OnPropertyChanged("CurrentEmployee");
              

              
            }
        }



        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }

        private string preference="MySql";

        public string Preference
        {
            get { return preference; }
            set
            {
                preference = value;
                System.Diagnostics.Debug.WriteLine(Preference);
                OnPropertyChanged("Preference");


                MainWindow mWin = new MainWindow();
                mWin.Close();

                MainWindow mainWindow = new MainWindow(Preference);
                mainWindow.Show();
              

                System.Diagnostics.Debug.WriteLine(Preference);

            }
        }

        private RelayCommand saveCommand;

        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
        }


        public void Save()
        {
            try
            {

                if (ValidateEmployee())
                {
                    var IsSaved = ObjEmployeeService.Add(CurrentEmployee);
                    LoadData();

                    if (IsSaved)
                        Message = "Employee saved";

                    else
                        Message = "Save operation failed";
                }

               
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }


        private bool ValidateEmployee()
        {
           return   CurrentEmployee.ValidateAllProperties();
        }



        private RelayCommand searchCommand;

        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
            
        }

        public void Search()
        {
            try
            {
                var ObjEmployee=ObjEmployeeService.Search(CurrentEmployee.Id);
                if(ObjEmployee!=null)
                {
                    CurrentEmployee.Name= ObjEmployee.Name;
                    CurrentEmployee.Age = ObjEmployee.Age;
                    CurrentEmployee.DateOfJoined = ObjEmployee.DateOfJoined;
                    CurrentEmployee.ImagePath = ObjEmployee.ImagePath;
                    CurrentEmployee.Salary= ObjEmployee.Salary;
                }
                else
                {
                    Message = "Employee Not found";
                }
            }
            catch(Exception ex)
            {

            }
        }



        private RelayCommand updateCommand;

        public RelayCommand UpdateCommand
        {
            get { return updateCommand; }
             
        }

        public void Update()
        {
            try
            {
                var IsUpdated = ObjEmployeeService.Update(CurrentEmployee);

                if(IsUpdated)
                {
                    Message = "Employee Updated";
                    LoadData();
                }
                else
                {
                    Message = "Update Operation Failed";
                }

            }catch(Exception ex)
            {
                Message = ex.Message;
            }
        }



        private RelayCommand deleteCommand;

        public RelayCommand DeleteCommand
        {
            get { return deleteCommand; }
          
        }

        public void Delete()
        {
            try
            {
                var IsDelete = ObjEmployeeService.Delete(CurrentEmployee.Id);
                if(IsDelete)
                {
                    Message = "Employee deleted";
                    LoadData();
                }
                else
                {
                    Message = "Delete operation failed";
                }
            } 
            catch(Exception ex)
            {
                Message = ex.Message;
            }
        }


        private RelayCommand uploadImageCommand;

        public RelayCommand UploadImageCommand
        {
            get { return uploadImageCommand; }
            set { uploadImageCommand = value; }
        }

        public void Upload()
        {
 
           try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == true)
                {
                    byte[] imageData = File.ReadAllBytes(openFileDialog.FileName);
                    CurrentEmployee.ImagePath = imageData;
                    System.Diagnostics.Debug.WriteLine("IMAGEPATH" + CurrentEmployee.ImagePath);

                    OnPropertyChanged("ImagePath");
                
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

        }



        private RelayCommand viewCommand;

        public RelayCommand ViewCommand
        {
            get { return viewCommand; }
             
        }

        private Employee selectedEmployee;

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }


        private void View()
        {

            System.Diagnostics.Debug.WriteLine("View" + SelectedEmployee.Name);


            if (SelectedEmployee != null)
            {
                ShowEmplyeeDetailsWindow(SelectedEmployee);
            }
        }

        private void ShowEmplyeeDetailsWindow(Employee employee)
        {
            System.Diagnostics.Debug.WriteLine("ShowEmplyeeDetailsWindow" + employee.Name);

            MessageBox.Show($"ID: {employee.Id}\nName:{employee.Name}\nAge:{employee.Age}\nDate of joined:{employee.DateOfJoined}","Employee Details",MessageBoxButton.OK,MessageBoxImage.Information);
        }

      

       

      




    }
}
