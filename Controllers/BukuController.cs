using AdvisoryTest.Provider;
using AdvisoryTest.ViewModel;
using AdvisoryTest.ViewModel.Buku;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvisoryTest.Controllers
{
    public class BukuController : Controller
    {
        private BukuProvider _provider = new BukuProvider();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult List(DTParameters param)
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var Data = _provider.List();
                /*
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    Data = Data.OrderBy(sortColumn + " " + sortColumnDirection);
                }*/
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Data = Data.Where(m => m.Judul.Contains(searchValue)
                                        || m.Pengarang.Contains(searchValue)
                                        || m.Penerbit.Contains(searchValue)
                                        || m.Tahun.ToString().Contains(searchValue)
                                        || m.Stok.ToString().Contains(searchValue)
                                        || m.Harga.ToString().Contains(searchValue));
                }
                recordsTotal = Data.Count();
                var data = Data.ToList().Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult Create()
        {
            var model = new CreateEditViewModel();
            return PartialView("CreateEdit", model);
        }

        public IActionResult Edit(int ID)
        {
            var model = new CreateEditViewModel();
            var buku = _provider.GetByID(ID);

            if (buku != null)
            {
                model.ID = buku.Id;
                model.Judul = buku.Judul;
                model.Pengarang = buku.Pengarang;
                model.Penerbit = buku.Penerbit;
                model.Tahun = buku.Tahun;
                model.Stok = buku.Stok;
                model.Harga = buku.HargaPerHari;
            }
            return PartialView("CreateEdit", model);
        }

        [HttpPost]
        public IActionResult CreateEdit(CreateEditViewModel model)
        {
            if (model.ID > 0)
            {
                var result = _provider.Edit(model);
                return Json(result);
            }
            else
            {
                var result = _provider.Create(model);
                return Json(result);
            }
        }

        [HttpPost]
        public IActionResult Delete(int ID)
        {
            var result = _provider.Delete(ID);
            return Json(result);
        }

        [HttpPost]
        public IActionResult ListTahun()
        {
            try
            {
                var tahun = _provider.GetListTahun();
                return Json(tahun);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
