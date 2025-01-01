using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ChatServer;
using Database;
using ServerInterface;

namespace Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = true)]
    internal class DataServer : DataServerInterface
    {
        private static ChatServer.Database db = new ChatServer.Database();


        public int GetNumberofEntries()
        {
            return db.GetALLUsers();
        }

        public int AllchatRooms()
        {
            return db.TotalRooms();
        }

        public bool UserAdd(string username)
        {
            bool added;
            if (CheckIfuserExists(username))
            {
                added = false;
            }
            else
            {
                db.useradd(username);
                added = true;
            }
            return added;
        }

        public string RoomCreate(string roomN, string userN)
        {
            string newRoom = null;
            if (!CheckIfroomexists(roomN))
            {
                db.Addroom(roomN);
                db.Addusertotheroom(roomN, userN);
                newRoom = roomN;
            }
            return newRoom;
        }

        public string RoomJoin(string roomName, string username)
        {
            string nowRoomName = null;
            if (CheckIfroomexists(roomName))
            {
                db.Addusertotheroom(roomName, username);
                nowRoomName = roomName;
            }
            return nowRoomName;
        }

        public void RoomLeave(string roomName, string username)
        {
            if (CheckIfroomexists(roomName))
            {
                db.Removechatroomuser(roomName, username);
            }
        }

        public void PublicSending(string roomName, string username, string message)
        {
            if (CheckIfroomexists(roomName))
            {
                db.sndMsg(roomName, username, null, message);
            }
        }

        public void sndImgPublic(string roomName, string username, string imgData)
        {
            if (CheckIfroomexists(roomName))
            {
                db.sndImgPublic(roomName, username, null, imgData);
            }
        }

        public void TXTFilepublicsnd(string roomName, string username, string[] textFileData)
        {
            if (CheckIfroomexists(roomName))
            {
                db.TXTFileSend(roomName, username, null, textFileData);
            }
        }

        public void pvtMessages(string roomName, string fromUser, string toUser, string message)
        {
            if (CheckIfroomexists(roomName))
            {
                if (CheckUserExistedInRoom(roomName, toUser))
                {
                    db.sndMsg(roomName, fromUser, toUser, message);
                }
            }
        }

        public void pvtImage(string roomName, string fromUser, string toUser, string imgData)
        {
            if (CheckIfroomexists(roomName))
            {
                if (CheckUserExistedInRoom(roomName, toUser))
                {
                    db.sndImgPublic(roomName, fromUser, toUser, imgData);
                }
            }
        }

        public void pvtTXTfile(string roomName, string fromUser, string toUser, string[] textFileData)
        {
            if (CheckIfroomexists(roomName))
            {
                if (CheckUserExistedInRoom(roomName, toUser))
                {
                    db.TXTFileSend(roomName, fromUser, toUser, textFileData);
                }
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Message> GetMessages(string roomName, string username)
        {
            List<Message> userMessages = new List<Message>();
            for (int i = 0; i < db.TotalRooms(); i++)
            {
                db.roomNamebIndex(i, out string temproomname);
                if (roomName.Equals(temproomname))
                {
                    db.GetRoomMessages(i, out List<Message> messages);
                    //userMessages = messages;
                    for(int y = 0; y < messages.Count; y++)
                    {
                        if (messages[y].toTheuser == null) //If the Message is public
                        {
                            userMessages.Add(messages[y]);
                        }
                        else //If the Message is private
                        {
                            if ((messages[y].toTheuser).Equals(username) || (messages[y].frmUser).Equals(username)) //Check if the private message is issued to the user or is the sender
                            {
                                userMessages.Add(messages[y]);
                            }
                        }
                    }
                    break;
                }
            }
            return userMessages;
        }

        public List<string> GetRoomList()
        {
            List<string> roomNameList = new List<string>();
            for (int i = 0; i < db.TotalRooms(); i++)
            {
                db.roomNamebIndex(i, out string roomName);
                roomNameList.Add(roomName);
            }
            return roomNameList;
        }

        public HashSet<string> GetOnlineUsers(string roomName)
        {
            HashSet<string> userOnline = db.GetUserListInRoom(roomName);
            return userOnline;
        }

        private bool CheckIfuserExists(string username)
        {
            bool existed = false;
            for (int i=0; i<db.GetALLUsers(); i++) 
            {
                db.usernamebIndex(i, out string tempusername);
                if (username.Equals(tempusername))
                {
                    existed = true;
                    break;
                }
            }
            return existed;
        }

        private bool CheckIfroomexists(string roomName)
        {
            bool existed = false;
            for (int i = 0; i < db.TotalRooms(); i++)
            {
                db.roomNamebIndex(i, out string tempRoomName);
                if (roomName.Equals(tempRoomName))
                {
                    existed = true;
                    break;
                }
            }
            return existed;
        }

        private bool CheckUserExistedInRoom(string roomName, string username)
        {
            HashSet<string> userOnline = db.GetUserListInRoom(roomName);
            bool existed = userOnline.Contains(username);
            return existed;
        }
    }
}









