using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_jppierre.Data;
using Fall2025_Project3_jppierre.Models;

namespace Fall2025_Project3_jppierre.Pages.Actors
{
    public class EditModel : PageModel
    {
        private readonly Fall2025_Project3_jppierre.Data.ApplicationDbContext _context;

        public EditModel(Fall2025_Project3_jppierre.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Actor Actor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor =  await _context.Actor.FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            Actor = actor;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? PhotoFile)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Handle new photo upload if provided
            if (PhotoFile != null && PhotoFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await PhotoFile.CopyToAsync(memoryStream);
                    Actor.Photo = memoryStream.ToArray();
                }
            }
            else
            {
                // Keep the existing photo if no new file uploaded
                var existingActor = await _context.Actor.AsNoTracking().FirstOrDefaultAsync(a => a.Id == Actor.Id);
                if (existingActor != null)
                {
                    Actor.Photo = existingActor.Photo;
                }
            }

            _context.Attach(Actor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(Actor.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ActorExists(int id)
        {
            return _context.Actor.Any(e => e.Id == id);
        }
    }
}
