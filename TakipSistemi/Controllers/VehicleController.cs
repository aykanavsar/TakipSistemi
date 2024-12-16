using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using TakipSistemi.Models;

namespace TakipSistemi.Controllers
{
    public class VehicleController : Controller
    {
        // Statik araç listesi
        public static List<Vehicle> Vehicles = new List<Vehicle>();

        // Araç listesi sayfası
        public IActionResult Index()
        {
            return View(Vehicles);
        }

        // Araç ekleme sayfası (GET)
        [HttpGet]
        public IActionResult Create()
        {
            // Şirket listesini kontrol et ve dropdown için ViewBag ile gönder
            var companyList = CompanyController.Companies;
            ViewBag.Companies = companyList != null && companyList.Any()
                ? new SelectList(companyList, "Name", "Name")
                : new SelectList(new List<string>(), "Name", "Name");

            ViewBag.FuelTypes = new SelectList(new List<string> { "Benzin", "Dizel", "Elektrik", "Hibrit" });

            return View();
        }

        // Araç ekleme işlemi (POST)
        [HttpPost]
        public IActionResult Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                // ID otomatik artış
                vehicle.Id = Vehicles.Any() ? Vehicles.Max(v => v.Id) + 1 : 1;
                Vehicles.Add(vehicle);
                return RedirectToAction("Index");
            }

            // Dropdown listeleri tekrar doldur
            ViewBag.Companies = new SelectList(CompanyController.Companies, "Name", "Name");
            ViewBag.FuelTypes = new SelectList(new List<string> { "Benzin", "Dizel", "Elektrik", "Hibrit" });

            return View(vehicle);
        }

        // Araç düzenleme sayfası (GET)
        public IActionResult Edit(int id)
        {
            var vehicle = Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle == null) return NotFound();

            ViewBag.Companies = new SelectList(CompanyController.Companies, "Name", "Name", vehicle.CompanyName);
            ViewBag.FuelTypes = new SelectList(new List<string> { "Benzin", "Dizel", "Elektrik", "Hibrit" }, vehicle.FuelType);

            return View(vehicle);
        }

        // Araç düzenleme işlemi (POST)
        [HttpPost]
        public IActionResult Edit(Vehicle vehicle)
        {
            var existingVehicle = Vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
            if (existingVehicle != null)
            {
                existingVehicle.CompanyName = vehicle.CompanyName;
                existingVehicle.Brand = vehicle.Brand;
                existingVehicle.Model = vehicle.Model;
                existingVehicle.ModelYear = vehicle.ModelYear;
                existingVehicle.FuelType = vehicle.FuelType;
            }

            return RedirectToAction("Index");
        }

        // Araç silme işlemi
        public IActionResult Delete(int id)
        {
            var vehicle = Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle != null)
            {
                Vehicles.Remove(vehicle);
            }

            return RedirectToAction("Index");
        }

        // Araç detayları
        public IActionResult Details(int id)
        {
            var vehicle = Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }
    }
}
