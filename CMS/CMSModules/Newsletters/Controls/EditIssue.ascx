<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditIssue.ascx.cs" Inherits="CMSModules_Newsletters_Controls_EditIssue" %>

<%@ Register Src="~/CMSModules/Newsletters/Controls/Newsletter_ContentEditor.ascx"
    TagPrefix="cms" TagName="Newsletter_ContentEditor" %>
<%@ Register Src="~/CMSModules/Newsletters/FormControls/NewsletterTemplateSelector.ascx"
    TagPrefix="cms" TagName="NewsletterTemplateSelector" %>
<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagName="UniSelector"
    TagPrefix="cms" %>

<%-- Issue base properties --%>
<cms:CMSUpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="topPanel" class="header-panel" onmouseover="RememberFocusedRegion(); return false;">
            <div class="form-horizontal">
                <asp:Panel CssClass="form-group" runat="server" ID="pnlIssueSubject">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="lblSubject" runat="server" ResourceString="general.subject"
                            DisplayColon="true" EnableViewState="false" AssociatedControlID="txtSubject" ShowRequiredMark="True" />
                    </div>
                    <div class="editing-form-value-cell control-group-inline-forced">
                        <cms:CMSTextBox ID="txtSubject" runat="server" MaxLength="450" />
                        <div class="show-advanced-link">
                            <asp:Image runat="server" ID="imgToggleAdvanced" CssClass="NewItemImage" EnableViewState="false" />
                            <asp:LinkButton ID="lnkToggleAdvanced" runat="server" OnClick="lnkToggleAdvanced_Click" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:PlaceHolder ID="plcAdvanced" runat="server" Visible="false">
                    <asp:Panel CssClass="form-group" runat="server" ID="pnlIssueSenderName">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblSenderName" runat="server" ResourceString="newsletterissue.sender.name"
                                DisplayColon="true" EnableViewState="false" AssociatedControlID="txtSenderName" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSTextBox ID="txtSenderName" runat="server" MaxLength="200" />
                        </div>
                    </asp:Panel>
                    <asp:Panel CssClass="form-group" runat="server" ID="pnlIssueSenderEmail">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblSenderEmail" runat="server" ResourceString="newsletterissue.sender.email"
                                DisplayColon="true" EnableViewState="false" AssociatedControlID="txtSenderEmail" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSTextBox ID="txtSenderEmail" runat="server" MaxLength="200" />
                        </div>
                    </asp:Panel>
                    <asp:Panel CssClass="form-group" runat="server" ID="pnlIssueTemplate" EnableViewState="False">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblTemplate" runat="server" ResourceString="newsletterissue.template"
                                DisplayColon="true" EnableViewState="false" AssociatedControlID="issueTemplate" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:NewsletterTemplateSelector ID="issueTemplate" runat="server" AutoPostBack="true" />
                        </div>
                    </asp:Panel>
                    <asp:Panel CssClass="form-group" runat="server" ID="pnlIssueArchive">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblArchive" runat="server" ResourceString="newslettertemplate_edit.showinarchive"
                                DisplayColon="true" EnableViewState="false" AssociatedControlID="chkShowInArchive" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSCheckBox runat="server" ID="chkShowInArchive" />
                        </div>
                    </asp:Panel>
                    <asp:Panel CssClass="form-group" runat="server" ID="pnlIssueUseUTM">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblIssueUseUTM" runat="server" ResourceString="newsletterissue.utm.use"
                                DisplayColon="true" EnableViewState="false" AssociatedControlID="chkIssueUseUTM" />
                        </div>
                        <div class="editing-form-value-cell control-group-inline-forced">
                            <cms:CMSCheckBox runat="server" ID="chkIssueUseUTM" CssClass="checkbox-no-label" AutoPostBack="True" OnCheckedChanged="chkIssueUseUTM_CheckedChanged" />
                            <span class="info-icon">
                                <cms:LocalizedLabel runat="server" ID="lblScreenReaderIssueUseUTM" CssClass="sr-only"></cms:LocalizedLabel>
                                <cms:CMSIcon ID="iconHelpIssueUseUTM" runat="server" CssClass="icon-question-circle" EnableViewState="false" aria-hidden="true" data-html="true" />
                            </span>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlUTMParameters">
                        <asp:Panel CssClass="form-group" runat="server" ID="pnlIssueUTMSource">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblIssueUTMSource" runat="server" ResourceString="newsletterissue.utm.source"
                                    DisplayColon="true" EnableViewState="false" AssociatedControlID="txtIssueUTMSource" ShowRequiredMark="True" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtIssueUTMSource" runat="server" MaxLength="200" />
                                <span class="info-icon">
                                    <cms:LocalizedLabel runat="server" ID="lblScreenReaderIssueUTMSource" CssClass="sr-only"></cms:LocalizedLabel>
                                    <cms:CMSIcon ID="iconHelpIssueUTMSource" runat="server" CssClass="icon-question-circle" EnableViewState="false" aria-hidden="true" data-html="true" />
                                </span>
                            </div>
                        </asp:Panel>
                        <asp:Panel CssClass="form-group" runat="server" ID="pnlIssueUTMMedium">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblIssueUTMMedium" runat="server" ResourceString="newsletterissue.utm.medium"
                                    DisplayColon="true" EnableViewState="false" AssociatedControlID="txtIssueUTMMedium" ShowRequiredMark="True" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtIssueUTMMedium" runat="server" MaxLength="200" Enabled="False" />
                                <span class="info-icon">
                                    <cms:LocalizedLabel runat="server" ID="lblScreenReaderIssueUTMMedium" CssClass="sr-only"></cms:LocalizedLabel>
                                    <cms:CMSIcon ID="iconHelpIssueUTMMedium" runat="server" CssClass="icon-question-circle" EnableViewState="false" aria-hidden="true" data-html="true" />
                                </span>
                            </div>
                        </asp:Panel>
                        <asp:Panel CssClass="form-group" runat="server" ID="pnlIssueUTMCampaign">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblIssueUTMCampaign" runat="server" ResourceString="newsletterissue.utm.campaign"
                                    DisplayColon="true" EnableViewState="false" ShowRequiredMark="True" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSRadioButton runat="server" ID="radUTMCampaignExisting" GroupName="utmCampaign" ResourceString="newsletterissue.utm.campaign.existing" Checked="True" AutoPostBack="True" OnCheckedChanged="radUTMCampaign_OnCheckedChanged" />
                                <div class="selector-subitem">
                                    <cms:UniSelector ID="selectorUTMCampaign" runat="server" MaxLength="200" ObjectType="analytics.campaign" ObjectSiteName="#currentsite" AllowEmpty="False" SelectionMode="SingleDropDownList" ReturnColumnName="CampaignUTMCode" />
                                </div>
                                <cms:CMSRadioButton runat="server" ID="radUTMCampaignNew" GroupName="utmCampaign" ResourceString="newsletterissue.utm.campaign.new" AutoPostBack="True" OnCheckedChanged="radUTMCampaign_OnCheckedChanged" />
                                <div class="selector-subitem">
                                    <cms:CMSTextBox ID="txtIssueUTMCampaign" runat="server" MaxLength="200" Enabled="False" />
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel CssClass="form-group" runat="server" ID="pnlIssueUTMCampaignTextBox">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblIssueUTMCampaignTextBox" runat="server" ResourceString="newsletterissue.utm.campaign"
                                    DisplayColon="true" EnableViewState="false" AssociatedControlID="txtIssueUTMCampaignTextBox" ShowRequiredMark="True" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtIssueUTMCampaignTextBox" runat="server" MaxLength="200" />
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                </asp:PlaceHolder>
            </div>
        </div>
        <asp:HiddenField ID="hdnTemplateID" runat="server" EnableViewState="false" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkToggleAdvanced" EventName="click" />
    </Triggers>
</cms:CMSUpdatePanel>
<%-- Newletter issue content editor --%>
<cms:CMSUpdatePanel ID="pnlBodyUpdate" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <cms:Newsletter_ContentEditor ID="contentBody" runat="server" ShortID="ce" />
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="issueTemplate" />
    </Triggers>
</cms:CMSUpdatePanel>
<asp:HiddenField ID="hdnIssueContent" runat="server" EnableViewState="false" />
<script type="text/javascript">
    //<![CDATA[
    var EditIssue = (function ($) {
        'use strict';

        /**
         * Changes the placeholder for the utm source based on newsletter name and subject.
         */
        var utmSourcePlaceholderUpdate = function () {
            var txtSubject = $('#<%= txtSubject.ClientID %>'),
                txtUTMSource = $('#<%= txtIssueUTMSource.ClientID %>'),
                newsletterName = '<%= Newsletter.NewsletterName.ToLower() %>';

            if (!txtUTMSource.length) {
                return;
            }

            txtSubject.on('input', function () {
                txtUTMSource.attr('placeholder', newsletterName + '_' + $(this).val().toLowerCase().replace(/\s/g, '_'));
            });

            txtUTMSource.attr('placeholder', newsletterName + '_' + txtSubject.val().toLowerCase().replace(/\s/g, '_'));
        };

        return {
            init: function () {
                utmSourcePlaceholderUpdate();
            }
        };
    }($cmsj));

    $cmsj(document).ready(function () {
        EditIssue.init();
    });

    // Called when partial postback happened caused by update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        EditIssue.init();
    });

    //]]>
</script>