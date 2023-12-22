using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MvvmDemo.Models
{
    public class Employee : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        public event PropertyChangedEventHandler PropertyChanged;
     
        private void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

   

        public Employee()
        {
            DateOfJoined = DateTime.Now;

        }


        private int id;

        public int Id
        {
            get { return id; }
            set { id = value;
                ValidateProperty(value, nameof(Id));
                OnPropertyChanged("Id");
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value;

                ValidateProperty(value, nameof(Name));
                OnPropertyChanged("Name"); }
        }

        private int age;

        public int Age
        {
            get { return age; }
            set { age = value;
                ValidateProperty(value, nameof(Age));
                OnPropertyChanged("Age");
            }
        }

        private DateTime dateOfJoined;

        public DateTime DateOfJoined
        {
            get { return dateOfJoined; }
            set { dateOfJoined = value;
                ValidateProperty(value,nameof(DateOfJoined));
                OnPropertyChanged("DateOfJoined"); }
        }

        private byte[] imagePath;

        public byte[] ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; OnPropertyChanged("ImagePath"); }
        }


        private decimal salary;

        public decimal Salary
        {
            get { return salary; }
            set { salary = value;
                ValidateProperty(value, nameof(Salary));
                OnPropertyChanged("Salary"); }
        }

     
        /*    public string Error => null;

            public string this[string columnName]
            {
                get
                {
                    switch(columnName)
                    {
                        case nameof(Id):

                            if (Id <= 0)
                                return "Employee Id must be greater than 0";
                                break;

                        case nameof(Name):

                            if (String.IsNullOrEmpty(Name))
                                return "Name is required";
                                break;



                        case nameof(Salary):

                            if (Salary <= 0)
                                return "Employee Salary must be greater than 0";
                            break;



                        default:
                                 break;
                    }

                    return null;
                }
            } */



        public bool  ValidateAllProperties()
        {
            ValidateProperty(Id,nameof(Id));
            ValidateProperty(Name, nameof(Name));
            ValidateProperty(Age, nameof(Age));
            ValidateProperty(Salary, nameof(Salary));
            ValidateProperty(DateOfJoined, nameof(DateOfJoined)); 

            if (errors.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }





        private void ValidateProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {

            switch (propertyName)
            {
                case nameof(Age):

                    if (value == null)
                    {
                        AddError(propertyName,"Age cannot be null");
                    }

                    else if (!int.TryParse(value.ToString(),out int ageValue) )
                    {
                        AddError(propertyName,"Age should be a valid integer");
                    }
                    else
                    {
                        if (ageValue<20 || ageValue>60)
                        {
                            AddError(propertyName,"Age should be between 20 and 60");
                        }
                         else
                        {
                            RemoveError(propertyName);
                        }
                    }

                    break;

                case nameof(Name):

                    if (value==null)
                    {
                        AddError(propertyName,"Name required");
                    }

                    else if (!(value is string nameValue))
                    {
                        AddError(propertyName,"Name should be string");
                    }
                    else if (nameValue.Length < 3 || nameValue.Length > 50)
                    {
                        AddError(propertyName, "Name should have 3 to 50 characters");

                    }
                    else
                    {
                        RemoveError(propertyName);
                    }

                    break;

                case nameof(Id):

                    if (value == null)
                    {
                        AddError(propertyName, "Id cannot be null");
                    }

                    else if (!int.TryParse(value.ToString(), out int idValue))
                    {
                        AddError(propertyName, "Id should be a valid integer");
                    }
                    else
                    {
                        if (idValue <1000)
                        {
                            AddError(propertyName, "Id should be greater than 1000");
                        }
                        else
                        {
                            RemoveError(propertyName);
                        }
                    }

                    break;

                case nameof(Salary):

                    if (value == null)
                    {
                        AddError(propertyName, "Salary cannot be null");
                    }

                    else if (!decimal.TryParse(value.ToString(), out decimal salaryValue))
                    {
                        AddError(propertyName, "Salary should be a valid integer");
                    }
                    else
                    {
                        if (salaryValue < 20000  )
                        {
                            AddError(propertyName, "Salary should be greater than 20000");
                        }
                        else
                        {
                            RemoveError(propertyName);
                        }
                    }

                    break;

                case nameof(DateOfJoined):

                    if (value == null)
                    {
                        AddError(propertyName, "Date of joined cannot be null");
                    }
                    else if(!(value is DateTime dateOfJoinedvalue))
                    {
                        AddError(propertyName,"Date of joined should be a valid DateTime ");
                    }
                    else
                    {
                        DateTime today = DateTime.Today.Date;

                        if (dateOfJoinedvalue.Date>today)
                        {
                            System.Diagnostics.Debug.WriteLine("GGGGGG"+dateOfJoinedvalue+"  "+today);

                            AddError(propertyName,"Date of joined should not be greater than today's date");
                        }
                        else
                        {
                            RemoveError(propertyName);
                        }
                    }

                    break;



                default:
                    RemoveError(propertyName);
                    break;
            }




        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !errors.ContainsKey(propertyName))
                return null;

            return errors[propertyName];
        }

        public bool HasErrors => errors.Count > 0;

        private readonly Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        private void AddError(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(error))
                errors[propertyName].Add(error);

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void RemoveError(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }


    }
}
