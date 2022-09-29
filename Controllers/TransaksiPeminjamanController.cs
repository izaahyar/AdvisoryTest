using AdvisoryTest.Provider;
using AdvisoryTest.ViewModel;
using AdvisoryTest.ViewModel.TransaksiPeminjaman;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvisoryTest.Controllers
{
    public class TransaksiPeminjamanController : Controller
    {
        private BukuProvider _bukuprovider = new BukuProvider();
        private TransaksiPeminjamanProvider _tpprovider = new TransaksiPeminjamanProvider();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var model = new CreateEditViewModel();
            model.TanggalPengembalian = DateTime.Now.Date;
            return PartialView("CreateEdit", model);
        }

        public IActionResult Edit(int ID)
        {
            var model = new CreateEditViewModel();
            var header = _tpprovider.GetByID(ID);

            if (header != null)
            {
                model.ID = header.Id;
                model.PeminjamanHeaderId = ID;
                model.TanggalPengembalian = header.TanggalPengembalian;
            }
            return PartialView("CreateEdit", model);
        }

        [HttpPost]
        public IActionResult Delete(int ID)
        {
            var result = _tpprovider.Delete(ID);
            return Json(result);
        }

        [HttpPost]
        public IActionResult GetListPeminjaman()
        {
            //string userID = HttpContext.Session.GetString(Constants.SessionName.Username);
            //string ID = HttpContext.Session.GetString(Constants.SessionName.ID);
            string ID = "0";

            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var Data = _tpprovider.GetList(ID);

            if (!string.IsNullOrEmpty(searchValue))
            {
                Data = Data.Where(m => m.Peminjam.Contains(searchValue)
                                    || m.TanggalPinjam.Contains(searchValue)
                                    || m.TanggalPengembalian.Contains(searchValue)
                                    || m.TotalBiaya.ToString().Contains(searchValue));
            }
            recordsTotal = Data.Count();
            var data = Data.ToList().Skip(skip).Take(pageSize).ToList();

            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult GetListBookSelected(ListTransaksiPeminjamanVM model)
        {
            //string userID = HttpContext.Session.GetString(Constants.SessionName.Username);
            //string ID = HttpContext.Session.GetString(Constants.SessionName.ID);
            string ID = "0";
            var data = _tpprovider.GetListBookSelected(ID, model);
            return Json(data);
        }

        [HttpPost]
        public IActionResult GetCalculator(CalculatorViewModel model)
        {
            CalculatorViewModel data = _tpprovider.GetCalculator(model);
            return Json(data);
        }

        [HttpPost]
        public JsonResult SavePeminjaman(CreateEditViewModel model)
        {
            //string userID = HttpContext.Session.GetString(Constants.SessionName.Username);
            //string ID = HttpContext.Session.GetString(Constants.SessionName.ID);
            string userID = "0";
            string ID = "0";

            var result = _tpprovider.SavePeminjaman(model, userID, ID);
            return Json(result);

        }

        [HttpGet]
        public JsonResult GetListBook()
        {
            try
            {
                var data = _bukuprovider.List();
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json("E-" + ex.Message);
            }
        }
    }
}
