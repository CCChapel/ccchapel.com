<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="CMSModules_Newsletters_Controls_SubscriberBlockedFilter"  Codebehind="SubscriberBlockedFilter.ascx.cs" %>
    
<asp:Panel runat="server" ID="pnlFilter">
    <cms:CMSDropDownList ID="ddlBounceFilter" runat="server" CssClass="ContentDropdown" />    
</asp:Panel>