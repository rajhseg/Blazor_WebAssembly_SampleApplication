using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssemblyApp.Client.Models
{
	public class UpdateAuthorViewModel
	{
		public IBrowserFile Photo { get; set; }
      
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string PhotoName { get; set; }
       
    }
}
