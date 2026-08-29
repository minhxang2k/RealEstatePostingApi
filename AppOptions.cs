namespace RealEstatePostingApi
{
    public sealed class AppOptions
    {
        public const string SectionName = "App";

        public string Servicename { get; set; } = "Sample API";
        public string EnvironmentLabel { get; set; } = "Development";
    }
}
