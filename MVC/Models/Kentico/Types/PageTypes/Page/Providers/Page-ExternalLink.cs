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

[assembly: RegisterDocumentType(ExternalLink.CLASS_NAME, typeof(ExternalLink))]

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type ExternalLink.
    /// </summary>
    public partial class ExternalLink : TreeNode
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "pages.externalLink";


        /// <summary>
        /// The instance of the class that provides extended API for working with ExternalLink fields.
        /// </summary>
        private readonly ExternalLinkFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// ExternalLinkID.
        /// </summary>
        [DatabaseIDField]
        public int ExternalLinkID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ExternalLinkID"), 0);
            }
            set
            {
                SetValue("ExternalLinkID", value);
            }
        }


        /// <summary>
        /// URL.
        /// </summary>
        [DatabaseField]
        public string URL
        {
            get
            {
                return ValidationHelper.GetString(GetValue("URL"), "");
            }
            set
            {
                SetValue("URL", value);
            }
        }


        /// <summary>
        /// Keywords.
        /// </summary>
        [DatabaseField]
        public string Keywords
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Keywords"), "");
            }
            set
            {
                SetValue("Keywords", value);
            }
        }


        /// <summary>
        /// Used for search results.
        /// </summary>
        [DatabaseField]
        public string Description
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Description"), "");
            }
            set
            {
                SetValue("Description", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with ExternalLink fields.
        /// </summary>
        public ExternalLinkFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with ExternalLink fields.
        /// </summary>
        public partial class ExternalLinkFields
        {
            /// <summary>
            /// The content item of type ExternalLink that is a target of the extended API.
            /// </summary>
            private readonly ExternalLink mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="ExternalLinkFields" /> class with the specified content item of type ExternalLink.
            /// </summary>
            /// <param name="instance">The content item of type ExternalLink that is a target of the extended API.</param>
            public ExternalLinkFields(ExternalLink instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// ExternalLinkID.
            /// </summary>
            public int ID
            {
                get
                {
                    return mInstance.ExternalLinkID;
                }
                set
                {
                    mInstance.ExternalLinkID = value;
                }
            }


            /// <summary>
            /// URL.
            /// </summary>
            public string URL
            {
                get
                {
                    return mInstance.URL;
                }
                set
                {
                    mInstance.URL = value;
                }
            }


            /// <summary>
            /// Keywords.
            /// </summary>
            public string Keywords
            {
                get
                {
                    return mInstance.Keywords;
                }
                set
                {
                    mInstance.Keywords = value;
                }
            }


            /// <summary>
            /// Used for search results.
            /// </summary>
            public string Description
            {
                get
                {
                    return mInstance.Description;
                }
                set
                {
                    mInstance.Description = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalLink" /> class.
        /// </summary>
        public ExternalLink() : base(CLASS_NAME)
        {
            mFields = new ExternalLinkFields(this);
        }

        #endregion
    }
}