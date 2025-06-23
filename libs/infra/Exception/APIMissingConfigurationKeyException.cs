namespace Api.Services.Infra.Exception
{
    public class APIMissingConfigurationKeyException : System.Exception
    {
        public string Key { get; }
        public APIMissingConfigurationKeyException(string key)
            : base($"Configuration key '{key}' is missing.")
        {
            Key = key;
        }

        public APIMissingConfigurationKeyException(string key, string  message)
            : base(message)
        {
            Key = key;
        }

        public APIMissingConfigurationKeyException(string key, string message, System.Exception innerException)
            : base(message, innerException)
        {
            Key = key;
        }
    }
}
