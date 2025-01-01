using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class ChatRoom
    {
        public string roomN;
        public HashSet<string> roomUsers;
        public List<Message> messages;

        public ChatRoom()
        {
            roomN = null;
            roomUsers = new HashSet<string>();
            messages = new List<Message>();
        }
    }
}
