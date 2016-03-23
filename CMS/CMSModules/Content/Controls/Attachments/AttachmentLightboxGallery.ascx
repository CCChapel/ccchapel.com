<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Content_Controls_Attachments_AttachmentLightboxGallery"  Codebehind="AttachmentLightboxGallery.ascx.cs" %>
<%@ Register Src="~/CMSModules/Content/Controls/Attachments/DocumentAttachments/DocumentAttachments.ascx" TagName="DocumentAttachments"
    TagPrefix="cms" %>
<cms:DocumentAttachments ID="ucAttachments" runat="server" />
<cms:LightboxExtender ID="extGalleryLightbox" runat="server" />
