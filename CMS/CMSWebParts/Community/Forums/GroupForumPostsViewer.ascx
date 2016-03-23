<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Community_Forums_GroupForumPostsViewer"  Codebehind="~/CMSWebParts/Community/Forums/GroupForumPostsViewer.ascx.cs" %>
<%@ Register TagPrefix="cms" Namespace="CMS.Forums" Assembly="CMS.Forums" %>
<cms:BasicRepeater runat="server" ID="repLatestPosts" />
<cms:ForumPostsDataSource runat="server" ID="forumDataSource" />
<div class="Pager">
    <cms:UniPager ID="pagerElem" runat="server" />
</div>
