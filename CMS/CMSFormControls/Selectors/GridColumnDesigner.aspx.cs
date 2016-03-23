using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;

using CMS.Base;
using CMS.DataEngine;
using CMS.ExtendedControls;
using CMS.Helpers;
using CMS.UIControls;

public partial class CMSFormControls_Selectors_GridColumnDesigner : DesignerPage
{
    #region "Variables"

    public string[,] tmpColumns;
    private string mSelColId;
    private string mColId;

    #endregion


    #region "Properties"

    /// <summary>
    /// Save count of loads page.
    /// </summary>
    public int FirtsLoad
    {
        get
        {
            return ValidationHelper.GetInteger(ViewState["FirstLoad"], 0);
        }
        set
        {
            ViewState["FirstLoad"] = value;
        }
    }


    /// <summary>
    /// Changed columns in viewstate string.
    /// </summary>
    public string ViewColumns
    {
        get
        {
            return ValidationHelper.GetString(ViewState["ViewColumns"], "");
        }
        set
        {
            ViewState["ViewColumns"] = value;
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Page load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(QueryHelper.GetString("queryname", string.Empty)) || QueryHelper.ValidateHash("hash"))
        {
            int mTypeOfInput = 1;

            // Handle to ListBox postback
            ItemSelection1.OnPostBack += ItemSelection1_OnPostBack;

            // Initialize resource strings
            PageTitle.TitleText = GetString("GridColumnDesigner.Title");
            ItemSelection1.LeftColumnLabel.Text = GetString("ItemSelection.Avaliable");
            ItemSelection1.RightColumnLabel.Text = GetString("ItemSelection.Displayed");
            mSelColId = QueryHelper.GetString("SelColId", "");
            mColId = QueryHelper.GetString("ColId", "");

            // Set-up ColumnListBox
            ItemSelection1.RightColumListBox.SelectionMode = ListSelectionMode.Single;
            ItemSelection1.RightColumListBox.AutoPostBack = true;

            // Get classnames or queryname from querystring
            if (!string.IsNullOrEmpty(Request.QueryString["classnames"]))
            {
                hdnClassNames.Value = Request.QueryString["classnames"];
                mTypeOfInput = 1;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["queryname"]))
            {
                hdnClassNames.Value = Request.QueryString["queryname"];
                mTypeOfInput = 2;
            }

            // Get data from viewstate
            tmpColumns = FromView(ViewColumns);

            // Load Columns names
            if ((!RequestHelper.IsPostBack()) || (FirtsLoad < 2))
            {
                FirtsLoad++;

                // Use dataClass or Query to get names of columns
                switch (mTypeOfInput)
                {
                    case 1:
                        LoadFromDataClass();
                        break;

                    case 2:
                        LoadFromQuery();
                        break;
                }

                // Read XML
                ReadXML(hdnSelectedColumns.Value);
                
                if (!string.IsNullOrEmpty(ViewColumns))
                {
                    radSelect.Checked = true;
                }

                // Move selected columns
                if (tmpColumns != null)
                {
                    for (int i = tmpColumns.GetLowerBound(0); i <= tmpColumns.GetUpperBound(0); i++)
                    {
                        ListItem LI = ItemSelection1.LeftColumListBox.Items.FindByText(tmpColumns[i, 1]);
                        if (LI != null)
                        {
                            ItemSelection1.RightColumListBox.Items.Add(LI);
                            ItemSelection1.LeftColumListBox.Items.Remove(LI);
                        }
                    }
                }

                // Reload page to get data from parent in javascript
                string script = String.Format(@"
document.getElementById('{0}').value = wopener.GetClassNames('{1}');
document.getElementById('{2}').value = wopener.GetSelectedColumns('{3}');
document.body.onload = function () {{ {4} }};
", hdnClassNames.ClientID, ScriptHelper.GetString(mColId, false), hdnSelectedColumns.ClientID, ScriptHelper.GetString(mSelColId, false), ControlsHelper.GetPostBackEventReference(ItemSelection1.RightColumListBox, ""));

                ScriptHelper.RegisterStartupScript(this, typeof(string), "LoadScript", script, true);
            }

            // Show or hide dialog
            ItemSelection1.Visible = !radGenerate.Checked;
            pnlProperties.Visible = !radGenerate.Checked;
        }
        else
        {
            URLHelper.Redirect(UIHelper.GetErrorPageUrl("dialogs.badhashtitle", "dialogs.badhashtext"));
        }
    }


    /// <summary>
    /// On click OK button save changes.
    /// </summary>
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (ItemSelection1.Visible)
        {
            ListItem mLI = ItemSelection1.RightColumListBox.SelectedItem;

            if (mLI != null)
            {
                SetInSelected(Convert.ToInt32(mLI.Value), mLI.Text, txtHeaderText.Text, chkDisplayAsLink.Checked.ToString());
                ShowChangesSaved();
            }
            else
            {
                ShowWarning(GetString("grid.noselection"));
            }
        }
    }


    /// <summary>
    /// On click to close button create XML and send to parent and close window.
    /// </summary>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        if (radSelect.Checked)
        {
            CreateXML();
        }
        else
        {
            hdnSelectedColumns.Value = "<columns></columns>";
        }

        hdnTextClassNames.Value = ConvertXML(hdnSelectedColumns.Value);

        string script = String.Format(@"
var columnsElem = document.getElementById('{0}'),
    classNamesElem = document.getElementById('{1}');
wopener.SetValue(columnsElem.value, classNamesElem.value,'{2}','{3}');
CloseDialog();
", hdnSelectedColumns.ClientID, hdnTextClassNames.ClientID, ScriptHelper.GetString(mSelColId, false), ScriptHelper.GetString(mColId, false));

        ScriptHelper.RegisterStartupScript(this, typeof (string), "CloseScript", script, true);
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Load Columns names into listbox.
    /// </summary>
    protected void LoadFromDataClass()
    {
        // Get selected classes from hiddenfield.
        ArrayList classesList = new ArrayList();
        ArrayList columnList = new ArrayList();
        if (!string.IsNullOrEmpty(hdnClassNames.Value))
        {
            classesList = new ArrayList(hdnClassNames.Value.Split(';'));
        }

        classesList.Add("CMS.Tree");
        classesList.Add("CMS.Document");

        // Fill columnList with column names from all classes.
        foreach (string className in classesList)
        {
            try
            {
                if (!String.IsNullOrEmpty(className))
                {
                    IDataClass dc = DataClassFactory.NewDataClass(className);
                    DataClassInfo dci = DataClassInfoProvider.GetDataClassInfo(className);
                    // Get columns only from couplet classes.
                    if (dci.ClassIsCoupledClass)
                    {
                        foreach (string columnName in dc.StructureInfo.ColumnNames)
                        {
                            columnList.Add(columnName);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        // Move columns from array list to string array and add indexes.
        string[,] columns = new string[columnList.Count, 2];
        int index = 0;
        foreach (string columnName in columnList)
        {
            columns[index, 0] = index.ToString();
            columns[index, 1] = columnName;
            index += 1;
        }

        ItemSelection1.LeftItems = columns;
        ItemSelection1.fill();
    }


    /// <summary>
    /// Load Columns names with Query.
    /// </summary>
    protected void LoadFromQuery()
    {
        string queryName = hdnClassNames.Value;
        var q = new DataQuery(queryName);
        if (q.HasResults())
        {
            DataView dv = q.Result.Tables[0].DefaultView;
            string[,] mLI = new string[dv.Table.Columns.Count, 2];

            int i = 0;
            foreach (DataColumn dc in dv.Table.Columns)
            {
                mLI[i, 0] = i.ToString();
                mLI[i, 1] = dc.ColumnName;
                i++;
            }

            ItemSelection1.LeftItems = mLI;
            ItemSelection1.fill();
        }
    }


    /// <summary>
    /// Handle for Right Column List Box.
    /// </summary>
    private void ItemSelection1_OnPostBack(object sender, EventArgs e)
    {
        if (ItemSelection1.RightColumListBox.SelectedItem != null)
        {
            int mIndex = SearchInSelected(Convert.ToInt32(ItemSelection1.RightColumListBox.SelectedItem.Value));

            if (mIndex >= 0)
            {
                // text
                txtHeaderText.Text = !string.IsNullOrEmpty(tmpColumns[mIndex, 2]) ? tmpColumns[mIndex, 2] : ItemSelection1.RightColumListBox.SelectedItem.Text;

                // checked
                if (!string.IsNullOrEmpty(tmpColumns[mIndex, 3]))
                {
                    if (tmpColumns[mIndex, 3] != null && tmpColumns[mIndex, 3] == "link")
                    {
                        chkDisplayAsLink.Checked = true;
                    }
                    else
                    {
                        chkDisplayAsLink.Checked = false;
                    }
                }
                else
                {
                    chkDisplayAsLink.Checked = false;
                }
            }
            else
            {
                txtHeaderText.Text = ItemSelection1.RightColumListBox.SelectedItem.Text;
                chkDisplayAsLink.Checked = false;
            }
        }
    }


    /// <summary>
    /// Search if selected item is in field.
    /// </summary>
    /// <param name="value">Value in Column list box</param>
    protected int SearchInSelected(int value)
    {
        if (tmpColumns == null)
        {
            return -1;
        }

        for (int i = tmpColumns.GetLowerBound(0); i <= tmpColumns.GetUpperBound(0); i++)
        {
            if ((!string.IsNullOrEmpty(tmpColumns[i, 0])) && (value == Convert.ToInt32(tmpColumns[i, 0])))
            {
                return i;
            }
        }

        return -1;
    }


    /// <summary>
    /// Sets attribute in selected item or insert new if attribute is not in.
    /// </summary>
    /// <param name="Value">Value in Column list box</param>
    /// <param name="Name">Name of column</param>
    /// <param name="HeaderText">Header text</param>
    /// <param name="Type">Type (link|bound))</param>
    protected void SetInSelected(int Value, string Name, string HeaderText, string Type)
    {
        if (SearchInSelected(Value) >= 0)
        {
            tmpColumns[SearchInSelected(Value), 2] = HeaderText;
            tmpColumns[SearchInSelected(Value), 3] = Convert.ToBoolean(Type) ? "link" : "bound";
        }
        else // Insert new
        {
            int to = (tmpColumns != null) ? (tmpColumns.GetUpperBound(0) + 1) : 0;

            string[,] mField = new string[to + 1, 4];

            for (int i = 0; i < to; i++)
            {
                mField[i, 0] = tmpColumns[i, 0];
                mField[i, 1] = tmpColumns[i, 1];
                mField[i, 2] = tmpColumns[i, 2];
                mField[i, 3] = tmpColumns[i, 3];
            }

            mField[to, 0] = Value.ToString();
            mField[to, 1] = Name;
            mField[to, 2] = HeaderText;
            mField[to, 3] = Convert.ToBoolean(Type) ? "link" : "bound";

            tmpColumns = mField;
        }

        ViewColumns = ToView(tmpColumns);
    }


    /// <summary>
    /// Read XML and set properties.
    /// </summary>
    /// <param name="mXML">XML document</param>
    protected void ReadXML(string mXML)
    {
        if (DataHelper.GetNotEmpty(mXML, "") != "")
        {
            XmlDocument mXMLDocument = new XmlDocument();

            mXMLDocument.LoadXml(mXML);

            XmlNodeList NodeList = mXMLDocument.DocumentElement.GetElementsByTagName("column");

            string[,] mAtributes = new string[NodeList.Count, 4];

            int i = 0;

            foreach (XmlNode node in NodeList)
            {
                string mValue = "";
                for (int j = ItemSelection1.LeftItems.GetLowerBound(0); j <= ItemSelection1.LeftItems.GetUpperBound(0); j++)
                {
                    if (ItemSelection1.LeftItems[j, 1] == XmlHelper.GetXmlAttributeValue(node.Attributes["name"], ""))
                    {
                        mValue = ItemSelection1.LeftItems[j, 0];
                    }
                }

                mAtributes[i, 0] = mValue;
                mAtributes[i, 1] = XmlHelper.GetXmlAttributeValue(node.Attributes["name"], "");
                mAtributes[i, 2] = XmlHelper.GetXmlAttributeValue(node.Attributes["header"], "");
                mAtributes[i, 3] = XmlHelper.GetXmlAttributeValue(node.Attributes["type"], "");
                i++;
            }

            tmpColumns = mAtributes;
            ViewColumns = ToView(tmpColumns);
        }
    }


    /// <summary>
    /// Return.
    /// </summary>
    /// <returns>Id</returns>
    protected string[,] SearchInChangedColumnsById(int mId)
    {
        string[,] mReturn = new string[1, 4];

        if (tmpColumns != null)
        {
            for (int i = tmpColumns.GetLowerBound(0); i <= tmpColumns.GetUpperBound(0); i++)
            {
                if (tmpColumns[i, 0] == mId.ToString())
                {
                    mReturn[0, 0] = tmpColumns[i, 0];
                    mReturn[0, 1] = tmpColumns[i, 1];
                    mReturn[0, 2] = tmpColumns[i, 2];
                    mReturn[0, 3] = tmpColumns[i, 3];
                    return mReturn;
                }
            }
        }

        return null;
    }


    /// <summary>
    /// Creates new XML document for columns.
    /// </summary>
    protected void CreateXML()
    {
        string[,] mChanged = null;

        XmlDocument xmldoc = new XmlDocument();
        XmlNode xmlRoot = xmldoc.CreateElement("columns");
        xmldoc.AppendChild(xmlRoot);

        for (int i = 0; i < ItemSelection1.RightColumListBox.Items.Count; i++)
        {
            bool mIsChanged = false;

            if (SearchInSelected(Convert.ToInt32(ItemSelection1.RightColumListBox.Items[i].Value)) >= 0)
            {
                mIsChanged = true;

                mChanged = SearchInChangedColumnsById(Convert.ToInt32(ItemSelection1.RightColumListBox.Items[i].Value));

                if (mChanged == null)
                {
                    mIsChanged = false;
                }
            }

            XmlElement xmlColumn = xmldoc.CreateElement("column");

            // Prepare attributes for "column" node
            var attributes = new Dictionary<string, string>
            {
                { "name", ItemSelection1.RightColumListBox.Items[i].Text },
                { "header", (mIsChanged ? mChanged[0, 2] : String.Empty) },
                { "type", ((mIsChanged) && (mChanged[0, 3] != null && mChanged[0, 3].EqualsCSafe("link", true)) ? "link" : "bound") }
            };

            xmlColumn.AddAttributes(attributes);

            xmlRoot.AppendChild(xmlColumn);
        }

        hdnSelectedColumns.Value = xmldoc.ToFormattedXmlString(true);
    }


    /// <summary>
    /// Convert XML to TextBox.
    /// </summary>
    /// <param name="mXML">XML document</param>
    protected static string ConvertXML(string mXML)
    {
        if (DataHelper.GetNotEmpty(mXML, "") == "")
        {
            return "";
        }
        string mToReturn = "";
        XmlDocument mXMLDocument = new XmlDocument();
        mXMLDocument.LoadXml(mXML);
        XmlNodeList NodeList = mXMLDocument.DocumentElement.GetElementsByTagName("column");
        int i = 0;

        foreach (XmlNode node in NodeList)
        {
            if (i > 0)
            {
                mToReturn += ";";
            }
            mToReturn += XmlHelper.GetXmlAttributeValue(node.Attributes["name"], "");
            i++;
        }

        return mToReturn;
    }


    /// <summary>
    /// Convert 2D string array to viewstate string.
    /// </summary>
    protected static string ToView(string[,] input)
    {
        string mToReturn = "";

        for (int i = 0; i <= input.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= input.GetUpperBound(1); j++)
            {
                mToReturn += input[i, j];
                mToReturn += "~";
            }

            mToReturn += ";";
        }

        return mToReturn;
    }


    /// <summary>
    /// Convert from Viewstate string to 2dimensional string array.
    /// </summary>
    protected static string[,] FromView(string input)
    {
        if (input == null)
        {
            return null;
        }

        string[] first = input.Split(';');

        string[,] mToReturn = new string[first.GetUpperBound(0), 4];

        for (int i = 0; i < first.GetUpperBound(0); i++)
        {
            string[] second = first[i].Split('~');

            mToReturn[i, 0] = second[0];
            mToReturn[i, 1] = second[1];
            mToReturn[i, 2] = second[2];
            mToReturn[i, 3] = second[3];
        }

        return mToReturn;
    }

    #endregion
}