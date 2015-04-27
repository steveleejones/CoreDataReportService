using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreDataLibrary;
using CoreDataLibrary.Data;
using CoreDataLibrary.Helpers;

namespace CoreDataReportCreator
{
    public partial class PropertyExporter : System.Web.UI.Page
    {
        //private const string SERVER_EXPORTFILE_PATH = @"\\mssqldev\E$\CoreData\ExportFiles\";
        public const string SERVER_EXPORTFILE_PATH = @"\\svrsql4\E$\Coredata\ExportFiles\";
        private static ExportItem s_exportItem;
        private static ExportItem s_originalItem;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<ListItem> languages = CoreDataLibrary.Data.Get.GetLanguages();
                DropDownListLanguage.Items.AddRange(languages.ToArray());
                UpdateCountries();
                UpdateRegions();
                UpdateResorts();
                UpdateExportedItems();
                UpdateFtpList();
                UpdateExportFormat();
                UpdateExportEncoding();
                CheckBoxExcludeCountry.Enabled = false;
                UpdateDownloadLink();
                for (int i = 1; i <= 24; i++)
                    DropDownRunTime.Items.Add(i.ToString());
            }
        }

        private void UpdateExportEncoding()
        {
            List<string> exportEncodings = CoreDataLibrary.Data.Get.GetExportFileEncodings();
            DropDownListExportEncoding.Items.Clear();
            DropDownListExportEncoding.Items.Add(new ListItem("None", "0"));
            foreach (string encoding in exportEncodings)
            {
                DropDownListExportEncoding.Items.Add(encoding);
            }
        }

        private void UpdateExportFormat()
        {
            DropDownListExportType.Items.Clear();
            DropDownListExportType.Items.Add(new ListItem("None", "0"));
            DropDownListExportType.Items.Add("csv");
            DropDownListExportType.Items.Add("xml");

        }
        private void UpdateFtpList()
        {
            DropDownListFTPSites.Items.Clear();
            DropDownListFTPSites.Items.Add(new ListItem("None", "0"));
            List<ListItem> ftpSites = Get.GetFtpListItems();
            DropDownListFTPSites.Items.AddRange(ftpSites.ToArray());
        }

        private void UpdateCountries()
        {
            DropDownListCountry.Items.Clear();
            List<ListItem> countries = Get.GetCountryItems();
            DropDownListCountry.Items.Add(new ListItem("None", "0"));
            DropDownListCountry.Items.AddRange(countries.ToArray());
        }

        private void UpdateResorts()
        {
            DropDownListResort.Items.Clear();
            List<ListItem> resorts = Get.GetResortItems(Convert.ToInt32(DropDownListRegion.SelectedValue));
            DropDownListResort.Items.Add(new ListItem("None", "0"));
            DropDownListResort.Items.AddRange(resorts.ToArray());
        }

        private void UpdateRegions()
        {
            DropDownListRegion.Items.Clear();
            List<ListItem> regions = Get.GetRegionItems(Convert.ToInt32(DropDownListCountry.SelectedValue));
            DropDownListRegion.Items.Add(new ListItem("None", "0"));
            DropDownListRegion.Items.AddRange(regions.ToArray());
        }

        private void UpdateExportedItems()
        {
            int i = 0;
            DropDownListExportFile.Items.Clear();
            DropDownListExportFile.Items.Add(new ListItem("None", "0"));
            List<ListItem> exportedItemsList = Get.GetExportItems();
            ExportItem exportItem = Get.GetExportItem(TextBoxFilename.Text);
            DropDownListExportFile.Items.AddRange(exportedItemsList.ToArray());
            if (exportItem != null)
            {
                foreach (ListItem item in DropDownListExportFile.Items)
                {
                    if (item.Text == exportItem.ExportItemName)
                        break;
                    i++;

                }
                DropDownListExportFile.SelectedIndex = i;
            }
        }

        protected void ButtonExport_Click(object sender, EventArgs e)
        {
            //CoreDataLibrary.Data.Insert.AddExportQueItem(TextBoxFilename.Text);
            //try
            //{
            //    if (m_exportItem == null)
            //        m_exportItem = new ExportItem();

            //    m_exportItem.ExportItemName = TextBoxFilename.Text;
            //    m_exportItem.ExportItemFtpId = Convert.ToInt32(DropDownListFTPSites.SelectedValue);
            //    PopulateValues(m_exportItem);

            //    m_exportItem.SelectStatementBuilder.SelectStatement();
            //    CsvDataExporter dataExporter = new CsvDataExporter(m_exportItem);
            //    dataExporter.CsvExport();
            //}
            //catch (Exception exception)
            //{
            //    Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataReprtService : Error ", "ReportCreator->ButtonExport_Click", exception);
            //}
        }

        private void PopulateValues(ExportItem exportItem)
        {
            if (exportItem.StatementBuilder == null)
                exportItem.StatementBuilder = new SelectStatementBuilder();
            exportItem.ExportType = DropDownListExportType.SelectedValue;
            exportItem.StatementBuilder.FTPFile = DropDownListFTPSites.Text != "No FTP";
            exportItem.StatementBuilder.FTPSite = DropDownListFTPSites.SelectedValue;
            //exportItem.SelectStatementBuilder.ExportType = DropDownListExportType.SelectedValue;
            exportItem.StatementBuilder.RunTime = Convert.ToInt32(DropDownRunTime.SelectedItem.Text);
            exportItem.StatementBuilder.LanguageId = DropDownListLanguage.SelectedItem.Value;
            exportItem.StatementBuilder.ExcludePropertyReferenceId = CheckBoxPropertyReferenceId.Checked;
            exportItem.StatementBuilder.ExcludeTTI = CheckBoxTTI.Checked;
            exportItem.StatementBuilder.ExcludePropertyName = CheckBoxPropertyName.Checked;
            exportItem.StatementBuilder.ExcludeRating = CheckBoxRating.Checked;
            exportItem.StatementBuilder.ExcludePropertyType = CheckBoxPropertyType.Checked;
            exportItem.StatementBuilder.ExcludeIATACode = CheckBoxIATACode.Checked;
            exportItem.StatementBuilder.ExcludeResortId = CheckBoxResortId.Checked;
            exportItem.StatementBuilder.ExcludeRegionId = CheckBoxRegionId.Checked;
            exportItem.StatementBuilder.ExcludeCountryId = CheckBoxCountryId.Checked;
            exportItem.StatementBuilder.ExcludeResort = CheckBoxResort.Checked;
            exportItem.StatementBuilder.ExcludeRegion = CheckBoxRegion.Checked;
            exportItem.StatementBuilder.ExcludeCountry = CheckBoxCountry.Checked;
            exportItem.StatementBuilder.ExcludeAddress = CheckBoxAddress.Checked;
            exportItem.StatementBuilder.ExcludeTownCity = CheckBoxTownCity.Checked;
            exportItem.StatementBuilder.ExcludeCounty = CheckBoxCounty.Checked;
            exportItem.StatementBuilder.ExcludePostcodeZip = CheckBoxPostcodeZip.Checked;
            exportItem.StatementBuilder.ExcludeTelephone = CheckBoxTelephone.Checked;
            exportItem.StatementBuilder.ExcludeFax = CheckBoxFax.Checked;
            exportItem.StatementBuilder.ExcludeLatitude = CheckBoxLatitude.Checked;
            exportItem.StatementBuilder.ExcludeLongitude = CheckBoxLongitude.Checked;
            exportItem.StatementBuilder.ExcludeStrapline = CheckBoxStrapline.Checked;
            exportItem.StatementBuilder.ExcludeDescription = CheckBoxDescription.Checked;
            exportItem.StatementBuilder.ExcludeDistanceFromAirport = CheckBoxDistanceFromAirport.Checked;
            exportItem.StatementBuilder.ExcludeTransferTime = CheckBoxTransferTime.Checked;
            exportItem.StatementBuilder.ExcludeRightChoice = CheckBoxRightChoice.Checked;
            exportItem.StatementBuilder.ExcludeLocationAndResort = CheckBoxLocationAndResort.Checked;
            exportItem.StatementBuilder.ExcludeSwimmingPools = CheckBoxSwimmingPools.Checked;
            exportItem.StatementBuilder.ExcludeEatingAndDrinking = CheckBoxEatingAndDrinking.Checked;
            exportItem.StatementBuilder.ExcludeAccomodation = CheckBoxAccomodation.Checked;
            exportItem.StatementBuilder.ExcludeSuitableFor = CheckBoxSuitableFor.Checked;
            exportItem.StatementBuilder.ExcludeUrl = CheckBoxUrl.Checked;
            exportItem.StatementBuilder.ExcludeMainImage = CheckBoxMainImage.Checked;
            exportItem.StatementBuilder.ExcludeMainImageThumbnail = CheckBoxMainImageThumbnail.Checked;
            exportItem.StatementBuilder.ExcludeImageList = CheckBoxImageList.Checked;
            exportItem.StatementBuilder.ExcludeFacilitiesList = CheckBoxFacilitiesList.Checked;
            exportItem.StatementBuilder.OwnStock = RadioButtonOwnStock.Checked;
            exportItem.StatementBuilder.OwnStockLive = RadioButtonOwnStockLive.Checked;
            exportItem.StatementBuilder.NotOwnStock = CheckBoxNotOwnStock.Checked;
            exportItem.StatementBuilder.ExportEnabled = CheckBoxExportEnabled.Checked;
            foreach (ListItem item in ListBoxExclusions.Items)
            {
                exportItem.StatementBuilder.Exclusions.Add(item);
            }
            foreach (ListItem item in ListBoxInclusions.Items)
            {
                exportItem.StatementBuilder.Inclusions.Add(item);
            }
        }

        protected void ButtonSaveExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextBoxFilename.Text == "")
                    ShowMessageBox("Invalid filename.");
                else
                {
                    s_exportItem = Get.GetExportItem(TextBoxFilename.Text);
                    if (s_exportItem == null)
                        s_exportItem = new ExportItem();

                    s_exportItem.ExportItemName = TextBoxFilename.Text;
                    s_exportItem.ExportItemFtpId = Convert.ToInt32(DropDownListFTPSites.SelectedValue);
                    s_exportItem.ExportItemRunTime = Convert.ToInt32(DropDownRunTime.SelectedItem.Text);
                    s_exportItem.ExportType = DropDownListExportType.SelectedItem.Text;
                    s_exportItem.ExportEncoding = DropDownListExportEncoding.Text;
                    if (CheckBoxExportEnabled.Checked)
                        s_exportItem.ExportEnabled = 1;
                    else
                        s_exportItem.ExportEnabled = 0;

                    PopulateValues(s_exportItem);
                    s_exportItem.Save();
                    UpdateExportedItems();
                    UpdateExportType();
                }
            }
            catch (Exception exception)
            {
                Emailer.SendEmail("Steven.Jones@lowcostholidays.com", "CoreDataReportService : Error ", "ReportCreator->ButtonSaveExport_Click", exception);
            }
        }

        protected void ButtonLoadExportItem_Click(object sender, EventArgs e)
        {
            LoadExportItem();
        }

        private void LoadExportItem()
        {
            if (DropDownListExportFile.SelectedItem != null)
            {
                //m_exportItem = new ExportItem();
                s_exportItem = CoreDataLibrary.Data.Get.GetExportItemByName(DropDownListExportFile.SelectedItem.Text);

                s_exportItem.StatementBuilder = SelectStatementBuilder.StaticLoadSelectStatementBuilder(DropDownListExportFile.SelectedItem.Text);
                LoadExportedItem(s_exportItem.StatementBuilder);
                DropDownListLanguage.SelectedValue = s_exportItem.StatementBuilder.LanguageId;
                DropDownListExportType.SelectedValue = s_exportItem.ExportType;
                DropDownListExportEncoding.SelectedValue = s_exportItem.ExportEncoding;
                if (s_exportItem.StatementBuilder.FTPFile)
                    DropDownListFTPSites.SelectedValue = s_exportItem.StatementBuilder.FTPSite;
                else
                    DropDownListFTPSites.SelectedValue = "0";

                CheckBoxExportEnabled.Checked = s_exportItem.StatementBuilder.ExportEnabled;

                DropDownRunTime.SelectedValue = s_exportItem.StatementBuilder.RunTime.ToString();
                s_originalItem = new ExportItem();
                s_originalItem.StatementBuilder = SelectStatementBuilder.StaticLoadSelectStatementBuilder(DropDownListExportFile.SelectedItem.Text);
                LoadExportedItem(s_originalItem.StatementBuilder);
                TextBoxLinkToFile.Text = SERVER_EXPORTFILE_PATH + DropDownListExportFile.SelectedItem.Text + "." + DropDownListExportType.SelectedItem;
            }
        }

        private void LoadExportedItem(SelectStatementBuilder ssb)
        {
            CheckBoxPropertyReferenceId.Checked = ssb.ExcludePropertyReferenceId;
            CheckBoxTTI.Checked = ssb.ExcludeTTI;
            CheckBoxPropertyName.Checked = ssb.ExcludePropertyName;
            CheckBoxRating.Checked = ssb.ExcludeRating;
            CheckBoxPropertyType.Checked = ssb.ExcludePropertyType;
            CheckBoxIATACode.Checked = ssb.ExcludeIATACode;
            CheckBoxResortId.Checked = ssb.ExcludeResortId;
            CheckBoxRegionId.Checked = ssb.ExcludeRegionId;
            CheckBoxCountryId.Checked = ssb.ExcludeCountryId;
            CheckBoxResort.Checked = ssb.ExcludeResort;
            CheckBoxRegion.Checked = ssb.ExcludeRegion;
            CheckBoxCountry.Checked = ssb.ExcludeCountry;
            CheckBoxAddress.Checked = ssb.ExcludeAddress;
            CheckBoxTownCity.Checked = ssb.ExcludeTownCity;
            CheckBoxCounty.Checked = ssb.ExcludeCounty;
            CheckBoxPostcodeZip.Checked = ssb.ExcludePostcodeZip;
            CheckBoxTelephone.Checked = ssb.ExcludeTelephone;
            CheckBoxFax.Checked = ssb.ExcludeFax;
            CheckBoxLatitude.Checked = ssb.ExcludeLatitude;
            CheckBoxLongitude.Checked = ssb.ExcludeLongitude;
            CheckBoxStrapline.Checked = ssb.ExcludeStrapline;
            CheckBoxDescription.Checked = ssb.ExcludeDescription;
            CheckBoxDistanceFromAirport.Checked = ssb.ExcludeDistanceFromAirport;
            CheckBoxTransferTime.Checked = ssb.ExcludeTransferTime;
            CheckBoxRightChoice.Checked = ssb.ExcludeRightChoice;
            CheckBoxLocationAndResort.Checked = ssb.ExcludeLocationAndResort;
            CheckBoxSwimmingPools.Checked = ssb.ExcludeSwimmingPools;
            CheckBoxEatingAndDrinking.Checked = ssb.ExcludeEatingAndDrinking;
            CheckBoxAccomodation.Checked = ssb.ExcludeAccomodation;
            CheckBoxSuitableFor.Checked = ssb.ExcludeSuitableFor;
            CheckBoxUrl.Checked = ssb.ExcludeUrl;
            CheckBoxMainImage.Checked = ssb.ExcludeMainImage;
            CheckBoxMainImageThumbnail.Checked = ssb.ExcludeMainImageThumbnail;
            CheckBoxImageList.Checked = ssb.ExcludeImageList;
            CheckBoxFacilitiesList.Checked = ssb.ExcludeFacilitiesList;
            RadioButtonOwnStock.Checked = ssb.OwnStock;
            RadioButtonOwnStockLive.Checked = ssb.OwnStockLive;
            CheckBoxNotOwnStock.Checked = ssb.NotOwnStock;
            ListBoxExclusions.Items.Clear();
            ListBoxInclusions.Items.Clear();
            foreach (ListItem item in ssb.Exclusions)
            {
                ListBoxExclusions.Items.Add(item);
            }
            foreach (ListItem item in ssb.Inclusions)
            {
                ListBoxInclusions.Items.Add(item);
            }
            CheckBoxExportEnabled.Checked = ssb.ExportEnabled;
        }

        protected void DropDownListExportFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListExportFile.SelectedIndex != 0)
            {
                LoadExportItem();
                TextBoxFilename.Text = DropDownListExportFile.SelectedItem.Text;
                UpdateDownloadLink();
                UpdateRunTime();
                UpdateExportFormat();
                UpdateExportType();
                UpdateExportEncoding();
                UpdateExportEncodingType();
            }
            else
            {
                ResetInputs();
                TextBoxFilename.Text = "";
            }
            UpdateFtpDetails();
        }

        private void UpdateExportEncodingType()
        {
            ExportItem exportItem = CoreDataLibrary.Data.Get.GetExportItemByName(DropDownListExportFile.SelectedItem.Text);
            if (exportItem.ExportType != "xml")
            {
                exportEncoding.Visible = true;
                DropDownListExportEncoding.Text = exportItem.ExportEncoding;
            }
            else
            {
                exportEncoding.Visible = false;
            }
        }

        private void ResetInputs()
        {
            CheckBoxPropertyReferenceId.Checked = false;
            CheckBoxPropertyName.Checked = false;
            CheckBoxRating.Checked = false;
            CheckBoxPropertyType.Checked = false;
            CheckBoxIATACode.Checked = false;
            CheckBoxResortId.Checked = false;
            CheckBoxRegionId.Checked = false;
            CheckBoxCountryId.Checked = false;
            CheckBoxResort.Checked = false;
            CheckBoxRegion.Checked = false;
            CheckBoxCountry.Checked = false;
            CheckBoxAddress.Checked = false;
            CheckBoxTownCity.Checked = false;
            CheckBoxCounty.Checked = false;
            CheckBoxPostcodeZip.Checked = false;
            CheckBoxTelephone.Checked = false;
            CheckBoxFax.Checked = false;
            CheckBoxLatitude.Checked = false;
            CheckBoxLongitude.Checked = false;
            CheckBoxStrapline.Checked = false;
            CheckBoxDescription.Checked = false;
            CheckBoxDistanceFromAirport.Checked = false;
            CheckBoxTransferTime.Checked = false;
            CheckBoxRightChoice.Checked = false;
            CheckBoxLocationAndResort.Checked = false;
            CheckBoxSwimmingPools.Checked = false;
            CheckBoxEatingAndDrinking.Checked = false;
            CheckBoxAccomodation.Checked = false;
            CheckBoxSuitableFor.Checked = false;
            CheckBoxUrl.Checked = false;
            CheckBoxMainImage.Checked = false;
            CheckBoxMainImageThumbnail.Checked = false;
            CheckBoxImageList.Checked = false;
            CheckBoxFacilitiesList.Checked = false;
            RadioButtonOwnStock.Checked = false;
            RadioButtonOwnStockLive.Checked = false;
            CheckBoxNotOwnStock.Checked = false;
            ListBoxExclusions.Items.Clear();
            ListBoxInclusions.Items.Clear();
            TextBoxLinkToFile.Text = "";
            DropDownListFTPSites.SelectedIndex = 0;
            CheckBoxExportEnabled.Checked = false;
            DropDownListExportType.SelectedIndex = 0;
        }

        private void UpdateRunTime()
        {
            DropDownRunTime.SelectedIndex = s_exportItem.StatementBuilder.RunTime - 1;
        }

        private void UpdateExportType()
        {
            ExportItem exportItem = CoreDataLibrary.Data.Get.GetExportItemByName(DropDownListExportFile.SelectedItem.Text);
            DropDownListExportType.Text = exportItem.ExportType;
            UpdateDownloadLink();
        }

        private void UpdateDownloadLink()
        {
            if (DropDownListExportFile.SelectedIndex > 0)
                TextBoxLinkToFile.Text = SERVER_EXPORTFILE_PATH + DropDownListExportFile.SelectedItem.Text + "." + DropDownListExportType.SelectedItem;
            else
                TextBoxLinkToFile.Text = "";
        }

        protected void DropDownListCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCountry.SelectedIndex == 0)
            {
                CheckBoxExcludeCountry.Checked = false;
                CheckBoxExcludeCountry.Enabled = false;
                RegionsSelector.Visible = false;
                ResortsSelector.Visible = false;
            }
            else
            {
                CheckBoxExcludeCountry.Enabled = true;
                CheckBoxExcludeCountry.Checked = true;
                CheckBoxExcludeRegion.Checked = false;
                CheckBoxExcludeResort.Checked = false;
            }
            UpdateRegions();
            UpdateResorts();
        }

        protected void DropDownListRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListRegion.SelectedIndex == 0)
            {
                CheckBoxExcludeRegion.Checked = false;
                CheckBoxExcludeRegion.Enabled = false;
                CheckBoxExcludeCountry.Checked = true;
                RegionsSelector.Visible = false;
                ResortsSelector.Visible = false;
            }
            else
            {
                CheckBoxExcludeRegion.Enabled = true;
                CheckBoxExcludeRegion.Checked = true;
                CheckBoxExcludeCountry.Checked = false;
                CheckBoxExcludeResort.Checked = false;
            }
            UpdateResorts();
        }

        protected void DropDownListResort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListResort.SelectedIndex == 0)
            {
                CheckBoxExcludeResort.Checked = false;
                CheckBoxExcludeResort.Enabled = false;
                CheckBoxExcludeRegion.Checked = true;
                ResortsSelector.Visible = false;
            }
            else
            {
                CheckBoxExcludeResort.Enabled = true;
                CheckBoxExcludeResort.Checked = true;
                CheckBoxExcludeRegion.Checked = false;
                CheckBoxExcludeCountry.Checked = false;
                ResortsSelector.Visible = true;
            }
        }

        protected void ButtonAddExclusion_Click(object sender, EventArgs e)
        {
            string excludedItem = "";
            string value = "";

            if (CheckBoxExcludeCountry.Checked)
            {
                excludedItem = "Exclude Properties in " + DropDownListCountry.SelectedItem.Text;
                value = " p1.CountryId != " + DropDownListCountry.SelectedValue.ToString() + " ";
            }
            else if (CheckBoxExcludeRegion.Checked)
            {
                excludedItem = "Exclude Properties in " + DropDownListRegion.SelectedItem.Text;
                value = " p1.RegionId != " + DropDownListRegion.SelectedValue.ToString() + " ";
            }
            else if (CheckBoxExcludeResort.Checked)
            {
                excludedItem = "Exclude Properties in " + DropDownListResort.SelectedItem.Text;
                value = " p1.ResortId != " + DropDownListResort.SelectedValue.ToString() + " ";
            }
            else
            {
                ShowMessageBox("No Country, Region or Resort selected.");
            }
            ListBoxExclusions.Items.Add(new ListItem(excludedItem, value));
        }

        protected void ButtonAddInclusion_Click(object sender, EventArgs e)
        {
            string includedItem = "";
            string value = "";

            if (CheckBoxExcludeCountry.Checked)
            {
                includedItem = "Include Properties in " + DropDownListCountry.SelectedItem.Text;
                value = " p1.CountryId = " + DropDownListCountry.SelectedValue.ToString() + " ";
            }
            else if (CheckBoxExcludeRegion.Checked)
            {
                includedItem = "Include Properties in " + DropDownListRegion.SelectedItem.Text;
                value = " p1.RegionId = " + DropDownListRegion.SelectedValue.ToString() + " ";
            }
            else if (CheckBoxExcludeResort.Checked)
            {
                includedItem = "Include Properties in " + DropDownListResort.SelectedItem.Text;
                value = " p1.ResortId = " + DropDownListResort.SelectedValue.ToString() + " ";
            }
            ListBoxInclusions.Items.Add(new ListItem(includedItem, value));
        }

        protected void DropDownListCountryOnly_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRegions();
            UpdateResorts();
        }

        protected void DropDownListRegionOnly_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResorts();
        }

        protected void ButtonRemoveInclusion_Click(object sender, EventArgs e)
        {
            ListBoxInclusions.Items.Remove(ListBoxInclusions.SelectedItem);
        }

        protected void ButtonRemoveExclusion_Click(object sender, EventArgs e)
        {
            ListBoxExclusions.Items.Remove(ListBoxExclusions.SelectedItem);
        }

        protected void TextBoxFilename_TextChanged(object sender, EventArgs e)
        {
        }

        protected void ButtonDownload_Click(object sender, EventArgs e)
        {
        }

        protected void CheckBoxFtp_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void CheckBoxPropertyReferenceId_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void CheckBoxExcludeCountry_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxExcludeCountry.Checked || DropDownListCountry.SelectedIndex == 0)
            {
                CheckBoxExcludeRegion.Checked = false;
                CheckBoxExcludeResort.Checked = false;
                RegionsSelector.Visible = false;
                ResortsSelector.Visible = false;
            }
            else
            {
                CheckBoxExcludeCountry.Enabled = true;
                DropDownListRegion.SelectedIndex = 0;
                CheckBoxExcludeRegion.Enabled = false;
                RegionsSelector.Visible = true;
                ResortsSelector.Visible = false;
            }
        }

        protected void CheckBoxExcludeRegion_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxExcludeRegion.Checked)
            {
                ResortsSelector.Visible = false;
                DropDownListResort.SelectedIndex = 0;
                CheckBoxExcludeResort.Checked = false;
                CheckBoxExcludeCountry.Checked = false;
                CheckBoxExcludeResort.Checked = false;
            }
            else
            {
                if (DropDownListRegion.SelectedIndex == 0)
                    DropDownListRegion.SelectedIndex = 0;
                else
                {
                    ResortsSelector.Visible = true;
                    DropDownListResort.SelectedIndex = 0;
                }
            }
        }

        protected void CheckBoxExcludeResort_CheckedChanged(object sender, EventArgs e)
        {
            if (!CheckBoxExcludeResort.Checked)
            {
                CheckBoxExcludeRegion.Checked = true;
                ResortsSelector.Visible = true;
            }
        }

        private void ShowMessageBox(string message)
        {
            Response.Write("<script type=\"text/javascript\">alert('" + message + "');</script>");
        }

        protected void ButtonAddFtpSite_Click(object sender, EventArgs e)
        {
        }

        protected void CancelNewFtpSite_Click(object sender, EventArgs e)
        {
            TextBoxFtpName.Text = "";
            TextBoxFtpAddress.Text = "";
            TextBoxFtpUserName.Text = "";
            TextBoxFtpPassword.Text = "";
        }

        protected void SaveNewFtpSite_Click(object sender, EventArgs e)
        {
            int ftpId = Get.GetFtpItemId(TextBoxFtpName.Text);
            if (ftpId > 0)
                Update.UpdateFtp(TextBoxFtpName.Text, TextBoxFtpAddress.Text, TextBoxFtpUserName.Text, TextBoxFtpPassword.Text, ftpId);
            else
                Insert.AddFtpSite(TextBoxFtpName.Text, TextBoxFtpAddress.Text, TextBoxFtpUserName.Text, TextBoxFtpPassword.Text);
            UpdateFtpList();
        }

        protected void DropDownListFTPSites_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFtpDetails();
        }

        private void UpdateFtpDetails()
        {
            int ftpId = Get.GetFtpItemId(DropDownListFTPSites.SelectedItem.Text);
            FtpItem ftpItem = Get.GetFtpItem(ftpId);
            if (ftpItem != null)
            {
                TextBoxFtpName.Text = ftpItem.FtpName;
                TextBoxFtpAddress.Text = ftpItem.FtpAddress;
                TextBoxFtpUserName.Text = ftpItem.FtpUsername;
                TextBoxFtpPassword.Text = ftpItem.FtpPassword;
            }
        }

        protected void ButtonRunNow_OnClick(object sender, EventArgs e)
        {
            CoreDataLibrary.Data.Insert.AddExportQueItem(TextBoxFilename.Text);
        }

        //protected void DropDownListExportType_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    m_exportItem.ExportItemExportType = DropDownListExportType.SelectedItem.Text;
        //}
    }
}