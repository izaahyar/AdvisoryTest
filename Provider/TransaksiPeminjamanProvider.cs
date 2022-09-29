using AdvisoryTest.Models;
using AdvisoryTest.ViewModel;
using AdvisoryTest.ViewModel.TransaksiPeminjaman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvisoryTest.Provider
{
    public class TransaksiPeminjamanProvider
    {
        private DB_AdvisoryTestContext context = new DB_AdvisoryTestContext();

        public TblTPeminjamanHeader GetByID(int ID)
        {
            return context.TblTPeminjamanHeader.Where(e => e.Id == ID).FirstOrDefault();
        }

        public IQueryable<ListTransaksiPeminjamanVM> GetList(string ID)
        {
            // pindah ke SP
            var queryA = context.TblTPeminjamanHeader.Where(e => !e.DelDate.HasValue && e.UserId == Convert.ToInt32(ID)).AsEnumerable()
                       .Select((a, index) => new
                       {
                           ID = a.Id,
                           Peminjam = a.Peminjam,
                           TanggalPinjam = a.TanggalPinjam,
                           TanggalPengembalian = a.TanggalPengembalian,
                           TotalBiaya = a.TotalBiaya,
                           No = index + 1
                       }).AsQueryable();

            var query = from a in queryA
                        select new ListTransaksiPeminjamanVM
                        {
                            ID = a.ID,
                            No = a.No,
                            Peminjam = a.Peminjam,
                            TanggalPinjam = a.TanggalPinjam.ToString("dd MMM yyyy"),
                            TanggalPengembalian = a.TanggalPengembalian.ToString("dd MMM yyyy"),
                            TotalBiaya = a.TotalBiaya
                        };
            return query;
        }

        public IQueryable<ListBookSelectVM> GetListBookSelected(string ID, ListTransaksiPeminjamanVM model)
        {
            int PeminjamanHeaderId = model.ID;
            var queryA = context.TblTPeminjamanDetail.Where(e => !e.DelDate.HasValue && e.PeminjamanHeaderId == PeminjamanHeaderId).AsEnumerable()
                       .Select((a, index) => new
                       {
                           ID = a.Id,
                           PeminjamanHeaderId = PeminjamanHeaderId,
                           BukuId = a.BukuId,
                           No = index + 1
                       }).AsQueryable();

            var query = from a in queryA
                        join b in context.TblMBuku on a.BukuId equals b.Id
                        select new ListBookSelectVM
                        {
                            ID = a.ID,
                            No = a.No,
                            Judul = b.Judul,
                            Harga = b.HargaPerHari
                        };
            return query;
        }

        public CalculatorViewModel GetCalculator(CalculatorViewModel model)
        {
            CalculatorViewModel data = new CalculatorViewModel();

            CalculatorService.CalculatorSoapClient client = new CalculatorService.CalculatorSoapClient(CalculatorService.CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);

            var ress = client.MultiplyAsync(model.Value1, model.Value2);
            data.Result = ress.Result;
            return data;
        }


        public AjaxViewModel SavePeminjaman(CreateEditViewModel model, string userID, string ID)
        {
            var result = new AjaxViewModel();
            try
            {
                TblTPeminjamanHeader head = new TblTPeminjamanHeader();
                head.UserId = 0;
                head.Peminjam = "";
                head.TanggalPinjam = DateTime.Now;
                head.TanggalPengembalian = model.TanggalPengembalian;
                head.TotalBiaya = model.TotalBiayaPinjaman;
                head.CreatedBy = 100;
                head.CreatedDate = DateTime.Now;
                context.TblTPeminjamanHeader.Add(head);
                context.SaveChanges();

                var addDetail = from a in model.ListData
                                select new TblTPeminjamanDetail
                                {
                                    PeminjamanHeaderId = head.Id,
                                    BukuId = a.BukuID,
                                    CreatedBy = 100,
                                    CreatedDate = DateTime.Now
                                };
                context.TblTPeminjamanDetail.AddRange(addDetail);
                context.SaveChanges();

                UpdateStok(model.ListData, "save");
            }
            catch (Exception ex)
            {
                result.SetValues(false, null, ex.InnerException.Message);
                return result;
            }
            result.SetValues(true, null, "Save Success");
            return result;
        }

        public AjaxViewModel Delete(int ID)
        {
            var result = new AjaxViewModel();
            try
            {
                TblTPeminjamanHeader tm = context.TblTPeminjamanHeader.Where(e => e.Id == ID).FirstOrDefault();
                if (tm != null)
                {
                    tm.UserId = tm.UserId;
                    tm.Peminjam = tm.Peminjam;
                    tm.TanggalPinjam = tm.TanggalPinjam;
                    tm.TanggalPengembalian = tm.TanggalPengembalian;
                    tm.TotalBiaya = tm.TotalBiaya;
                    tm.CreatedBy = tm.CreatedBy;
                    tm.CreatedDate = tm.CreatedDate;
                    tm.DelBy = 100;
                    tm.DelDate = DateTime.Now;
                    context.TblTPeminjamanHeader.Update(tm);
                    context.SaveChanges();

                    var detail = from a in context.TblTPeminjamanDetail.Where(e => e.PeminjamanHeaderId == ID)
                                 select new TblTPeminjamanDetail
                                 {
                                     Id = a.Id,
                                     PeminjamanHeaderId = ID,
                                     BukuId = a.BukuId,
                                     CreatedBy = a.CreatedBy,
                                     CreatedDate = a.CreatedDate,
                                     DelBy = 100,
                                     DelDate = DateTime.Now
                                 };
                    context.TblTPeminjamanDetail.UpdateRange(detail);
                    context.SaveChanges();

                    var ListData = (from a in detail
                                    select new ListBookSelectVM
                                    {
                                        BukuID = Convert.ToInt32(a.BukuId)
                                    }).ToList();

                    UpdateStok(ListData, "delete");

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
        private void UpdateStok(List<ListBookSelectVM> ListData, string action)
        {
            var addordevide = from a in ListData
                              group a by a.BukuID into g
                              select new
                              {
                                  BukuId = g.First().BukuID,
                                  Total = g.Count()
                              };

            var query = from a in context.TblMBuku
                        join b in addordevide on a.Id equals b.BukuId
                        select new TblMBuku
                        {
                            Id = a.Id,
                            Judul = a.Judul,
                            Pengarang = a.Pengarang,
                            Penerbit = a.Penerbit,
                            Tahun = a.Tahun,
                            Stok = action == "save" ? a.Stok - b.Total : a.Stok + b.Total,
                            HargaPerHari = a.HargaPerHari,
                            CreatedBy = a.CreatedBy,
                            CreatedDate = a.CreatedDate
                        };

            context.TblMBuku.UpdateRange(query);
            context.SaveChanges();

        }
    }
}
