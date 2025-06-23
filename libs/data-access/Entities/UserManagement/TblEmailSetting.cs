using System;
using System.Collections.Generic;

namespace Api.Services.DataAccess.Entities.UserManagement;

public partial class TblEmailSetting
{
    public Guid Id { get; set; }

    public Guid ApplicationId { get; set; }

    public string EmailId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string SmtpAddress { get; set; } = null!;

    public int PortNumber { get; set; }

    public bool EnableSsl { get; set; }

    public bool IsActive { get; set; }

    public virtual TblApplication Application { get; set; } = null!;
}
