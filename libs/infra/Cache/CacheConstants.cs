namespace Api.Services.Infra.Cache
{
    public static class CacheConstants
    {
        public const string CacheKeyPrefix = "Api.Services.UserManagement";
        public const string ApplicationCacheKey = $"{CacheKeyPrefix}.Application";
        public const string ClientCacheKey = $"{CacheKeyPrefix}.Client";
        public const string UserCacheKey = $"{CacheKeyPrefix}.User";
        public const string RoleCacheKey = $"{CacheKeyPrefix}.Role";
        public const string PermissionCacheKey = $"{CacheKeyPrefix}.Permission";
        public const string UserRoleCacheKey = $"{CacheKeyPrefix}.UserRole";
        public const string UserPermissionCacheKey = $"{CacheKeyPrefix}.UserPermission";
    }
}
