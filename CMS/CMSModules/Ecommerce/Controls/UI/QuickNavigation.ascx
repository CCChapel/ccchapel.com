<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="QuickNavigation.ascx.cs"
    Inherits="CMSModules_Ecommerce_Controls_UI_QuickNavigation" %>
<asp:Panel ID="pnlWrapper" runat="server" CssClass="QuickNavigation">
    <ul runat="server">
        <asp:ListView ID="lstNavigationItems" runat="server">
            <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            </LayoutTemplate>
            <ItemTemplate>
                <li><a href="#<%# Eval("Value") %>">
                    <%# Eval("Key") %>
                </a></li>
            </ItemTemplate>
        </asp:ListView>
    </ul>
</asp:Panel>
