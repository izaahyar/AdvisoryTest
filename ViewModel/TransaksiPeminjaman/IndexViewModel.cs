using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvisoryTest.ViewModel.TransaksiPeminjaman
{
    public class IndexViewModel
    {
        public int ID { get; set; }
        public string Judul { get; set; }
        public string Peminjam { get; set; }
        public string TanggalPinjam { get; set; }
        public string TanggalKembali { get; set; }
        public int Harga { get; set; }
        public List<ListBookSelectVM> ListData { get; set; }
    }

    public class ListBookSelectVM
    {
        public int ID { get; set; }
        public int No { get; set; }
        public int BukuID { get; set; }
        public string Judul { get; set; }
        public decimal Harga { get; set; }

    }
}
