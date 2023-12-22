using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmDemo.Models
{
    public interface IEmployeeService
    {

        public List<Employee> GetAll();
        public bool Add(Employee objNewEmployee);
        public bool Update(Employee objEmployeeToUpdate);
        public bool Delete(int id);
        public Employee Search(int id);


    }
}
