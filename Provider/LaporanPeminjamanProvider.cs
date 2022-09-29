using AdvisoryTest.Models;
using AdvisoryTest.ViewModel.LaporanPeminjaman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvisoryTest.Provider
{
    public class LaporanPeminjamanProvider
    {
        private DB_AdvisoryTestContext context = new DB_AdvisoryTestContext();

        public IQueryable<ListLaporanPeminjamanVM> GetList(string ID)
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

            var queryCount = from a in queryA
                             join b in context.TblTPeminjamanDetail on a.ID equals b.PeminjamanHeaderId
                             group b by new { b.PeminjamanHeaderId } into g
                             select new
                             {
                                 PeminjamanHeaderId = g.Key.PeminjamanHeaderId,
                                 Count = g.Count()
                             };

            var query = from a in queryA
                        join b in queryCount on a.ID equals b.PeminjamanHeaderId
                        select new ListLaporanPeminjamanVM
                        {
                            ID = a.ID,
                            Peminjam = a.Peminjam,
                            TanggalPinjam = a.TanggalPinjam.ToString("dd MMMM YYYY"),
                            TanggalPengembalian = a.TanggalPengembalian.ToString("dd MMMM YYYY"),
                            TotalBiaya = a.TotalBiaya,
                            JUmlahBuku = b.Count,
                            No = a.No
                        };

            //JUmlahBuku
            return query;
        }
    }
}
