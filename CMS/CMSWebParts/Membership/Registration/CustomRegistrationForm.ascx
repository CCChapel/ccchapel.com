<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Membership_Registration_CustomRegistrationForm" CodeBehind="~/CMSWebParts/Membership/Registration/CustomRegistrationForm.ascx.cs" %>
<%@ Register Src="~/CMSFormControls/Captcha/SecurityCode.ascx" TagName="SecurityCode" TagPrefix="uc1" %>
<cms:MessagesPlaceHolder ID="plcMess" runat="server" />
<asp:Panel ID="pnlRegForm" runat="server" DefaultButton="btnRegister">
    <cms:BasicForm ID="formUser" runat="server" IsLiveSite="true" />
    <asp:PlaceHolder runat="server" ID="plcCaptcha">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblCaptcha" />
                </div>
                <div class="editing-form-value-cell">
                    <uc1:SecurityCode ID="captchaElem" runat="server" />
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
    <cms:CMSButton ID="btnRegister" runat="server" CssClass="RegisterButton" ButtonStyle="Default" />
</asp:Panel>
