using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.PortalEngine.Internal;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.WebAnalytics;

[EditedObject(CampaignInfo.OBJECT_TYPE, "campaignId")]

[UIElement("CMS.WebAnalytics", "Campaign.Reports")]
public partial class CMSModules_WebAnalytics_Pages_Tools_Campaign_Tab_Reports : CMSCampaignPage
{
    private CampaignInfo mCampaignInfo;


    protected void Page_Load(object sender, EventArgs e)
    {
        mCampaignInfo = EditedObject as CampaignInfo;
        if (mCampaignInfo == null)
        {
            return;
        }

        // Validate SiteID for non administrators
        if (!MembershipContext.AuthenticatedUser.IsGlobalAdministrator)
        {
            if (mCampaignInfo.CampaignSiteID != SiteContext.CurrentSiteID)
            {
                RedirectToAccessDenied(GetString("cmsmessages.accessdenied"));
            }
        }

        FillDataForBasicCampaignInfo();
        InitializeControls();
    }


    private void FillDataForBasicCampaignInfo()
    {
        lblInfoNameValue.Text = HTMLHelper.HTMLEncode(mCampaignInfo.CampaignDisplayName);
        lblInfoFromValue.Text = (mCampaignInfo.CampaignOpenFrom == DateTime.MinValue) ? GetString("campaign.basicinfo.notset") : mCampaignInfo.CampaignOpenFrom.ToLongDateString();
        lblInfoToValue.Text = (mCampaignInfo.CampaignOpenTo == DateTime.MinValue) ? GetString("campaign.basicinfo.notset") : mCampaignInfo.CampaignOpenTo.ToLongDateString();
        if (!string.IsNullOrEmpty(mCampaignInfo.CampaignDescription))
        {
            lblInfoDescriptionValue.Text = HTMLHelper.HTMLEncode(mCampaignInfo.CampaignDescription);
        }
        else
        {
            lblInfoDescription.Visible = false;
            lblInfoDescriptionValue.Visible = false;
        }
    }


    /// <summary>
    /// Creates the channel conversions table and conversion funnel.
    /// </summary>
    private void InitializeControls()
    {
        // Get raw data from database
        var conversionSources = ConversionCampaignSourceInfoProvider.GetConversionCampaignSources(mCampaignInfo.CampaignID).ToList();

        if (!conversionSources.Any())
        {
            pnlCampaignColumnChart.Visible = false;
            gridChannels.Visible = false;
            lblNoData.Visible = true;
            return;
        }

        // Process data into more suitable format
        var conversionSourcesById = conversionSources
                          .GroupBy(key => key.ConversionID, (key, group) =>
                          {
                              var groupList = group.ToList();
                              var conversion = ConversionInfoProvider.GetConversionInfo(key);
                              return new
                              {
                                  DisplayName = conversion.ConversionDisplayName,
                                  CodeName = conversion.ConversionName,
                                  Items = groupList,
                                  Sum = groupList.Sum(x => x.ConversionHits)
                              };
                          })
                          .OrderByDescending(x => x.Sum);

        // Initialize chart
        double onePercentOfFirstItemSum = conversionSourcesById.First().Sum / 100.0;
        var chartData = conversionSourcesById.Select(x => new
        {
            title = x.DisplayName,
            value = x.Sum,
            percent = onePercentOfFirstItemSum > 0 ? Decimal.Round(x.Sum / (decimal)onePercentOfFirstItemSum) : 0,
            formattedValue = x.Sum.ToString("N0"),
        });

        ScriptHelper.RegisterModule(pnlCampaignColumnChart, "CMS.Charts/CampaignColumnChart", new
        {
            chartDiv = pnlCampaignColumnChart.ClientID,
            legendDiv = "legend",
            data = chartData,
            maxValue = conversionSourcesById.First().Sum,
        });

        // Create data table
        var table = new DataTable();
        table.Columns.Add("Channel", typeof(string));
        table.Columns.Add("EmailReports", typeof(string));

        // Define first header column
        AddColumnToGridView(gridChannels, "Channel", String.Empty);

        // Define column for email report links
        AddColumnToGridView(gridChannels, "EmailReports", GetString("campaign.report.emailreports"));

        // Get channels
        var channels = conversionSources.GroupBy(x => x.SourceName).OrderBy(x => x.Key);

        // Create table header
        foreach (var conversion in conversionSourcesById)
        {
            AddColumnToGridView(gridChannels, conversion.CodeName, conversion.DisplayName);
            table.Columns.Add(conversion.CodeName, typeof(string));
        }

        // Create table rows
        foreach (var channel in channels)
        {
            var dr = table.NewRow();
            dr["Channel"] = channel.Key;

            if (ModuleEntryManager.IsModuleLoaded(ModuleName.NEWSLETTER))
            {
                var issues =
                    new ObjectQuery(PredefinedObjectType.NEWSLETTERISSUE).WhereEquals("IssueUTMCampaign",
                        mCampaignInfo.CampaignUTMCode).WhereEquals("IssueUTMSource", channel.Key);
                dr["EmailReports"] = GetEmailReportLinks(issues.ToList()).Join("</br></br>");
            }

            // Create other cells with conversion values
            foreach (var item in conversionSourcesById)
            {
                var conversion = item.Items.Where(x => x.SourceName == channel.Key).ToList();
                dr[item.CodeName] = conversion.Any() ? conversion.First().ConversionHits.ToString("N0") : "0";
            }

            table.Rows.Add(dr);
        }

        gridChannels.DataSource = table;
        gridChannels.DataBind();
    }


    /// <summary>
    /// Row data bound - gridChannels.
    /// </summary>
    protected void gridChannels_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = "<span class=\"control-label\">" + e.Row.Cells[0].Text + "</span>";
            e.Row.Cells[1].Text = HTMLHelper.HTMLDecode(e.Row.Cells[1].Text);
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                cell.Text = "<span class=\"control-label\">" + cell.Text + "</span>";
            }
        }
    }


    /// <summary>
    /// Gets email report links.
    /// </summary>
    /// <param name="issues">Newsletter issues</param>
    private IEnumerable<string> GetEmailReportLinks(IEnumerable<BaseInfo> issues)
    {
        var uiLinkProvider = Service.Entry<IUILinkProvider>();

        foreach (var issue in issues)
        {
            var url = URLHelper.GetAbsoluteUrl(uiLinkProvider.GetSingleObjectLink("CMS.Newsletter", "EditIssueProperties", new ObjectDetailLinkParameters
            {
                ObjectIdentifier = issue.Generalized.ObjectID,
                AllowNavigationToListing = true,
                TabName = "Newsletter.Issue.Reports.Overview"
            }));

            yield return "<a href=\"" + url + "\" target=\"_top\">" + HTMLHelper.HTMLEncode(issue.GetValue("IssueSubject").ToString()) + "</a>";
        }
    }


    /// <summary>
    /// Adds new column to specified GridView control.
    /// </summary>
    /// <param name="gv">GridView control.</param>
    /// <param name="dataField">Name of the data field.</param>
    /// <param name="headerText">Header text.</param>
    private void AddColumnToGridView(GridView gv, string dataField, string headerText)
    {
        // Define first header column
        BoundField boundfield = new BoundField
        {
            DataField = dataField,
            HeaderText = headerText
        };

        gv.Columns.Add(boundfield);
    }
}