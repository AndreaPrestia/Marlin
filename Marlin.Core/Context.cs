using System;
using System.Collections.Generic;
using System.Security;

namespace Marlin.Core
{
    public class Context
    {
        [ThreadStatic]
        private static Context _context;

        private Context()
        { }

        public Dictionary<string, object> Claims { get; private set; }

        public static T GetClaim<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var claimFound = _context.Claims.TryGetValue(name.ToLowerInvariant().Trim(), out var claimContent);

            if (!claimFound || claimContent == null)
            {
                throw new SecurityException(string.Format(Messages.TokenClaimNotProvided, name));
            }

            return (T)claimContent;
        }

        public static void Add<T>(string name, T value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if(value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _context.Claims ??= new Dictionary<string, object>();

            if (_context.Claims.ContainsKey(name.ToLowerInvariant().Trim()))
            {
                _context.Claims[name] = value;
            }
            else
            {
                _context.Claims.Add(name.ToLowerInvariant().Trim(), value);
            }
        }

        public static bool IsLoaded => _context != null && _context.Claims != null;

        public static bool HasClaim(string name) => _context.Claims.ContainsKey(name.ToLowerInvariant().Trim());

        public void Reset()
        {
            this.Claims = null;
        }

        public static Context Current
        {
            get
            {
                _context ??= new Context();

                return _context;
            }
        }
    }
}
