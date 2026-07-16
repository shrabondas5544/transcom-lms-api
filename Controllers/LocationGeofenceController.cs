using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using transcom_lms_api.Data;
using transcom_lms_api.Models;

namespace transcom_lms_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationGeofenceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocationGeofenceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LocationGeofence
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.LocationGeofences
                .OrderBy(l => l.LocationName)
                .ToListAsync();
            return Ok(list);
        }

        // GET: api/LocationGeofence/locations - returns distinct location names from EmployeeProfiles
        [HttpGet("locations")]
        public async Task<IActionResult> GetDistinctLocations()
        {
            var locations = await _context.EmployeeProfiles
                .Where(e => !string.IsNullOrEmpty(e.Showroom))
                .Select(e => e.Showroom)
                .Distinct()
                .OrderBy(l => l)
                .ToListAsync();

            return Ok(locations);
        }

        // POST: api/LocationGeofence
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationGeofence body)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Upsert: if a record with the same location name exists, update it
            var existing = await _context.LocationGeofences
                .FirstOrDefaultAsync(l => l.LocationName == body.LocationName);

            if (existing != null)
            {
                existing.Latitude = body.Latitude;
                existing.Longitude = body.Longitude;
                existing.AllowedRadiusMeters = body.AllowedRadiusMeters;
                await _context.SaveChangesAsync();
                return Ok(existing);
            }

            _context.LocationGeofences.Add(body);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = body.Id }, body);
        }

        // PUT: api/LocationGeofence/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LocationGeofence body)
        {
            var existing = await _context.LocationGeofences.FindAsync(id);
            if (existing == null) return NotFound();

            existing.LocationName = body.LocationName;
            existing.Latitude = body.Latitude;
            existing.Longitude = body.Longitude;
            existing.AllowedRadiusMeters = body.AllowedRadiusMeters;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // DELETE: api/LocationGeofence/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.LocationGeofences.FindAsync(id);
            if (existing == null) return NotFound();

            _context.LocationGeofences.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
