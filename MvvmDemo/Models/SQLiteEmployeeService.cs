using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.Sqlite;

namespace MvvmDemo.Models
{
    public class SQLiteEmployeeService:IEmployeeService
    {

        static SQLiteEmployeeService()
        {
            SQLitePCL.Batteries.Init();
        }



        SqliteConnection ObjSQLiteConnection;
        SqliteCommand ObjSQLiteCommand;

        public SQLiteEmployeeService()
        {                                     
            ObjSQLiteConnection = new SqliteConnection(ConfigurationManager.AppSettings["SQLiteConnection"]);
            ObjSQLiteCommand = new SqliteCommand();
            ObjSQLiteCommand.Connection = ObjSQLiteConnection;
            ObjSQLiteCommand.CommandType = CommandType.Text;  // SQLite does not use stored procedures by default
            System.Diagnostics.Debug.WriteLine("Awa bolaw Awa!!");
        }

        public List<Employee> GetAll()
        {
            System.Diagnostics.Debug.WriteLine("Get All ekatath Awa!!");
            List<Employee> ObjEmployeesList = new List<Employee>();
            try
            {
                ObjSQLiteCommand.Parameters.Clear();
                ObjSQLiteCommand.CommandText = "SELECT * FROM Employees";

                ObjSQLiteConnection.Open();
                var ObjSQLiteDataReader = ObjSQLiteCommand.ExecuteReader();

                if (ObjSQLiteDataReader.HasRows)
                {
                    Employee ObjEmployee = null;
                    while (ObjSQLiteDataReader.Read())
                    {
                        ObjEmployee = new Employee();
                        ObjEmployee.Id = ObjSQLiteDataReader.GetInt32(0);
                        ObjEmployee.Name = ObjSQLiteDataReader.GetString(1);
                        ObjEmployee.Age = ObjSQLiteDataReader.GetInt32(2);
                        ObjEmployee.DateOfJoined = ObjSQLiteDataReader.GetDateTime(3);
                        ObjEmployee.ImagePath = (byte[])ObjSQLiteDataReader["ImagePath"];
                        ObjEmployee.Salary = ObjSQLiteDataReader.GetDecimal(5);

                        System.Diagnostics.Debug.WriteLine("ObjEmployee.ImagePath" + ObjEmployee.ImagePath);

                        ObjEmployeesList.Add(ObjEmployee);
                    }
                }
                ObjSQLiteDataReader.Close();
            }
            catch (SqliteException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSQLiteConnection.Close();
            }

            return ObjEmployeesList;
        }

        public bool Add(Employee objNewEmployee)
        {
            bool IsAdded = false;

            if (objNewEmployee.Age < 21 || objNewEmployee.Age > 58)
            {
                throw new ArgumentException("Invalid age limit for employee");
            }
            if (objNewEmployee.DateOfJoined > DateTime.Now)
            {
                throw new ArgumentException("Date of joined cannot be a future date");
            }

            if (objNewEmployee.Id <= 0)
            {
                throw new ArgumentException("Id must be a positive integer");
            }

            if (string.IsNullOrEmpty(objNewEmployee.Name))
            {
                throw new ArgumentException("Name cannot be empty");
            }
            if (objNewEmployee.ImagePath == null)
            {
                throw new ArgumentException("Please upload profile image");
            }

            System.Diagnostics.Debug.WriteLine("SQLITE" + objNewEmployee.Name + " " + objNewEmployee.Salary);




            try
            {
                ObjSQLiteCommand.Parameters.Clear();
                ObjSQLiteCommand.CommandText = "INSERT INTO Employees (Id, Name, Age, DateOfJoined, ImagePath, Salary) " +
                                               "VALUES (@Id, @Name, @Age, @DateOfJoined, @ImagePath, @Salary)";
                ObjSQLiteCommand.Parameters.AddWithValue("@Id", objNewEmployee.Id);
                ObjSQLiteCommand.Parameters.AddWithValue("@Name", objNewEmployee.Name);
                ObjSQLiteCommand.Parameters.AddWithValue("@Age", objNewEmployee.Age);
                ObjSQLiteCommand.Parameters.AddWithValue("@DateOfJoined", objNewEmployee.DateOfJoined);
                ObjSQLiteCommand.Parameters.AddWithValue("@ImagePath", objNewEmployee.ImagePath);
                ObjSQLiteCommand.Parameters.AddWithValue("@Salary", objNewEmployee.Salary);

                ObjSQLiteConnection.Open();

                int NoOfRowsAffected = ObjSQLiteCommand.ExecuteNonQuery();
                IsAdded = NoOfRowsAffected > 0;
            }
            catch (SqliteException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSQLiteConnection.Close();
            }

            return IsAdded;
        }

        public bool Update(Employee objEmployeeToUpdate)
        {
            bool IsUpdated = false;

            if (objEmployeeToUpdate.Age < 21 || objEmployeeToUpdate.Age > 58)
            {
                throw new ArgumentException("Invalid age limit for employee");
            }
            if (objEmployeeToUpdate.DateOfJoined > DateTime.Now)
            {
                throw new ArgumentException("Date of joined cannot be a future date");
            }


            try
            {
                ObjSQLiteCommand.Parameters.Clear();
                ObjSQLiteCommand.CommandText = "UPDATE Employees " +
                                               "SET Name = @Name, Age = @Age, DateOfJoined = @DateOfJoined, " +
                                               "ImagePath = @ImagePath, Salary = @Salary " +
                                               "WHERE Id = @Id";
                ObjSQLiteCommand.Parameters.AddWithValue("@Id", objEmployeeToUpdate.Id);
                ObjSQLiteCommand.Parameters.AddWithValue("@Name", objEmployeeToUpdate.Name);
                ObjSQLiteCommand.Parameters.AddWithValue("@Age", objEmployeeToUpdate.Age);
                ObjSQLiteCommand.Parameters.AddWithValue("@DateOfJoined", objEmployeeToUpdate.DateOfJoined);
                ObjSQLiteCommand.Parameters.AddWithValue("@ImagePath", objEmployeeToUpdate.ImagePath);
                ObjSQLiteCommand.Parameters.AddWithValue("@Salary", objEmployeeToUpdate.Salary);

                ObjSQLiteConnection.Open();

                int NoOfRowsAffected = ObjSQLiteCommand.ExecuteNonQuery();
                IsUpdated = NoOfRowsAffected > 0;
            }
            catch (SqliteException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSQLiteConnection.Close();
            }

            return IsUpdated;
        }

        public bool Delete(int id)
        {
            bool IsDeleted = false;

            try
            {
                ObjSQLiteCommand.Parameters.Clear();
                ObjSQLiteCommand.CommandText = "DELETE FROM Employees WHERE Id = @Id";
                ObjSQLiteCommand.Parameters.AddWithValue("@Id", id);

                ObjSQLiteConnection.Open();

                int NoOfRowsAffected = ObjSQLiteCommand.ExecuteNonQuery();
                IsDeleted = NoOfRowsAffected > 0;
            }
            catch (SqliteException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSQLiteConnection.Close();
            }

            return IsDeleted;
        }

        public Employee Search(int id)
        {
            Employee ObjEmployee = null;

            try
            {
                ObjSQLiteCommand.Parameters.Clear();
                ObjSQLiteCommand.CommandText = "SELECT * FROM Employees WHERE Id = @Id";
                ObjSQLiteCommand.Parameters.AddWithValue("@Id", id);

                ObjSQLiteConnection.Open();
                var ObjSQLiteDataReader = ObjSQLiteCommand.ExecuteReader();

                if (ObjSQLiteDataReader.HasRows)
                {
                    ObjSQLiteDataReader.Read();
                    ObjEmployee = new Employee();
                    ObjEmployee.Id = ObjSQLiteDataReader.GetInt32(0);
                    ObjEmployee.Name = ObjSQLiteDataReader.GetString(1);
                    ObjEmployee.Age = ObjSQLiteDataReader.GetInt32(2);
                    ObjEmployee.DateOfJoined = ObjSQLiteDataReader.GetDateTime(3);
                    ObjEmployee.ImagePath = (byte[])ObjSQLiteDataReader["ImagePath"];
                    ObjEmployee.Salary = ObjSQLiteDataReader.GetDecimal(5);

                    System.Diagnostics.Debug.WriteLine("SEARCH" + ObjEmployee.Id + " " + ObjEmployee.Name + " " + ObjEmployee.ImagePath);
                }

                ObjSQLiteDataReader.Close();
            }
            catch (SqliteException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSQLiteConnection.Close();
            }

            return ObjEmployee;
        }
    }
}
