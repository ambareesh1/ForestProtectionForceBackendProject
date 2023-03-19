using ForestProtectionForce.Data;
using Microsoft.EntityFrameworkCore;

namespace ForestProtectionForce.Services
{
    public class BaselineService
    {
        private readonly ForestProtectionForceContext _context;
        public BaselineService(ForestProtectionForceContext context)
        {
            _context = context;
        }

        public string GenerateCaseNumber()
        {
            // Get the last inserted case number from the database
            var lastCaseNumber = _context.Baseline?
                .OrderByDescending(b => b.CaseNo)
                .FirstOrDefault()?.CaseNo;

            // Extract the numeric portion of the last case number
            var lastNumber = int.TryParse(lastCaseNumber?.Substring(4) ?? "00000", out var number)
                ? number
                : 0;

            // Generate the new case number with the next number
            var nextNumber = lastNumber + 1;
            var newCaseNumber = $"WCCB{nextNumber:D5}";

            return newCaseNumber;
        }


    }
}
