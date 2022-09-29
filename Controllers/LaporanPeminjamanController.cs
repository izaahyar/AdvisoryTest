using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvisoryTest.Controllers
{
    public class LaporanPeminjamanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
