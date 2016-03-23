<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSInlineControls_RatingControl"  Codebehind="RatingControl.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/ContentRating/RatingControl.ascx" TagName="RatingControl"
    TagPrefix="cms" %>
<div>
    <cms:RatingControl ID="elemRating" runat="server" />
</div>
