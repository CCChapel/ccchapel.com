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

[assembly: RegisterCustomTable(LocationsItem.CLASS_NAME, typeof(LocationsItem))]

namespace CMS.CustomTables.Types
{
    /// <summary>
    /// Represents a content item of type LocationsItem.
    /// </summary>
    public partial class LocationsItem : CustomTableItem
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "general.locations";


        /// <summary>
        /// The instance of the class that provides extended API for working with LocationsItem fields.
        /// </summary>
        private readonly LocationsItemFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// Name.
        /// </summary>
        [DatabaseField]
        public string LocationName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationName"), "");
            }
            set
            {
                SetValue("LocationName", value);
            }
        }


        /// <summary>
        /// Map Name.
        /// </summary>
        [DatabaseField]
        public string LocationMapName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationMapName"), "");
            }
            set
            {
                SetValue("LocationMapName", value);
            }
        }


        /// <summary>
        /// Street Address.
        /// </summary>
        [DatabaseField]
        public string LocationStreetAddress
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationStreetAddress"), "");
            }
            set
            {
                SetValue("LocationStreetAddress", value);
            }
        }


        /// <summary>
        /// City.
        /// </summary>
        [DatabaseField]
        public string LocationCity
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationCity"), "");
            }
            set
            {
                SetValue("LocationCity", value);
            }
        }


        /// <summary>
        /// State.
        /// </summary>
        [DatabaseField]
        public int LocationStateID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("LocationStateID"), 0);
            }
            set
            {
                SetValue("LocationStateID", value);
            }
        }


        /// <summary>
        /// Zip Code.
        /// </summary>
        [DatabaseField]
        public string LocationZipCode
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationZipCode"), "");
            }
            set
            {
                SetValue("LocationZipCode", value);
            }
        }


        /// <summary>
        /// Latitude.
        /// </summary>
        [DatabaseField]
        public double LocationLatitude
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("LocationLatitude"), 0);
            }
            set
            {
                SetValue("LocationLatitude", value);
            }
        }


        /// <summary>
        /// Longitude.
        /// </summary>
        [DatabaseField]
        public double LocationLongitude
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("LocationLongitude"), 0);
            }
            set
            {
                SetValue("LocationLongitude", value);
            }
        }


        /// <summary>
        /// Phone Number.
        /// </summary>
        [DatabaseField]
        public string LocationPhoneNumber
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationPhoneNumber"), "");
            }
            set
            {
                SetValue("LocationPhoneNumber", value);
            }
        }


        /// <summary>
        /// Website.
        /// </summary>
        [DatabaseField]
        public string LocationWebsite
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationWebsite"), "");
            }
            set
            {
                SetValue("LocationWebsite", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with LocationsItem fields.
        /// </summary>
        public LocationsItemFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with LocationsItem fields.
        /// </summary>
        public partial class LocationsItemFields
        {
            /// <summary>
            /// The content item of type LocationsItem that is a target of the extended API.
            /// </summary>
            private readonly LocationsItem mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="LocationsItemFields" /> class with the specified content item of type LocationsItem.
            /// </summary>
            /// <param name="instance">The content item of type LocationsItem that is a target of the extended API.</param>
            public LocationsItemFields(LocationsItem instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// Name.
            /// </summary>
            public string LocationName
            {
                get
                {
                    return mInstance.LocationName;
                }
                set
                {
                    mInstance.LocationName = value;
                }
            }


            /// <summary>
            /// Map Name.
            /// </summary>
            public string LocationMapName
            {
                get
                {
                    return mInstance.LocationMapName;
                }
                set
                {
                    mInstance.LocationMapName = value;
                }
            }


            /// <summary>
            /// Street Address.
            /// </summary>
            public string LocationStreetAddress
            {
                get
                {
                    return mInstance.LocationStreetAddress;
                }
                set
                {
                    mInstance.LocationStreetAddress = value;
                }
            }


            /// <summary>
            /// City.
            /// </summary>
            public string LocationCity
            {
                get
                {
                    return mInstance.LocationCity;
                }
                set
                {
                    mInstance.LocationCity = value;
                }
            }


            /// <summary>
            /// State.
            /// </summary>
            public int LocationStateID
            {
                get
                {
                    return mInstance.LocationStateID;
                }
                set
                {
                    mInstance.LocationStateID = value;
                }
            }


            /// <summary>
            /// Zip Code.
            /// </summary>
            public string LocationZipCode
            {
                get
                {
                    return mInstance.LocationZipCode;
                }
                set
                {
                    mInstance.LocationZipCode = value;
                }
            }


            /// <summary>
            /// Latitude.
            /// </summary>
            public double LocationLatitude
            {
                get
                {
                    return mInstance.LocationLatitude;
                }
                set
                {
                    mInstance.LocationLatitude = value;
                }
            }


            /// <summary>
            /// Longitude.
            /// </summary>
            public double LocationLongitude
            {
                get
                {
                    return mInstance.LocationLongitude;
                }
                set
                {
                    mInstance.LocationLongitude = value;
                }
            }


            /// <summary>
            /// Phone Number.
            /// </summary>
            public string LocationPhoneNumber
            {
                get
                {
                    return mInstance.LocationPhoneNumber;
                }
                set
                {
                    mInstance.LocationPhoneNumber = value;
                }
            }


            /// <summary>
            /// Website.
            /// </summary>
            public string LocationWebsite
            {
                get
                {
                    return mInstance.LocationWebsite;
                }
                set
                {
                    mInstance.LocationWebsite = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsItem" /> class.
        /// </summary>
        public LocationsItem() : base(CLASS_NAME)
        {
            mFields = new LocationsItemFields(this);
        }

        #endregion
    }
}