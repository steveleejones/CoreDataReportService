using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreDataLibrary.Objects
{
    public class ActiveLogStep
    {
        public int Id { get; set; }
        public int LogItemId { get; set; }
        public DateTime StartTimeStamp { get; set; }
        public DateTime EndTimeStamp { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public string Messages { get; set; }
    }
}
