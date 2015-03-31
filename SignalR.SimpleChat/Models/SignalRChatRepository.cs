using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR;
using System.Data;

namespace SignalR.Chat.Models
{
    public static class Chat
    {
        public static List<UserOnline> UsersOnline = new List<UserOnline>();
        public static List<string> Log = new List<string>();
    }

    public class SignalRChatRepository
    {
        SignalRChatContext db = new SignalRChatContext();

        //public RoomOnline GetOrLoadRoomOnline(Guid roomId)
        //{
        //    RoomOnline roomOnline = Chat.RoomsOnline.SingleOrDefault(ro => ro.RoomId == roomId);
        //    Room room;

        //    if (roomOnline == null)
        //    {
        //        room = db.Rooms.SingleOrDefault(r => r.RoomId == roomId);
        //        if (room == null)
        //        {
        //            return null;
        //        }

        //        roomOnline = new RoomOnline();
        //        roomOnline.RoomId = roomId;
        //        roomOnline.OwnerUserId = CurrentUser().UserId;

        //        Chat.RoomsOnline.Add(roomOnline);
        //    }

        //    return roomOnline;
        //}

        public List<RoomViewModel> GetAllRoomsOnline()
        {
            List<RoomViewModel> rooms = new List<RoomViewModel>();

            foreach (Room room in db.Rooms)
            {
                rooms.Add(GetRoomViewModel(room.RoomId));
            }

            return rooms;
        }

        public MessageViewModel SerializeMessage(Message message)
        {
            MessageViewModel mvm = new MessageViewModel();
            mvm.Text = message.Text;
            mvm.Time = message.Timestamp.ToString("HH:mm:ss");
            mvm.User = db.Users.SingleOrDefault(u => u.UserId == message.UserId).Nickname;
            return mvm;
        }

        public RoomViewModel GetRoomViewModel(Guid id)
        {
            RoomViewModel rvm = new RoomViewModel();
            rvm.Room = db.Rooms.SingleOrDefault(r => r.RoomId == id);
            rvm.Owner = db.Users.SingleOrDefault(u => u.UserId == rvm.Room.OwnerId);
            rvm.Messages = new List<MessageViewModel>();
            rvm.CurrentUser = CurrentUser();

            foreach (Message message in rvm.Room.Messages.OrderBy(m => m.Timestamp))
            {
                rvm.Messages.Add(SerializeMessage(message));
            }

            rvm.OnlineUserList = OnlineUserList(id);

            return rvm;
        }

        public Room GetRoom(Guid id)
        {
            return db.Rooms.SingleOrDefault(r => r.RoomId == id);
        }

        public void AddRoom(Room room)
        {
            room.RoomId = Guid.NewGuid();
            room.OwnerId = CurrentUser().UserId;
            db.Rooms.Add(room);
            db.SaveChanges();
        }

        public void DeleteRoom(Guid id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
        }

        public void ModifyRoom(Room room)
        {
            room.OwnerId = CurrentUser().UserId;
            db.Entry(room).State = EntityState.Modified;
            db.SaveChanges();
        }

        public User CurrentUser()
        {
            Guid userGuid = Helper.UserGuid();
            return db.Users.SingleOrDefault(u => u.UserId == userGuid);
        }

        public UserOnline GetUserOnline()
        {
            var uid = CurrentUser().UserId;
            UserOnline uo = Chat.UsersOnline.SingleOrDefault(u => u.UserId == uid);

            if (uo == null)
            {
                uo = new UserOnline();
                uo.UserId = uid;
                uo.ConnIdRoomIdPairs = new List<UserConnIdRoomId>();
                Chat.UsersOnline.Add(uo);
            }
            return uo;
        }
        
        public List<string> OnlineUserList(Guid roomId)
        {
            var users = new List<string>();

            foreach (UserOnline userOnline in Chat.UsersOnline)
            {
                if (userOnline.ConnIdRoomIdPairs.Count(uo => uo.RoomId == roomId) > 0)
                {
                    users.Add(db.Users.SingleOrDefault(u => u.UserId == userOnline.UserId).Nickname);
                }
            }
            return users;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void DisposeDb()
        {
            db.Dispose();
        }

        public void AddMessage(Message message)
        {
            message.MessageId = Guid.NewGuid();
            db.Messages.Add(message);
            db.SaveChanges();
        }
    }
}
