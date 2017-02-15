﻿//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at http://docs.kentico.com.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using CMS;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine.Types;
using CMS.DocumentEngine;

[assembly: RegisterDocumentType(CcbEvent.CLASS_NAME, typeof(CcbEvent))]

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type CcbEvent.
    /// </summary>
    public partial class CcbEvent : TreeNode
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "events.ccbEvent";


        /// <summary>
        /// The instance of the class that provides extended API for working with CcbEvent fields.
        /// </summary>
        private readonly CcbEventFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// CcbEventID.
        /// </summary>
        [DatabaseIDField]
        public int CcbEventID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("CcbEventID"), 0);
            }
            set
            {
                SetValue("CcbEventID", value);
            }
        }


        /// <summary>
        /// Name.
        /// </summary>
        [DatabaseField]
        public string EventName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("EventName"), "");
            }
            set
            {
                SetValue("EventName", value);
            }
        }


        /// <summary>
        /// Find the ID of the event in CCB by going to its page and looking at the URL for the "id" parameter.
        /// </summary>
        [DatabaseField]
        public int EventCcbID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("EventCcbID"), 0);
            }
            set
            {
                SetValue("EventCcbID", value);
            }
        }


        /// <summary>
        /// Graphic.
        /// </summary>
        [DatabaseField]
        public string EventGraphic
        {
            get
            {
                return ValidationHelper.GetString(GetValue("EventGraphic"), "ac2dc13f-0412-4d10-9e62-54708c0aec52");
            }
            set
            {
                SetValue("EventGraphic", value);
            }
        }


        /// <summary>
        /// Location Name.
        /// </summary>
        [DatabaseField]
        public string EventLocationName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("EventLocationName"), "");
            }
            set
            {
                SetValue("EventLocationName", value);
            }
        }


        /// <summary>
        /// Date Description.
        /// </summary>
        [DatabaseField]
        public string EventDateDescription
        {
            get
            {
                return ValidationHelper.GetString(GetValue("EventDateDescription"), "");
            }
            set
            {
                SetValue("EventDateDescription", value);
            }
        }


        /// <summary>
        /// Override Event Registration?.
        /// </summary>
        [DatabaseField]
        public bool EventRegistrationOverride
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("EventRegistrationOverride"), false);
            }
            set
            {
                SetValue("EventRegistrationOverride", value);
            }
        }


        /// <summary>
        /// Registration Form.
        /// </summary>
        [DatabaseField]
        public string EventRegistrationForm
        {
            get
            {
                return ValidationHelper.GetString(GetValue("EventRegistrationForm"), "");
            }
            set
            {
                SetValue("EventRegistrationForm", value);
            }
        }


        /// <summary>
        /// Registration End Date.
        /// </summary>
        [DatabaseField]
        public DateTime EventRegistrationEndDate
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("EventRegistrationEndDate"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("EventRegistrationEndDate", value);
            }
        }


        /// <summary>
        /// CSS Class.
        /// </summary>
        [DatabaseField]
        public string CssClass
        {
            get
            {
                return ValidationHelper.GetString(GetValue("CssClass"), "");
            }
            set
            {
                SetValue("CssClass", value);
            }
        }


        /// <summary>
        /// Styles.
        /// </summary>
        [DatabaseField]
        public string Styles
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Styles"), "");
            }
            set
            {
                SetValue("Styles", value);
            }
        }


        /// <summary>
        /// Macro Condition.
        /// </summary>
        [DatabaseField]
        public string MacroCondition
        {
            get
            {
                return ValidationHelper.GetString(GetValue("MacroCondition"), "");
            }
            set
            {
                SetValue("MacroCondition", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with CcbEvent fields.
        /// </summary>
        public CcbEventFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with CcbEvent fields.
        /// </summary>
        public partial class CcbEventFields
        {
            /// <summary>
            /// The content item of type CcbEvent that is a target of the extended API.
            /// </summary>
            private readonly CcbEvent mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="CcbEventFields" /> class with the specified content item of type CcbEvent.
            /// </summary>
            /// <param name="instance">The content item of type CcbEvent that is a target of the extended API.</param>
            public CcbEventFields(CcbEvent instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// CcbEventID.
            /// </summary>
            public int ID
            {
                get
                {
                    return mInstance.CcbEventID;
                }
                set
                {
                    mInstance.CcbEventID = value;
                }
            }


            /// <summary>
            /// Name.
            /// </summary>
            public string EventName
            {
                get
                {
                    return mInstance.EventName;
                }
                set
                {
                    mInstance.EventName = value;
                }
            }


            /// <summary>
            /// Find the ID of the event in CCB by going to its page and looking at the URL for the "id" parameter.
            /// </summary>
            public int EventCcbID
            {
                get
                {
                    return mInstance.EventCcbID;
                }
                set
                {
                    mInstance.EventCcbID = value;
                }
            }


            /// <summary>
            /// Graphic.
            /// </summary>
            public string EventGraphic
            {
                get
                {
                    return mInstance.EventGraphic;
                }
                set
                {
                    mInstance.EventGraphic = value;
                }
            }


            /// <summary>
            /// Attach related items here for event's cross sell section.
            /// </summary>
            public IEnumerable<TreeNode> EventRelatedItems
            {
                get
                {
                    return mInstance.GetRelatedDocuments("EventRelatedItems");
                }
            }


            /// <summary>
            /// Location Name.
            /// </summary>
            public string EventLocationName
            {
                get
                {
                    return mInstance.EventLocationName;
                }
                set
                {
                    mInstance.EventLocationName = value;
                }
            }


            /// <summary>
            /// Date Description.
            /// </summary>
            public string EventDateDescription
            {
                get
                {
                    return mInstance.EventDateDescription;
                }
                set
                {
                    mInstance.EventDateDescription = value;
                }
            }


            /// <summary>
            /// Override Event Registration?.
            /// </summary>
            public bool EventRegistrationOverride
            {
                get
                {
                    return mInstance.EventRegistrationOverride;
                }
                set
                {
                    mInstance.EventRegistrationOverride = value;
                }
            }


            /// <summary>
            /// Registration Form.
            /// </summary>
            public string EventRegistrationForm
            {
                get
                {
                    return mInstance.EventRegistrationForm;
                }
                set
                {
                    mInstance.EventRegistrationForm = value;
                }
            }


            /// <summary>
            /// Registration End Date.
            /// </summary>
            public DateTime EventRegistrationEndDate
            {
                get
                {
                    return mInstance.EventRegistrationEndDate;
                }
                set
                {
                    mInstance.EventRegistrationEndDate = value;
                }
            }


            /// <summary>
            /// CSS Class.
            /// </summary>
            public string CssClass
            {
                get
                {
                    return mInstance.CssClass;
                }
                set
                {
                    mInstance.CssClass = value;
                }
            }


            /// <summary>
            /// Styles.
            /// </summary>
            public string Styles
            {
                get
                {
                    return mInstance.Styles;
                }
                set
                {
                    mInstance.Styles = value;
                }
            }


            /// <summary>
            /// Macro Condition.
            /// </summary>
            public string MacroCondition
            {
                get
                {
                    return mInstance.MacroCondition;
                }
                set
                {
                    mInstance.MacroCondition = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="CcbEvent" /> class.
        /// </summary>
        public CcbEvent() : base(CLASS_NAME)
        {
            mFields = new CcbEventFields(this);
        }

        #endregion
    }
}