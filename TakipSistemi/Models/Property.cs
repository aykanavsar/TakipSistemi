using System;

namespace TakipSistemi.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }          // Bağlı Olduğu Şirket
        public string PropertyType { get; set; }         // Taşınmaz Türü (Ev, İş Yeri, Arsa)
        public string PropertyName { get; set; }         // Taşınmazın Adı
        public string City { get; set; }                 // İl
        public string District { get; set; }             // İlçe
        public string Address { get; set; }              // Adres

        // Sigorta Bilgileri (Ay cinsinden geçerlilik süresi)
        public string InsuranceCompany { get; set; }
        public decimal InsurancePrice { get; set; }
        public DateTime InsuranceDate { get; set; }
        public int InsuranceValidityMonths { get; set; }

        // Kasko Bilgileri (Ay cinsinden geçerlilik süresi)
        public string CascoCompany { get; set; }
        public decimal CascoPrice { get; set; }
        public DateTime CascoDate { get; set; }
        public int CascoValidityMonths { get; set; }
    }
}
