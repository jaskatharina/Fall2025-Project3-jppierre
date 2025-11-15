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
    public class IndexModel : PageModel
    {
        private readonly Fall2025_Project3_jppierre.Data.ApplicationDbContext _context;

        public IndexModel(Fall2025_Project3_jppierre.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Actor> Actors { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Actors = await _context.Actor.ToListAsync();
        }
    }
}
