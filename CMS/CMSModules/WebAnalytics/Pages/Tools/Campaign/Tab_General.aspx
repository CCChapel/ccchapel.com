<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/EmptyPage.master"
    Title="Campaign properties - General" Inherits="CMSModules_WebAnalytics_Pages_Tools_Campaign_Tab_General"
    Theme="Default" CodeBehind="Tab_General.aspx.cs" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="cpAfterForm">
<asp:Panel runat="server" CssClass="PageContent">
    <cms-autosave data-ng-controller="Autosave">
        <div data-ng-controller="CampaignEdit" class="cms-campaigns-edit" data-ng-cloak="cloak">
            <div class="editing-form-category campaign-category-description">
                <div class="campaign-description-textboxes">
                    <campaign-textbox
                        data-value="model.campaign.displayName"
                        data-maxlength="100"
                        data-required="true"
                        title='{{"campaign.displayname.description"|resolve}}'
                        data-label='{{"campaign.displayname"|resolve}}'
                        data-id="descriptionDisplayName"
                        data-classes="campaign-input-padding"
                        data-disabled="model.campaign.status !== 'Draft'">
                    </campaign-textbox>
                </div>
                <div class="campaign-description-textboxes">
                    <campaign-textbox
                        data-value="model.campaign.utmCode"
                        data-maxlength="200"
                        data-required="true"
                        data-pattern="[0-9a-zA-Z_.-]+"
                        data-pattern-error='campaign.utmparameter.wrongformat'
                        title='{{"campaign.utmcampaign.description"|resolve}}'
                        data-label='{{"campaign.utmcampaign"|resolve}}'
                        data-id="descriptionUTMCampaign"
                        data-classes="campaign-input-padding"
                        data-disabled="model.campaign.status !== 'Draft'">
                    </campaign-textbox>
                </div>
                <div class="ClearBoth"></div>
                <campaign-textarea
                    data-ng-model="model.campaign.description"
                    title='{{"campaign.description.description"|resolve}}'
                    data-rows="4"
                    data-label='{{"campaign.description"|resolve}}'
                    data-id="descriptionDescription"
                    data-classes="campaign-input-padding">
                </campaign-textarea>
            </div>
        </div>
        <div data-ng-controller="Asset" class="cms-campaigns-edit" data-ng-cloak="cloak">
            <div class="campaign-form-category" data-ng-if="model.isNewsletterModuleLoaded">
                <h3 class="control-label">{{"campaign.promotion"|resolve}}</h3>
                <%-- Resource string contains html and needs to go through resolveFilter. --%>
                <p data-ng-bind-html="resolveFilter('campaign.promotion.description')"></p>
                <div data-ng-repeat='asset in getPromotionChannels(model.assets) | orderBy: "assetID" '>
                    <campaign-asset-list-item data-ng-if="asset.id != 0">
                        <div class="col-xs-7">
                            <i class="icon-message cms-icon-100 campaign-assetlist-icon" aria-hidden="true"></i>
                            <div class="campaign-assetlist-header">
                                <h4 class="control-label">{{"campaign.assets." + asset.type|resolve}}</h4>
                                <div data-ng-if="asset.id > 0" class="campaign-assetlist-header-description"><a data-ng-href="{{asset.link}}" target="_blank">{{asset.name}}</a></div>
                                <div data-ng-if="asset.id === -1" class="campaign-assetlist-header-description campaign-assetlist-deleted">{{asset.name}}</div>
                            </div>
                        </div>
                        <campaign-textbox
                            data-value="asset.additionalProperties.utmSource" 
                            data-maxlength="100" 
                            data-required="true"
                            data-pattern="[0-9a-zA-Z_.-]+"
                            data-pattern-error='campaign.utmparameter.wrongformat'
                            title='{{"campaign.assetlist.email"|resolve}}' 
                            data-label='{{"campaign.utmsource"|resolve}}'
                            data-id="UTMsource{{asset.id}}"
                            data-classes="campaign-email-source col-xs-3"
                            data-disabled="campaign.status !== 'Draft'"
                            data-if="asset.id > 0">
                        </campaign-textbox>
                        <div class="col-xs-2 campaign-assetlist-icons">
                            <div class="icons-vertical-align">
                                <campaign-button-icon 
                                    title='{{"campaign.assetlist.removeemail"|resolve}}' 
                                    icon-class="icon-bin" 
                                    data-click="removeAsset(asset)"
                                    data-disabled="campaign.status !== 'Draft'">
                                </campaign-button-icon>
                            </div>
                        </div>
                    </campaign-asset-list-item>
                </div>
                <div class="content-block-50">
                    <cms-select-item
                        data-ng-model="model.newEmailAsset"
                        data-change="addEmailAsset()"
                        data-click="ensureExistingCampaign()"
                        title='{{"campaign.assets.email.description"|resolve}}' 
                        data-text='{{"campaign.assets.email.add"|resolve}}' 
                        data-id="campaignAssetEmail"
                        data-dialog-name='ItemSelection'
                        data-dialog-url='{{"url.selectemailpath"|resolve}}'
                        data-dialog-width='90%' 
                        data-dialog-height='85%'
                        data-button-id="campaignAssetEmailButton"
                        data-disabled="campaign.status !== 'Draft'">
                    </cms-select-item>
                    <button
                        class="btn btn-default" 
                        data-ng-click="ensureExistingCampaign() && showCreateEmailDialog()"
                        data-ng-disabled="campaign.status !== 'Draft'"
                        title="{{'campaign.create.email.description' | resolve}}">
                          {{"campaign.create.email" | resolve}}
                    </button>
                </div>
            </div>
            <div class="campaign-form-category">
                <h3 class="control-label">{{"campaign.conversion"|resolve}}</h3>
                <p>{{"campaign.conversion.description"|resolve}}</p>
                <div data-ng-repeat='asset in getTrackingAssets(model.assets) | orderBy: "assetID" '>
                    <campaign-asset-list-item data-ng-if="asset.id != 0">
                        <div data-ng-class='{"col-xs-4": asset.type == "cms.document"}'>
                            <i data-ng-class='{ "cms-icon-100": true, "campaign-assetlist-icon": true, "icon-doc": asset.type === "cms.document" }' aria-hidden="true"></i>
                            <div class="campaign-assetlist-header">
                                <h4 class="control-label">{{"campaign.assets." + asset.type|resolve}}</h4>
                                <div data-ng-if="asset.id > 0" class="campaign-assetlist-header-description"><a data-ng-href="{{asset.link}}" target="_blank">{{asset.name}}</a></div>
                                <div data-ng-if="asset.id === -1" class="campaign-assetlist-header-description campaign-assetlist-deleted">{{asset.name}}</div>
                            </div>
                        </div>
                        <campaign-textbox
                            data-value="asset.additionalProperties.conversionDisplayName" 
                            data-maxlength="100" 
                            data-required="true"
                            data-label='{{"conversion.name"|resolve}}'
                            data-id="conversionDisplayName{{asset.id}}"
                            data-classes="campaign-email-source col-xs-3"
                            data-if='asset.type == "cms.document" && asset.id > 0'
                            data-disabled="campaign.status !== 'Draft'">
                        </campaign-textbox>

                        <campaign-textbox
                            data-value="asset.additionalProperties.conversionValue" 
                            data-required="true"
                            data-label='{{"conversion.value"|resolve}}'
                            data-id="conversionValue{{asset.id}}"
                            data-classes="campaign-email-source col-xs-3"
                            data-input-type="positive-integer"
                            data-if='asset.type == "cms.document" && asset.id > 0'
                            data-disabled="campaign.status !== 'Draft'">
                        </campaign-textbox>

                        <div class="col-xs-2 campaign-assetlist-icons">
                            <div class="icons-vertical-align">
                                <campaign-button-icon 
                                    title='{{"campaign.assetlist.removeasset"|resolve}}' 
                                    icon-class="icon-bin" 
                                    data-click="removeAsset(asset)"
                                    data-disabled="campaign.status != 'Draft'">
                                </campaign-button-icon>
                            </div>
                            <div class="icons-vertical-align">
                                <campaign-button-icon 
                                    title='{{(campaign.utmCode ? "campaign.getcontentlink.button.title" : "campaign.getcontentlink.emptyutmcampaign.button.title")|resolve}}' 
                                    icon-class="icon-chain" 
                                    data-ng-click="showGetContentLinkDialog(asset)"
                                    data-disabled="!campaign.utmCode"
                                    data-ng-if="asset.id > 0">
                                </campaign-button-icon>
                            </div>
                        </div>
                    </campaign-asset-list-item>
                </div>
                <div class="content-block-50">
                    <cms-select-path
                        data-ng-model="model.newPageAsset"
                        data-change="addPageAsset()"
                        data-click="ensureExistingCampaign()"
                        data-id="campaignAssetLandingPage"
                        data-text='{{"campaign.assets.content.add"|resolve}}'
                        data-dialog-name='ContentSelection' 
                        data-dialog-url='{{"url.selectcontentpath"|resolve}}' 
                        data-dialog-width='90%' 
                        data-dialog-height='85%'
                        data-button-id="campaignAssetLandingPageButton"
                        title='{{"campaign.assets.content.description"|resolve}}'
                        data-disabled="campaign.status !== 'Draft'">
                    </cms-select-path>
                    <cms-create-page
                        data-ng-model="model.newPageAsset"
                        data-change="addPageAsset()"
                        data-click="ensureExistingCampaign()"
                        data-id="campaignAssetNewLandingPage"
                        data-text='{{"campaign.assets.content.create"|resolve}}'
                        data-dialog-name='newpage'
                        data-dialog-url='{{"url.createpagepath"|resolve}}' 
                        data-dialog-width='90%' 
                        data-dialog-height='90%'
                        data-button-id="campaignAssetCreatePageButton"
                        title='{{"campaign.assets.content.create.description"|resolve}}'
                        data-disabled="campaign.status !== 'Draft'">
                    </cms-create-page>
                </div>
            </div>
        </div> 
        <div data-ng-controller="CampaignEdit" class="cms-campaigns-edit" data-ng-cloak="cloak">
            <div class="campaign-form-category" ng-show="model.campaign.status === 'Draft'"> 
                <h3>{{"campaign.launch"|resolve}}</h3>
                <h5>{{"campaign.launchchecklist"|resolve}}</h5>
                <ul>
                    <li><span>{{"campaign.launchchecklist.subscribers"|resolve}}</span></li>
                    <li><span>{{"campaign.launchchecklist.messages"|resolve}}</span></li>
                    <li><span>{{"campaign.launchchecklist.syncteam"|resolve}}</span></li>
                    <li><span>{{"campaign.launchchecklist.marketingautomation"|resolve}}</span></li>
                    <li><span>{{"campaign.launchchecklist.external"|resolve}}</span></li>    
                </ul>
            </div>
            <div class="campaign-form-category" data-ng-show="model.campaign.status === 'Launched'"> 
                <p>{{"campaign.launchchecklist.launchedstatus"|resolve}}</p>
            </div>
            <div class="campaign-form-category" data-ng-show="model.campaign.status === 'Finished'"> 
                <p>{{"campaign.launchchecklist.finishedstatus"|resolve}}</p>
            </div>
            <div data-cms-messages-placeholder></div>
            <button 
                id="Button1" 
                data-ng-click="launchCampaign()" 
                class="btn btn-primary" 
                data-ng-if="model.campaign.campaignID && model.campaign.status === 'Draft'"
                data-ng-disabled="formController.$invalid || savingInProgress">{{"campaign.launch"|resolve}}
            </button>
            <button 
                id="Button2" 
                data-ng-click="finishCampaign()" 
                class="btn btn-primary" 
                data-ng-if="model.campaign.campaignID && model.campaign.status === 'Launched'"
                data-ng-disabled="formController.$invalid || savingInProgress">{{"campaign.finish"|resolve}}
            </button>
        </div>
    </cms-autosave>
    
    <div id="create-email-dialog-container" data-ng-controller="CampaignNewEmailDialog" data-ng-cloak="cloak" class="campaign-edit-dialog">
        <form class="form-horizontal" name="newEmailForm"> 
            <cms-textbox 
                data-value="emailSubject" 
                data-maxlength="100" 
                data-required="true" 
                title='{{"campaign.create.email.subject"|resolve}}' 
                data-label='{{"campaign.create.email.subject"|resolve}}' 
                data-id="email-subject">
            </cms-textbox>
            
            <cms-radio-button 
                data-value="emailCampaignType" 
                data-options="[ 
                    {   
                        id: 'new-email-new-campaign', 
                        label: 'campaign.create.email.select.type.new', 
                        value: 'new'
                    }, 
                    {
                        id: 'assign-email-campaign', 
                        label: 'campaign.create.email.select.type.assign', 
                        value: 'assign'
                    }
                ]"
                data-name="assignOrCreateEmailCampaign"
                data-id="email-campaign-select">
            </cms-radio-button>  
        
            <div data-ng-show="emailCampaignType === 'new'">  
                <cms-textbox 
                    data-value="emailDisplayName" 
                    data-maxlength="100" 
                    data-required="emailCampaignType === 'new'"
                    title='{{"newsletter_edit.newsletterdisplaynamelabel"|resolve}}' 
                    data-label='{{"newsletter_edit.newsletterdisplaynamelabel"|resolve}}' 
                    data-id="email-display-name" >
                </cms-textbox>    
        
                <cms-textbox 
                    data-value="emailSenderName" 
                    data-maxlength="100" 
                    data-required="emailCampaignType === 'new'" 
                    title='{{"newsletter_edit.newslettersendernamelabel"|resolve}}' 
                    data-label='{{"newsletter_edit.newslettersendernamelabel"|resolve}}' 
                    data-id="email-sender-name">
                </cms-textbox> 
        
                <cms-textbox 
                    data-value="emailSenderAddress" 
                    data-input-type="email"
                    data-pattern="{ value: EMAIL_REGEXP, display:'afterSubmission' }"
                    data-pattern-error="campaign.create.email.campaign.senderaddress.invalid.email"
                    data-maxlength="100" 
                    data-required="emailCampaignType === 'new'" 
                    title='{{"newsletter_edit.newslettersenderemaillabel"|resolve}}' 
                    data-label='{{"newsletter_edit.newslettersenderemaillabel"|resolve}}' 
                    data-id="email-sender-address">
                </cms-textbox> 
        
                <cms-select 
                    data-value="templateSubscription" 
                    data-options="data.emailTemplates.subscription"
                    data-required="emailCampaignType === 'new'" 
                    title='{{"newsletter_edit.subscriptiontemplate"|resolve}}' 
                    data-label='{{"newsletter_edit.subscriptiontemplate"|resolve}}' 
                    data-id="template-subscription">
                </cms-select>
        
                <cms-select 
                    data-value="templateUnsubscription" 
                    data-options="data.emailTemplates.unsubscription"
                    data-required="emailCampaignType === 'new'" 
                    title='{{"newsletter_edit.unsubscriptiontemplate"|resolve}}' 
                    data-label='{{"newsletter_edit.unsubscriptiontemplate"|resolve}}' 
                    data-id="template-unsubscription">
                </cms-select>          

                <cms-select 
                    data-value="templateIssue" 
                    data-options="data.emailTemplates.issue"
                    data-required="emailCampaignType === 'new'" 
                    title='{{"newsletter_edit.newslettertemplate"|resolve}}' 
                    data-label='{{"newsletter_edit.newslettertemplate"|resolve}}' 
                    data-id="template-issue">
                </cms-select>
            </div>    

            <div data-ng-show="emailCampaignType === 'assign'"> 
                <cms-select 
                    data-value="emailCampaignSelect" 
                    data-options="data.emailCampaigns"
                    data-required="emailCampaignType === 'assign'" 
                    title='{{"newsletter_newsletter.select.selectitem"|resolve}}' 
                    data-label='{{"newsletter_newsletter.select.selectitem"|resolve}}' 
                    data-id="email-campaign-select">
                </cms-select>                
            </div>
        </form>    
    </div>
    
    <div id="get-content-link-dialog-container" data-ng-controller="CampaignGetContentLinkDialog" data-ng-cloak="cloak" class="campaign-edit-dialog">
        <p>
            {{"campaign.getcontentlink.dialog.description"|resolve}}
        </p>
        <form class="form-horizontal" name="getContentLinkForm"> 
            <cms-textbox 
                data-value="utmSource" 
                data-maxlength="100" 
                title='{{"campaign.utmsource"|resolve}}' 
                data-label='{{"campaign.utmsource"|resolve}}'
                data-pattern="'[0-9a-zA-Z_.-]+'"
                data-pattern-error='campaign.utmparameter.wrongformat'
                data-required="true"
                data-explanation-text='{{"campaign.getcontentlink.dialog.utmsource.explanationtext"|resolve}}'
                data-id="get-link-dialog-utm-source">
            </cms-textbox>
                
            <cms-textbox 
                data-value="utmMedium" 
                data-maxlength="100" 
                title='{{"campaign.utmmedium"|resolve}}' 
                data-pattern="'[0-9a-zA-Z_.-]+'"
                data-pattern-error='campaign.utmparameter.wrongformat'
                data-label='{{"campaign.utmmedium"|resolve}}' 
                data-explanation-text='{{"campaign.getcontentlink.dialog.utmmedium.explanationtext"|resolve}}'
                data-id="get-link-dialog-utm-medium">
            </cms-textbox>
            
            <cms-textarea
                data-value="link"
                title='{{"campaign.getcontentlink.dialog.linktextarea.label"|resolve}}'
                data-label='{{"campaign.getcontentlink.dialog.linktextarea.label"|resolve}}'
                data-id="get-link-dialog-built-link"
                data-rows="4"
                data-ng-click="textAreaClick($event)"
                data-readonly="true">
            </cms-textarea>
        </form>    
    </div>
</asp:Panel>
</asp:Content>
