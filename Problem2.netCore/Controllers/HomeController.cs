using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Problem2.netCore.Models;
using System.Data.SqlClient;

namespace Problem2.netCore.Controllers
{
    public class HomeController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();
        List<Employee> emp = new List<Employee>(); 

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            con.ConnectionString = Problem2.netCore.Properties.Resources.ConnectionString;

        }

        public IActionResult Index()
        {
            Getdata();
            return View(emp);
        }

        private void Getdata()
        {
            if (emp.Count > 0)
            {
                emp.Clear();
            }

            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT [EmpId],[EmployeeName],[EmployeeAddress],[EmployeeContact],[EmployeeEmail] FROM [TestEmployeeDB].[dbo].[EmployeeTbl]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    emp.Add(new Employee() {
                        EmpId=Convert.ToInt32(dr["EmpId"].ToString()),
                        EmployeeName=dr["EmployeeName"].ToString(),
                        EmployeeAddress=dr["EmployeeAddress"].ToString(),
                        EmployeeContact=dr["EmployeeContact"].ToString(),
                        EmployeeEmail=dr["EmployeeEmail"].ToString(),
                        
                        });
                }
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
