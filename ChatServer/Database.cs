using Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Database
    {
        List<string> usernames;
        List<ChatRoom> rooms;
        public Database() 
        {
            usernames = new List<string>();
            rooms = new List<ChatRoom>();
        }

        public void useradd(string username)
        {
            usernames.Add(username);
        }

        public void Addroom(string chatRoomName)
        {
            ChatRoom chatRoom = new ChatRoom();
            chatRoom.roomN = chatRoomName;
            rooms.Add(chatRoom);
        }

        public bool Addusertotheroom(string roomName, string username)
        {
            bool val = false;
            for (int m=0; m<rooms.Count; m++)
            {
                if (rooms[m].roomN.Equals(roomName))
                {
                    val = rooms[m].roomUsers.Add(username);
                    break;
                }
            }
            return val;
        }

        public void Removechatroomuser(string roomName, string username)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].roomN.Equals(roomName))
                {
                    rooms[i].roomUsers.Remove(username);
                    break;
                }
            }
        }

        public void sndMsg(string roomName, string fromUser, string toUser, string message)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].roomN.Equals(roomName))
                {
                    Message mssge = new Message();
                    mssge.frmUser = fromUser; 
                    mssge.toTheuser = toUser;
                    mssge.msg = message;
                    rooms[i].messages.Add(mssge);
                    break;
                }
            }
        }

        public void sndImgPublic(string roomName, string fromUser, string toUser, string base64ImageData)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].roomN.Equals(roomName))
                {
                    Message mssge = new Message();
                    mssge.frmUser = fromUser;
                    mssge.toTheuser = toUser;
                    mssge.imgDATA = base64ImageData;
                    rooms[i].messages.Add(mssge);
                    break;
                }
            }
        }

        public void TXTFileSend(string roomName, string fromUser, string toUser, string[] textFileData)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].roomN.Equals(roomName))
                {
                    Message mssge = new Message();
                    mssge.frmUser = fromUser;
                    mssge.toTheuser = toUser;
                    mssge.DATAtxtfile = textFileData;
                    rooms[i].messages.Add(mssge);
                    break;
                }
            }
        }

        public HashSet<string> GetUserListInRoom(string roomName)
        {
            HashSet<string> userList = new HashSet<string>();
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].roomN.Equals(roomName))
                {
                    userList = rooms[i].roomUsers;
                    break;
                }
            }
            return userList;
        }

        public void usernamebIndex(int index, out string username) 
        {
            username = usernames[index];
        }

        public void roomNamebIndex(int index, out string roomName)
        {
            roomName = rooms[index].roomN;
        }

        public void GetRoomMessages(int roomID, out List<Message> messages)
        {
            messages = rooms[roomID].messages;
        }

        public int GetALLUsers()
        {
            return usernames.Count;
        }

        public int TotalRooms()
        {
            return rooms.Count;
        }
    }
}
