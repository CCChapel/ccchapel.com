using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CMS.ApplicationDashboard;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.OnlineForms;

[assembly: RegisterLiveTileModelProvider(ModuleName.BIZFORM, "Form", typeof(FormsLiveTileModelProvider))]

namespace CMS.OnlineForms
{
    /// <summary>
    /// Provides live model for the Forms dashboard tile.
    /// </summary>
    internal class FormsLiveTileModelProvider : ILiveTileModelProvider
    {
        /// <summary>
        /// Loads model for the dashboard live tile.
        /// </summary>
        /// <param name="liveTileContext">Context of the live tile. Contains information about the user and the site the model is requested for</param>
        /// <exception cref="ArgumentNullException"><paramref name="liveTileContext"/> is null</exception>
        /// <returns>Live tile model</returns>
        public LiveTileModel GetModel(LiveTileContext liveTileContext)
        {
            if (liveTileContext == null)
            {
                throw new ArgumentNullException("liveTileContext");
            }

            return CacheHelper.Cache(() =>
            {
                // If site has more than 1 form on lower license, BizFormItemProvider.GetItems method returns null which results in exception
                // Standard license check
                if (!BizFormItemProvider.LicenseVersionCheck(RequestContext.CurrentDomain, ObjectActionEnum.Edit))
                {
                    return null;
                }

                var formsClassNames = BizFormInfoProvider.GetBizForms()
                                                         .OnSite(liveTileContext.SiteInfo.SiteID)
                                                         .Source(s => s.Join<DataClassInfo>("FormClassID", "ClassID"))
                                                         .Column("ClassName")
                                                         .GetListResult<string>();

                // Leave tile empty if there are no forms on current site
                if (!formsClassNames.Any())
                {
                    return null;
                }

                int newSubmissionsCount = formsClassNames.Sum(className => BizFormItemProvider.GetItems(className)
                                                                                              .WhereGreaterThan("FormInserted", DateTime.Now.AddDays(-7).Date)
                                                                                              .Count);

                return new LiveTileModel
                {
                    Value = newSubmissionsCount,
                    Description = ResHelper.GetString("bizform.livetiledescription"),
                };
            }
            , new CacheSettings(5, "FormsLiveTileModelProvider", liveTileContext.SiteInfo.SiteID));
        }
    }
}
