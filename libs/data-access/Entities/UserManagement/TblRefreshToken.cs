using System;
using System.Collections.Generic;

namespace Api.Services.DataAccess.Entities.UserManagement;

public partial class TblRefreshToken
{
    public Guid UserId { get; set; }

    public Guid ApplicationId { get; set; }

    public string? TokenId { get; set; }

    public string? RefreshToken { get; set; }

    public bool? IsActive { get; set; }

    public virtual TblApplication Application { get; set; } = null!;
}
