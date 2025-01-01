using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Database;

namespace ServerInterface
{
    [ServiceContract]
    public interface DataServerInterface
    {
        // FUNCTIONS FOR LOGIN PAGE //
        [OperationContract]
        bool UserAdd(string userName);



        // FUNCTIONS FOR LOGGED IN PAGE //
        [OperationContract]
        int GetNumberofEntries();

        [OperationContract]
        int AllchatRooms();

        [OperationContract]
        string RoomCreate(string roomName, string userName);

        [OperationContract]
        string RoomJoin(string roomName, string userName);

        [OperationContract]
        void RoomLeave(string roomName, string userName);

        [OperationContract]
        void PublicSending(string roomName, string userName, string message);

        //SendPublicImage: able to send image data
        [OperationContract]
        void sndImgPublic(string roomName, string userName, string imgData);

        //SendPublicTextFile: able to send textFile data
        [OperationContract]
        void TXTFilepublicsnd(string roomName, string userName, string[] textFileData);

        [OperationContract]
        void pvtMessages(string roomName, string fromUser, string toUser, string message);

        //SendPrivateImage: able to send image data
        [OperationContract]
        void pvtImage(string roomName, string fromUser, string toUser, string imgData);

        //SendPrivateTextFIle: able to send textFile data
        [OperationContract]
        void pvtTXTfile(string roomName, string fromUser, string toUser, string[] textFileData);

        [OperationContract]
        List<Message> GetMessages(string roomName, string userName);

        [OperationContract]
        List<string> GetRoomList();

        [OperationContract]
        HashSet<string> GetOnlineUsers(string roomName);
    }
}
