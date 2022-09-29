using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvisoryTest.ViewModel.TransaksiPeminjaman
{
    public class ListTransaksiPeminjamanVM
    {
        public int ID { get; set; }
        public int No { get; set; }
        public string Peminjam { get; set; }
        public string TanggalPinjam { get; set; }
        public string TanggalPengembalian { get; set; }
        public decimal TotalBiaya { get; set; }
    }
}
