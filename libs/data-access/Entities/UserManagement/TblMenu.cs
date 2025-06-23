using System;
using System.Collections.Generic;

namespace Api.Services.DataAccess.Entities.UserManagement;

public partial class TblMenu
{
    public string Id { get; set; } = null!;

    public Guid ApplicationId { get; set; }

    public string? Name { get; set; }

    public string? LinkName { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TblApplication Application { get; set; } = null!;
}
