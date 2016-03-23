using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.ExtendedControls;
using CMS.FormControls;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.WebFarmSync.Internal;
using CMS.WebFarmSync;

/// <summary>
/// Custom class registration.
/// </summary>
[assembly: RegisterCustomClass("WebFarmServerEditExtender", typeof(WebFarmServerEditExtender))]

/// <summary>
/// Web farm server UIForm extender. For new/edit forms.
/// </summary>
public class WebFarmServerEditExtender : ControlExtender<UIForm>
{
    /// <summary>
    /// OnInit event.
    /// </summary>
    public override void OnInit()
    {
        Control.OnAfterValidate += OnAfterValidate;
    }


    /// <summary>
    /// Handles after validation event of UIForm.
    /// </summary>
    protected void OnAfterValidate(object sender, EventArgs e)
    {
        // Perform additional validation if web farm server is enabled
        if (ValidationHelper.GetBoolean(Control.GetFieldValue("ServerEnabled"), false))
        {
            // Get the web farm server object
            var serverId = QueryHelper.GetInteger("objectid", 0);
            var webFarmServer = WebFarmServerInfoProvider.GetWebFarmServerInfo(serverId) ?? new WebFarmServerInfo();

            if (!webFarmServer.ServerEnabled && !WebFarmLicenseHelper.CanAddServer)
            {
                // Set validation message
                Control.ValidationErrorMessage = ResHelper.GetStringFormat("licenselimitation.infopagemessage", FeatureEnum.Webfarm.ToStringRepresentation());
                Control.StopProcessing = true;

                // Log "servers exceeded" event
                var message = ResHelper.GetString("licenselimitation.serversexceeded");
                EventLogProvider.LogEvent(EventType.WARNING, "WebFarms", LicenseHelper.LICENSE_LIMITATION_EVENTCODE, message, RequestContext.CurrentURL);
            }
        }
    }
}