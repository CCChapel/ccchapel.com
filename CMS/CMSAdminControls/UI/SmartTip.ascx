<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="SmartTip.ascx.cs"
    Inherits="CMSAdminControls_UI_SmartTip" %>

<cms:CMSUpdatePanel ID="pnlUpdate" runat="server">
    <ContentTemplate>
        <asp:Panel runat="server" CssClass="alert-dismissable alert alert-smarttip" ID="pnlContainer">
            <span class="alert-icon">
                <cms:CMSIcon ID="iconAlert" runat="server" CssClass="icon-light-bulb" AlternativeText="{$general.smarttip$}" />
            </span>
            <div class="alert-label">
                <asp:Literal runat="server" ID="ltlContent" />
            </div>
            <asp:LinkButton runat="server" ID="btnClose" OnClick="btnClose_Click">
                <span class="alert-close">
                    <i class="close icon-modal-close"></i>
                    <span class="sr-only">Close</span>
                </span>
            </asp:LinkButton>
        </asp:Panel>
    </ContentTemplate>
</cms:CMSUpdatePanel>