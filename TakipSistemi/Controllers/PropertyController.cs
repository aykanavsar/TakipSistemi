using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using TakipSistemi.Models;

namespace TakipSistemi.Controllers
{
    public class PropertyController : Controller
    {
        // Taşınmaz listesi
        public static List<Property> Properties = new List<Property>();

        // Şehir ve ilçe listesi
        private static Dictionary<string, List<string>> CitiesAndDistricts = new Dictionary<string, List<string>>
        {
            { "İstanbul", new List<string> { "Kadıköy", "Beşiktaş", "Üsküdar" } },
            { "Ankara", new List<string> { "Çankaya", "Keçiören", "Sincan" } },
            { "İzmir", new List<string> { "Konak", "Karşıyaka", "Bornova" } }
        };

        // Taşınmaz tipleri
        private static List<string> PropertyTypes = new List<string> { "Ev", "İş Yeri", "Arsa" };

        // Taşınmaz listesi sayfası
        public IActionResult Index()
        {
            return View(Properties);
        }

        // Taşınmaz ekleme sayfası (GET)
        public IActionResult Create()
        {
            ViewBag.Companies = GetCompanySelectList();
            ViewBag.PropertyTypes = new SelectList(PropertyTypes);
            ViewBag.Cities = new SelectList(CitiesAndDistricts.Keys);
            ViewBag.Districts = new SelectList(new List<string>());

            return View();
        }

        // Taşınmaz ekleme işlemi (POST)
        [HttpPost]
        public IActionResult Create(Property property)
        {
            if (ModelState.IsValid)
            {
                property.Id = Properties.Any() ? Properties.Max(p => p.Id) + 1 : 1;
                Properties.Add(property);
                return RedirectToAction("Index");
            }

            ViewBag.Companies = GetCompanySelectList();
            ViewBag.PropertyTypes = new SelectList(PropertyTypes);
            ViewBag.Cities = new SelectList(CitiesAndDistricts.Keys, property.City);
            ViewBag.Districts = new SelectList(
                CitiesAndDistricts.ContainsKey(property.City) ? CitiesAndDistricts[property.City] : new List<string>(),
                property.District);

            return View(property);
        }

        // Taşınmaz düzenleme sayfası (GET)
        public IActionResult Edit(int id)
        {
            var property = Properties.FirstOrDefault(p => p.Id == id);
            if (property == null) return NotFound();

            ViewBag.Companies = GetCompanySelectList(property.CompanyName);
            ViewBag.PropertyTypes = new SelectList(PropertyTypes, property.PropertyType);
            ViewBag.Cities = new SelectList(CitiesAndDistricts.Keys, property.City);
            ViewBag.Districts = new SelectList(
                CitiesAndDistricts.ContainsKey(property.City) ? CitiesAndDistricts[property.City] : new List<string>(),
                property.District);

            return View(property);
        }

        // Taşınmaz düzenleme işlemi (POST)
        [HttpPost]
        public IActionResult Edit(Property property)
        {
            var existingProperty = Properties.FirstOrDefault(p => p.Id == property.Id);
            if (existingProperty != null)
            {
                // Genel Bilgiler
                existingProperty.CompanyName = property.CompanyName;
                existingProperty.PropertyType = property.PropertyType;
                existingProperty.PropertyName = property.PropertyName;
                existingProperty.City = property.City;
                existingProperty.District = property.District;
                existingProperty.Address = property.Address;

                // Sigorta Bilgileri
                existingProperty.InsuranceCompany = property.InsuranceCompany;
                existingProperty.InsurancePrice = property.InsurancePrice;
                existingProperty.InsuranceDate = property.InsuranceDate;
                existingProperty.InsuranceValidityMonths = property.InsuranceValidityMonths;

                // Kasko Bilgileri
                existingProperty.CascoCompany = property.CascoCompany;
                existingProperty.CascoPrice = property.CascoPrice;
                existingProperty.CascoDate = property.CascoDate;
                existingProperty.CascoValidityMonths = property.CascoValidityMonths;
            }

            return RedirectToAction("Index");
        }

        // Şehir seçimine göre ilçeleri döndüren AJAX çağrısı
        [HttpGet]
        public JsonResult GetDistricts(string city)
        {
            if (!string.IsNullOrEmpty(city) && CitiesAndDistricts.ContainsKey(city))
            {
                return Json(CitiesAndDistricts[city]);
            }
            return Json(new List<string>());
        }

        // Detay sayfası
        public IActionResult Details(int id)
        {
            var property = Properties.FirstOrDefault(p => p.Id == id);
            if (property == null) return NotFound();

            return View(property);
        }

        // Silme işlemi
        public IActionResult Delete(int id)
        {
            var property = Properties.FirstOrDefault(p => p.Id == id);
            if (property != null)
            {
                Properties.Remove(property);
            }
            return RedirectToAction("Index");
        }

        // Şirket listesini kontrol edip geri döndüren metod
        private SelectList GetCompanySelectList(string selectedCompany = null)
        {
            var companies = CompanyController.Companies?.Select(c => c.Name).ToList();
            return companies != null && companies.Any()
                ? new SelectList(companies, selectedCompany)
                : new SelectList(new List<string>());
        }
    }
}
