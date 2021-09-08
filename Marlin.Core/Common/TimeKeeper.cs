using System;

namespace Marlin.Core.Common
{
    class TimeKeeper
    {
        private readonly DateTime _start = DateTime.MinValue;
        private DateTime _end = DateTime.MinValue;

        internal TimeKeeper()
        {
            _start = DateTime.Now;
        }

        internal TimeKeeper Stop()
        {
            _end = DateTime.Now;
            return this;
        }

        internal double TotalMilliseconds
        {
            get
            {
                return _end.Subtract(_start).TotalMilliseconds;
            }
        }
    }
}
