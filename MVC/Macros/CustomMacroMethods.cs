using System;
using System.Linq;

using MVC;

using CMS;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Base;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types;
using CMS.CustomTables;
using CMS.CustomTables.Types;

using CCC.Helpers;

/// <summary>
/// Example of custom module with custom macro methods registration.
/// </summary>
[assembly: RegisterExtension(typeof(CustomMacroMethods), typeof(SystemNamespace))]
public class CustomMacroMethods : MacroMethodContainer
{
    #region "Macro methods implementation"

    ///// <summary>
    ///// Concatenates the given string with " default" string.
    ///// </summary>
    ///// <param name="param1">String to be concatenated with " default"</param>
    //public static string MyMethod(string param1)
    //{
    //    return MyMethod(param1, "default");
    //}


    ///// <summary>
    ///// Concatenates two strings.
    ///// </summary>
    ///// <param name="param1">First string to concatenate</param>
    ///// <param name="param2">Second string to concatenate</param>
    //public static string MyMethod(string param1, string param2)
    //{
    //    return param1 + " " + param2;
    //}


    // Add your own custom methods here
    public static string CurrentCampus()
    {
        return CCC.Helpers.MiscellaneousHelpers.CurrentCampusName;
    }

    /// <summary>
    /// Returns the latest sermon without regards to campus
    /// </summary>
    /// <returns>The latest sermon across all campuses</returns>
    public static Sermon LatestSermon()
    {
        return Sermon.Latest;
    }
    /// <summary>
    /// Gets the latest Sermon for the specified campus
    /// </summary>
    /// <param name="campusName">Name of Campus</param>
    /// <returns>Latest Sermon for specified Campus</returns>
    public static Sermon LatestSermon(string campusName)
    {
        return Sermon.LatestForCampus(campusName);
    }

    /// <summary>
    /// Get the Speaker based on ID
    /// </summary>
    /// <param name="speakerID">ItemID of speaker</param>
    /// <returns>Speaker with specified ID</returns>
    public static SpeakersItem GetSpeaker(int speakerID)
    {
        return CustomTableItemProvider.GetItem<SpeakersItem>(speakerID);
    }

    /// <summary>
    /// Gets the Series for the specified sermon.
    /// </summary>
    /// <param name="sermon">Sermon to get Series for</param>
    /// <returns>Series the specified Sermon belongs to</returns>
    public static Series GetSeries(Sermon sermon)
    {
        return sermon.MessageSeries;
    }
    /// <summary>
    /// Gets the Series with the specified NodeGUID.
    /// </summary>
    /// <param name="nodeGuid">The value of the Node GUID for the Series</param>
    /// <returns>The Series with the specified Node GUID</returns>
    public static Series GetSeries(Guid nodeGuid)
    {
        return SeriesProvider.GetSeries(nodeGuid, SiteHelpers.SiteCulture, SiteHelpers.SiteName);
    }

    public static Guid LatestSeriesBackground()
    {
        return Series.Latest.Background.NodeGUID;
    }

    public static string GetSeriesTitleTreatment(Sermon sermon)
    {
        return string.Format("/getattachment/{0}/image.jpg",
            sermon.MessageSeries.SeriesTitleTreatment.ToString());
    }

    public static string GetSeriesLink(Series series)
    {
        return series.RouteUrl;
    }

    public static string GetSermonLink(Sermon sermon)
    {
        return sermon.RouteUrl;
    }

    /// <summary>
    /// Gets the FormStack form embed markup
    /// </summary>
    /// <param name="formUrl">The value of the URL set in FormStack at Settings > General.</param>
    /// <returns>Embed markup for the form</returns>
    public static string FormStackForm(string formUrl)
    {
        return new FormStack.FormStackForm(formUrl).FormHtml;
    }
    
    /// <summary>
    /// Creates the opening of an anchor element
    /// </summary>
    /// <param name="href">href value for the link</param>
    /// <returns>Opening element to link</returns>
    public static string BeginLink(string href)
    {
        return string.Format("<a href=\"{0}\">", href);
    }
    public static string BeginLink(string href, string classes)
    {
        return string.Format("<a class=\"{0}\" href=\"{1}\">", classes, href);
    }
    public static string BeginLink(string href, string classes, string dataCampus)
    {
        return string.Format("<a class=\"{0}\" href=\"{1}\" data-campus=\"{2}\">", 
            classes, href, dataCampus);
    }

    /// <summary>
    /// Creates a closing anchor element
    /// </summary>
    /// <returns>A closing anchor element</returns>
    public static string EndLink()
    {
        return "</a>";
    }

    //public static string GetPageUrl(string linkText, string classes = "")
    //{
    //    return string.Format("<a href=\"{0}\" class=\"{1}\">{2}</a>", )
    //}

    /// <summary>
    /// Gets the Class Name of the Page
    /// </summary>
    /// <param name="NodeGUID">The Node GUID of the Page</param>
    /// <returns>The Class Name of the Page</returns>
    public static string PageClassName(Guid NodeGUID)
    {
        return (from i in DocumentHelper.GetDocuments().Published()
                where i.NodeGUID == NodeGUID
                select i).FirstOrDefault().ClassName;
    }

    public static string SearchQuickAccessKeywords()
    {
        var keywords = (from q in QuickAccessKeywordsProvider.GetQuickAccessKeywords().Published()
                        where DocumentHelpers.ResolveMacroCondition(q.MacroCondition)
                        select q);

        return keywords.ToHtmlString();
    }

    public static DocumentQuery<StaffTeamMember> StaffTeamMembers(string staffTeamName)
    {
        //Get the team
        var teams = (from t in StaffTeamProvider.GetStaffTeams().Published()
                    where t.TeamName.ToLower() == staffTeamName.ToLower()
                    select t);

        if (teams.Any())
        {
            return teams.FirstOrDefault().TeamMembers;
        }

        throw new NullReferenceException();
    }

    public static Person GetPerson(int nodeID)
    {
        return PersonProvider.GetPerson(nodeID, SiteHelpers.SiteCulture, SiteHelpers.SiteName);
    }

    public static string SeriesImageSizingClass(Series series)
    {
        return series.Fields.TitleTreatment.ImageSizingClass();
    }

    public static string CurrentRoute()
    {
        return UrlHelpers.CurrentRoute;
    }
    #endregion


    #region "MacroResolver wrapper methods"

    ///// <summary>
    ///// Wrapper method of MyMethod suitable for MacroResolver.
    ///// </summary>
    ///// <param name="context">Evaluation context with child resolver</param>
    ///// <param name="parameters">Parameters of the method</param>
    //[MacroMethod(typeof(string), "Returns concatenation of two strings.", 1)]
    //[MacroMethodParam(0, "param1", typeof(string), "First string to concatenate.")]
    //[MacroMethodParam(1, "param2", typeof(string), "Second string to concatenate.")]
    //public static object MyMethod(EvaluationContext context, params object[] parameters)
    //{
    //    switch (parameters.Length)
    //    {
    //        case 1:
    //            // Overload with one parameter
    //            return MyMethod(ValidationHelper.GetString(parameters[0], ""));

    //        case 2:
    //            // Overload with two parameters
    //            return MyMethod(ValidationHelper.GetString(parameters[0], ""), ValidationHelper.GetString(parameters[1], ""));

    //        default:
    //            // No other overload is supported
    //            throw new NotSupportedException();
    //    }
    //}


    ///// <summary>
    ///// Compares two strings according to resolver IsCaseSensitiveComparison setting.
    ///// </summary>
    ///// <param name="context">Evaluation context with child resolver</param>
    ///// <param name="parameters">Parameters of the method</param>
    //[MacroMethod(typeof(string), "Compares two strings according to resolver IsCaseSensitiveComparison setting.", 2)]
    //[MacroMethodParam(0, "param1", typeof(string), "First string to compare.")]
    //[MacroMethodParam(1, "param2", typeof(string), "Second string to compare.")]
    //public static object MyComparisonMethod(EvaluationContext context, params object[] parameters)
    //{
    //    switch (parameters.Length)
    //    {
    //        case 2:
    //            // Overload with two parameters
    //            return ValidationHelper.GetString(parameters[0], "").EqualsCSafe(ValidationHelper.GetString(parameters[1], ""), !context.CaseSensitive);

    //        default:
    //            // No other overload is supported
    //            throw new NotSupportedException();
    //    }
    //}

    [MacroMethod(typeof(string), "Returns the current campus.", 0)]
    public static object CurrentCampus(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 0:
                return CurrentCampus();

            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(Sermon), "Returns the latest sermon.", 0)]
    public static object LatestSermon(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 1:
                return LatestSermon(ValidationHelper.GetString(parameters[0], "Hudson"));
            case 0:
                return LatestSermon();
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(SpeakersItem), "Returns the speaker with specified ID.", 1)]
    public static object GetSpeaker(EvaluationContext context, params object[] parameters)
    {
        switch(parameters.Length)
        {
            case 1:
                return GetSpeaker(ValidationHelper.GetInteger(parameters[0], 0));
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(Series), "Returns the Series of the specified Sermon.", 1)]
    public static object GetSeriesFromSermon(EvaluationContext context, params object[] parameters)
    {
        switch(parameters.Length)
        {
            case 1:
                return GetSeries((Sermon)parameters[0]);
            default:
                throw new NotSupportedException();
        }
    }
    [MacroMethod(typeof(Series), "Returns the Series with the specified Node GUID.", 1)]
    public static object GetSeriesFromGuid(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 1:
                return GetSeries(ValidationHelper.GetGuid(parameters[0], new Guid()));
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Returns the embed markup for the FormStack form", 1)]
    public static object FormStackForm(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 1:
                return FormStackForm(ValidationHelper.GetString(parameters[0], null));
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Creates a beginning link tag.", 1)]
    public static object BeginLink(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 3:
                return BeginLink(
                    ValidationHelper.GetString(parameters[0], null),
                    ValidationHelper.GetString(parameters[1], null),
                    ValidationHelper.GetString(parameters[2], null));
            case 2:
                return BeginLink(
                    ValidationHelper.GetString(parameters[0], null),
                    ValidationHelper.GetString(parameters[1], null));
            case 1:
                return BeginLink(ValidationHelper.GetString(parameters[0], null));
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Inserts a closing anchor tag", 0)]
    public static object EndLink(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 0:
                return EndLink();
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Gets the ClassName of the Page", 1)]
    public static object PageClassName(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 1:
                return PageClassName(ValidationHelper.GetGuid(parameters[0], new Guid()));
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Gets the title treatment of the given series.", 1)]
    public static object GetSeriesTitleTreatment(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 1:
                return GetSeriesTitleTreatment((Sermon)parameters[0]);
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Gets the current Quick Access Keywords for search.", 0)]
    public static object SearchQuickAccessKeywords(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 0:
                return SearchQuickAccessKeywords();
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Gets a link to the series page.", 1)]
    public static object GetSeriesLink(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 1:
                return GetSeriesLink((Series)parameters[0]);
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Gets a link to the sermon page.", 1)]
    public static object GetSermonLink(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 1:
                return GetSermonLink((Sermon)parameters[0]);
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Gets the GUID representing the latest series graphics.", 0)]
    public static object LatestSeriesBackground(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 0:
                return LatestSeriesBackground();
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Gets an enumerable object of team members for the specified team.", 1)]
    public static object StaffTeamMembers(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 1:
                return StaffTeamMembers(ValidationHelper.GetString(parameters[0], string.Empty));
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Gets the Person object by Node ID.", 1)]
    public static object GetPerson(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 1:
                return GetPerson(ValidationHelper.GetInteger(parameters[0], 0));
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(Attachment), "Gets the Image Sizing Class property from the attachment.", 1)]
    public static object SeriesImageSizingClass(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 1:
                return SeriesImageSizingClass((Series)parameters[0]);
            default:
                throw new NotSupportedException();
        }
    }

    [MacroMethod(typeof(string), "Gets the current route.", 0)]
    public static object CurrentRoute(EvaluationContext context, params object[] parameters)
    {
        switch (parameters.Length)
        {
            case 0:
                return CurrentRoute();
            default:
                throw new NotSupportedException();
        }
    }
    #endregion
}