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
using CMS.CustomTables.Types;
using CMS.CustomTables;

[assembly: RegisterCustomTable(SpeakersItem.CLASS_NAME, typeof(SpeakersItem))]

namespace CMS.CustomTables.Types
{
    /// <summary>
    /// Represents a content item of type SpeakersItem.
    /// </summary>
    public partial class SpeakersItem : CustomTableItem
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "ccc.speakers";


        /// <summary>
        /// The instance of the class that provides extended API for working with SpeakersItem fields.
        /// </summary>
        private readonly SpeakersItemFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// First Name.
        /// </summary>
        [DatabaseField]
        public string SpeakerFirstName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SpeakerFirstName"), "");
            }
            set
            {
                SetValue("SpeakerFirstName", value);
            }
        }


        /// <summary>
        /// Last Name.
        /// </summary>
        [DatabaseField]
        public string SpeakerLastName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SpeakerLastName"), "");
            }
            set
            {
                SetValue("SpeakerLastName", value);
            }
        }


        /// <summary>
        /// Is Active.
        /// </summary>
        [DatabaseField]
        public bool SpeakerIsActive
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("SpeakerIsActive"), true);
            }
            set
            {
                SetValue("SpeakerIsActive", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with SpeakersItem fields.
        /// </summary>
        public SpeakersItemFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with SpeakersItem fields.
        /// </summary>
        public partial class SpeakersItemFields
        {
            /// <summary>
            /// The content item of type SpeakersItem that is a target of the extended API.
            /// </summary>
            private readonly SpeakersItem mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="SpeakersItemFields" /> class with the specified content item of type SpeakersItem.
            /// </summary>
            /// <param name="instance">The content item of type SpeakersItem that is a target of the extended API.</param>
            public SpeakersItemFields(SpeakersItem instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// First Name.
            /// </summary>
            public string SpeakerFirstName
            {
                get
                {
                    return mInstance.SpeakerFirstName;
                }
                set
                {
                    mInstance.SpeakerFirstName = value;
                }
            }


            /// <summary>
            /// Last Name.
            /// </summary>
            public string SpeakerLastName
            {
                get
                {
                    return mInstance.SpeakerLastName;
                }
                set
                {
                    mInstance.SpeakerLastName = value;
                }
            }


            /// <summary>
            /// Is Active.
            /// </summary>
            public bool SpeakerIsActive
            {
                get
                {
                    return mInstance.SpeakerIsActive;
                }
                set
                {
                    mInstance.SpeakerIsActive = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeakersItem" /> class.
        /// </summary>
        public SpeakersItem() : base(CLASS_NAME)
        {
            mFields = new SpeakersItemFields(this);
        }

        #endregion
    }
}