using CMR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMR.Controllers
{
    public class TestController : Controller
    {
        public string getString()
        {
            return "Hello World is old now. It&rsquo;s time for wassup bro ;)";
        }

        public ViewResult getView()
        {
            Employee employee = new Employee();
            employee.FirstName = "Sukesh";
            employee.LastName = "Marla";
            employee.Salary = 20000;

            ViewData["employee"] = employee;
            ViewBag.Employee = employee;
            return View("MyView");
        }
	}
}