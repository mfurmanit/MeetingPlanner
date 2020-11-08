using System.Reflection;
using Microsoft.Extensions.Localization;

namespace MeetingPlanner.Others.Localization
{
    public class LocalizationService
    {
        private readonly IStringLocalizer _localizer;

        public LocalizationService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName ?? string.Empty);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public LocalizedString GetTranslation(string key)
        {
            return _localizer[key];
        }
    }
}
