<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/CoreDataReport.Master" AutoEventWireup="true" CodeBehind="PropertyExporter.aspx.cs" Inherits="CoreDataReportCreator.PropertyExporter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" style="padding-bottom: 40px; padding-top: 20px;">
        <div class="page-header">
            <h1>Property Exporter</h1>
        </div>
        <%--        <form id="form1" runat="server">--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="row">
            <div class="col-xs-2">
                <div class="input-group">
                    <span class="input-group-addon">
                        <asp:Label ID="LabelEnableExport" runat="server" class="form-control-static" Text="Enabled" />
                    </span>
                    <asp:CheckBox ID="CheckBoxExportEnabled" class="form-control" runat="server" />
                </div>
            </div>
            <div class="col-xs-5">
                <div class="input-group">
                    <span class="input-group-addon">
                        <asp:Label ID="LabeCurrentFile" runat="server" class="form-control-static" Text="File" />
                    </span>
                    <asp:TextBox ID="TextBoxFilename" runat="server" class="form-control" placeholder="New filename" OnTextChanged="TextBoxFilename_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <span class="input-group-btn">
                        <asp:Button ID="ButtonSaveExport" runat="server" class="btn btn-info btn-default" Text="Save" OnClick="ButtonSaveExport_Click" />
                    </span>
                </div>
            </div>
            <div class="col-xs-5">
                <div class="input-group">
                    <span class="input-group-addon">
                        <asp:Label runat="server" Text="Exports" class="form-control-static"></asp:Label>
                    </span>
                    <asp:DropDownList ID="DropDownListExportFile" runat="server" placeholder="Select Report" class="form-control" OnSelectedIndexChanged="DropDownListExportFile_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <span class="input-group-btn">
                        <asp:Button ID="ButtonRunNow" runat="server" class="btn btn-info btn-default" Text="Add To Que" OnClick="ButtonRunNow_OnClick" />
                    </span>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <div class="input-group" style="margin-top: 10px;">
                    <span class="input-group-addon">
                        <label class="form-control-static">File Link</label>
                    </span>
                    <asp:TextBox ID="TextBoxLinkToFile" class="form-control" Enabled="true" ReadOnly="True" runat="server" TextMode="SingleLine" placeholder="File unavailable."></asp:TextBox>
                </div>
            </div>
        </div>
        <div id="accordion" class="panel-group" style="margin-top: 10px; margin-bottom: 10px;">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <p class="panel-title">
                        <div class="input-group">
                            <span class="input-group-addon">
                                <asp:Label ID="Label1" runat="server" Text="FTP Site" class="form-control-static"></asp:Label>
                            </span>
                            <asp:DropDownList ID="DropDownListFTPSites" runat="server" class="form-control" OnSelectedIndexChanged="DropDownListFTPSites_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <span class="input-group-addon" style="width: 100px">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne">FTP Details</a>
                            </span>
                        </div>
                    </p>
                </div>
                <div id="collapseOne" class="panel-collapse collapse">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-3" style="margin-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon" style="width: 100px">
                                        <asp:Label ID="FTPName" runat="server" class="form-control-static" Text="Ftp name" />
                                    </span>
                                    <asp:TextBox ID="TextBoxFtpName" runat="server" class="form-control" placeholder="New FTP Name"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-xs-3" style="margin-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon" style="width: 100px">
                                        <asp:Label ID="FtpAddress" runat="server" class="form-control-static" Text="Ftp address" />
                                    </span>
                                    <asp:TextBox ID="TextBoxFtpAddress" runat="server" class="form-control" placeholder="New FTP Address"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-3" style="margin-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon" style="width: 100px">
                                        <asp:Label ID="FtpUserName" runat="server" class="form-control-static" Text="User name" />
                                    </span>
                                    <asp:TextBox ID="TextBoxFtpUserName" runat="server" class="form-control" placeholder="Name"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-3" style="margin-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon" style="width: 100px">
                                        <asp:Label ID="FTPPassword" runat="server" class="form-control-static" Text="Password" />
                                    </span>
                                    <asp:TextBox ID="TextBoxFtpPassword" runat="server" class="form-control" placeholder="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12" style="margin-bottom: 10px;">
                                <asp:Button ID="ButtonSave" class="btn btn-info btn-default pull-right" runat="server" Text="Update FTP Details" OnClick="SaveNewFtpSite_Click" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="padding-bottom: 10px;">
            <div class="col-xs-6">
                <div class="input-group" style="margin-top: 5px;">
                    <span class="input-group-addon">
                        <asp:Label class="form-control-static">Export Format</asp:Label>
                    </span>
                    <asp:DropDownList ID="DropDownListExportType" runat="server" class="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-6" id="exportEncoding" runat="server">
                <div class="input-group" style="margin-top: 5px;">
                    <span class="input-group-addon">
                        <asp:Label class="form-control-static">Export Encoding</asp:Label>
                    </span>
                    <asp:DropDownList ID="DropDownListExportEncoding" runat="server" class="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
        <%--        <div class="panel-group" style="margin-top: 10px; margin-bottom: 10px;">--%>
 <%--       <div class="panel panel-default">
            <div class="panel-heading">
                <p class="panel-title">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:Label ID="Label2" runat="server" Text="Export Format" class="form-control-static"></asp:Label>
                        </span>
                        <asp:DropDownList ID="DropDownListExportType" runat="server" class="form-control"></asp:DropDownList>--%>
                        <%-- <span class="input-group-addon" style="width: 100px">
                                <a data-toggle="collapse" data-parent="#accordionType" href="#collapseExportType">ExportType</a>
  <%--                          </span>--%>
  <%--                  </div>
                </p>
            </div>--%>
            <%--               <div id="collapseExportType" class="panel-collapse collapse">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-3" style="margin-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon" style="width: 100px">
                                        <asp:Label ID="Label3" runat="server" class="form-control-static" Text="Ftp name" />
                                    </span>
                                    <asp:TextBox ID="TextBox1" runat="server" class="form-control" placeholder="New FTP Name"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-xs-3" style="margin-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon" style="width: 100px">
                                        <asp:Label ID="Label4" runat="server" class="form-control-static" Text="Ftp address" />
                                    </span>
                                    <asp:TextBox ID="TextBox2" runat="server" class="form-control" placeholder="New FTP Address"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-3" style="margin-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon" style="width: 100px">
                                        <asp:Label ID="Label5" runat="server" class="form-control-static" Text="User name" />
                                    </span>
                                    <asp:TextBox ID="TextBox3" runat="server" class="form-control" placeholder="Name"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-3" style="margin-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon" style="width: 100px">
                                        <asp:Label ID="Label6" runat="server" class="form-control-static" Text="Password" />
                                    </span>
                                    <asp:TextBox ID="TextBox4" runat="server" class="form-control" placeholder="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12" style="margin-bottom: 10px;">
                                <asp:Button ID="Button1" class="btn btn-info btn-default pull-right" runat="server" Text="Update FTP Details" OnClick="SaveNewFtpSite_Click" />
                            </div>
                        </div>

                    </div>
                </div>--%>
        <%--</div>--%>
        <%--</div>--%>
        <div class="row">
            <div class="col-xs-4">
                <div class="input-group">
                    <span class="input-group-addon">
                        <asp:Label ID="LabelLanguage" runat="server" class="control-label" Text="Language To Export"></asp:Label>
                    </span>
                    <asp:DropDownList ID="DropDownListLanguage" runat="server" class="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-2">
                <div class="input-group">
                    <span class="input-group-addon">
                        <asp:RadioButton ID="RadioButtonOwnStock" GroupName="OwnStock" runat="server" />
                    </span>
                    <asp:Label ID="LabelOwnStock" runat="server" Text="Own Stock" class="form-control"></asp:Label>
                </div>
            </div>
            <div class="col-xs-2">
                <div class="input-group">
                    <span class="input-group-addon">
                        <asp:RadioButton ID="RadioButtonOwnStockLive" GroupName="OwnStock" runat="server" Checked="true" />
                    </span>
                    <asp:Label ID="LabelOwnStockLive" runat="server" Text="Own Stock Live" class="form-control"></asp:Label>
                </div>
            </div>
            <div class="col-xs-2">
                <div class="input-group">
                    <span class="input-group-addon">
                        <asp:CheckBox ID="CheckBoxNotOwnStock" runat="server" />
                    </span>
                    <asp:Label ID="LabelNotOwnStock" runat="server" Text="Not Own Stock" class="form-control"></asp:Label>
                </div>
            </div>
            <div class="col-xs-2">
                <div class="input-group">
                    <span class="input-group-addon">
                        <asp:Label runat="server" Text="Run Time" class="form-control-static"></asp:Label>
                    </span>
                    <asp:DropDownList ID="DropDownRunTime" runat="server" class="form-control" Width="70"></asp:DropDownList>
                </div>
            </div>
        </div>
        <hr />
        <div>
            <h3><small>Select items to exclude from exported file.</small></h3>
            <div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxPropertyReferenceId" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxPropertyReferenceId_CheckedChanged" />
                        </span>
                        <asp:Label ID="LabelPropertyReferenceId" class="form-control" runat="server" Text="Property Reference Id" Style="margin-right: 20px;"> </asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxPropertyName" runat="server" />
                        </span>
                        <asp:Label ID="LabelPropertyName" class="form-control" runat="server" Text="Property Name"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxRating" runat="server" />
                        </span>
                        <asp:Label ID="LabelRating" class="form-control" runat="server" Text="Rating"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxPropertyType" runat="server" />
                        </span>
                        <asp:Label ID="LabelPropertyType" class="form-control" runat="server" Text="Property Type"></asp:Label>
                    </div>
                </div>
            </div>
            <div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxIATACode" runat="server" />
                        </span>
                        <asp:Label ID="LabelIATACode" class="form-control" runat="server" Text="IATA Code"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxResortId" runat="server" />
                        </span>
                        <asp:Label ID="LabelResortId" class="form-control" runat="server" Text="Resort Id"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxRegionId" runat="server" />
                        </span>
                        <asp:Label ID="LabelRegionId" class="form-control" runat="server" Text="Region Id"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxCountryId" runat="server" />
                        </span>
                        <asp:Label ID="LabelCountryId" class="form-control" runat="server" Text="Country Id"></asp:Label>
                    </div>
                </div>
            </div>
            <div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxResort" runat="server" />
                        </span>
                        <asp:Label ID="LabelResort" class="form-control" runat="server" Text="Resort"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxRegion" runat="server" />
                        </span>
                        <asp:Label ID="LabelRegion" class="form-control" runat="server" Text="Region"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxCountry" runat="server" />
                        </span>
                        <asp:Label ID="LabelCountry" class="form-control" runat="server" Text="Country"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxAddress" runat="server" />
                        </span>
                        <asp:Label ID="LabelAddress" class="form-control" runat="server" Text="Address"></asp:Label>
                    </div>
                </div>
            </div>
            <div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxTownCity" runat="server" />
                        </span>
                        <asp:Label ID="LabelTownCity" class="form-control" runat="server" Text="Town City"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxCounty" runat="server" />
                        </span>
                        <asp:Label ID="LabelCounty" class="form-control" runat="server" Text="County"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxPostcodeZip" runat="server" />
                        </span>
                        <asp:Label ID="LabelPostcodeZip" class="form-control" runat="server" Text="Postcode Zip"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxTelephone" runat="server" />
                        </span>
                        <asp:Label ID="LabelTelephone" class="form-control" runat="server" Text="Telephone"></asp:Label>
                    </div>
                </div>
            </div>
            <div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxFax" runat="server" />
                        </span>
                        <asp:Label ID="LabelFax" class="form-control" runat="server" Text="Fax"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxLatitude" runat="server" />
                        </span>
                        <asp:Label ID="LabelLatitude" class="form-control" runat="server" Text="Latitude"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxLongitude" runat="server" />
                        </span>
                        <asp:Label ID="LabelLongitude" class="form-control" runat="server" Text="Longitude"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxStrapline" runat="server" />
                        </span>
                        <asp:Label ID="LabelStrapline" class="form-control" runat="server" Text="Strapline"></asp:Label>
                    </div>
                </div>
            </div>
            <div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxDescription" runat="server" />
                        </span>
                        <asp:Label ID="LabelDescription" class="form-control" runat="server" Text="Description"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxDistanceFromAirport" runat="server" />
                        </span>
                        <asp:Label ID="LabelDistanceFromAirport" class="form-control" runat="server" Text="Distance From Airport"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxTransferTime" runat="server" />
                        </span>
                        <asp:Label ID="LabelTransferTime" class="form-control" runat="server" Text="Transfer Time"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxRightChoice" runat="server" />
                        </span>
                        <asp:Label ID="LabelRightChoice" class="form-control" runat="server" Text="Right Choice"></asp:Label>
                    </div>
                </div>
            </div>
            <div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxLocationAndResort" runat="server" />
                        </span>
                        <asp:Label ID="LabelLocationAndResort" class="form-control" runat="server" Text="Location And ResortExclusion"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxSwimmingPools" runat="server" />
                        </span>
                        <asp:Label ID="LabelSwimmingPools" class="form-control" runat="server" Text="Swimming Pools"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxEatingAndDrinking" runat="server" />
                        </span>
                        <asp:Label ID="LabelEatingAndDrinking" class="form-control" runat="server" Text="Eating And Drinking"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxAccomodation" runat="server" />
                        </span>
                        <asp:Label ID="LabelAccomodation" class="form-control" runat="server" Text="Accomodation"></asp:Label>
                    </div>
                </div>
            </div>
            <div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxSuitableFor" runat="server" />
                        </span>
                        <asp:Label ID="LabelSuitableFor" class="form-control" runat="server" Text="Suitable For"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxUrl" runat="server" />
                        </span>
                        <asp:Label ID="LabelUrl" class="form-control" runat="server" Text="Url"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxMainImage" runat="server" />
                        </span>
                        <asp:Label ID="LabelMainImage" class="form-control" runat="server" Text="Main Image"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxMainImageThumbnail" runat="server" />
                        </span>
                        <asp:Label ID="LabelMainImageThumbnail" class="form-control" runat="server" Text="Main Image Thumbnail"></asp:Label>
                    </div>
                </div>
            </div>
            <div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxImageList" runat="server" />
                        </span>
                        <asp:Label ID="LabelImageList" class="form-control" runat="server" Text="Image List"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxFacilitiesList" runat="server" />
                        </span>
                        <asp:Label ID="LabelFacilitiesList" class="form-control" runat="server" Text="FacilitiesList List"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <asp:CheckBox ID="CheckBoxTTI" runat="server" />
                        </span>
                        <asp:Label ID="LabelTTI" class="form-control" runat="server" Text="TTI Codes"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <asp:UpdatePanel ID="UpdatePanelFilters" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-xs-10">
                        <h2><small>Filters.</small></h2>
                    </div>
                    <div>
                        <div class="col-xs-4">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <asp:Label ID="LabelExcludeIncludeCountry" runat="server" Text="Country" />
                                </span>
                                <asp:DropDownList ID="DropDownListCountry" class="form-control" runat="server" OnSelectedIndexChanged="DropDownListCountry_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <span class="input-group-addon">
                                    <asp:CheckBox ID="CheckBoxExcludeCountry" runat="server" OnCheckedChanged="CheckBoxExcludeCountry_CheckedChanged" AutoPostBack="true" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div id="RegionsSelector" class="col-xs-4" runat="server" visible="false">
                        <div class="input-group">
                            <span class="input-group-addon">
                                <asp:Label ID="LabelExcludeIncludeRegion" runat="server" Text="Region" />
                            </span>
                            <asp:DropDownList ID="DropDownListRegion" class="form-control" runat="server" OnSelectedIndexChanged="DropDownListRegion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <span class="input-group-addon">
                                <asp:CheckBox ID="CheckBoxExcludeRegion" runat="server" OnCheckedChanged="CheckBoxExcludeRegion_CheckedChanged" AutoPostBack="true" />
                            </span>
                        </div>
                    </div>
                    <div id="ResortsSelector" class="col-xs-4" runat="server" visible="false">
                        <div class="input-group">
                            <span class="input-group-addon">
                                <asp:Label ID="LabelExcludeIncludeResort" runat="server" Text="Resort" />
                            </span>
                            <asp:DropDownList ID="DropDownListResort" class="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListResort_SelectedIndexChanged"></asp:DropDownList>
                            <span class="input-group-addon">
                                <asp:CheckBox ID="CheckBoxExcludeResort" runat="server" OnCheckedChanged="CheckBoxExcludeResort_CheckedChanged" AutoPostBack="true" />
                            </span>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanelExclude" runat="server">
            <ContentTemplate>
                <div class="panel panel-default" style="margin-top: 20px">
                    <div class="panel-heading">
                        Exclude.
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Button ID="ButtonAddExclusion" runat="server" class="btn btn-info btn-default" Style="margin: 10px 10px" Text="Add Exclusion" OnClick="ButtonAddExclusion_Click" />
                            </div>
                            <div class="col-xs-4">
                                <asp:Button ID="ButtonRemoveExclusion" runat="server" class="btn btn-info btn-default" Style="margin: 10px 10px" Text="Remove Exclusion" OnClick="ButtonRemoveExclusion_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12" style="padding-bottom: 10px">
                                <asp:ListBox ID="ListBoxExclusions" class="form-control" runat="server" AutoPostBack="true"></asp:ListBox>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanelInclude" runat="server">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Include.
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Button ID="ButtonAddInclusion" runat="server" class="btn btn-info btn-default" Style="margin: 10px 10px" Text="Add Inclusion" OnClick="ButtonAddInclusion_Click" />
                            </div>
                            <div class="col-xs-4">
                                <asp:Button ID="ButtonRemoveInclusion" runat="server" class="btn btn-info btn-default" Style="margin: 10px 10px" Text="Remove Inclusion" OnClick="ButtonRemoveInclusion_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12" style="padding-bottom: 10px">
                                <asp:ListBox ID="ListBoxInclusions" class="form-control" runat="server" AutoPostBack="true"></asp:ListBox>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--        </form>--%>
    </div>
</asp:Content>
