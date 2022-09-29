using System;
using System.Collections.Generic;

namespace AdvisoryTest.Models
{
    public partial class TblTPeminjamanHeader
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Peminjam { get; set; }
        public DateTime TanggalPinjam { get; set; }
        public DateTime TanggalPengembalian { get; set; }
        public decimal TotalBiaya { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DelBy { get; set; }
        public DateTime? DelDate { get; set; }
    }
}
