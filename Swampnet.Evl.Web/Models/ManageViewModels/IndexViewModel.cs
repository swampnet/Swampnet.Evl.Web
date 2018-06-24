using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

        [Display(Name = "Active Api-Key")]
        public string ActiveApiKey { get; set; }

        public string TimeZone { get; set; }


        public IEnumerable<SelectListItem> TimeZones { get; } = TimeZoneInfo.GetSystemTimeZones().Select(tz =>
            new SelectListItem { Value = tz.Id, Text = tz.DisplayName }
        );
    }
}
