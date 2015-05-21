using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscoverableService
{
    public class StringService : IStringService
    {
        public string ToUpper(string content)
        {
            return content.ToUpper();
        }
    }
}
