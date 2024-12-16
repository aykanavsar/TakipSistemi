namespace TakipSistemi.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ModelYear { get; set; }
        public string ChassisNumber { get; set; }
        public string Plate { get; set; }
        public string FuelType { get; set; }
        public string AssignedDepartment { get; set; }
        public string AssignedPerson { get; set; }
        public string AssignedPersonEmail { get; set; }
        public string InsuranceCompany { get; set; }
        public decimal InsurancePrice { get; set; }
        public DateTime InsuranceDate { get; set; }
        public int InsuranceValidityMonths { get; set; }
        public string CascoCompany { get; set; }
        public decimal CascoPrice { get; set; }
        public DateTime CascoDate { get; set; }
        public int CascoValidityMonths { get; set; }
        public DateTime InspectionDate { get; set; }
        public int InspectionValidityMonths { get; set; }
    }
}
