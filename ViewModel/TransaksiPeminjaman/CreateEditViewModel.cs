using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvisoryTest.ViewModel.TransaksiPeminjaman
{
    public class CreateEditViewModel
    {
        public int ID { get; set; }
        public int PeminjamanHeaderId { get; set; }
        public int Buku { get; set; }
        public int Stok { get; set; }
        public DateTime TanggalPengembalian { get; set; }
        public decimal TotalBiayaPinjaman{ get; set; }
        public List<ListBookSelectVM> ListData { get; set; }

    }
}
