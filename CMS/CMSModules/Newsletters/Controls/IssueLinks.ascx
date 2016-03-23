<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IssueLinks.ascx.cs" Inherits="CMSModules_Newsletters_Controls_IssueLinks" %>

<%@ Register Src="~/CMSModules/Newsletters/Controls/TrackedLinksFilter.ascx" TagPrefix="cms"
    TagName="LinkFilter" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagPrefix="cms" TagName="UniGrid" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<cms:LinkFilter runat="server" ID="fltLinks" ShortID="f" />
<cms:UniGrid runat="server" ID="ugLinks" ShortID="g" IsLiveSite="false" ShowObjectMenu="False" ShowExportMenu="True">
    <GridActions Parameters="LinkID;LinkOutdated">
        <ug:Action Name="deleteoutdated" Caption="$Unigrid.Newsletter.Actions.DeleteOutdated$"
            FontIconClass="icon-bin" FontIconStyle="Critical" Confirmation="$General.ConfirmDelete$" ExternalSourceName="deleteoutdated" />
    </GridActions>
    <GridColumns>
        <ug:Column Source="LinkTarget" ExternalSourceName="linktarget" Caption="$general.link$"
            Wrap="false">
            <Tooltip Source="LinkTarget" ExternalSourceName="linktargettooltip" />
        </ug:Column>
        <ug:Column Source="LinkDescription" ExternalSourceName="linkdescription" Caption="$general.description$"
            Wrap="false">
            <Tooltip Source="LinkDescription" ExternalSourceName="linkdescriptiontooltip" />
        </ug:Column>
        <ug:Column Source="##ALL##" Caption="$unigrid.newsletter_issue_trackedlinks.columns.totalclicks$"
            Wrap="false" CssClass="TableCell" ExternalSourceName="totalclicks" />
        <ug:Column Source="##ALL##" Caption="$unigrid.newsletter_issue_trackedlinks.columns.uniqueclicks$"
            Wrap="false" CssClass="TableCell" ExternalSourceName="uniqueclicks" />
        <ug:Column Source="##ALL##" Name="clickrate" ExternalSourceName="clickrate" Caption="$unigrid.newsletter_issue_trackedlinks.columns.clickrate$"
            Wrap="false" CssClass="TableCell" />
        <ug:Column CssClass="filling-column" />
    </GridColumns>
</cms:UniGrid>
