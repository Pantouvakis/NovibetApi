using Microsoft.Extensions.Caching.Memory;
using NovibetApi.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NovibetApi.Services
{
    public class IpInfoService
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public IpInfoService(IMemoryCache cache, ApplicationDbContext context, HttpClient httpClient)
        {
            _cache = cache;
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<IPAddress> GetIpInfoAsync(string ip)
        {
            try
            {
                // cache
                if (_cache.TryGetValue(ip, out IPAddress cachedIpInfo))
                {
                    return cachedIpInfo;
                }

                // database Entity FM
                var dbIpInfo = await _context.IPAddresses
                                             .Include(ip => ip.Country)
                                             .FirstOrDefaultAsync(i => i.IP == ip);//where

                if (dbIpInfo != null)
                {
                    _cache.Set(ip, dbIpInfo, TimeSpan.FromMinutes(10)); // Cache expiration
                    return dbIpInfo;
                }

                // IP2C
                var ip2cResponse = await _httpClient.GetStringAsync($"https://ip2c.org/{ip}");

                var parts = ip2cResponse.Split(';');
                if (parts[0] != "1")
                {
                    return null;
                }

                var countryTwoLetterCode = parts[1];
                var countryThreeLetterCode = parts[2];
                var countryName = parts[3];

                var country = await _context.Countries.FirstOrDefaultAsync(c => c.TwoLetterCode == countryTwoLetterCode);

                if (country == null)
                {
                    country = new Country
                    {
                        Name = countryName,
                        TwoLetterCode = countryTwoLetterCode,
                        ThreeLetterCode = countryThreeLetterCode,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Countries.Add(country);
                    await _context.SaveChangesAsync();
                }

                var ipAddress = new IPAddress
                {
                    IP = ip,
                    CountryId = country.Id,
                    Country = country,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Store in the database
                _context.IPAddresses.Add(ipAddress);
                await _context.SaveChangesAsync();

                // Store in the cache
                _cache.Set(ip, ipAddress, TimeSpan.FromMinutes(10));

                return ipAddress;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
