using Microsoft.JSInterop;
using WebAssemblyApp.Shared.Services;

namespace WebAssemblyApp.Client.Services
{
    public interface IJsConsole
    {
        Task Log(string message);

        Task Log(object obj);
    }

    public class JsConsole : IJsConsole
    {
        private readonly IJSRuntime _runtime;

        public JsConsole(IJSRuntime jSRuntime)
        {
            this._runtime = jSRuntime;
        }

        public async Task Log(string message)
        {
            await this._runtime.InvokeVoidAsync("console.log", message);
        }

        public async Task Log(object obj)
        {
            var json = obj.ConvertToJson();
            await this._runtime.InvokeVoidAsync("console.log", json);
        }
    }
}
