using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactNativeMSAdal
{

    public enum AuthenticationStatus
    {
        /// <summary>
        /// Authentication Success.
        /// </summary>
        Success = 0,

        /// <summary>
        /// Authentication failed due to error on client side.
        /// </summary>
        ClientError = -1,

        /// <summary>
        /// Authentication failed due to error returned by service.
        /// </summary>
        ServiceError = -2,
    }

    public class AuthenticationResult
    {

        public AuthenticationResult(string accessToken, DateTimeOffset expiresOn)
        {
            this.AccessToken = accessToken;
            this.Status = AuthenticationStatus.Success;
        }

        AuthenticationResult(string errorCode, string errorDescription, string[] errorCodes)
        {
            this.Status = AuthenticationStatus.ServiceError;
        }


        public string AccessTokenType { get; private set; } = "Bearer";

        public string AccessToken { get; private set; }

        public DateTimeOffset ExpiresOn { get; internal set; }

        public string TenantId { get; internal set; }

        public UserInfo UserInfo { get; internal set; }

        public string IdToken { get; internal set; }

        public AuthenticationStatus Status { get; private set; }

        /// <summary>
        /// Gets provides error type in case of error.
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// Gets detailed information in case of error.
        /// </summary>
        public string ErrorDescription { get; private set; }

        /// <summary>
        /// Gets the status code returned from http layer if any error happens. This status code is either the HttpStatusCode in the inner WebException response or
        /// NavigateError Event Status Code in browser based flow (See http://msdn.microsoft.com/en-us/library/bb268233(v=vs.85).aspx).
        /// You can use this code for purposes such as implementing retry logic or error investigation.
        /// </summary>
        public int statusCode { get; internal set; }

        public bool isMultipleResourceRefreshToken { get; internal set; }
    }
}
