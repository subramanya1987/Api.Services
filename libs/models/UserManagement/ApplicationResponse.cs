using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Models.UserManagement
{
    public class ApplicationResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? Address3 { get; set; }

        public string? Address4 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? PinCode { get; set; }

        public string? Phone1 { get; set; }

        public string? Phone2 { get; set; }

        public string? Mobile1 { get; set; }

        public string? Mobile2 { get; set; }

        public string? Email1 { get; set; }

        public string? Email2 { get; set; }

        public bool? IsActive { get; set; }
    }
}
