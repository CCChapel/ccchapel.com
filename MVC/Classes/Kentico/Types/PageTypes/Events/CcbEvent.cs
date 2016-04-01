using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

using ChurchCommunityBuilder.Api;
using ChurchCommunityBuilder.Api.Events.Entity;

//using CCB;

using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type CcbEvent.
    /// </summary>
    public partial class CcbEvent : TreeNode
    {
        #region Properties
        public static string KenticoPath
        {
            get
            {
                return "/Events";
            }
        }

        public Event CcbEventData
        {
            get
            {
                return CCB.Event.GetEvent(EventCcbID);
            }
        }

        public bool IsEventDataInCache
        {
            get
            {
                return CCB.Event.IsEventInCache(EventCcbID);
            }
        }

        public DateTime StartDateTime
        {
            get
            {
                return CcbEventData.StartDateTime ?? default(DateTime);
            }
        }

        public DateTime EndDateTime
        {
            get
            {
                return CcbEventData.EndDateTime ?? default(DateTime);
            }
        }

        public string Description
        {
            get
            {
                return DocumentHelpers.LineBreaksToParagraphs(CcbEventData.Description);
            }
        }

        /// <summary>
        /// Route Values for the Event
        /// </summary>
        public object RouteValues
        {
            get
            {
                return new
                {
                    controller = "Event",
                    action = "Index",
                    year = StartDateTime.Year.ToString("0000"),
                    month  = StartDateTime.Month.ToString("00"),
                    day = StartDateTime.Day.ToString("00"),
                    eventName = NodeAlias
                };
            }
        }

        /// <summary>
        /// Get a URL to the Page
        /// </summary>
        public string RouteUrl
        {
            get
            {
                return UrlHelpers.UrlHelper.RouteUrl(RouteValues);
            }
        }

        public bool HasRegistration
        {
            get
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(Form.Url))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }

            }
        }

        public CCB.Form Form
        {
            get
            {
                try
                {
                    if (CcbEventData.Registration.Forms.Any())
                    {
                        //Get First Form
                        RegistrationForm form = CcbEventData.Registration.Forms.First();

                        return new CCB.Form(form);
                    }
                }
                catch
                {
                }

                return new CCB.Form();
            }
        }
        
        public Attachment GraphicAttachment
        {
            get
            {
                Guid guid = new Guid(Fields.EventGraphic);

                Background image = BackgroundProvider.GetBackground(guid, SiteHelpers.SiteCulture, SiteHelpers.SiteName);

                return image.Fields.ImageFile;
            }
        }
        #endregion
    }

    public static partial class Extensions
    {
        public static string ToAddressString(this Location input)
        {
            return string.Format("{0}, {1}, {2} {3}",
                input.StreetAddress,
                input.City,
                input.State,
                input.Zip);
        }
    }
}