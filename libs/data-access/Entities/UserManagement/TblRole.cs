using System;
using System.Collections.Generic;

namespace Api.Services.DataAccess.Entities.UserManagement;

public partial class TblRole
{
    public Guid Id { get; set; }

    public Guid ApplicationId { get; set; }

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public bool? IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual TblApplication Application { get; set; } = null!;

    public virtual ICollection<TblUserRole> TblUserRoles { get; set; } = new List<TblUserRole>();
}
