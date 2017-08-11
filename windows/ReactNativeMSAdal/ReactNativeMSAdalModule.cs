using ReactNative.Bridge;
using System;
using System.Linq;
// using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Generic;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using System.Threading.Tasks;

namespace ReactNativeMSAdal
{
    public class ReactNativeMSAdalModule : ReactContextNativeModuleBase, ILifecycleEventListener
    {

        public ReactNativeMSAdalModule(ReactContext reactContext)
            : base(reactContext)
        {
        }

        async private Task<WebAccountProvider> getOrCreateContext(string authority)
        {
            return await WebAuthenticationCoreManager.FindAccountProviderAsync(authority);
        }

        [ReactMethod]
        async public void createAsync(string authority, bool validateAuthority, IPromise promise)
        {
            try
            {
                await getOrCreateContext(authority);
            }
            catch (Exception ex)
            {
                promise.Reject(ex);
                return;
            }
            promise.Resolve(true);
        }

        [ReactMethod]
        async public void acquireTokenAsync(
          string authority,
          bool validateAuthority,
          string resourceUrl,
          string clientId,
          string redirectUrl,
          string userId,
          string extraQueryParams,
          IPromise promise)
        {
            WebAccountProvider authContext;
            try
            {
                authContext = await getOrCreateContext(authority);
            }
            catch (Exception ex)
            {
                promise.Reject(ex);
                return;
            }

            WebTokenRequest wtr = new WebTokenRequest(authContext, string.Empty, clientId);


            WebTokenRequestResult wtrr = await WebAuthenticationCoreManager.RequestTokenAsync(wtr);
            WebAccount userAccount;
            if (wtrr.ResponseStatus == WebTokenRequestStatus.Success)
            {
                WebTokenResponse response = wtrr.ResponseData[0];
                userAccount = response.WebAccount;
                AuthenticationResult result = new AuthenticationResult(response.Token, DateTimeOffset.UtcNow)
                {
                    UserInfo = new UserInfo
                    {
                        UniqueId = response.WebAccount.Id,
                        UserId = response.WebAccount.Id,
                    }
                };

                promise.Resolve(result);
            }           
        }

        [ReactMethod]
        public void acquireTokenSilentAsync(
          String authority,
          bool validateAuthority,
          String resourceUrl,
          String clientId,
          String userId,
          IPromise promise)
        {
            promise.Reject(new NotImplementedException());
        }

        [ReactMethod]
        private void tokenCacheReadItems(String authority, bool validateAuthority, IPromise promise)
        {
            promise.Reject(new NotImplementedException());
        }

        [ReactMethod]
        public void tokenCacheDeleteItem(
          string authority,
          bool validateAuthority,
          string itemAuthority,
          string resourceId,
          string clientId,
          string userId,
          bool isMultipleResourceRefreshToken,
          IPromise promise)
        {
            promise.Reject(new NotImplementedException());
        }

        public void tokenCacheClear(string authority, bool validateAuthority, IPromise promise)
        {
            promise.Reject(new NotImplementedException());
        }

        public override string Name
        {
            get
            {
                return "RNAdalPlugin";
            }
        }

        public void OnDestroy()
        {
            throw new NotImplementedException();
        }

        public void OnResume()
        {
            throw new NotImplementedException();
        }

        public void OnSuspend()
        {
            throw new NotImplementedException();
        }
    }
}
