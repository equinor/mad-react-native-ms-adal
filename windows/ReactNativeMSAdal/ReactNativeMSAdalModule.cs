using ReactNative.Bridge;
using System;
using System.Linq;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Generic;

namespace ReactNativeMSAdal
{
    public class ReactNativeMSAdalModule : ReactContextNativeModuleBase, ILifecycleEventListener
    {

        public ReactNativeMSAdalModule(ReactContext reactContext)
            : base(reactContext)
        {
            _platformParameters = new PlatformParameters(PromptBehavior.Auto, false);
        }

        private IPlatformParameters _platformParameters;

        private Dictionary<string, AuthenticationContext> contexts = new Dictionary<string, AuthenticationContext>();

        private AuthenticationContext currentContext = null;

        public override string Name
        {
            get
            {
                return "RNAdalPlugin";
            }
        }

        [ReactMethod]
        public void createAsync(string authority, bool validateAuthority, IPromise promise) {
            try
            {
                getOrCreateContext(authority, validateAuthority);
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
            AuthenticationContext authContext;
            try
            {
                authContext = getOrCreateContext(authority, validateAuthority);
            }
            catch(Exception ex)
            {
                promise.Reject(ex);
                return;
            }

            if (userId != null) {
                TokenCache cache = authContext.TokenCache;

                List<TokenCacheItem> tokensForUserId = cache
                    .ReadItems()
                    .Where(item => item.DisplayableId.Equals(userId, StringComparison.CurrentCulture);

                if (tokensForUserId.Any())
                {
                    userId = tokensForUserId.First().DisplayableId;
                }
            }

            AuthenticationResult result = await authContext.AcquireTokenAsync(
                resourceUrl,
                clientId,
                new Uri(redirectUrl),
                _platformParameters,
                new UserIdentifier(userId, UserIdentifierType.OptionalDisplayableId),
                extraQueryParams);

            promise.Resolve(result);
        }

        [ReactMethod]
        async public void  acquireTokenSilentAsync(
          String authority,
          bool validateAuthority,
          String resourceUrl,
          String clientId,
          String userId,
          IPromise promise)
        {
            AuthenticationContext authContext;
            try
            {
                authContext = getOrCreateContext(authority, validateAuthority);
            }
            catch (Exception ex)
            {
                promise.Reject(ex);
                return;
            }
            AuthenticationResult result = await authContext.AcquireTokenSilentAsync(
                resourceUrl, 
                clientId, 
                new UserIdentifier(userId, UserIdentifierType.OptionalDisplayableId), 
                _platformParameters);

            promise.Resolve(result);
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

        private AuthenticationContext getOrCreateContext(String authority, bool validateAuthority) {

            AuthenticationContext result;
            if (!contexts.ContainsKey(authority)) {
                result = new AuthenticationContext(authority, validateAuthority);
                this.contexts.Add(authority, result);
            } else {
                result = contexts[authority];
            }
        
            // Last asked for context
            currentContext = result;
            return result;
        }

        public void OnDestroy()
        {
            currentContext = null;
            contexts.Clear();
        }

        public void OnResume()
        {
        }

        public void OnSuspend()
        {
        }
    }
}
