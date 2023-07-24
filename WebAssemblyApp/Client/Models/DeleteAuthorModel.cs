using Microsoft.AspNetCore.Components;

namespace WebAssemblyApp.Client.Models
{
    public class DeleteModel<T>
    {
        public T Model { get; set; }

        public RenderFragment PopupMessage { get; set; }

        public RenderFragment PopupHeader { get; set; }
    }
}
