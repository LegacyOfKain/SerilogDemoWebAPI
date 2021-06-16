using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SerilogDemoWebAPI.Utils
{
    public class Trace
    {
        
        public static void TraceMessage(ILogger _logger, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            //_logger.LogInformation($"{message} {memberName} {sourceFilePath}:{sourceLineNumber}");
            _logger.LogInformation($"{Path.GetFileName(sourceFilePath)}:{sourceLineNumber} {message}");

            
        }
    }
}
