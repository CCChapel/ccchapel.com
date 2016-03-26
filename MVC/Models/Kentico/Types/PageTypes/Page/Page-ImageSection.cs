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

[assembly: RegisterDocumentType(ImageSection.CLASS_NAME, typeof(ImageSection))]

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type ImageSection.
    /// </summary>
    public partial class ImageSection : TreeNode
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "pages.imageSection";


        /// <summary>
        /// The instance of the class that provides extended API for working with ImageSection fields.
        /// </summary>
        private readonly ImageSectionFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// ImageSectionID.
        /// </summary>
        [DatabaseIDField]
        public int ImageSectionID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ImageSectionID"), 0);
            }
            set
            {
                SetValue("ImageSectionID", value);
            }
        }


        /// <summary>
        /// Include Content Wrapper.
        /// </summary>
        [DatabaseField]
        public bool SectionIncludeWrapper
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("SectionIncludeWrapper"), false);
            }
            set
            {
                SetValue("SectionIncludeWrapper", value);
            }
        }


        /// <summary>
        /// Use Parallax?.
        /// </summary>
        [DatabaseField]
        public bool SectionIsParallax
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("SectionIsParallax"), false);
            }
            set
            {
                SetValue("SectionIsParallax", value);
            }
        }


        /// <summary>
        /// Height.
        /// </summary>
        [DatabaseField]
        public string SectionHeight
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SectionHeight"), "auto");
            }
            set
            {
                SetValue("SectionHeight", value);
            }
        }


        /// <summary>
        /// Content.
        /// </summary>
        [DatabaseField]
        public string SectionContent
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SectionContent"), "");
            }
            set
            {
                SetValue("SectionContent", value);
            }
        }


        /// <summary>
        /// CSS Class.
        /// </summary>
        [DatabaseField]
        public string SectionCssClass
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SectionCssClass"), "");
            }
            set
            {
                SetValue("SectionCssClass", value);
            }
        }


        /// <summary>
        /// Styles.
        /// </summary>
        [DatabaseField]
        public string SectionStyles
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SectionStyles"), "");
            }
            set
            {
                SetValue("SectionStyles", value);
            }
        }


        /// <summary>
        /// Custom JavaScript.
        /// </summary>
        [DatabaseField]
        public string SectionJavaScript
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SectionJavaScript"), "");
            }
            set
            {
                SetValue("SectionJavaScript", value);
            }
        }


        /// <summary>
        /// Section will only display if the specified macro expression resolves to true.
        /// </summary>
        [DatabaseField]
        public string SectionMacroCondition
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SectionMacroCondition"), "");
            }
            set
            {
                SetValue("SectionMacroCondition", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with ImageSection fields.
        /// </summary>
        public ImageSectionFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with ImageSection fields.
        /// </summary>
        public partial class ImageSectionFields
        {
            /// <summary>
            /// The content item of type ImageSection that is a target of the extended API.
            /// </summary>
            private readonly ImageSection mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="ImageSectionFields" /> class with the specified content item of type ImageSection.
            /// </summary>
            /// <param name="instance">The content item of type ImageSection that is a target of the extended API.</param>
            public ImageSectionFields(ImageSection instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// ImageSectionID.
            /// </summary>
            public int ID
            {
                get
                {
                    return mInstance.ImageSectionID;
                }
                set
                {
                    mInstance.ImageSectionID = value;
                }
            }


            /// <summary>
            /// Images.
            /// </summary>
            public IEnumerable<TreeNode> SectionImages
            {
                get
                {
                    return mInstance.GetRelatedDocuments("SectionImages");
                }
            }


            /// <summary>
            /// Include Content Wrapper.
            /// </summary>
            public bool SectionIncludeWrapper
            {
                get
                {
                    return mInstance.SectionIncludeWrapper;
                }
                set
                {
                    mInstance.SectionIncludeWrapper = value;
                }
            }


            /// <summary>
            /// Use Parallax?.
            /// </summary>
            public bool SectionIsParallax
            {
                get
                {
                    return mInstance.SectionIsParallax;
                }
                set
                {
                    mInstance.SectionIsParallax = value;
                }
            }


            /// <summary>
            /// Height.
            /// </summary>
            public string SectionHeight
            {
                get
                {
                    return mInstance.SectionHeight;
                }
                set
                {
                    mInstance.SectionHeight = value;
                }
            }


            /// <summary>
            /// Content.
            /// </summary>
            public string SectionContent
            {
                get
                {
                    return mInstance.SectionContent;
                }
                set
                {
                    mInstance.SectionContent = value;
                }
            }


            /// <summary>
            /// CSS Class.
            /// </summary>
            public string SectionCssClass
            {
                get
                {
                    return mInstance.SectionCssClass;
                }
                set
                {
                    mInstance.SectionCssClass = value;
                }
            }


            /// <summary>
            /// Styles.
            /// </summary>
            public string SectionStyles
            {
                get
                {
                    return mInstance.SectionStyles;
                }
                set
                {
                    mInstance.SectionStyles = value;
                }
            }


            /// <summary>
            /// Custom JavaScript.
            /// </summary>
            public string SectionJavaScript
            {
                get
                {
                    return mInstance.SectionJavaScript;
                }
                set
                {
                    mInstance.SectionJavaScript = value;
                }
            }


            /// <summary>
            /// Section will only display if the specified macro expression resolves to true.
            /// </summary>
            public string SectionMacroCondition
            {
                get
                {
                    return mInstance.SectionMacroCondition;
                }
                set
                {
                    mInstance.SectionMacroCondition = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSection" /> class.
        /// </summary>
        public ImageSection() : base(CLASS_NAME)
        {
            mFields = new ImageSectionFields(this);
        }

        #endregion
    }
}