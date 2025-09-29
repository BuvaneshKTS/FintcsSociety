using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FintcsApi.Models
{
    public class Demand
    {
        [Key]
        public int DemandId { get; set; }

        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public Member Member { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }

        public int CD { get; set; }
        public int OD { get; set; }
        public int Share { get; set; }
        public int NetLoanAmount { get; set; }
        public int BuildingFund { get; set; }

        public decimal PenalAmount { get; set; }
        public decimal PenalInterest { get; set; }

        // Navigation property for loan-wise demands
        public List<DemandLoan> LoanDemands { get; set; } = new List<DemandLoan>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class DemandLoan
    {
        [Key]
        public int DemandLoanId { get; set; }

        public int DemandId { get; set; }

        [ForeignKey("DemandId")]
        public Demand Demand { get; set; }

        public string LoanType { get; set; }   // General Loan, Emergency Loan, etc.
        public decimal PendingAmount { get; set; }
        public decimal Installment { get; set; }
        public decimal Interest { get; set; }
    }
}
