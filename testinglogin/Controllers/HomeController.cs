

using Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;

using testinglogin.Models;
using static testinglogin.Models.Chart;

namespace testinglogin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;



        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult login()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Logged") == "true")
            {
                string query = "select sum(Enrollment) from DataReport where Enrollment > 0";
                DataTable dt = new DataTable();
                dt = DBHelpers.ExecuteQuery(query);
                int Totalen = Convert.ToInt32(dt.Rows[0][0]);

                string query1 = "select sum(EnrollmentFailure) from DataReport";
                DataTable dt1 = new DataTable();
                dt1 = DBHelpers.ExecuteQuery(query1);
                int Enrollfail = Convert.ToInt32(dt1.Rows[0][0]);

                string query3 = "select sum(TotalSuccess) from DataReport where TotalSuccess> 0";
                DataTable dt2 = new DataTable();
                dt2 = DBHelpers.ExecuteQuery(query3);
                int verifisuccess = Convert.ToInt32(dt2.Rows[0][0]);

                string query4 = "select sum(Verification1) from Verificationdata where Verification1 < 70";
                DataTable dt3 = new DataTable();
                dt3 = DBHelpers.ExecuteQuery(query3);
                int verififail = Convert.ToInt32(dt3.Rows[0][0]);

                List<DataPoint> dataPoints = new List<DataPoint>();

                dataPoints.Add(new DataPoint("Enrollment", Totalen));
                dataPoints.Add(new DataPoint("Verification", verifisuccess));
                dataPoints.Add(new DataPoint("Verification Failure", verififail));
                dataPoints.Add(new DataPoint("False Acceptance", 00));
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                ViewBag.EC = Totalen;
                ViewBag.EF = Enrollfail;
                ViewBag.VS = verifisuccess;
                ViewBag.VF = verififail;
            }
            return View();
        }

        public IActionResult DataReport()
        {
            DataTable dataTable = new DataTable();

            List<datareport1> allocatelist = new List<datareport1>();

            string query = "Select * from  [loginerdata].[dbo].[DataReport]";

            dataTable = DBHelpers.ExecuteQuery(query);

            foreach (DataRow dataallocate in dataTable.Rows)

            {

                datareport1 verify2 = new datareport1();

                verify2.Date = (string)dataallocate["Date"];

                verify2.Enrollment = (int)dataallocate["Enrollment"];

                verify2.EnrollmentFailure = (int)dataallocate["EnrollmentFailure"];

                verify2.LoginAttempt = (string)dataallocate["LoginAttempt"];

                verify2.Loginsuccess = (int)dataallocate["Loginsuccess"];

                verify2.Loginmodel = (string)dataallocate["Loginmodel"];

                verify2.TotalSuccess = (int)dataallocate["TotalSuccess"];

                verify2.LoginFailed = (string)dataallocate["LoginFailed"];

                allocatelist.Add(verify2);

            }
            return View(allocatelist);
        }


        public IActionResult Enrollmentreport()
        {
            DataTable dataTable = new DataTable();

            List<Enrollmentdetail> allocatelist1 = new List<Enrollmentdetail>();

            string query = "Select * from  [loginerdata].[dbo].[Enrollmentdata]";

            dataTable = DBHelpers.ExecuteQuery(query);

            foreach (DataRow dataallocate in dataTable.Rows)

            {

                Enrollmentdetail enroll = new Enrollmentdetail();

                enroll.date = (string)dataallocate["date"];

                enroll.Customer_Id = (string)dataallocate["Customer_id"];

                enroll.Uniqueid = (string)dataallocate["Uniqueid"];

                enroll.EnrollmentId = (string)dataallocate["EnrollmentId"];


                allocatelist1.Add(enroll);

            }
            return View(allocatelist1);
        }

        public IActionResult verificationreport()
        {


            DataTable dataTable = new DataTable();

            List<verification> alist = new List<verification>();

            string query = "Select * from  [loginerdata].[dbo].[Verificationdata]";

            dataTable = DBHelpers.ExecuteQuery(query);

            foreach (DataRow ds in dataTable.Rows)

            {

                verification verify = new verification();

                verify.DateTime = (string)ds["DateTime"];

                verify.CustomerID = (string)ds["CustomerID"];

                verify.UniqueID = (string)ds["UniqueID"];

                verify.Verification1 = (string)ds["Verification1"];

                verify.CaptchaID = (string)ds["CaptchaID"];

                verify.CaptchaReturn = (string)ds["CaptchaReturn"];

                verify.Digit = (string)ds["Digit"];

                verify.Verification2 = (string)ds["Verification2"];

                alist.Add(verify);

            }
            return View(alist);
        }
        [HttpPost]
        public IActionResult Enrollmentreport(string StartDate,string EndDate)
        
        {
            DateTime DStart=Convert.ToDateTime(StartDate);
            DateTime DEnd = Convert.ToDateTime(EndDate);
            string Start = DStart.ToString("dd'-'M'-'yyyy");
            string End = DEnd.ToString("dd'-'M'-'yyyy");
            DataTable dt = new DataTable();
            string query = $"SELECT * from Enrollmentdata WHERE date BETWEEN '{Start}' AND '{End}'";
            dt = DBHelpers.ExecuteQuery(query);
            List<Enrollmentdetail> En = new List<Enrollmentdetail>();
            foreach(DataRow dr in dt.Rows)
            {
             Enrollmentdetail er = new Enrollmentdetail();
                er.date = (string)dr["date"];
                er.Customer_Id = (string)dr["Customer_Id"];
                er.Uniqueid = (string)dr["Uniqueid"];
                er.EnrollmentId = (string)dr["EnrollmentId"];
                En.Add(er);
            }
           
            return View(En);
        }

            [HttpPost]
        public IActionResult Login(login1 model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Password)){
                    if (IsValidUser(model.Username, model.Password))
                    {
                        HttpContext.Session.SetString("Logged","true");
                        return RedirectToAction("Dashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                }
            }
            return View();
        }
        private bool IsValidUser(string username, string password)
        {
            string query = $"SELECT * FROM[loginerdata].[dbo].[loginbase] where USERNAME =  '{username}' and PASSWORD = '{password}'; ";
            DataTable dataTable = DBHelpers.ExecuteQuery(query);
            if (dataTable.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
      
        public IActionResult Privacy()
        {
            return View();
        }
       
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(Registration model)
        {
            if (ModelState.IsValid)
            {


               string query = $"INSERT INTO [loginerdata].[dbo].[loginbase] (username,password,Confirm_password,email,phone) values('{model.Username}','{model.password}','{model.Confirm_password}','{model.email}','{model.phone}')";

               DBHelpers.ExecuteQuery(query);

                return RedirectToAction("login");


            }
            return View();
        
           
        }
        public IActionResult LogOut() {
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}