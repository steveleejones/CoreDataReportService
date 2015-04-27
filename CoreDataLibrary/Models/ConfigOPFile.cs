using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataLibrary.Models
{
    public class ConfigOPFile
    {
        public int FileCriteriaID { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public string Site { get; set; }

        public string PackageGroup { get; set; }

        public string SellingCurrency { get; set; }

        public int MaxOffers { get; set; }

        public int StartDays { get; set; }

        public int EndDays { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal MinPrice { get; set; }

        public string OutputName { get; set; }

        public string OutputPath { get; set; }

        public string Template { get; set; }

        public string Frequency { get; set; }

        public string Login { get; set; }

        public bool CastlesOnly { get; set; }

        public bool Include { get; set; }
    }
}
