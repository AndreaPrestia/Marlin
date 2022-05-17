using System;

namespace Marlin.Core.Attributes
{
    public sealed class Secured : Attribute
    {
        private string _claim;
        private string _value;

        public Secured(string claim, string value)
        {
            _claim = claim;
            _value = value;
        }

        public string Claim { get => _claim; }
        public string Value { get => _value; }
    }
}
