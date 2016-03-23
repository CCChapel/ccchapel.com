using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type Notification.
    /// </summary>
    public partial class Notification : TreeNode
    {
        public static IQueryable<Notification> CurrentNotifications
        {
            get
            {
                return (from n in NotificationProvider.GetNotifications().Published()
                        where DocumentHelpers.ResolveMacroCondition(n.MacroCondition)
                        select n);
            }
        }
    }
}