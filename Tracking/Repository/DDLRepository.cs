using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracking.Repository
{
    public class DDLRepository : IDDLRepository
    {
        private readonly DataContext _db;

        public DDLRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<List<SelectListItem>> LoadSenders(int ide)
        => await _db.Users.Where(e => e.Username != "banka" && e.UserID == ide).Select(t => new SelectListItem { Text = t.Username, Value = t.UserID.ToString() }).ToListAsync();

        public async Task<List<SelectListItem>> LoadReceivers(int ide)
        => await _db.Users.Where(e => e.Username != "banka" && e.UserID != ide).Select(t => new SelectListItem { Text = t.Username, Value = t.UserID.ToString() }).ToListAsync();

        public async Task<List<SelectListItem>> LoadBank()
        => await _db.Users.Where(e => e.Username.Equals("banka")).Select(t => new SelectListItem { Text = t.Username, Value = t.UserID.ToString() }).ToListAsync();

        
    }
}
