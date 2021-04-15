using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Platform.Models
{
    public class SeedData
    {
        private CalculationContext _context;
        private ILogger<SeedData> _logger;

        private static Dictionary<int, long> _data = new Dictionary<int, long>()
        {
            {1, 1}, {2, 3}, {3, 6}, {4, 10}, {5, 15},
            {6, 21}, {7, 28}, {8, 36}, {9, 45}, {10, 55}
        };

        public SeedData(CalculationContext context, ILogger<SeedData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void SeedDatabase()
        {
            _context.Database.Migrate();
            if (_context.Calculations.Count() == 0)
            {
                _logger.LogInformation("Preparing to seed database");
                _context.Calculations!.AddRange(_data.Select(kvp => new Calculation {
                    Count = kvp.Key, Result = kvp.Value
                }));
                _context.SaveChanges();
                _logger.LogInformation("Database Seeded");
            }
            else
            {
                _logger.LogInformation("Database not Seeded");
            }
        }
    }
}