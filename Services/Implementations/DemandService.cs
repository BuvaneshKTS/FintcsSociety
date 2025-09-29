using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using FintcsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace FintcsApi.Services.Implementations
{
    public class DemandService : IDemandService
    {
        private readonly AppDbContext _context;
        private readonly IMemberService _memberService;
        private readonly ILoanService _loanService;
        private readonly IVoucherService _voucherService;
        private readonly ILedgerService _ledgerService;
        private readonly ILoanTypeService _loanTypeService;

        public DemandService(
            AppDbContext context,
            IMemberService memberService,
            ILoanService loanService,
            IVoucherService voucherService,
            ILoanTypeService loanTypeService,
            ILedgerService ledgerService)
        {
            _context = context;
            _memberService = memberService;
            _loanService = loanService;
            _voucherService = voucherService;
            _ledgerService = ledgerService;
        }

        // Create demand
        public async Task<ApiResponse<bool>> CreateDemandAsync(DemandCreateDto dto)
        {
            try
            {
                // Compute preview data (console only)
                // var membersResponse = await _memberService.GetAllMembersBySocietyAsync(dto.SocietyId);
                // var members = membersResponse.Data ?? new List<MemberDto>();
                // var allLedgers = await _ledgerService.GetAllLedgerAccountsAsync();
                // // var allLedgerTransactions = await _ledgerService.
                // var allVouchers = await _voucherService.GetAllVouchersAsync();

                // foreach (var member in members)
                // {
                //     var memberLoansResponse = await _loanService.GetLoansByMemberAsync(member.Id);
                //     var memberLoans = memberLoansResponse.Data?.ToList() ?? new List<LoanDto>();
                //     var memberLedgers = allLedgers.Where(l => l.MemberId == member.Id).ToList();
                //     var memberVouchers = allVouchers.Where(v => v.MemberId == member.Id).ToList();

                //     Console.WriteLine($"--- Member: {member.Name} (ID: {member.Id}) ---");

                //     // Print all Loan details
                //     Console.WriteLine($"Loans ({memberLoans.Count}):");
                //     foreach (var loan in memberLoans)
                //     {
                //         foreach (var prop in loan.GetType().GetProperties())
                //         {
                //             Console.WriteLine($"  {prop.Name}: {prop.GetValue(loan)}");
                //         }
                //         Console.WriteLine();
                //     }

                //     // Print all Ledger details
                //     Console.WriteLine($"Ledgers ({memberLedgers.Count}):");
                //     foreach (var ledger in memberLedgers)
                //     {
                //         foreach (var prop in ledger.GetType().GetProperties())
                //         {
                //             Console.WriteLine($"  {prop.Name}: {prop.GetValue(ledger)}");
                //         }
                //         Console.WriteLine();
                //     }

                //     // Print all Voucher details
                //     Console.WriteLine($"Vouchers ({memberVouchers.Count}):");
                //     foreach (var voucher in memberVouchers)
                //     {
                //         foreach (var prop in voucher.GetType().GetProperties())
                //         {
                //             Console.WriteLine($"  {prop.Name}: {prop.GetValue(voucher)}");
                //         }
                //         Console.WriteLine();
                //     }

                //     Console.WriteLine(new string('-', 50)); // separator between members
                // }


                // No DB save, just return success
                return ApiResponse<bool>.SuccessResponse(true, "Preview generated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Failed to generate preview", new List<string> { ex.Message });
            }
        }



        // Get demands by year/month
        public async Task<ApiResponse<List<DemandViewDto>>> GetDemandByMonthYearAsync(int year, int month)
        {
            try
            {
                var demands = await _context.Demands
                    .Include(d => d.Member)
                    .Include(d => d.LoanDemands)
                    .Where(d => d.Year == year && d.Month == month)
                    .ToListAsync();

                if (!demands.Any())
                    return ApiResponse<List<DemandViewDto>>.ErrorResponse("No demand records found");

                var demandDtos = demands.Select(d => new DemandViewDto
                {
                    MemberId = d.MemberId,
                    MemberName = d.Member.Name,
                    CD = d.CD,
                    OD = d.OD,
                    Share = d.Share,
                    LoanDemands = d.LoanDemands.Select(l => new LoanDemandDto
                    {
                        LoanType = l.LoanType,
                        PendingAmount = l.PendingAmount,
                        Installment = l.Installment,
                        Interest = l.Interest
                    }).ToList(),
                    PenalAmount = d.PenalAmount,
                    PenalInterest = d.PenalInterest
                }).ToList();

                return ApiResponse<List<DemandViewDto>>.SuccessResponse(demandDtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DemandViewDto>>.ErrorResponse("Error fetching demands", new List<string> { ex.Message });
            }
        }

        // Delete demands by year/month
        public async Task<ApiResponse<bool>> DeleteDemandAsync(int year, int month)
        {
            try
            {
                var demands = await _context.Demands
                    .Where(d => d.Year == year && d.Month == month)
                    .ToListAsync();

                if (!demands.Any())
                    return ApiResponse<bool>.ErrorResponse("No demand records found to delete");

                _context.Demands.RemoveRange(demands);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Demand deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Failed to delete demands", new List<string> { ex.Message });
            }
        }
    }
}
