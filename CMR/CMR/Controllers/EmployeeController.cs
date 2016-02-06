using CMR.Models;
using CMR.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMR.Layers.BusinessLayer;
using CMR.Layers.DataAccessLayer;

namespace CMR.Controllers
{
    public class EmployeeController : Controller
    {
        public ViewResult Index()
        {
            EmployeeListViewModel list = new EmployeeListViewModel();
            EmployeeBusinessLayer layer = new EmployeeBusinessLayer();
            List<Employee> employees = layer.GetEmployees();

            List<EmployeeViewModel> models = new List<EmployeeViewModel>();

            foreach (Employee e in employees)
            {
                EmployeeViewModel m1 = new EmployeeViewModel();
                m1.EmployeeName = e.FirstName + " " + e.LastName;
                m1.Salary = e.Salary.ToString("C");
                if (e.Salary > 15000)
                {
                    m1.SalaryColor = "yellow";
                }
                else
                {
                    m1.SalaryColor = "green";
                }
                models.Add(m1);
            }

            list.Employees = models;

            return View("Index", list);
        }

        public ActionResult AddNew()
        {
            return View("CreateEmployee");
        }

        public ActionResult SaveEmployee(Employee e, String BtnSubmit)
        {
            switch (BtnSubmit)
            {
                case "Save Employee":
                    if (ModelState.IsValid)
                    {
                        EmployeeBusinessLayer empBal = new EmployeeBusinessLayer();
                        empBal.SaveEmployee(e);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("CreateEmployee");
                    }
                case "Cancel":
                    return RedirectToAction("Index");
            }

            return new EmptyResult();
        }
	}
}