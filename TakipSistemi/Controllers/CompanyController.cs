using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


using TakipSistemi.Models;

namespace TakipSistemi.Controllers
{
    public class CompanyController : Controller
    {
        public static List<Company> Companies { get; set; } = new List<Company>();

        public ActionResult Index()
        {
            return View(Companies);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Company company)
        {
            company.Id = Companies.Count + 1;
            Companies.Add(company);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var company = Companies.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost]
        public ActionResult Edit(Company company)
        {
            var existingCompany = Companies.FirstOrDefault(c => c.Id == company.Id);
            if (existingCompany != null)
            {
                existingCompany.Name = company.Name;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var company = Companies.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var company = Companies.FirstOrDefault(c => c.Id == id);
            if (company != null)
            {
                Companies.Remove(company);
            }
            return RedirectToAction("Index");
        }
    }
}
