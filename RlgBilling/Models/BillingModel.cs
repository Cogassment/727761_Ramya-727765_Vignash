using System.ComponentModel.DataAnnotations;

namespace RlgBilling.Models
{
    public class BillingModel
    {

        public int ProjectID { get; set; }

        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = " Only Alphabets are allowed")]
        public string ProjectName { get; set; }

        [RegularExpression("^[a-zA-Z, ]*$", ErrorMessage = " Only Alphabets are allowed")]
        public string ManagerName { get; set; }

        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Please Enter Valid AssociateID with 6 characters")]
        public int AssociateID { get; set; }

        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = " Only Alphabets are allowed")]
        public string AssociateName { get; set; }

        public int AllocationPercent { get; set; }

        [RegularExpression("^[O-ON-NF-F ]{2}$", ErrorMessage = " Enter ON or OF")]
        public string OnOff { get; set; }

        [Required(ErrorMessage = "values is required.")]
        public int RateCard { get; set; }

        [Required(ErrorMessage = "values is required.")]
        public int BillableDays { get; set; }
        public int LeaveDays { get; set; }
        public int Amount { get; set; }
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = " Only Alphabets are allowed")]
        public string Comments { get; set; }
    }
}