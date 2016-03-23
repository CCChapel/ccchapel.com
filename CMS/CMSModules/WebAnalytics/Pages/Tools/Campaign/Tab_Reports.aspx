<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="Tab_Reports.aspx.cs" Inherits="CMSModules_WebAnalytics_Pages_Tools_Campaign_Tab_Reports"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Campaign reports"
    EnableEventValidation="false" Theme="Default" %>

<asp:Content ID="cntContent" runat="server" ContentPlaceHolderID="plcContent">
    <div class="header-panel">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblInfoName" runat="server" ResourceString="campaign.basicinfo.name"
                        EnableViewState="false" />
                </div>
                <div class="editing-form-value-cell">
                    <asp:Label CssClass="form-control-text" ID="lblInfoNameValue" runat="server" EnableViewState="false" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblInfoFrom" runat="server" ResourceString="campaign.basicinfo.start"
                        EnableViewState="false" />
                </div>
                <div class="editing-form-value-cell">
                    <asp:Label CssClass="form-control-text" ID="lblInfoFromValue" runat="server" EnableViewState="false" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblInfoTo" runat="server" ResourceString="campaign.basicinfo.end"
                        EnableViewState="false" />
                </div>
                <div class="editing-form-value-cell">
                    <asp:Label CssClass="form-control-text" ID="lblInfoToValue" runat="server" EnableViewState="false" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblInfoDescription" runat="server" ResourceString="campaign.basicinfo.description"
                        EnableViewState="false" />
                </div>
                <div class="editing-form-value-cell campaign-description-text">
                    <asp:Label CssClass="form-control-text" ID="lblInfoDescriptionValue" runat="server" EnableViewState="false" />
                </div>
            </div>
         </div>
    </div>
    <div class="header-panel campaign-table-chart-container">
        <cms:LocalizedLabel ID="lblNoData" runat="server" ResourceString="campaign.nodata" Visible="false" EnableViewState="false" />
        <asp:Panel runat="server" ID="pnlCampaignColumnChart" CssClass="column-chart" style="height:250px;"/>
        <div id="legend" class="legend"></div>
        <asp:GridView runat="server" ID="gridChannels" EnableViewState="false" OnRowDataBound="gridChannels_RowDataBound"
                      BorderWidth="0" GridLines="None" CssClass="table table-responsive table-with-fixed-column-width" AutoGenerateColumns="false" />
    </div>
</asp:Content>
