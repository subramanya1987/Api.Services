using System;
using System.Collections.Generic;

namespace Api.Services.DataAccess.Entities.UserManagement;

public partial class TblUserRole
{
    public Guid Id { get; set; }

    public Guid ApplicationId { get; set; }

    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public virtual TblApplication Application { get; set; } = null!;

    public virtual TblRole Role { get; set; } = null!;

    public virtual TblUser User { get; set; } = null!;
}
