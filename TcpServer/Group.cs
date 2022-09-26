using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    internal class Group
    {
        public string Name { get; set; }
        public List<User> Members { get; set; } = new List<User>();
        public List<string> GroupChat { get; set; } = new List<string>();
    }
}
