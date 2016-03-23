using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using CMS.DataCom;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.UIControls;

/// <summary>
/// A control that displays Data.com contact mapping details.
/// </summary>
public partial class CMSModules_ContactManagement_Controls_UI_DataCom_ContactMapping : CMSAdminControl
{
    #region "Variables"

    /// <summary>
    /// The Data.com contact mapping to display.
    /// </summary>
    private EntityMapping mEntityMapping;


    /// <summary>
    /// Indicates whether the control is enabled.
    /// </summary>
    private bool mEnabled = true;

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets or sets the Data.com contact mapping to display.
    /// </summary>
    public EntityMapping Mapping
    {
        get
        {
            return mEntityMapping;
        }
        set
        {
            mEntityMapping = value;
        }
    }


    /// <summary>
    /// Gets or sets the value indicating whether the control is enabled.
    /// </summary>
    public bool Enabled
    {
        get
        {
            return mEnabled;
        }
        set
        {
            mEnabled = value;
        }
    }

    #endregion


    #region "Life cycle methods"

    protected override void OnPreRender(EventArgs e)
    {
        if (Mapping != null)
        {
            try
            {
                FormInfo formInfo = DataComHelper.GetContactFormInfo();
                EntityInfo entityInfo = DataComHelper.GetContactEntityInfo();
                Panel mappingPanel = CreateMappingPanel(formInfo, entityInfo, Mapping);
                Controls.Add(mappingPanel);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        base.OnPreRender(e);
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Creates and initializes a new instance of the Panel class with the specified Data.com contact mapping, and returns it.
    /// </summary>
    /// <param name="formInfo">The CMS contact form info.</param>
    /// <param name="entityInfo">The Data.com contact entity info.</param>
    /// <param name="mapping">The Data.com contact mapping.</param>
    /// <returns>A new instance of the Panel class initialized with the specified Data.com contact mapping.</returns>
    private Panel CreateMappingPanel(FormInfo formInfo, EntityInfo entityInfo, EntityMapping mapping)
    {
        Panel mappingPanel = new Panel { CssClass = "mapping"};
        HtmlTable mappingTable = new HtmlTable();
        
        mappingTable.Controls.Add(CreateHeaderPanel());
        mappingPanel.Controls.Add(mappingTable);

        foreach (IDataDefinitionItem formItem in formInfo.ItemsList)
        {
            FormFieldInfo formField = formItem as FormFieldInfo;
            if (formField != null)
            {
                EntityMappingItem mappingItem = mapping.GetItem(formField.Name);
                if (mappingItem != null)
                {
                    EntityAttributeInfo entityAttribute = entityInfo.GetAttributeInfo(mappingItem.EntityAttributeName);
                    if (entityAttribute != null)
                    {
                        HtmlTableRow row = new HtmlTableRow();
                        mappingTable.Controls.Add(row);
                        
                        HtmlTableCell formFieldCell = new HtmlTableCell();
                        formFieldCell.Controls.Add(new Literal { Text = ResHelper.LocalizeString(formField.GetDisplayName(MacroContext.CurrentResolver)) });

                        HtmlTableCell entityAttributeCell = new HtmlTableCell();
                        entityAttributeCell.Controls.Add(new Literal { Text = ResHelper.LocalizeString(entityAttribute.DisplayName) });

                        row.Controls.Add(formFieldCell);
                        row.Controls.Add(entityAttributeCell);
                    }
                }
            }
        }

        return mappingPanel;
    }


    /// <summary>
    /// Creates a header for the mapping with Data.com contact mapping details, and returns it.
    /// </summary>
    /// <returns>A header for the mapping with Data.com contact mapping details.</returns>
    private HtmlTableRow CreateHeaderPanel()
    {
        HtmlTableRow row = new HtmlTableRow();
        
        HtmlTableCell sourceCell = new HtmlTableCell();
        sourceCell.Controls.Add(new Literal { Text = "<strong>" + GetString("datacom.kenticocms") + "</strong>" });

        HtmlTableCell destinationCell = new HtmlTableCell();
        destinationCell.Controls.Add(new Literal { Text = "<strong>" + GetString("datacom.datacom") + "</strong>" });

        row.Controls.Add(sourceCell);
        row.Controls.Add(destinationCell);

        return row;
    }


    /// <summary>
    /// Displays an error message and logs the specified exception to the event log.
    /// </summary>
    /// <param name="exception">The exception to handle.</param>
    private void HandleException(Exception exception)
    {
        ErrorSummary.Report(exception);
        EventLogProvider.LogException("Data.com Connector", "ContactMappingControl", exception);
    }

    #endregion
}