using System;
using System.Collections.Generic;
using System.Text;

namespace Prank.DAL
{
    public class DbStatusResult
    {
        public bool Status { get; set; }
        public int Id { get; set; }
        public long LongId { get; set; }
        public string Message { get; set; }
        public string ResultData { get; set; }
        public /*Guid*/string Guid { get; set; }
        public object Key { get; set; }
        public int RecordCount { get; set; }
    }
}
