using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CCC.Models.App.Objects
{
    public partial class ActionSheet
    {
        protected List<ActionSheetActions> _items = new List<ActionSheetActions>();
        public List<ActionSheetActions> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }

        public static ActionSheet Create(string plainTextBody, string url, string htmlBody)
        {
            ActionSheetActions defaultShare = new ActionSheetActions();
            defaultShare.Actions.Add(new Actions.ShareAction()
            {
                Handler = Actions.ShareAction.Handlers.DefaultShare,
                Body = plainTextBody,
                Url = url
            });

            ActionSheetActions htmlShare = new ActionSheetActions();
            htmlShare.Actions.Add(new Actions.ShareAction()
            {
                Handler = Actions.ShareAction.Handlers.HtmlShare,
                Body = string.Format("<html>{0}</html>", htmlBody)
            });

            ActionSheet actionSheet = new ActionSheet();
            actionSheet.Items.Add(defaultShare);
            actionSheet.Items.Add(htmlShare);

            return actionSheet;
        }
    }

    public partial class ActionSheetActions
    {
        protected List<Actions.ShareAction> _actions = new List<Actions.ShareAction>();
        public List<Actions.ShareAction> Actions
        {
            get
            {
                return _actions;
            }
            set
            {
                _actions = value;
            }
        }
    }
}