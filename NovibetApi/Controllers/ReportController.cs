using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NovibetApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NovibetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly string _connectionString;

        public ReportController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("country-report")]
        public async Task<IActionResult> GetCountryReport([FromQuery] string[] countryCodes = null)
        {
            var reports = new List<CountryReport>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                SELECT 
                    c.Name AS CountryName,
                    COUNT(ip.ID) AS AddressesCount,
                    MAX(ip.UpdatedAt) AS LastAddressUpdated
                FROM Countries c
                LEFT JOIN IPAddresses ip ON c.ID = ip.CountryId";

                    if (countryCodes != null && countryCodes.Length > 0)
                    {
                        // Build the WHERE clause dynamically
                        var codesList = string.Join("','", countryCodes);
                        query += $" WHERE c.TwoLetterCode IN ('{codesList}')";
                    }

                    query += " GROUP BY c.Name";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var report = new CountryReport
                                {
                                    CountryName = reader.GetString(0),
                                    AddressesCount = reader.GetInt32(1),
                                    LastAddressUpdated = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2)
                                };
                                reports.Add(report);
                            }
                        }
                    }
                }
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
