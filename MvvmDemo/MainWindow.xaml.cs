using System;
using System.Windows;
using MvvmDemo.Models;
using MvvmDemo.ViewModels;

namespace MvvmDemo
{
    public partial class MainWindow : Window
    {
        EmployeeViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            InitializeViewModel();
        }

        public MainWindow(string gg)
        {
            InitializeComponent();
            InitializeViewModel(gg);
        }

        private void InitializeViewModel(string gg = null)
        {
            string databasePreference = gg ?? new EmployeeViewModel().Preference;

            IEmployeeService employeeService;

            if (databasePreference == "MySql" || databasePreference == "System.Windows.Controls.ComboBoxItem: MySql")
            {
                employeeService = new MsSqlEmployeeService();
            }
            else if (databasePreference == "SQLite" || databasePreference == "System.Windows.Controls.ComboBoxItem: SQLite")
            {
                employeeService = new SQLiteEmployeeService();
            }
            else
            {
                throw new Exception("Invalid database preference");
            }

            ViewModel = new EmployeeViewModel(employeeService);
            this.DataContext = ViewModel;
        }
    }
}
