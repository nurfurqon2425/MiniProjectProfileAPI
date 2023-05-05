using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProfileController(ApplicationDbContext db)
        {
            _db = db;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> GetProfile()
        {
            var profiles = await _db.Profile.ToListAsync();
            return Ok(profiles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetProfile(int id)
        {
            var profile = await _db.Profile.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(Profile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Profile.Add(profile);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfile), new { id = profile.Id }, profile);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] Profile profile)
        {
            if (profile == null || profile.Id != id)
            {
                return BadRequest();
            }

            var existingProfile = await _db.Profile.FindAsync(id);
            if (existingProfile == null)
            {
                return NotFound();
            }

            existingProfile.Name = profile.Name;
            existingProfile.Address = profile.Address;
            existingProfile.DateofBirth = profile.DateofBirth;
            existingProfile.Gender = profile.Gender;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500); // Handle the exception appropriately
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var profile = await _db.Profile.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _db.Profile.Remove(profile);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
