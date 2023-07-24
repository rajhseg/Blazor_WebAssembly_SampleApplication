using Microsoft.AspNetCore.Components.Authorization;
using WebAssemblyApp.Shared.Services;
using System.Security.Claims;
using WebAssemblyApp.Shared.ViewModel;
using WebAssemblyApp.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WebAssemblyApp.Client.Models.Filters
{
    public class BlazorAuthenticationStateProvider : AuthenticationStateProvider
    {        
        private readonly IJsConsole jsconsole;

        private readonly Blazored.SessionStorage.ISessionStorageService sessionStorage;
        private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public BlazorAuthenticationStateProvider(Blazored.SessionStorage.ISessionStorageService sessionStorage, IJsConsole jsconsole)
        {            
            this.sessionStorage = sessionStorage;
            this.jsconsole = jsconsole;
        }
    
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userstorageResult = await sessionStorage.GetItemAsync<string>("UserData");                

                if (userstorageResult == null)
                    return await Task.FromResult(new AuthenticationState(anonymous));

                var userInfo = userstorageResult.ConvertToObject<UserInfoObject>();

                if (userInfo == null)
                    return await Task.FromResult(new AuthenticationState(anonymous));

                var claims = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> {
                    new Claim(ClaimTypes.Name, userInfo.UserName),
                    new Claim(ClaimTypes.Role, userInfo.Role)
                }, authenticationType:"customAuth"));

                return await Task.FromResult(new AuthenticationState(claims));
            }
            catch(Exception)
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }

        public async Task UpdateAuthentication(UserInfoObject userData)
        {
            ClaimsPrincipal claimsPrincipal;
            
            if (userData != null)
            {
                userData.ExpiresTime = DateTime.Now.AddMinutes(userData.Expires);
                                
                string encodedObject = userData.ConvertToString();
                await sessionStorage.SetItemAsync<string>("UserData", encodedObject);

                 claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> {
                    new Claim(ClaimTypes.Name, userData.UserName),
                    new Claim(ClaimTypes.Role, userData.Role)
                }, authenticationType: "customAuth"));
            }
            else
            {
                await sessionStorage.RemoveItemAsync("UserData");
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task<string> GetTokenFromSession()
        {
            string token = string.Empty;

            var userstorageResult = await sessionStorage.GetItemAsync<string>("UserData");

            if (userstorageResult == null)
                return token;

            var userInfo = userstorageResult.ConvertToObject<UserInfoObject>();

            if (userInfo == null)
                return token;

            if (userInfo.ExpiresTime > DateTime.Now)
            {
                return userInfo.Token;
            };

            return "Token Expired";
        }
    }
}
