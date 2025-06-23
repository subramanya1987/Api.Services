// Execute the below DB commend on the Data Access Project
Scaffold-DbContext "Data Source=DESKTOP-DU3PCRV;Initial Catalog=UserManagement;User ID=sa;Password=asd123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities\UserManagement

// UserManagementContext

 protected readonly IConfiguration _configuration = null;
 public UserManagementContext()
 {
 }
 public UserManagementContext(IConfiguration configuration) => _configuration = configuration;

 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 {
     var env = Environment.GetEnvironmentVariable("CONTEXT_ENVIRONMENT");

     if (!string.IsNullOrEmpty(env) && env.Contains("Development", StringComparison.OrdinalIgnoreCase))
     {
         // Development-specific configuration can be set here
         var connectionString = _configuration.GetConnectionString("Data Source=DESKTOP-DU3PCRV;Initial Catalog=UserManagement;User ID=sa;Password=asd123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
         optionsBuilder.UseSqlServer(connectionString);
     }
     else if (!string.IsNullOrEmpty(env) && env.Equals("Production", StringComparison.OrdinalIgnoreCase))
     {
         // Production-specific configuration can be set here
         optionsBuilder.UseSqlServer(_configuration["UserManagementDB"]);
     }
 }