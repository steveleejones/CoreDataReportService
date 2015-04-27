using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Web.UI.WebControls;

namespace CoreDataLibrary
{
    public class SelectStatementBuilder
    {
        private const string DefultLanguageId = "1";

        private readonly char[] charsToTrim = { ',', ' ' };
        private readonly List<string> outputHeaders = new List<string>();
        private string languageId = DefultLanguageId;

        public int RunTime { get; set; }
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
        public bool ExcludeTTI { get; set; }
        public bool OwnStock { get; set; }
        public bool OwnStockLive { get; set; }
        public bool NotOwnStock { get; set; }
        public List<ListItem> Exclusions { get; set; }
        public List<ListItem> Inclusions { get; set; }
        public bool ExportEnabled { get; set; }
        private static ExportItem _exportItem;

        public SelectStatementBuilder()
        {
            Exclusions = new List<ListItem>();
            Inclusions = new List<ListItem>();
        }

        public List<string> GetOutputHeaders
        {
            get { return outputHeaders; }
        }

        public string LanguageId
        {
            get { return languageId; }
            set { languageId = value; }
        }

        public string SelectStatement()
        {
            return BuildSelectStatement();
        }

        private string BuildSelectStatement()
        {
            StringBuilder selectStatement = new StringBuilder("SELECT ");

            selectStatement.Append(PopulateFields());
            if (CoreDataLib.IsLive())
            {
                selectStatement.Append(@" FROM [CoreData].[dbo].[Properties] AS p1 JOIN [CoreData].[dbo].[PropertyDetails] AS p3 ON p3.PropertyReferenceId = p1.PropertyReferenceId LEFT JOIN [CoreData].[dbo].[PropertyDescription] AS p4 ON p4.PropertyReferenceId = p1.PropertyReferenceId AND p4.LanguageId = ");
                selectStatement.Append(LanguageId);
                selectStatement.Append(" LEFT JOIN [CoreData].[dbo].[ResortAirport] AS p5 ON p5.ResortId = p1.ResortId LEFT JOIN [CoreData].[dbo].[Airports] AS p6 ON p6.AirportId = p5.AirportId LEFT JOIN [CoreData].[dbo].[PropertyHighQualityImageLists] AS p7 ON p7.PropertyReferenceId = p1.PropertyReferenceId LEFT JOIN [CoreData].[dbo].[PropertyFacilitiesList] AS p9 ON p9.PropertyReferenceId = p1.PropertyReferenceId AND p9.LanguageId = ");
                selectStatement.Append(LanguageId);
                selectStatement.Append(" LEFT JOIN [CoreData].[dbo].[TTIProperty] AS p11 ON p11.ID = p1.PropertyReferenceId");
                if (LanguageId != "1")
                {
                    selectStatement.Append(" JOIN [IVDB].[LCH].[dbo].[TranslationLookup] as p10 ON p10.SourceID = p1.CountryId AND p10.TranslationID = 5 AND p10.LanguageId = ");
                    selectStatement.Append(LanguageId);
                }
                selectStatement.Append(" LEFT JOIN [CoreData].[dbo].[PropertyUrls] AS p13 ON p13.PropertyReferenceId = p1.PropertyReferenceId AND p13.LanguageID = ");
                selectStatement.Append(LanguageId);
            }
            else
            {
                selectStatement.Append(@" FROM [CoreData_Test].[dbo].[Properties] AS p1 JOIN [CoreData_Test].[dbo].[PropertyDetails] AS p3 ON p3.PropertyReferenceId = p1.PropertyReferenceId LEFT JOIN [CoreData_Test].[dbo].[PropertyDescription] AS p4 ON p4.PropertyReferenceId = p1.PropertyReferenceId AND p4.LanguageId = ");
                selectStatement.Append(LanguageId);
                selectStatement.Append(" LEFT JOIN [CoreData_Test].[dbo].[ResortAirport] AS p5 ON p5.ResortId = p1.ResortId LEFT JOIN [CoreData_Test].[dbo].[Airports] AS p6 ON p6.AirportId = p5.AirportId LEFT JOIN [CoreData_Test].[dbo].[PropertyHighQualityImageLists] AS p7 ON p7.PropertyReferenceId = p1.PropertyReferenceId LEFT JOIN [CoreData_Test].[dbo].[PropertyFacilitiesList] AS p9 ON p9.PropertyReferenceId = p1.PropertyReferenceId AND p9.LanguageId = ");
                selectStatement.Append(LanguageId);
                selectStatement.Append(" LEFT JOIN [CoreData_Test].[dbo].[TTIProperty] AS p11 ON p11.ID = p1.PropertyReferenceId");
                if (LanguageId != "1")
                {
                    selectStatement.Append(" JOIN [IVDB].[LCH].[dbo].[TranslationLookup] as p10 ON p10.SourceID = p1.CountryId AND p10.TranslationID = 5 AND p10.LanguageId = ");
                    selectStatement.Append(LanguageId);
                }
                selectStatement.Append(" LEFT JOIN [CoreData_Test].[dbo].[PropertyUrls] AS p13 ON p13.PropertyReferenceId = p1.PropertyReferenceId AND p13.LanguageID = ");
                selectStatement.Append(LanguageId);
            }
            selectStatement.Append(CreateStockClause());
            selectStatement.Append(CreateWhereClause(selectStatement));
            return selectStatement.ToString();
        }

        private string CreateWhereClause(StringBuilder selectStatement)
        {
            bool includeRegionsAdded = false;
            bool includeResortsAdded = false;

            string clause = "";
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
            if (OwnStock & NotOwnStock)
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
            outputHeaders.Clear();
            StringBuilder stringBuilder = new StringBuilder();
            if (!ExcludePropertyReferenceId)
            {
                outputHeaders.Add("PropertyReferenceId");
                stringBuilder.Append("DISTINCT p1.PropertyReferenceId AS PropertyReferenceId, ");
                //stringBuilder.Append("TOP 5000 p1.PropertyReferenceId AS PropertyReferenceId, ");
            }
            if (!ExcludeTTI)
            {
                outputHeaders.Add("TTI");
                stringBuilder.Append("p11.TTI AS TTI, ");
            }
            if (!ExcludePropertyName)
            {
                outputHeaders.Add("PropertyName");
                stringBuilder.Append("p1.PropertyName AS PropertyName, ");
                //stringBuilder.Append("REPLACE(p1.PropertyName, ''|'', '''') AS PropertyName, ");
            }
            if (!ExcludeRating)
            {
                outputHeaders.Add("Rating");
                stringBuilder.Append("p1.Rating AS Rating, ");
            }
            if (!ExcludePropertyType)
            {
                outputHeaders.Add("PropertyType");
                stringBuilder.Append("p3.PropertyType AS PropertyType, ");
            }
            if (!ExcludeIATACode)
            {
                outputHeaders.Add("AirportCode");
                stringBuilder.Append("p6.IATACode  AS IATACode, ");
            }
            if (!ExcludeResortId)
            {
                outputHeaders.Add("ResortID");
                stringBuilder.Append("p1.ResortId AS ResortId, ");
            }
            if (!ExcludeRegionId)
            {
                outputHeaders.Add("RegionID");
                stringBuilder.Append("p1.RegionId AS RegionId, ");
            }
            if (!ExcludeCountryId)
            {
                outputHeaders.Add("CountryID");
                stringBuilder.Append("p1.CountryId AS CountryId, ");
            }
            if (!ExcludeResort)
            {
                outputHeaders.Add("Resort");
                stringBuilder.Append(@"p1.Resort AS Resort, ");
                //stringBuilder.Append(@"REPLACE(p1.Resort, ''|'', '''') AS Resort, ");
            }
            if (!ExcludeRegion)
            {
                outputHeaders.Add("Region");
                stringBuilder.Append("p1.Region AS Region, ");
                //stringBuilder.Append("REPLACE(p1.Region, ''|'', '''') AS Region, ");
            }
            if (!ExcludeCountry)
            {
                outputHeaders.Add("Country");

                if (LanguageId == "1")
                    stringBuilder.Append("p1.Country AS Country, ");
                //stringBuilder.Append("REPLACE(p1.Country, ''|'', '''') AS Country, ");
                else
                    stringBuilder.Append("p10.Translation AS Country, ");
            }
            if (!ExcludeAddress)
            {
                outputHeaders.Add("Address");
                //stringBuilder.Append("p3.Address1 AS Address, ");
                //// NOTE : Need to use single ' if not using BCP.
                if (_exportItem.ExportType == "xml")
                    stringBuilder.Append("REPLACE(p3.Address1, '|', '') AS Address, ");
                else if (_exportItem.ExportType == "csv")
                    stringBuilder.Append("REPLACE(p3.Address1, ''|'', '''') AS Address, ");
            }
            if (!ExcludeTownCity)
            {
                outputHeaders.Add("TownCity");
                stringBuilder.Append("p3.TownCity AS TownCity, ");
                //stringBuilder.Append("REPLACE(p3.TownCity, ''|'', '''') AS TownCity, ");
            }
            if (!ExcludeCounty)
            {
                outputHeaders.Add("County");
                stringBuilder.Append("p3.County AS County, ");
                //stringBuilder.Append("REPLACE(p3.County, ''|'', '''') AS County, ");
            }
            if (!ExcludePostcodeZip)
            {
                outputHeaders.Add("PostCodeZip");
                stringBuilder.Append("p3.PostcodeZip AS PostcodeZip, ");
            }
            if (!ExcludeTelephone)
            {
                outputHeaders.Add("Telephone");
                stringBuilder.Append("p3.Telephone AS Telephone, ");
            }
            if (!ExcludeFax)
            {
                outputHeaders.Add("Fax");
                stringBuilder.Append("p3.Fax AS Fax, ");
            }
            if (!ExcludeLatitude)
            {
                outputHeaders.Add("Latitude");
                stringBuilder.Append("p3.Latitude AS Latitude, ");
            }
            if (!ExcludeLongitude)
            {
                outputHeaders.Add("Longitude");
                stringBuilder.Append("p3.Longitude AS Longitude, ");
            }
            if (!ExcludeStrapline)
            {
                outputHeaders.Add("StrapLine");
                stringBuilder.Append("p3.Strapline  AS Strapline, ");
                //stringBuilder.Append("REPLACE(p3.Strapline, ''|'' ,'''')  AS Strapline, ");
            }
            if (!ExcludeDescription)
            {
                outputHeaders.Add("Description");
                stringBuilder.Append("p4.Description  AS Description, ");
                //stringBuilder.Append("REPLACE(p4.Description, ''|'', '''')  AS Description, ");
            }
            if (!ExcludeDistanceFromAirport)
            {
                outputHeaders.Add("DistancefromAirport");
                stringBuilder.Append("p4.DistanceFromAirport AS DistanceFromAirport, ");
                //stringBuilder.Append("REPLACE(p4.DistanceFromAirport, ''|'', '''') AS DistanceFromAirport, ");
            }
            if (!ExcludeTransferTime)
            {
                outputHeaders.Add("TransferTime");
                stringBuilder.Append("p4.TransferTime AS TransferTime, ");
                //stringBuilder.Append("REPLACE(p4.TransferTime, ''|'', '''') AS TransferTime, ");
            }
            if (!ExcludeRightChoice)
            {
                outputHeaders.Add("RightChoice");
                stringBuilder.Append("p4.RightChoice AS RightChoice, ");
                //stringBuilder.Append("REPLACE(p4.RightChoice, ''|'', '''') AS RightChoice, ");
            }
            if (!ExcludeLocationAndResort)
            {
                outputHeaders.Add("LocationandResort");
                stringBuilder.Append("p4.LocationAndResort AS LocationAndResort, ");
                //stringBuilder.Append("REPLACE(p4.LocationAndResort, ''|'', '''') AS LocationAndResort, ");
            }
            if (!ExcludeSwimmingPools)
            {
                outputHeaders.Add("SwimmingPools");
                stringBuilder.Append("p4.SwimmingPools AS SwimmingPools, ");
                //stringBuilder.Append("REPLACE(p4.SwimmingPools, ''|'', '''') AS SwimmingPools, ");
            }
            if (!ExcludeEatingAndDrinking)
            {
                outputHeaders.Add("EatingandDrinking");
                stringBuilder.Append("p4.EatingAndDrinking AS EatingAndDrinking, ");
                //stringBuilder.Append("REPLACE(p4.EatingAndDrinking, ''|'', '''') AS EatingAndDrinking, ");
            }
            if (!ExcludeAccomodation)
            {
                outputHeaders.Add("Accommodation");
                stringBuilder.Append("p4.Accomodation AS Accomodation, ");
                //stringBuilder.Append("REPLACE(p4.Accomodation, ''|'', '''') AS Accomodation, ");
            }
            if (!ExcludeSuitableFor)
            {
                outputHeaders.Add("Suitablefor");
                stringBuilder.Append("p4.SuitableFor AS SuitableFor, ");
                //stringBuilder.Append("REPLACE(p4.SuitableFor, ''|'', '''') AS SuitableFor, ");
            }
            if (!ExcludeUrl)
            {
                outputHeaders.Add("CMSBaseURL");
                //stringBuilder.Append("p4.Url AS Url, ");
                stringBuilder.Append("p13.Url AS Url, ");
            }
            if (!ExcludeMainImage)
            {
                outputHeaders.Add("MainImage");
                stringBuilder.Append("p3.MainImage AS MainImage, ");
            }
            if (!ExcludeMainImageThumbnail)
            {
                outputHeaders.Add("MainImageThumbnail");
                stringBuilder.Append("p3.MainImageThumbnail AS MainImageThumbnail, ");
            }
            if (!ExcludeImageList)
            {
                outputHeaders.Add("Images");
                stringBuilder.Append("p7.HighQualityImageList AS ImageList, ");
            }
            if (!ExcludeFacilitiesList)
            {
                outputHeaders.Add("Facilities");
                stringBuilder.Append("p9.FacilitiesList AS FacilitiesList, ");
            }

            stringBuilder.Length -= 2;
            return stringBuilder.ToString();
        }

        public string PopulateWhereFields()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!ExcludePropertyReferenceId)
            {
                stringBuilder.Append("p1.PropertyReferenceId, ");
            }
            if (!ExcludeTTI)
            {
                stringBuilder.Append("p11.TTI, ");
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
                //stringBuilder.Append("p4.Url, ");
                stringBuilder.Append("p13.Url, ");
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

            stringBuilder.Length -= 2;
            return stringBuilder.ToString();
        }

        public static SelectStatementBuilder LoadSelectStatementBuilder(string ExportItemName)
        {
            _exportItem = Data.Get.GetExportItem(ExportItemName);
            if (_exportItem != null)
                return DeserializeFromXml(_exportItem);
            return null;
        }

        public static SelectStatementBuilder StaticLoadSelectStatementBuilder(string name)
        {
            ExportItem exportItem = Data.Get.GetExportItem(name);
            if (exportItem != null)
                return DeserializeFromXml(exportItem);
            return null;
        }

        private static SelectStatementBuilder DeserializeFromXml(ExportItem exportItem)
        {
            StringReader reader = new StringReader(exportItem.ExportItemData);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SelectStatementBuilder));
            SelectStatementBuilder ssb = (SelectStatementBuilder)xmlSerializer.Deserialize(reader);
            return ssb;
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
