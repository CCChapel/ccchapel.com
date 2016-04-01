using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type Person.
    /// </summary>
    public partial class Person : TreeNode
    {
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}