<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Campaign list"
    Inherits="CMSModules_WebAnalytics_Pages_Tools_Campaign_List" Theme="Default" CodeBehind="List.aspx.cs" EnableViewState="false" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="cms-campaigns-list" data-ng-controller="CampaignList" data-ng-cloak="cloak">
        <div class="campaigns-filter">
            <label class="control-label" for="campaignName">{{"campaign.codename"|resolve}}:</label>
            <input type="text" id="campaignName" class="form-control content-block-25" data-ng-model="campaignNameFilter" />
            
            <label class="control-label" for="statusFilter">{{"campaign.status"|resolve}}:</label>
            <select data-ng-model="statusFilter" id="statusFilter" class="form-control content-block-25" data-ng-init="statusFilter = ''">
                <option value="">{{"general.all"|resolve}}</option>
                <option value="draft">{{"campaigns.campaign.status.draft"|resolve}}</option>
                <option value="launched">{{"campaigns.campaign.status.launched"|resolve}}</option>
                <option value="finished">{{"campaigns.campaign.status.finished"|resolve}}</option>
            </select>
            
            <label for="sortBy" class="control-label">{{"campaign.sortby"|resolve}}:</label>
            <select data-ng-model="sortBy" id="sortBy" class="form-control" data-ng-init="sortBy=''" >
                <option value="" data-ng-if="statusFilter === ''">{{"campaign.status"|resolve}}</option>
                <option value="displayName">{{"campaign.name"|resolve}}</option>
                <option value="-visitors" data-ng-if="statusFilter !== 'draft'">{{"campaign.visitors"|resolve}}</option>
            </select>
        </div>
        <div class="campaigns-list">
            <div>
                <div class="header">
                    <div class="header-actions-container">
                        <div class="header-actions-main">
                            <button data-ng-click="model.newCampaignClick()" type="button" class="btn btn-primary">{{"campaigns.list.campaign.new" | resolve}}</button>
                        </div>
                    </div>
                </div>

                <div class="content-container">
                    <div data-ng-repeat="campaign in model.campaigns | filter: { displayName: campaignNameFilter, status: statusFilter } | orderBy: sortBy ">
                        <div data-campaign-detail="" data-campaign="campaign" class="campaign-detail-container"></div>
                    </div>
                </div>

                <script type="text/ng-template" id="CampaignDetailTemplate.html">
            <div class="campaign-detail">
                <div class="header-container col-md-5 col-xs-12">
                    <h4><a href="{{campaign.detailLink}}" title="{{'campaigns.list.campaign.edit' | resolve}}" target="_top">{{campaign.displayName}}</a></h4>
                    <span class="tag {{model.getStatusClass()}}">{{model.getStatusResourceString() | resolve}}</span>
                    <span>{{campaign.openFrom | date:'longDate'}}</span>
                    <span data-ng-if="campaign.openTo">&ndash;</span>
                    <span>{{campaign.openTo | date:'longDate'}}</span>
                </div>
                   
                <div class="stats-container col-md-4 col-xs-10" data-ng-if="campaign.status == 'Launched' || campaign.status == 'Finished'">
                    <dl class="col-xs-4">
                        <dt>{{campaign.visitors | shortNumber}}</dt>
                        <dd>{{'campaign.visitors' | resolve}}</dd>
                    </dl>
                    <dl class="col-xs-4">
                        <dt>{{campaign.conversions | shortNumber}}</dt>
                        <dd>{{'campaign.list.conversions' | resolve}}</dd>
                    </dl>
                    <dl class="col-xs-4">
                        <dt>{{campaign.conversionRate * 100 | number : 2}} %</dt>
                        <dd>{{'campaign.conversionrate' | resolve}}</dd>
                    </dl>
                </div>
            
                <div class="info-text-container col-md-4 col-xs-10" data-ng-if="campaign.status == 'Scheduled'">
                    <h4 class="col-xs-12">{{'campaigns.campaign.scheduled.info' | resolve | stringFormat:model.daysToRun(campaign.openFrom)}}</h4>
                </div>
            
                <div class="info-text-container col-md-4 col-xs-10" data-ng-if="campaign.status == 'Draft'">
                    <h4 class="col-xs-12">{{'campaigns.campaign.draft.info' | resolve}}</h4>
                </div>

                <div class="icons-container col-md-2 col-xs-2 pull-right">
                    <a  href="{{campaign.detailLink}}" 
                        title="{{'campaigns.list.campaign.edit' | resolve}}" 
                        target="_top" class="icon-only btn-icon btn">
                            <span><i aria-hidden="true" class="icon-edit" class="sr-only"></i></span>
                    </a>
                    <button 
                        type="button" 
                        data-ng-click="model.deleteCampaignClick()" 
                        title="{{'campaigns.list.campaign.delete' | resolve }}"
                        value="" class="aspNetDisabled btn-unigrid-action icon-only btn-icon btn">
                            <span><i aria-hidden="true" class="icon-bin" class="sr-only"></i></span>
                    </button>                       
                </div>    
            </div>
                </script>
            </div>
        </div>
    </div>
</asp:Content>
