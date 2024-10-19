using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwofacedPoker_Client
{
    class RoomInfo
    {
        public int RoomNumber { get; set; }
        public string RoomName { get; set; }

        public RoomInfo(int roomNumber, string roomName)
        {
            RoomNumber = roomNumber;
            RoomName = roomName;
        }

        public override string ToString()
        {
            return RoomName;
        }
    }
}
