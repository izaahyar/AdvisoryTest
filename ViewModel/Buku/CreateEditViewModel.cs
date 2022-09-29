using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvisoryTest.ViewModel.Buku
{
    public class CreateEditViewModel
    {
        public int ID { get; set; }
        public string Judul { get; set; }
        public string Pengarang { get; set; }
        public string Penerbit { get; set; }
        public int Tahun { get; set; }
        public int Stok { get; set; }
        public decimal Harga { get; set; }
    }
}
