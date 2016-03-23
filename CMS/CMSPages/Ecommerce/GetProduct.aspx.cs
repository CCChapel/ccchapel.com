using System;
using System.Globalization;

using CMS.Helpers;
using CMS.SiteProvider;
using CMS.DocumentEngine;
using CMS.UIControls;
using CMS.Ecommerce;

public partial class CMSPages_Ecommerce_GetProduct : CMSPage
{
    #region "Variables"

    protected Guid skuGuid = Guid.Empty;
    protected int productId = 0;
    protected SiteInfo currentSite = null;
    protected string url = "";

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        // Set correct culture
        SetLiveCulture();

        // Initialization
        productId = QueryHelper.GetInteger("productId", 0);
        skuGuid = QueryHelper.GetGuid("skuguid", Guid.Empty);
        currentSite = SiteContext.CurrentSite;

        var skuObj = SKUInfoProvider.GetSKUInfo(productId);

        if ((skuObj != null) && skuObj.IsProductVariant)
        {
            // Get parent product of variant
            var parent = skuObj.Parent as SKUInfo;

            if (parent != null)
            {
                productId = parent.SKUID;
                skuGuid = parent.SKUGUID;
            }
        }

        string where = null;
        if (productId > 0)
        {
            where = "NodeSKUID = " + productId;
        }
        else if (skuGuid != Guid.Empty)
        {
            where = "SKUGUID = '" + skuGuid + "'";
        }

        if ((where != null) && (currentSite != null))
        {
            var node = DocumentHelper.GetDocuments()
                         .Path("/", PathTypeEnum.Section)
                         .Culture(CultureInfo.CurrentCulture.Name)
                         .CombineWithDefaultCulture()
                         .Where(where)
                         .Published()
                         .FirstObject;

            if (node != null)
            {
                // Get specified product url 
                url = DocumentURLProvider.GetUrl(node);
            }
        }

        if ((url != "") && (currentSite != null))
        {
            // Redirect to specified product 
            URLHelper.Redirect(url);
        }
        else
        {
            // Display error message
            lblInfo.Visible = true;
            lblInfo.Text = GetString("GetProduct.NotFound");
        }
    }
}