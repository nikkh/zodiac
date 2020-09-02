using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sirmione.Web.Controllers
{
    
        [AllowAnonymous]
        public class AccountController : Controller
        {
            [HttpGet]
            public IActionResult SignOut(string page)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    
}
