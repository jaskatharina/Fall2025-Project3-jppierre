using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_jppierre.Data;
using Fall2025_Project3_jppierre.Models;

namespace Fall2025_Project3_jppierre.Pages.Actors
{
    public class DeleteModel : PageModel
    {
        private readonly Fall2025_Project3_jppierre.Data.ApplicationDbContext _context;

        public DeleteModel(Fall2025_Project3_jppierre.Data.ApplicationDbContext context)
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

            var actor = await _context.Actor.FirstOrDefaultAsync(m => m.Id == id);

            if (actor is not null)
            {
                Actor = actor;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor.FindAsync(id);
            if (actor != null)
            {
                Actor = actor;
                _context.Actor.Remove(Actor);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
