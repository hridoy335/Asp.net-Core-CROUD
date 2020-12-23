using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Problem2.netCore.Models;
using System.Data.SqlClient;
using Problem2.netCore.Properties;

namespace Problem2.netCore.Controllers
{
    public class HomeController : Controller
    {
       // Properties.Resources.ConnectionString db = new Resources.ConnectionString();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();
        List<Employee> emp = new List<Employee>(); // create Employee class object
        

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            con.ConnectionString = Problem2.netCore.Properties.Resources.ConnectionString;

        }

        public IActionResult Index()
        {
            Getdata();// call Getdata method

            return View(emp);// retuen emp object
        }

        private void Getdata()
        {
            //  check emp object have any data or not
            if (emp.Count > 0)
            {
                emp.Clear();
            }

            try
            {
                con.Open(); // open connection
                com.Connection = con;
                com.CommandText = "SELECT [EmpId],[EmployeeName],[EmployeeAddress],[EmployeeContact],[EmployeeEmail] FROM [TestEmployeeDB].[dbo].[EmployeeTbl]";
                dr = com.ExecuteReader(); // Execute code
                while (dr.Read())
                {
                    // insert emp object data 
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
           // employee.EmployeeName;
            try
            {
                con.Open(); // open connection
                com.Connection = con;
                //com.CommandText = "SELECT [EmpId],[EmployeeName],[EmployeeAddress],[EmployeeContact],[EmployeeEmail] FROM [TestEmployeeDB].[dbo].[EmployeeTbl]";
                com.CommandText = "insert into EmployeeTbl (EmployeeName,EmployeeAddress,EmployeeContact,EmployeeEmail) values('"+employee.EmployeeName+ "','"+employee.EmployeeAddress+"','" + employee.EmployeeContact + "','" + employee.EmployeeEmail +"')";
                dr = com.ExecuteReader();
                con.Close();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                con.Close();
                
            }
            return View(employee);
        }
        public IActionResult View() 
        {
            Getdata();
            return View(emp);
        }

        public JsonResult Edit(int?id)
        {
            List<Employee> emp2=new List<Employee>();
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT [EmpId],[EmployeeName],[EmployeeAddress],[EmployeeContact],[EmployeeEmail] FROM [TestEmployeeDB].[dbo].[EmployeeTbl] WHERE [EmpId]="+id+"";
                //com.CommandText = "insert into EmployeeTbl (EmployeeName,EmployeeAddress,EmployeeContact,EmployeeEmail) values('" + employee.EmployeeName + "','" + employee.EmployeeAddress + "','" + employee.EmployeeContact + "','" + employee.EmployeeEmail + "')";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    // insert emp object data 
                    emp2.Add(new Employee()
                    {
                        EmpId = Convert.ToInt32(dr["EmpId"].ToString()),
                        EmployeeName = dr["EmployeeName"].ToString(),
                        EmployeeAddress = dr["EmployeeAddress"].ToString(),
                        EmployeeContact = dr["EmployeeContact"].ToString(),
                        EmployeeEmail = dr["EmployeeEmail"].ToString(),

                    });
                }
                con.Close();

            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }
            return Json(emp2);
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
