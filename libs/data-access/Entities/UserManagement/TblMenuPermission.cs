using System;
using System.Collections.Generic;

namespace Api.Services.DataAccess.Entities.UserManagement;

public partial class TblMenuPermission
{
    public Guid ApplicationId { get; set; }

    public Guid RoleId { get; set; }

    public string MenuId { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}
