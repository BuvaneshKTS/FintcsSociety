using Microsoft.EntityFrameworkCore;
using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintcsApi.Services.Implementations
{
    public class SocietyService : ISocietyService
    {
        private readonly AppDbContext _context;

        public SocietyService(AppDbContext context)
        {
            _context = context;
        }

        // Create a new society
        public async Task<ApiResponse<SocietyDto>> CreateSocietyAsync(SocietyCreateUpdateDto dto)
        {
            try
            {
                Console.WriteLine("Starting CreateSocietyAsync...");

                // Step 1: Check if society is unique
                Console.WriteLine("Checking if society is unique...");
                if (!await IsSocietyUniqueAsync(dto))
                {
                    Console.WriteLine("Society is not unique.");
                    return ApiResponse<SocietyDto>.ErrorResponse(
                        "A society with the same Name, Email, Phone, Website, or Registration Number already exists.");
                }
                Console.WriteLine("Society is unique. Proceeding to create...");

                // Step 2: Map DTO to entity
                var society = new Society
                {
                    Name = dto.Name,
                    Address = dto.Address,
                    City = dto.City,
                    Phone = dto.Phone,
                    Fax = dto.Fax,
                    Email = dto.Email,
                    Website = dto.Website,
                    RegistrationNumber = dto.RegistrationNumber,
                    ChequeBounceCharge = dto.ChequeBounceCharge,
                    CreatedAt = DateTime.UtcNow
                };
                Console.WriteLine($"Society entity created: {society.Name}, {society.Email}");

                // Step 3: Add to DbContext
                Console.WriteLine("Adding society to DbContext...");
                _context.Societies.Add(society);

                // Step 4: Save changes to database
                Console.WriteLine("Saving changes to database...");
                await _context.SaveChangesAsync();
                Console.WriteLine($"Society saved with Id: {society.Id}");

                // Step 5: Fetch the saved society
                Console.WriteLine("Fetching the society by Id...");
                var response = await GetSocietyByIdAsync(society.Id);
                Console.WriteLine("Society fetched successfully.");

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return ApiResponse<SocietyDto>.ErrorResponse($"Error creating society: {ex.Message}");
            }
        }


        // Get society by Id (includes LoanTypes + BankAccounts)
        public async Task<ApiResponse<SocietyDto>> GetSocietyByIdAsync(int id)
        {
            var society = await _context.Societies
                .Include(s => s.LoanTypes)
                .Include(s => s.BankAccounts)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (society == null)
                return ApiResponse<SocietyDto>.ErrorResponse("Society not found.");

            return ApiResponse<SocietyDto>.SuccessResponse(MapToDto(society));
        }

        // Get all societies
        public async Task<ApiResponse<List<SocietyDto>>> GetAllSocietiesAsync()
        {
            var societies = await _context.Societies
                .Include(s => s.LoanTypes)
                .Include(s => s.BankAccounts)
                .ToListAsync();

            var dtos = societies.Select(MapToDto).ToList();
            return ApiResponse<List<SocietyDto>>.SuccessResponse(dtos);
        }

        // Update society
        public async Task<ApiResponse<SocietyDto>> UpdateSocietyAsync(int id, SocietyCreateUpdateDto dto)
        {
            try
            {
                var society = await _context.Societies.FindAsync(id);
                if (society == null) return ApiResponse<SocietyDto>.ErrorResponse("Society not found.");

                if (!await IsSocietyUniqueAsync(dto, id))
                    return ApiResponse<SocietyDto>.ErrorResponse(
                        "A society with the same Name, Email, Phone, Website, or Registration Number already exists.");

                society.Name = dto.Name;
                society.Address = dto.Address;
                society.City = dto.City;
                society.Phone = dto.Phone;
                society.Fax = dto.Fax;
                society.Email = dto.Email;
                society.Website = dto.Website;
                society.RegistrationNumber = dto.RegistrationNumber;
                society.ChequeBounceCharge = dto.ChequeBounceCharge;
                society.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return await GetSocietyByIdAsync(society.Id);
            }
            catch (Exception ex)
            {
                return ApiResponse<SocietyDto>.ErrorResponse($"Error updating society: {ex.Message}");
            }
        }

        // Delete society
        public async Task<ApiResponse<bool>> DeleteSocietyAsync(int id)
        {
            try
            {
                var society = await _context.Societies.FindAsync(id);
                if (society == null) return ApiResponse<bool>.ErrorResponse("Society not found.");

                _context.Societies.Remove(society);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Society deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse($"Error deleting society: {ex.Message}");
            }
        }

        // --- Helper Methods ---
        private async Task<bool> IsSocietyUniqueAsync(SocietyCreateUpdateDto dto, int? id = null)
        {
            return !await _context.Societies.AnyAsync(s =>
                (s.Name == dto.Name || s.Email == dto.Email || s.Phone == dto.Phone ||
                 s.Website == dto.Website || s.RegistrationNumber == dto.RegistrationNumber) &&
                (id == null || s.Id != id.Value));
        }

        private SocietyDto MapToDto(Society society)
        {
            return new SocietyDto
            {
                Id = society.Id,
                Name = society.Name,
                Address = society.Address,
                City = society.City,
                Phone = society.Phone,
                Fax = society.Fax,
                Email = society.Email,
                Website = society.Website,
                RegistrationNumber = society.RegistrationNumber,
                ChequeBounceCharge = society.ChequeBounceCharge,
                LoanTypes = society.LoanTypes.Select(lt => new LoanTypeDto
                {
                    LoanTypeId = lt.LoanTypeId,
                    SocietyId = lt.SocietyId,
                    Name = lt.Name,
                    InterestPercent = lt.InterestPercent,
                    LimitAmount = lt.LimitAmount,
                    CompulsoryDeposit = lt.CompulsoryDeposit,
                    OptionalDeposit = lt.OptionalDeposit,
                    ShareAmount = lt.ShareAmount,
                    XTimes = lt.XTimes
                }).ToList(),
                BankAccounts = society.BankAccounts.Select(ba => new BankAccountDto
                {
                    Id = ba.Id,
                    SocietyId = ba.SocietyId,
                    BankName = ba.BankName,
                    AccountNumber = ba.AccountNumber,
                    IFSC = ba.IFSC,
                    Branch = ba.Branch,
                    Notes = ba.Notes
                }).ToList()
            };
        }
    }
}
