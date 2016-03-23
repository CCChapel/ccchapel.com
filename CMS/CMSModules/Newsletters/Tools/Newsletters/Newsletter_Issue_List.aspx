<%@ Page Language="C#" AutoEventWireup="true" Title="Tools - Newsletter issues"
    Inherits="CMSModules_Newsletters_Tools_Newsletters_Newsletter_Issue_List" Theme="Default"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"  Codebehind="Newsletter_Issue_List.aspx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<asp:Content ContentPlaceHolderID="plcContent" ID="content" runat="server">
    <cms:UniGrid runat="server" ID="UniGrid" ShortID="g" IsLiveSite="false" OrderBy="IssueStatus, CASE WHEN IssueMailoutTime IS NULL THEN 0 ELSE 1 END, IssueMailoutTime DESC"
        ObjectType="newsletter.issue" Columns="IssueID, IssueSubject, IssueMailoutTime, IssueSentEmails, IssueOpenedEmails, IssueUnsubscribed, IssueBounces, IssueIsABTest, IssueStatus, IssueVariantOfIssueID"
        RememberStateByParam="">
        <GridActions Parameters="IssueID">
            <ug:Action Name="edit" Caption="$General.Edit$" FontIconClass="icon-edit" FontIconStyle="Allow" />
            <ug:Action Name="delete" Caption="$General.Delete$" FontIconClass="icon-bin" FontIconStyle="Critical" Confirmation="$General.ConfirmDelete$" />            
        </GridActions>
        <GridColumns>
            <ug:Column Source="##ALL##" ExternalSourceName="IssueSubject" Caption="$unigrid.newsletter_issue.columns.issuesubject$"
                Wrap="false">
                <Filter Type="text" />
            </ug:Column>
            <ug:Column Source="IssueStatus" Caption="$newsletters.issuestatus$" ExternalSourceName="IssueStatus"
                Wrap="false"/>
            <ug:Column Source="IssueMailoutTime" Caption="$unigrid.newsletter_issue.columns.issuemailouttime$"
                Wrap="false" />
            <ug:Column Source="IssueSentEmails" Caption="$unigrid.newsletter_issue.columns.issuesentemails$"
                Wrap="false" CssClass="TableCell" ExternalSourceName="IssueSentEmails"/>
            <ug:Column Source="##ALL##" Caption="$newsletters.issuedeliveryrate$"
                Wrap="false" CssClass="TableCell" ExternalSourceName="DeliveryRate" Name="deliveryrate" />
            <ug:Column Source="##ALL##" ExternalSourceName="IssueOpenedEmails" Caption="$newsletters.issueopenrate$"
                Wrap="false" CssClass="TableCell" Name="openedemails" />
            <ug:Column Source="##ALL##" ExternalSourceName="IssueClickedLinks" Caption="$newsletters.issueclickrate$"
                Wrap="false" CssClass="TableCell" Name="issueclickedlinks" />
            <ug:Column Source="##ALL##" Caption="$newsletters.issueunsubscriberate$"
                Wrap="false" CssClass="TableCell" ExternalSourceName="unsubscriberate" />
            <ug:Column CssClass="filling-column" />
        </GridColumns>
        <GridOptions DisplayFilter="true" />
    </cms:UniGrid>
</asp:Content>
