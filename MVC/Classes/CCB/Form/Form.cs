using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.Helpers;

using ChurchCommunityBuilder.Api.Events.Entity;
using CCC.Helpers;

namespace CCB
{
    public partial class Form
    {
        protected RegistrationForm _Form { get; set; }

        public ChurchCommunityBuilder.Api.Events.Entity.Form Data
        {
            get
            {
                try
                {
                    string cacheID = CachingHelpers.CachingID("CcbForm", _Form.ID);

                    if (CachingHelpers.Cache.Contains(cacheID))
                    {
                        //Get Data From Cache
                        ChurchCommunityBuilder.Api.Events.Entity.Form form =
                            (ChurchCommunityBuilder.Api.Events.Entity.Form)CachingHelpers.Cache.Get(cacheID);

                        return form;
                    }
                    else
                    {
                        //Get Data From CCB & Add to Cache
                        ChurchCommunityBuilder.Api.Events.Entity.Form form =
                            Api.Client.Events.Forms.Get(_Form.ID);

                        CachingHelpers.Cache.Add(cacheID, form, CachingHelpers.Policy);

                        return form;
                    }
                }
                catch
                {
                    return new ChurchCommunityBuilder.Api.Events.Entity.Form();
                }
            }
        }
        public string Url
        {
            get
            {
                try
                { 
                    return _Form.Url;
                }
                catch
                {
                    return null;
                }
            }
        }

        public int ID
        {
            get
            {
                try
                {
                    return _Form.ID;
                }
                catch
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Is true if the current date and time is less than the end date and time of the form.
        /// </summary>
        public bool IsActive
        {
            get
            {
                //Check if Disabled
                if (Data.Disabled == true)
                {
                    return false;
                }

                //Check if Published
                if (Data.Published != true)
                {
                    return false;
                }

                //Check End Date
                if (Data.End.HasValue)
                {
                    if (DateTime.Now < Data.End.Value)
                    {
                        return true;
                    }

                    return false;
                }
                else
                {
                    //Check that Form Exists
                    if (!string.IsNullOrWhiteSpace(Url))
                    {
                        //No end date set
                        return true;
                    }

                    return false;
                }
            }
        }

        #region Constructors
        public Form(RegistrationForm form)
        {
            _Form = form;
        }

        public Form() { }
        #endregion
    }
}