using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactNativeMSAdal
{
    public class UserInfo
    {
        public string UniqueId { get; internal set; }

        public string UserId { get; internal set; }

        public string DisplayableId { get; internal set; }

        public string GivenName { get; internal set; }

        public string FamilyName { get; internal set; }

        public DateTimeOffset? PasswordExpiresOn { get; internal set; }

        public Uri PasswordChangeUrl { get; internal set; }

        public string IdentityProvider { get; internal set; }
    }
}
