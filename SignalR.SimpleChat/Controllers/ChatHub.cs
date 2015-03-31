using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;
using System.Threading.Tasks;

namespace SignalR.Chat.Models
{
    [HubName("chatHub")]
    public class ChatHub : Hub, IDisconnect
    {
        SignalRChatRepository repo = new SignalRChatRepository();
        SignalRChatContext ctx = new SignalRChatContext();

        public void SendUserList(string roomIdString)
        {
            Clients[roomIdString].userList(repo.OnlineUserList(new Guid(roomIdString)));
        }

        private void DoLeaveAll()
        {
            List<string> rooms = new List<string>();

            foreach (var conn in repo.GetUserOnline().ConnIdRoomIdPairs.Where(p => p.ConnIdString == Context.ConnectionId))
            {
                Clients[conn.ConnIdString].KickOut();
                Groups.Remove(Context.ConnectionId, conn.RoomId.ToString());

                SendSystemMessage(conn.RoomId.ToString(), "От нас ушел " + repo.CurrentUser().Nickname);
                SendUserList(conn.RoomId.ToString());
            }
            repo.GetUserOnline().ConnIdRoomIdPairs.RemoveAll(p => p.ConnIdString == Context.ConnectionId);
        }

        private void DoLeave(Guid oldRoomId)
        {
            foreach (var conn in repo.GetUserOnline().ConnIdRoomIdPairs.Where(r => r.RoomId == oldRoomId))
            {
                Clients[conn.ConnIdString].kickOut();
                Groups.Remove(conn.ConnIdString, oldRoomId.ToString()); //todo
            }

            SendUserList(oldRoomId.ToString());
            SendSystemMessage(oldRoomId.ToString(), "От нас ушел " + repo.CurrentUser().Nickname);
        }

        public void JoinedEvent(string newRoomIdString)
        {
            Guid newRoomId = new Guid(newRoomIdString);
            var pairs = repo.GetUserOnline().ConnIdRoomIdPairs;
            repo.GetUserOnline().LastResponse = DateTime.Now;

            foreach (var room in pairs.Where(uo => uo.RoomId != newRoomId))
            {
                DoLeave(room.RoomId);
            }

            if (pairs.Where(uo => uo.RoomId == newRoomId).Where(uo => uo.ConnIdString == Context.ConnectionId).Count() > 0)
            {
                return;
            }

            Groups.Add(Context.ConnectionId, newRoomIdString);

            if (pairs.Where(uo => uo.RoomId == newRoomId).Count() == 0)
            {
                SendUserList(newRoomIdString);
                SendSystemMessage(newRoomIdString, "К нам присоединился " + repo.CurrentUser().Nickname);
            }

            var pair = new UserConnIdRoomId();
            pair.ConnIdString = Context.ConnectionId;
            pair.RoomId = new Guid(newRoomIdString);
            repo.GetUserOnline().ConnIdRoomIdPairs.Add(pair);
        }

        private void DoSendMessage(Guid UserFromId, string RoomId, string Text)
        {
            //NOTE: Parameters passed to the method will be JSON serialized before being sent to the client
            //Room room = repo.GetRoom(message.RoomIdRef);

            Message message = new Message();
            message.UserId = UserFromId;
            message.RoomId = new Guid(RoomId);
            message.Text = Text;

            repo.AddMessage(message);

            Clients[RoomId].addMessage(repo.SerializeMessage(message));
        }

        private void SendSystemMessage(string RoomId, string Text)
        {
            var room = repo.GetRoom(new Guid(RoomId));

            DoSendMessage(room.OwnerId, RoomId, Text);
        }

        public void SendMessage(string RoomId, string Text)
        {
            DoSendMessage(repo.CurrentUser().UserId, RoomId, Text);
        }

        public Task Disconnect()
        {
            return Task.Factory.StartNew(() =>
            {
                DoLeaveAll();
            });
        }
    }
}