using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using WebAssemblyApp.Client.Models.Filters;

namespace WebAssemblyApp.Client.Services
{
	public class TokenHandler : DelegatingHandler
	{
		private readonly AuthenticationStateProvider authenticationStateProvider;
		private readonly IJsConsole jsConsole;

        public TokenHandler(AuthenticationStateProvider authenticationStateProvider, IJsConsole jsConsole)
		{
			this.authenticationStateProvider = authenticationStateProvider;
			this.jsConsole = jsConsole;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			// before process request
			BlazorAuthenticationStateProvider provider = (BlazorAuthenticationStateProvider)authenticationStateProvider;            
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);           
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await provider.GetTokenFromSession());           
            
			var response = await base.SendAsync(request, cancellationToken);

			// after process request can modify based on response					
			return response;
		}

    }
}
