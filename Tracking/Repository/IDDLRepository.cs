using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracking.Repository
{
    public interface IDDLRepository
    {
        Task<List<SelectListItem>> LoadSenders(int ide);
        Task<List<SelectListItem>> LoadReceivers(int ide);
        Task<List<SelectListItem>> LoadBank();
    }
}
