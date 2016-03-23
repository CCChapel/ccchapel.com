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

[assembly: RegisterDocumentType(BasicImage.CLASS_NAME, typeof(BasicImage))]

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type BasicImage.
    /// </summary>
    public partial class BasicImage : TreeNode
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "image.basicImage";


        /// <summary>
        /// The instance of the class that provides extended API for working with BasicImage fields.
        /// </summary>
        private readonly BasicImageFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// ImageID.
        /// </summary>
        [DatabaseIDField]
        public int ImageID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ImageID"), 0);
            }
            set
            {
                SetValue("ImageID", value);
            }
        }


        /// <summary>
        /// File.
        /// </summary>
        [DatabaseField]
        public Guid ImageFile
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("ImageFile"), Guid.Empty);
            }
            set
            {
                SetValue("ImageFile", value);
            }
        }


        /// <summary>
        /// Macro Condition.
        /// </summary>
        [DatabaseField]
        public string ImageMacroCondition
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ImageMacroCondition"), "");
            }
            set
            {
                SetValue("ImageMacroCondition", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with BasicImage fields.
        /// </summary>
        public BasicImageFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with BasicImage fields.
        /// </summary>
        public partial class BasicImageFields
        {
            /// <summary>
            /// The content item of type BasicImage that is a target of the extended API.
            /// </summary>
            private readonly BasicImage mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="BasicImageFields" /> class with the specified content item of type BasicImage.
            /// </summary>
            /// <param name="instance">The content item of type BasicImage that is a target of the extended API.</param>
            public BasicImageFields(BasicImage instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// ImageID.
            /// </summary>
            public int ImageID
            {
                get
                {
                    return mInstance.ImageID;
                }
                set
                {
                    mInstance.ImageID = value;
                }
            }


            /// <summary>
            /// File.
            /// </summary>
            public Attachment ImageFile
            {
                get
                {
                    return mInstance.GetFieldAttachment("ImageFile");
                }
            }


            /// <summary>
            /// Macro Condition.
            /// </summary>
            public string ImageMacroCondition
            {
                get
                {
                    return mInstance.ImageMacroCondition;
                }
                set
                {
                    mInstance.ImageMacroCondition = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicImage" /> class.
        /// </summary>
        public BasicImage() : base(CLASS_NAME)
        {
            mFields = new BasicImageFields(this);
        }

        #endregion
    }
}