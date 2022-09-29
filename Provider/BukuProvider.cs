using AdvisoryTest.Models;
using AdvisoryTest.ViewModel;
using AdvisoryTest.ViewModel.Buku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvisoryTest.Provider
{
    public class BukuProvider
    {
        private DB_AdvisoryTestContext context = new DB_AdvisoryTestContext();

        public TblMBuku GetByID(int ID)
        {
            return context.TblMBuku.Where(e => e.Id == ID).FirstOrDefault();
        }
        public IQueryable<ListBukuViewModel> List()
        {
            var queryA = context.TblMBuku.Where(e => !e.DelDate.HasValue).AsEnumerable()
                        .Select((a, index) => new {
                            ID = a.Id,
                            Judul = a.Judul,
                            Pengarang = a.Pengarang,
                            Penerbit = a.Penerbit,
                            Tahun = a.Tahun,
                            Stok = a.Stok,
                            Harga = a.HargaPerHari,
                            No = index + 1
                        }).AsQueryable();

            var query = from a in queryA
                        select new ListBukuViewModel
                        {
                            ID = a.ID,
                            No = a.No,
                            Judul = a.Judul,
                            Pengarang = a.Pengarang,
                            Penerbit = a.Penerbit,
                            Tahun = a.Tahun,
                            Stok = a.Stok,
                            Harga = a.Harga,
                        };
            return query;
        }

        public AjaxViewModel Create(CreateEditViewModel model)
        {
            var result = new AjaxViewModel();
            try
            {
                TblMBuku tm = new TblMBuku();
                tm.Judul = model.Judul;
                tm.Pengarang = model.Pengarang;
                tm.Penerbit = model.Penerbit;
                tm.Tahun = model.Tahun;
                tm.Stok = model.Stok;
                tm.HargaPerHari = model.Harga;
                tm.CreatedBy = 100;
                tm.CreatedDate = DateTime.Now;
                context.Add(tm);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                result.SetValues(false, null, ex.InnerException.Message);
                return result;
            }
            result.SetValues(true, null, "Save Success");
            return result;
        }

        public AjaxViewModel Edit(CreateEditViewModel model)
        {
            var result = new AjaxViewModel();
            try
            {
                TblMBuku tm = context.TblMBuku.Where(e => e.Id == Convert.ToInt32(model.ID)).FirstOrDefault();
                tm.Judul = model.Judul;
                tm.Pengarang = model.Pengarang;
                tm.Penerbit = model.Penerbit;
                tm.Tahun = model.Tahun;
                tm.Stok = model.Stok;
                tm.HargaPerHari = model.Harga;
                tm.UpdateBy = 100;
                tm.UpdateDate = DateTime.Now;
                context.TblMBuku.Update(tm);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                result.SetValues(false, null, ex.InnerException.Message);
                return result;
            }
            result.SetValues(true, null, "Edit Success");
            return result;
        }

        public AjaxViewModel Delete(int ID)
        {
            var result = new AjaxViewModel();
            try
            {
                TblMBuku tm = context.TblMBuku.Where(e => e.Id == ID).FirstOrDefault();
                if (tm != null)
                {
                    tm.DelBy = 100;
                    tm.DelDate = DateTime.Now;
                    context.TblMBuku.Update(tm);
                    context.SaveChanges();
                    result.IsSuccess = true;
                    result.Message = "Deleted";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Book not found";
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public IQueryable<DropDownListViewModel> GetListTahun()
        {
            List<DropDownListViewModel> queryList = new List<DropDownListViewModel>();
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 10; i <= currentYear + 10; i++)
            {
                queryList.Add(new DropDownListViewModel { Value = i.ToString(), Text = i.ToString() });
            }
            return queryList.AsQueryable();
        }
    }
}
