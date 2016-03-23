using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

using ChurchCommunityBuilder.Api;
using ChurchCommunityBuilder.Api.Events.Entity;

namespace CCB
{
    public partial class Event
    {
        public static ChurchCommunityBuilder.Api.Events.Entity.Event GetEvent(int id)
        {
            string cacheID = CachingHelpers.CachingID("CcbEventData", id);

            if (CachingHelpers.Cache.Contains(cacheID))
            {
                //Get Data From Cache
                ChurchCommunityBuilder.Api.Events.Entity.Event ev = 
                    (ChurchCommunityBuilder.Api.Events.Entity.Event)CachingHelpers.Cache.Get(cacheID);

                return ev;
            }
            else
            {
                //Get Data From CCB & Add to Cache
                ChurchCommunityBuilder.Api.Events.Entity.Event ev = Api.Client.Events.Profiles.Get(id);

                CachingHelpers.Cache.Add(cacheID, ev, CachingHelpers.Policy);

                return ev;
            }
        }
    }

    public static partial class Extensitons
    {
        public static Form GetForm(this ChurchCommunityBuilder.Api.Events.Entity.Event ev)
        {
            try
            {
                //Get First Form
                RegistrationForm form = ev.Registration.Forms.FirstOrDefault();

                return new Form(form);
            }
            catch
            {
                return new Form();
            }
        }

        public static bool HasRegistration(this ChurchCommunityBuilder.Api.Events.Entity.Event input)
        {
            try
            {
                var form = input.Registration.Forms.First();

                if (!string.IsNullOrWhiteSpace(form.Url))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static ChurchCommunityBuilder.Api.People.Entity.Individual GetOrganizer(this ChurchCommunityBuilder.Api.Events.Entity.Event input)
        {
            if (input.Organizer.CCBID.HasValue)
            {
                return Api.Client.People.Individuals.Get((int)input.Organizer.CCBID);
            }
            else
            {
                return new ChurchCommunityBuilder.Api.People.Entity.Individual();
            }
        }
    }
}