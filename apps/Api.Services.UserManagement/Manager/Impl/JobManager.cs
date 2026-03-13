using Api.Services.Models.UserManagement;
using Api.Services.UserManagement.Consumer;

namespace Api.Services.UserManagement.Manager.Impl
{
    public class JobManager : IJobManager
    {
        private readonly UserListener _userListener;
        private readonly IServiceProvider _serviceProvider;
        private ILogger<JobManager> _logger;
        public JobManager(UserListener userListener,IServiceProvider serviceProvider,ILogger<JobManager> logger)
        {
            _userListener = userListener ?? throw new ArgumentNullException(nameof(userListener));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));    
        }
        public Task<List<UserResponse>> GetUserList()
        {
            return null;
        }
    }
}
