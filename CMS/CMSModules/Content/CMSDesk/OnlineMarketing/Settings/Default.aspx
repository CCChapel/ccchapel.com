<%@ Page Language="C#" AutoEventWireup="true" Title="Online marketing settings" CodeBehind="Default.aspx.cs"
    Theme="Default" Inherits="CMSModules_Content_CMSDesk_OnlineMarketing_Settings_Default"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<%@ Register Src="~/CMSModules/WebAnalytics/FormControls/SelectConversion.ascx" TagName="ConversionSelector"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/Basic/DisabledModuleInfo.ascx" TagPrefix="cms"
    TagName="DisabledModule" %>
<%@ Register Src="~/CMSFormControls/Inputs/TextboxDoubleValidator.ascx" TagPrefix="cms"
    TagName="DoubleValidator" %>
<%@ Register Src="~/CMSAdminControls/UI/SmartTip.ascx" TagPrefix="cms"
    TagName="SmartTip" %>

<asp:Content ID="plcHeader" ContentPlaceHolderID="plcBeforeContent" runat="server">
    <cms:CMSDocumentPanel ID="pnlDoc" runat="server" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="server">
    <asp:Panel runat="server" ID="pnlDisabled">
        <cms:DisabledModule runat="server" ID="ucDisabledModule" />
    </asp:Panel>
    <div class="form-horizontal page-analytics-settings">
        <cms:SmartTip runat="server" ID="smrtpEditInCampaign" Visible="false" EnableViewState="false" IsDismissable="false" />
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblConversionName" runat="server" EnableViewState="false"
                    ResourceString="om.trackconversionname" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:ConversionSelector runat="server" ID="ucConversionSelector" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblConversionValue" runat="server" EnableViewState="false"
                    ResourceString="om.trackconversionvalue" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:DoubleValidator ID="txtConversionValue" runat="server"
                    MaxLength="200" />
            </div>
        </div>
        <asp:Panel CssClass="form-group" runat="server" ID="pnlTrackedCampaigns">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblCampaigns" runat="server" EnableViewState="false"
                    ResourceString="om.campaign.tracked" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <asp:ListView ID="listViewCampaings" runat="server">
                    <LayoutTemplate>
                        <div class="form-control-text">
                            <ul>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                            </ul>
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li><a href="<%# Eval("CampaignEditLink") %>" target="_blank" title="<%# Eval("EncodedCampaignDisplayName") %>"><%# Eval("EncodedCampaignDisplayName") %></a></li>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </asp:Panel>
    </div>
</asp:Content>