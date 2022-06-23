using Microsoft.Extensions.Localization;

namespace Moe.La.Common.Resources
{
    public class Localization<T>
    {
        private readonly IStringLocalizer<T> _localizer;
        public Localization(IStringLocalizer<T> localizer)
        {

            _localizer = localizer;
        }

        public string Translate(string key)
        {
            return _localizer[key].Value;
        }
    }
}
