using System;
using System.Collections.Generic;
using System.Linq;

namespace Employees
{
    public class Employees
    {
        public List<Employee> EmployeesList = new List<Employee>();

        public bool ValidateAllSalariesAreIntegers { get; set; } = false;
        public bool ValidateIsOneCEO { get; set; } = false;
        public bool ValidateEmployeesAreDistinct { get; set; } = false;
        public bool ValidateForNoCircularReference { get; set; } = false;
        public bool ValidateThatAllManagersAreEmployees { get; set; } = false;

        public bool ValidateCsvContainsInfo { get; set; } = false;

        private IEnumerable<Employee> _employeesInfoList { get; set; }


        public Employees(string employeesInfoCsv)
        {
            GetEmployeesInfoFromCsv(employeesInfoCsv);

            _employeesInfoList = EmployeesList;

            ValidateIsOneCEO = ValidateIfIsOneCEO(EmployeesList);
            ValidateEmployeesAreDistinct = ValidateIfemployeesAreDistinct(EmployeesList);
            ValidateForNoCircularReference = ValidateNoCircularReference(EmployeesList);
            ValidateThatAllManagersAreEmployees = ValidateAllManagersAreEmployees(EmployeesList);
            
        }
        public bool ValidateIfIsOneCEO(List<Employee> employeesList)
        {
            try
            {
                return employeesList.FindAll(x => string.IsNullOrWhiteSpace(x.ManagerId)).Count == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ValidateIfemployeesAreDistinct(List<Employee> employeesList)
        {
            try
            {
                return employeesList.GroupBy(x => x.EmployeeId).Count() == employeesList.Count;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool ValidateNoCircularReference(List<Employee> employeesList)
        {
            try
            {
                return employeesList.Select(i => new { i.EmployeeId, i.ManagerId }).ToList().All(i => !employeesList.Any(j => j.ManagerId == i.EmployeeId && j.EmployeeId == i.ManagerId)); ;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ValidateAllManagersAreEmployees(List<Employee> employeesList)
        {
            try
            {
                return employeesList.Where(j => !string.IsNullOrWhiteSpace(j.ManagerId))
                .Select(k => k.ManagerId)
                .All(l => employeesList
                .Select(i => i.EmployeeId)
                .Contains(l));
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void GetEmployeesInfoFromCsv(string employeesInfoCsv)
        {
            //Check if the information is empty
            if (string.IsNullOrWhiteSpace(employeesInfoCsv))
            {
                ValidateCsvContainsInfo = false;
                return;
            }
            //loops through the info from the CSV, line by line
            foreach (var employeeInfoCsv in employeesInfoCsv.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {

                var employeeInfo = employeeInfoCsv.Split(',');

                if (int.TryParse(employeeInfo[2], out var salary))
                {
                    EmployeesList.Add(new Employee
                    {
                        EmployeeId = employeeInfo[0],
                        ManagerId = employeeInfo[1],
                        Salary = salary
                    });
                    ValidateAllSalariesAreIntegers = true;
                    continue;
                }

                ValidateAllSalariesAreIntegers = false;
                return;

            }
        }

        public int GetManagerSalaryBudget(string managerId)
        {
            //recursively get reporting employees salary from the employees under the manager

            if (!ValidateAllSalariesAreIntegers) return 0;

            //get manager from employee list
            var manager = _employeesInfoList.FirstOrDefault(i => i.EmployeeId == managerId);

            //check the manager is valid
            if (manager == null)
            {
                return 0;
            }

            //get employees reporting to the manager
            var reportingEmployees = _employeesInfoList.Where(i => i.ManagerId == manager.EmployeeId).ToList();

            //get reporting employees salary from the employees under the manager
            var reportingEmployeesSalary = reportingEmployees.Sum(x => GetManagerSalaryBudget(x.EmployeeId));


            return manager.Salary + reportingEmployeesSalary;

        }


    }



}
