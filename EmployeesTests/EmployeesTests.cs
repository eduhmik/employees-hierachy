using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeesTests
{
    [TestClass]
    public class EmployeesTests
    {
        private const string EmployeeInfoWithCorrectValues = "Employee2,Employee1,800\r\nEmployee3,Employee1,500\r\nEmployee1,,1000\r\nEmployee4,Employee1,500\r\nEmployee5,Employee2,500\r\nEmployee6,Employee3,500";
        private const string SalaryWithNonIntegerValues = "Employee2,Employee1,800\r\nEmployee3,Employee1,xxxx\r\nEmployee1,,1000\r\nEmployee4,Employee1,4000";
        private const string EmployeeInfoWithMultipleCeos = "Employee2,Employee1,800\r\nEmployee3,Employee1,500\r\nEmployee1,,1000\r\nEmployee4,,500";
        private const string EmployeeInfoWithCircularReference = "Employee1,,1000\r\nEmployee2,Employee1,800\r\nEmployee1,Employee2,500\r\nEmployee4,Employee1,500\r\nEmployee5,Employee4,500,\r\nEmployee4,Employee5,500";
        private const string EmployeeInfoWithNoCircularReference = "Employee1,,1000\r\nEmployee2,Employee1,800\r\nEmployee4,Employee1,500\r\nEmployee5,Employee4,500";
        private const string EmployeeInfoWithBlankString = "";
         private const string EmployeesInfoWithoutCeo = "Employee2,Employee1,800\r\nEmployee3,Employee1,500\r\nEmployee4,Employee1,500";
        private const string ManagerWhoIsNotAnEmployee = "Employee1,,1000\r\nEmployee2,Employee1,800\r\nEmployee4,Employee8,500\r\nEmployee5,Employee4,500,\r\nEmployee4,Employee5,500";
        private const string EmployeeInfoWithOneCEO = "Employee2,Employee1,800\r\nEmployee3,Employee1,500\r\nEmployee1,,1000\r\nEmployee4,Employee1,500\r\nEmployee5,Employee2,500\r\nEmployee6,Employee3,500";

        [TestMethod]
        public void Adding_Empty_String_Indicates_Invalid_Employee_Info()
        {
            var employees = new Employees.Employees(EmployeeInfoWithBlankString);

            Assert.IsFalse(employees.ValidateCsvContainsInfo);
        }

        [TestMethod]
        public void Adding_Data_With_Multiple_CE0s_Indicates_Invalid_Employee_Data()
        {
            var employees = new Employees.Employees(EmployeeInfoWithMultipleCeos);

            Assert.IsFalse(employees.ValidateIsOneCEO);
        }

        [TestMethod]
        public void Adding_Employee_Info_Without_CEO_Indicates_InValid_Employee_Info()
        {
            var employees = new Employees.Employees(EmployeesInfoWithoutCeo);

            Assert.IsFalse(employees.ValidateIsOneCEO);
        }

        [TestMethod]
        public void Adding_Employee_Info_With_One_CEO_Indicates_Valid_CEO()
        {
            var employees = new Employees.Employees(EmployeeInfoWithOneCEO);

            Assert.IsTrue(employees.ValidateIsOneCEO);
        }
        [TestMethod]
        public void Adding_Employee_Info_With_Circular_Reference_Indicates_Invalid_Employee_Info()
        {
            var employees = new Employees.Employees(EmployeeInfoWithCircularReference);

            Assert.IsFalse(employees.ValidateForNoCircularReference);
        }
        [TestMethod]
        public void Adding_Employee_Info_With_No_Circular_Reference_Is_Correct()
        {
            var employees = new Employees.Employees(EmployeeInfoWithNoCircularReference);

            Assert.IsTrue(employees.ValidateForNoCircularReference);
        }
        [TestMethod]
        public void Adding_Employee_Info_With_A_Manager_Who_Is_Not_An_Employee_Indicates_Invalid_Employee_Info()
        {
            var employees = new Employees.Employees(ManagerWhoIsNotAnEmployee);

            Assert.IsFalse(employees.ValidateThatAllManagersAreEmployees);
        }
        [TestMethod]
        public void Adding_Invalid_Employee_Salary_Results_In_Invalid_Employee_Info()
        {
            var employees = new Employees.Employees(SalaryWithNonIntegerValues);

            Assert.IsFalse(employees.ValidateAllSalariesAreIntegers);
        }
        [TestMethod]
        public void Given_A_Non_Existent_Manager_To_Calculate_Salary_Budget_Return_Zero()
        {
            var employees = new Employees.Employees(EmployeeInfoWithCorrectValues);

            Assert.AreEqual(0, employees.GetManagerSalaryBudget("NonExistentEmployeeId"));
        }
        [TestMethod]
        public void Given_An_Existing_Manager_To_Calculate_Salary_Budget_Return_Correct_Sum()
        {
            const long expectedSum = 3800;

            var employees = new Employees.Employees(EmployeeInfoWithCorrectValues);
            
            Assert.AreEqual(expectedSum, employees.GetManagerSalaryBudget("Employee1"));
        }

    }
}
