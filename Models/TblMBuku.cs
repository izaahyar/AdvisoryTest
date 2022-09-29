using System;
using System.Collections.Generic;

namespace AdvisoryTest.Models
{
    public partial class TblMBuku
    {
        public int Id { get; set; }
        public string Judul { get; set; }
        public string Pengarang { get; set; }
        public string Penerbit { get; set; }
        public int Tahun { get; set; }
        public int Stok { get; set; }
        public decimal HargaPerHari { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DelBy { get; set; }
        public DateTime? DelDate { get; set; }
    }
}
