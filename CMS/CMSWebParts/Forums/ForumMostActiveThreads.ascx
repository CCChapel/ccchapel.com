<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Forums_ForumMostActiveThreads"  Codebehind="~/CMSWebParts/Forums/ForumMostActiveThreads.ascx.cs" %>
<%@ Register TagPrefix="cms" Namespace="CMS.Forums" Assembly="CMS.Forums" %>
<cms:BasicRepeater runat="server" ID="repMostActiveThread" />
<cms:ForumPostsDataSource runat="server" ID="forumDataSource" />
