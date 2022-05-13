using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tracking.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected DataContext db;

        public BaseController(DataContext _db)
        {
            db = _db;
        }

       
    }
}
