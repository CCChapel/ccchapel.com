<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedDocuments.ascx.cs"
    Inherits="CMSModules_Content_FormControls_Relationships_RelatedDocuments" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<div>
    <asp:Panel ID="pnlNewLink" runat="server" Style="margin-bottom: 8px;">
        <cms:LocalizedButton runat="server" ID="btnNewRelationship" ButtonStyle="Default" ResourceString="relationship.addrelateddocs" EnableViewState="false" CssClass="btn-group" />
    </asp:Panel>
    <div>
        <cms:CMSUpdatePanel ID="pnlUpdate" runat="server" class="">
            <ContentTemplate>
                <cms:MessagesPlaceHolder ID="plcMess" runat="server" />
                <cms:UniGrid ID="UniGridRelationship" runat="server" GridName="~/CMSModules/Content/FormControls/Relationships/RelatedDocuments_List.xml"
                    OrderBy="RelationshipNameID" IsLiveSite="false" ShowObjectMenu="false" />
            </ContentTemplate>
        </cms:CMSUpdatePanel>
    </div>
    <asp:HiddenField ID="hdnSelectedNodeId" runat="server" Value="" />
</div>
