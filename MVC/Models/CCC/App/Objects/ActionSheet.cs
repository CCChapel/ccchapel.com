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