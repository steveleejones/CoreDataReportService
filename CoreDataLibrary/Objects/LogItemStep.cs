using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CoreDataLibrary.Objects
{
    public class LogItemStep
    {
        [Key]public int Id { get; set; }
        public int LogItemId { get; set; }
        public DateTime StartTimeStamp { get; set; }
        public DateTime EndTimeStamp { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public string Messages { get; set; }

        public TimeSpan TimeTakenForStep
        {
            get
            {
                return EndTimeStamp - StartTimeStamp;
            }
        }

        public String StartTime
        {
            get
            {
                return StartTimeStamp.ToLongTimeString();
            }
        }

        public String EndTime
        {
            get
            {
                return EndTimeStamp.ToLongTimeString();
            }
        }
    }
}
