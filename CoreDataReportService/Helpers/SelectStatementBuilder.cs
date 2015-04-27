using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using CoreDataReportService.Data;


namespace CoreDataReportService.Helpers
{

    public class SelectStatementBuilder
    {
        char[] charsToTrim = { ',', ' ' };
        private readonly List<string> m_outputHeaders = new List<string>();
        private string m_languageId = "1";
        //private string m_name = "";

        public bool FTPFile { get; set; }
        public string FTPSite { get; set; }
        public bool ExcludePropertyReferenceId { get; set; }
        public bool ExcludePropertyName { get; set; }
        public bool ExcludeRating { get; set; }
        public bool ExcludePropertyType { get; set; }
        public bool ExcludeIATACode { get; set; }
        public bool ExcludeResortId { get; set; }
        public bool ExcludeRegionId { get; set; }
        public bool ExcludeCountryId { get; set; }
        public bool ExcludeResort { get; set; }
        public bool ExcludeRegion { get; set; }
        public bool ExcludeCountry { get; set; }
        public bool ExcludeAddress { get; set; }
        public bool ExcludeTownCity { get; set; }
        public bool ExcludeCounty { get; set; }
        public bool ExcludePostcodeZip { get; set; }
        public bool ExcludeTelephone { get; set; }
        public bool ExcludeFax { get; set; }
        public bool ExcludeLatitude { get; set; }
        public bool ExcludeLongitude { get; set; }
        public bool ExcludeStrapline { get; set; }
        public bool ExcludeDescription { get; set; }
        public bool ExcludeDistanceFromAirport { get; set; }
        public bool ExcludeTransferTime { get; set; }
        public bool ExcludeRightChoice { get; set; }
        public bool ExcludeLocationAndResort { get; set; }
        public bool ExcludeSwimmingPools { get; set; }
        public bool ExcludeEatingAndDrinking { get; set; }
        public bool ExcludeAccomodation { get; set; }
        public bool ExcludeSuitableFor { get; set; }
        public bool ExcludeUrl { get; set; }
        public bool ExcludeMainImage { get; set; }
        public bool ExcludeMainImageThumbnail { get; set; }
        public bool ExcludeImageList { get; set; }
        public bool ExcludeFacilitiesList { get; set; }
        public bool OwnStock { get; set; }
        public bool OwnStockLive { get; set; }
        public bool NotOwnStock { get; set; }
        public List<ListItem> Exclusions { get; set; }
        public List<ListItem> Inclusions { get; set; }

        public SelectStatementBuilder()
        {
            Exclusions = new List<ListItem>();
            Inclusions = new List<ListItem>();
        }

        //public string Name
        //{
        //    get { return m_name; }
        //    set { m_name = value; }
        //}

        public List<string> GetOutputHeaders
        {
            get { return m_outputHeaders; }
        }

        public string LanguageId
        {
            get { return m_languageId; }
            set { m_languageId = value; }
        }

        public string SelectStatement()
        {
            return BuildSelectStatement();
        }

        private string BuildSelectStatement()
        {
            StringBuilder selectStatement = new StringBuilder("SELECT ");

            selectStatement.Append(PopulateFields());

            selectStatement.Append(@" FROM [CoreData].[dbo].[Properties] AS p1 JOIN [CoreData].[dbo].[PropertyDetails] AS p3 ON p3.PropertyReferenceId = p1.PropertyReferenceId LEFT JOIN [CoreData].[dbo].[PropertyDescription] AS p4 ON p4.PropertyReferenceId = p1.PropertyReferenceId AND p4.LanguageId = ");
            selectStatement.Append(LanguageId);
            selectStatement.Append(" LEFT JOIN [CoreData].[dbo].[ResortAirport] AS p5 ON p5.ResortId = p1.ResortId LEFT JOIN [CoreData].[dbo].[Airports] AS p6 ON p6.AirportId = p5.AirportId LEFT JOIN [CoreData].[dbo].[PropertyImageLists] AS p7 ON p7.PropertyReferenceId = p1.PropertyReferenceId LEFT JOIN [CoreData].[dbo].[PropertyFacilitiesList] AS p9 ON p9.PropertyReferenceId = p1.PropertyReferenceId AND p9.LanguageId = ");
            selectStatement.Append(LanguageId);
            selectStatement.Append(" JOIN [CoreData].[dbo].[PropertyType] AS p8 ON p8.PropertyTypeId = p1.PropertyTypeId");

            selectStatement.Append(CreateStockClause());
            selectStatement.Append(CreateWhereClause(selectStatement));
            return selectStatement.ToString();
        }
        private string CreateWhereClause(StringBuilder selectStatement)
        {
            bool includeCountriesAdded = false;
            bool includeRegionsAdded = false;
            bool includeResortsAdded = false;

            string clause = "";
            bool whereToggle = selectStatement.ToString().Contains("WHERE");
            bool firstItem = true;
            List<string> excludeCountries = new List<string>();
            List<string> excludeRegions = new List<string>();
            List<string> excludeResorts = new List<string>();
            List<string> includeCountries = new List<string>();
            List<string> includeRegions = new List<string>();
            List<string> includeResorts = new List<string>();

            foreach (ListItem item in Exclusions)
            {
                if (item.Value.Contains("Country"))
                    excludeCountries.Add(item.Value.ToString().Split(' ')[3]);
                else if (item.Value.Contains("Region"))
                    excludeRegions.Add(item.Value.ToString().Split(' ')[3]);
                else if (item.Value.Contains("Resort"))
                    excludeResorts.Add(item.Value.ToString().Split(' ')[3]);
            }

            foreach (ListItem item in Inclusions)
            {
                if (item.Value.Contains("Country"))
                    includeCountries.Add(item.Value.ToString().Split(' ')[3]);
                else if (item.Value.Contains("Region"))
                    includeRegions.Add(item.Value.ToString().Split(' ')[3]);
                else if (item.Value.Contains("Resort"))
                    includeResorts.Add(item.Value.ToString().Split(' ')[3]);
            }

            if (excludeCountries.Any())
            {
                if (includeRegions.Any())
                {
                    clause = clause + " AND (p1.CountryId NOT IN " + GetWhereClauseItems(excludeCountries);
                    clause = clause + " OR p1.RegionId IN " + GetWhereClauseItems(includeRegions) + ")";
                    includeRegionsAdded = true;
                }
                else
                    clause = clause + " AND p1.CountryId NOT IN " + GetWhereClauseItems(excludeCountries);
            }
            if (excludeRegions.Any())
            {
                if (includeResorts.Any())
                {
                    clause = clause + " AND (p1.RegionId NOT IN " + GetWhereClauseItems(excludeRegions);
                    clause = clause + " OR p1.ResortId IN " + GetWhereClauseItems(includeResorts) + ")";
                    includeResortsAdded = true;
                }
                else
                    clause = clause + " AND p1.RegionId NOT IN " + GetWhereClauseItems(excludeRegions);
            }
            if (excludeResorts.Any())
            {
                clause = clause + " AND p1.ResortId NOT IN " + GetWhereClauseItems(excludeResorts);
            }

            bool needToOr = false;
            if (includeCountries.Any())
            {
                clause = clause + " AND p1.CountryId IN " + GetWhereClauseItems(includeCountries);
                needToOr = true;
            }
            if (includeRegions.Any() & !includeRegionsAdded)
            {
                if (needToOr)
                    clause = clause + " OR p1.RegionId IN " + GetWhereClauseItems(includeRegions);
                else
                    clause = clause + " AND p1.RegionId IN " + GetWhereClauseItems(includeRegions);
                needToOr = true;
            }
            if (includeResorts.Any() & !includeResortsAdded)
            {
                if (needToOr)
                    clause = clause + " OR p1.ResortId IN " + GetWhereClauseItems(includeResorts);
                else
                    clause = clause + " AND p1.ResortId IN " + GetWhereClauseItems(includeResorts);
            }
            return clause;
        }

        private string GetWhereClauseItems(List<string> itemList)
        {
            string value = "(";
            foreach (string i in itemList)
                value = value + i.ToString() + ", ";
            value = value.TrimEnd(charsToTrim);
            value = value + ") ";
            return value;
        }

        private string CreateStockClause()
        {
            if (OwnStock == true & NotOwnStock == true)
            {
                return " WHERE (IncludesOwnStock=1 OR IncludesOwnStock = 0) ";
            }
            else if (OwnStockLive == true & NotOwnStock == true)
            {
                return " WHERE ((IncludesOwnStock=1 AND ContractCount > 0) OR IncludesOwnStock = 0)";
            }
            else if (OwnStock == true & NotOwnStock == false)
            {
                return " WHERE (IncludesOwnStock=1) ";
            }
            else
            {
                return " WHERE (IncludesOwnStock=1 AND ContractCount > 0)";
            }
        }

        public string PopulateFields()
        {
            m_outputHeaders.Clear();
            StringBuilder stringBuilder = new StringBuilder();
            if (!ExcludePropertyReferenceId)
            {
                m_outputHeaders.Add("PropertyReferenceId");
                stringBuilder.Append("p1.PropertyReferenceId AS PropertyReferenceId, ");
            }
            if (!ExcludePropertyName)
            {
                m_outputHeaders.Add("PropertyPropertyName");
                stringBuilder.Append("p1.PropertyName AS PropertyName, ");
            }
            if (!ExcludeRating)
            {
                m_outputHeaders.Add("Rating");
                stringBuilder.Append("p1.Rating AS Rating, ");
            }
            if (!ExcludePropertyType)
            {
                m_outputHeaders.Add("PropertyType");
                stringBuilder.Append("p8.PropertyType AS PropertyType, ");
            }
            if (!ExcludeIATACode)
            {
                m_outputHeaders.Add("AirportCode");
                stringBuilder.Append("p6.IATACode  AS IATACode, ");
            }
            if (!ExcludeResortId)
            {
                m_outputHeaders.Add("ResortID");
                stringBuilder.Append("p1.ResortId AS ResortId, ");
            }
            if (!ExcludeRegionId)
            {
                m_outputHeaders.Add("RegionID");
                stringBuilder.Append("p1.RegionId AS RegionId, ");
            }
            if (!ExcludeCountryId)
            {
                m_outputHeaders.Add("CountryID");
                stringBuilder.Append("p1.CountryId AS CountryId, ");
            }
            if (!ExcludeResort)
            {
                m_outputHeaders.Add("Resort");
                stringBuilder.Append("p1.Resort AS Resort, ");
            }
            if (!ExcludeRegion)
            {
                m_outputHeaders.Add("Region");
                stringBuilder.Append("p1.Region AS Region, ");
            }
            if (!ExcludeCountry)
            {
                m_outputHeaders.Add("Country");
                stringBuilder.Append("p1.Country AS Country, ");
            }
            if (!ExcludeAddress)
            {
                m_outputHeaders.Add("Address");
                stringBuilder.Append("p3.Address1 AS Address, ");
            }
            if (!ExcludeTownCity)
            {
                m_outputHeaders.Add("TownCity");
                stringBuilder.Append("p3.TownCity AS TownCity, ");
            }
            if (!ExcludeCounty)
            {
            //m_selectStatementBuilder = SelectStatementBuilder.LoadSelectStatementBuilder(ExportItemName);
                m_outputHeaders.Add("County");
                stringBuilder.Append("p3.County AS County, ");
            }
            if (!ExcludePostcodeZip)
            {
                m_outputHeaders.Add("PostCodeZip");
                stringBuilder.Append("p3.PostcodeZip AS PostcodeZip, ");
            }
            if (!ExcludeTelephone)
            {
                m_outputHeaders.Add("Telephone");
                stringBuilder.Append("p3.Telephone AS Telephone, ");
            }
            if (!ExcludeFax)
            {
                m_outputHeaders.Add("Fax");
                stringBuilder.Append("p3.Fax AS Fax, ");
            }
            if (!ExcludeLatitude)
            {
                m_outputHeaders.Add("Latitude");
                stringBuilder.Append("p3.Latitude AS Latitude, ");
            }
            if (!ExcludeLongitude)
            {
                m_outputHeaders.Add("Logitude");
                stringBuilder.Append("p3.Longitude AS Longitude, ");
            }
            if (!ExcludeStrapline)
            {
                m_outputHeaders.Add("StrapLine");
                stringBuilder.Append("p3.Strapline  AS Strapline, ");
            }
            if (!ExcludeDescription)
            {
                m_outputHeaders.Add("Description");
                stringBuilder.Append("p4.Description  AS Description, ");
            }
            if (!ExcludeDistanceFromAirport)
            {
                m_outputHeaders.Add("DistancefromAirport");
                stringBuilder.Append("p4.DistanceFromAirport AS DistanceFromAirport, ");
            }
            if (!ExcludeTransferTime)
            {
                m_outputHeaders.Add("TransferTime");
                stringBuilder.Append("p4.TransferTime AS TransferTime, ");
            }
            if (!ExcludeRightChoice)
            {
                m_outputHeaders.Add("RightChoice");
                stringBuilder.Append("p4.RightChoice AS RightChoice, ");
            }
            if (!ExcludeLocationAndResort)
            {
                m_outputHeaders.Add("LocationandResort");
                stringBuilder.Append("p4.LocationAndResort AS LocationAndResort, ");
            }
            if (!ExcludeSwimmingPools)
            {
                m_outputHeaders.Add("SwimmingPools");
                stringBuilder.Append("p4.SwimmingPools AS SwimmingPools, ");
            }
            if (!ExcludeEatingAndDrinking)
            {
                m_outputHeaders.Add("EatingandDrinking");
                stringBuilder.Append("p4.EatingAndDrinking AS EatingAndDrinking, ");
            }
            if (!ExcludeAccomodation)
            {
                m_outputHeaders.Add("Accommodation");
                stringBuilder.Append("p4.Accomodation AS Accomodation, ");
            }
            if (!ExcludeSuitableFor)
            {
                m_outputHeaders.Add("Suitablefor");
                stringBuilder.Append("p4.SuitableFor AS SuitableFor, ");
            }
            if (!ExcludeUrl)
            {
                m_outputHeaders.Add("CMSBaseURL");
                stringBuilder.Append("p4.Url AS Url, ");
            }
            if (!ExcludeMainImage)
            {
                m_outputHeaders.Add("MainImage");
                stringBuilder.Append("p3.MainImage AS MainImage, ");
            }
            if (!ExcludeMainImageThumbnail)
            {
                m_outputHeaders.Add("MianImageThumbnail");
                stringBuilder.Append("p3.MainImageThumbnail AS MainImageThumbnail, ");
            }
            if (!ExcludeImageList)
            {
                m_outputHeaders.Add("Images");
                stringBuilder.Append("p7.ImageList AS ImageList, ");
            }
            if (!ExcludeFacilitiesList)
            {
                m_outputHeaders.Add("Facilities");
                stringBuilder.Append("p9.FacilitiesList AS FacilitiesList, ");
            }

            stringBuilder.Length -= 2; ;
            return stringBuilder.ToString();
        }

        public string PopulateWhereFields()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!ExcludePropertyReferenceId)
            {
                stringBuilder.Append("p1.PropertyReferenceId, ");
            }
            if (!ExcludePropertyName)
            {
                stringBuilder.Append("p1.PropertyName, ");
            }
            if (!ExcludeRating)
            {
                stringBuilder.Append("p1.Rating, ");
            }
            if (!ExcludePropertyType)
            {
                stringBuilder.Append("p8.PropertyType, ");
            }
            if (!ExcludeIATACode)
            {
                stringBuilder.Append("p6.IATACode, ");
            }
            if (!ExcludeResortId)
            {
                stringBuilder.Append("p1.ResortId, ");
            }
            if (!ExcludeRegionId)
            {
                stringBuilder.Append("p1.RegionId, ");
            }
            if (!ExcludeCountryId)
            {
                stringBuilder.Append("p1.CountryId, ");
            }
            if (!ExcludeResort)
            {
                stringBuilder.Append("p1.Resort, ");
            }
            if (!ExcludeRegion)
            {
                stringBuilder.Append("p1.Region, ");
            }
            if (!ExcludeCountry)
            {
                stringBuilder.Append("p1.Country, ");
            }
            if (!ExcludeAddress)
            {
                stringBuilder.Append("p3.Address, ");
            }
            if (!ExcludeTownCity)
            {
                stringBuilder.Append("p3.TownCity, ");
            }
            if (!ExcludeCounty)
            {
                stringBuilder.Append("p3.County, ");
            }
            if (!ExcludePostcodeZip)
            {
                stringBuilder.Append("p3.PostcodeZip, ");
            }
            if (!ExcludeTelephone)
            {
                stringBuilder.Append("p3.Telephone, ");
            }
            if (!ExcludeFax)
            {
                stringBuilder.Append("p3.Fax, ");
            }
            if (!ExcludeLatitude)
            {
                stringBuilder.Append("p3.Latitude, ");
            }
            if (!ExcludeLongitude)
            {
                stringBuilder.Append("p3.Longitude, ");
            }
            if (!ExcludeStrapline)
            {
                stringBuilder.Append("p3.Strapline, ");
            }
            if (!ExcludeDescription)
            {
                stringBuilder.Append("p4.Description, ");
            }
            if (!ExcludeDistanceFromAirport)
            {
                stringBuilder.Append("p4.DistanceFromAirport, ");
            }
            if (!ExcludeTransferTime)
            {
                stringBuilder.Append("p4.TransferTime, ");
            }
            if (!ExcludeRightChoice)
            {
                stringBuilder.Append("p4.RightChoice, ");
            }
            if (!ExcludeLocationAndResort)
            {
                stringBuilder.Append("p4.LocationAndResort, ");
            }
            if (!ExcludeSwimmingPools)
            {
                stringBuilder.Append("p4.SwimmingPools, ");
            }
            if (!ExcludeEatingAndDrinking)
            {
                stringBuilder.Append("p4.EatingAndDrinking, ");
            }
            if (!ExcludeAccomodation)
            {
                stringBuilder.Append("p4.Accomodation AS Accomodation, ");
            }
            if (!ExcludeSuitableFor)
            {
                stringBuilder.Append("p4.SuitableFor, ");
            }
            if (!ExcludeUrl)
            {
                stringBuilder.Append("p4.Url, ");
            }
            if (!ExcludeMainImage)
            {
                stringBuilder.Append("p3.MainImage, ");
            }
            if (!ExcludeMainImageThumbnail)
            {
                stringBuilder.Append("p3.MainImageThumbnail, ");
            }
            if (!ExcludeImageList)
            {
                stringBuilder.Append("p7.ImageList, ");
            }
            if (!ExcludeFacilitiesList)
            {
                stringBuilder.Append("p9.FacilitiesList, ");
            }

            stringBuilder.Length -= 2; ;
            return stringBuilder.ToString();
        }

        //public void SaveSelectStatement()
        //{
        //    if (this.FTPFile)
        //        Insert.AddExportItem(this.Name, SerializeToXml(this), Convert.ToInt32(this.FTPSite));
        //    else
        //        Insert.AddExportItem(this.Name, SerializeToXml(this), 0);
        //}

        public static SelectStatementBuilder LoadSelectStatementBuilder(string name)
        {
            ExportItem exportItem = Data.Get.GetExportItem(name);
            return DeserializeFromXml(exportItem);
        }

        private static SelectStatementBuilder DeserializeFromXml(ExportItem exportItem)
        {
            StringReader reader = new StringReader(exportItem.ExportItemData);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SelectStatementBuilder));
            return (SelectStatementBuilder)xmlSerializer.Deserialize(reader);
        }

        public string SerializeToXml()
        {
            StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
            XmlSerializer serializer = new XmlSerializer(typeof(SelectStatementBuilder));
            serializer.Serialize(stringWriter, this);
            return stringWriter.ToString();
        }
    }
}
