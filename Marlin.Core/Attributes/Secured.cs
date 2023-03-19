using System;

namespace Marlin.Core.Attributes
{
    public sealed class Secured : Attribute
    {
        public Secured(string claim, string value)
        {
            Claim = claim;
            Value = value;
        }

        public string Claim { get; }
        public string Value { get; }
    }
}
