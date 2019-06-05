using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ActiveBear.Models;

namespace ActiveBear.Pages.Messages
{
    public class IndexModel : PageModel
    {
        private readonly ActiveBear.Models.ActiveBearContext _context;

        public IndexModel(ActiveBear.Models.ActiveBearContext context)
        {
            _context = context;
        }

        public IList<Message> Message { get;set; }

        public async Task OnGetAsync()
        {
            Message = await _context.Message.ToListAsync();
        }
    }
}
