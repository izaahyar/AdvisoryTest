using System;
using System.Collections.Generic;

namespace AdvisoryTest.Models
{
    public partial class TblTPeminjamanDetail
    {
        public int Id { get; set; }
        public int PeminjamanHeaderId { get; set; }
        public int? BukuId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DelBy { get; set; }
        public DateTime? DelDate { get; set; }
    }
}
