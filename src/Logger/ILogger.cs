using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logger
{
    public interface ILogger
    {
        void AddToLog(string Type, String Entry);
    }
}
