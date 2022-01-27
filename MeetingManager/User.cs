using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingManager
{
    public class User
    {
        private string name;

        public void Name(string userName)
        {
            name = userName;
        }

        public string Name()
        {
            return name;
        }
    }
}
