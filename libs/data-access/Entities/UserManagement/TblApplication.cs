using System;
using System.Collections.Generic;

namespace Api.Services.DataAccess.Entities.UserManagement;

public partial class TblApplication
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

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

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual TblClient Client { get; set; } = null!;

    public virtual ICollection<TblEmailSetting> TblEmailSettings { get; set; } = new List<TblEmailSetting>();

    public virtual ICollection<TblMenu> TblMenus { get; set; } = new List<TblMenu>();

    public virtual ICollection<TblRole> TblRoles { get; set; } = new List<TblRole>();

    public virtual TblUserDocument? TblUserDocument { get; set; }

    public virtual ICollection<TblUserRole> TblUserRoles { get; set; } = new List<TblUserRole>();

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();
}
