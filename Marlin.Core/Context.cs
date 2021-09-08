using System;

namespace Marlin.Core
{
    public class Context
    {
        [ThreadStatic]
        static Context context;

        private Context()
        { }

        public Entities.User User { get; set; }

        public void Reset()
        {
            this.User = null;
        }

        public static Context Current
        {
            get
            {
                if (context == null)
                {
                    context = new Context();
                }

                return context;
            }
        }
    }
}
