using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.CustomTables;
using CMS.DataEngine;

namespace CCC.Helpers
{
    public static partial class DatabaseHelpers
    {
        #region Properties
        public static string OrderByCmsTree
        {
            get
            {
                return "NodeLevel, NodeOrder, NodeName";
            }
        }
        #endregion

        public static partial class CustomTables
        {
            #region Methods
            /// <summary>
            /// Creates a custom table record and adds it to the Kentico database
            /// </summary>
            /// <param name="customTableClassName">Custom Table class name</param>
            /// <param name="values">Values of the new record</param>
            public static void AddRecord(string customTableClassName, Dictionary<string, object> values)
            {
                // Gets the custom table
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    // Creates a new custom table item 
                    CustomTableItem newCustomTableItem = CustomTableItem.New(customTableClassName);

                    // Sets the values for the fields of the custom table
                    foreach (var value in values)
                    {
                        newCustomTableItem.SetValue(value.Key, value.Value);
                    }

                    // Save the new custom table record into the database
                    newCustomTableItem.Insert();
                }
            }
            #endregion
        }
    }
}