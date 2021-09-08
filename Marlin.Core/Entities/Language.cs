using System.Collections.Generic;

namespace Marlin.Core.Entities
{
    public class Language
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Translation> Translations { get; set; }
    }

    public class Translation
    {
        public string LanguageId { get; set; }
        public string Original { get; set; }
        public string Translated { get; set; }
    }
}
