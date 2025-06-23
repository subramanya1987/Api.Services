using Api.Services.DataAccess.Entities.UserManagement;
using Api.Services.Models.UserManagement;

namespace Api.Services.Infra.Cache
{
    /// <summary>
    /// Cache for references data.
    /// </summary>
    public interface IReferenceDataCache
    {
        /// <summary>
        /// Application Caching 
        /// </summary>
        /// 
        IEnumerable<ApplicationResponse> Applications { get; }
    }
}
