using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TakipSistemi.Models;
using TakipSistemi.Helpers;
using System.Diagnostics;

namespace TakipSistemi.Controllers
{
    public class RemindersController : Controller
    {
        public IActionResult Index(string filterType = "All", string filterAsset = "All")
        {
            var approachingItems = new List<ReminderItem>();

            // Araç verilerini kontrol et
            if (VehicleController.Vehicles != null && VehicleController.Vehicles.Any())
            {
                foreach (var v in VehicleController.Vehicles)
                {
                    AddIfApproaching(approachingItems, v.Id, v.CompanyName, $"{v.Brand} - {v.Plate}", "Araç", "Sigorta", v.InsuranceDate, v.InsuranceValidityMonths);
                    AddIfApproaching(approachingItems, v.Id, v.CompanyName, $"{v.Brand} - {v.Plate}", "Araç", "Kasko", v.CascoDate, v.CascoValidityMonths);
                    AddIfApproaching(approachingItems, v.Id, v.CompanyName, $"{v.Brand} - {v.Plate}", "Araç", "Muayene", v.InspectionDate, v.InspectionValidityMonths);
                }
            }

            // Taşınmaz verilerini kontrol et
            if (PropertyController.Properties != null && PropertyController.Properties.Any())
            {
                foreach (var p in PropertyController.Properties)
                {
                    AddIfApproaching(approachingItems, p.Id, p.CompanyName, p.PropertyName, "Taşınmaz", "Sigorta", p.InsuranceDate, p.InsuranceValidityMonths);
                    AddIfApproaching(approachingItems, p.Id, p.CompanyName, p.PropertyName, "Taşınmaz", "Kasko", p.CascoDate, p.CascoValidityMonths);
                }
            }

            // Filtreleme
            if (filterType != "All")
                approachingItems = approachingItems.Where(i => i.Type == filterType).ToList();
            if (filterAsset != "All")
                approachingItems = approachingItems.Where(i => i.AssetType == filterAsset).ToList();

            return View(approachingItems);
        }

        private void AddIfApproaching(List<ReminderItem> list, int id, string company, string name, string assetType, string type, DateTime startDate, int validityMonths)
        {
            if (startDate == default || validityMonths <= 0)
                return;

            var endDate = startDate.AddMonths(validityMonths);
            var remainingDays = (endDate - DateTime.Now).TotalDays;

            if (remainingDays <= 30)
            {
                list.Add(new ReminderItem
                {
                    Id = id, // Gerçek ID burada set edilir
                    Company = company,
                    AssetName = name,
                    AssetType = assetType,
                    Type = type,
                    StartDate = startDate,
                    ValidityMonths = validityMonths,
                    EndDate = endDate,
                    RemainingDays = (int)remainingDays
                });
            }
        }

        [HttpGet]
        public IActionResult UpdateRecord(int id, string assetType, string type)
        {
            Debug.WriteLine($"UpdateRecord çağrıldı. ID: {id}, AssetType: {assetType}, Type: {type}");

            string assetName = "";

            // Araç kontrolü
            if (assetType == "Arac")
            {
                var vehicle = VehicleController.Vehicles.FirstOrDefault(v => v.Id == id);
                if (vehicle == null)
                {
                    Debug.WriteLine($"Araç bulunamadı. ID: {id}");
                    return NotFound($"Araç bulunamadı. ID: {id}");
                }

                assetName = $"{vehicle.Brand} - {vehicle.Plate}";
            }
            // Taşınmaz kontrolü
            else if (assetType == "Tasinmaz")
            {
                var property = PropertyController.Properties.FirstOrDefault(p => p.Id == id);
                if (property == null)
                {
                    Debug.WriteLine($"Taşınmaz bulunamadı. ID: {id}");
                    return NotFound($"Taşınmaz bulunamadı. ID: {id}");
                }

                assetName = property.PropertyName;
            }
            else
            {
                Debug.WriteLine("Geçersiz varlık tipi.");
                return BadRequest("Geçersiz varlık tipi.");
            }

            Debug.WriteLine($"UpdateRecord tamamlandı. AssetName: {assetName}");

            var model = new UpdateViewModel
            {
                Id = id,
                AssetType = assetType,
                AssetName = assetName,
                Type = type
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateRecord(int Id, string AssetType, string Type, DateTime NewDate, decimal NewPrice, int NewValidityMonths)
        {
            Debug.WriteLine($"Güncelleme için gelen ID: {Id}, AssetType: {AssetType}, Type: {Type}, NewDate: {NewDate}, NewPrice: {NewPrice}, NewValidityMonths: {NewValidityMonths}");

            AssetType = Normalizer.NormalizeString(AssetType);
            Type = Normalizer.NormalizeString(Type);

            if (AssetType == "Arac")
            {
                var vehicle = VehicleController.Vehicles.FirstOrDefault(v => v.Id == Id);
                if (vehicle == null)
                {
                    Debug.WriteLine($"Araç bulunamadı. ID: {Id}");
                    return NotFound($"Araç bulunamadı. ID: {Id}");
                }

                if (Type == "Sigorta")
                {
                    vehicle.InsuranceDate = NewDate;
                    vehicle.InsurancePrice = NewPrice;
                    vehicle.InsuranceValidityMonths = NewValidityMonths;
                }
                else if (Type == "Kasko")
                {
                    vehicle.CascoDate = NewDate;
                    vehicle.CascoPrice = NewPrice;
                    vehicle.CascoValidityMonths = NewValidityMonths;
                }
                else if (Type == "Muayene")
                {
                    vehicle.InspectionDate = NewDate;
                    vehicle.InspectionValidityMonths = NewValidityMonths;
                }
            }
            else if (AssetType == "Tasinmaz")
            {
                var property = PropertyController.Properties.FirstOrDefault(p => p.Id == Id);
                if (property == null)
                {
                    Debug.WriteLine($"Taşınmaz bulunamadı. ID: {Id}");
                    return NotFound($"Taşınmaz bulunamadı. ID: {Id}");
                }

                if (Type == "Sigorta")
                {
                    property.InsuranceDate = NewDate;
                    property.InsurancePrice = NewPrice;
                    property.InsuranceValidityMonths = NewValidityMonths;
                }
                else if (Type == "Kasko")
                {
                    property.CascoDate = NewDate;
                    property.CascoPrice = NewPrice;
                    property.CascoValidityMonths = NewValidityMonths;
                }
            }

            return RedirectToAction("Index");
        }
    }
}
