using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwofacedPoker_Client
{
    public static class Constants
    {
        public const string INI_FILE_PATH = "server.ini";
        public const string CLOSE_SOCKET = "/Close_Socket";
        public const string EXIST_ROOM = "/Exist_Room";
        public const string NOT_EXIST_ROOM = "/Not_Exist_Room";
        public const string NO_ROOM = "/No_Room";
        public const string EXIT_ROOM = "/Exit_Room";
        public const string EXIT_ROOM_COMPLETE = "/Exit_Room_Complete";
        public const string GET_CHATTING_ROOM = "/Get_Chatting_Room";
        public const string CREATE_CHATTING_ROOM = "/Create_Chatting_Room ";
        public const string JOIN_CHATTING_ROOM = "/Join_Chatting_Room ";
        public const string LOGIN = "/Login";

        public const string ROOM_EVENT = "/Room_Event ";
        public const string USER_UPDATE = "/User_Update";
        public const string UPDATE_ID = "Update_ID ";
        public const string UPDATE_READY_STATE = "Update_Ready_State ";
        public const string USER_READY_STATE = "User_Ready_State ";
        public const string READY = "READY";
        public const string DONE = "DONE";

        public const string GAME_CLIENT_EVENT = "/Game_Client_Event ";
        public const string GAME_START = "Game_Start";
        public const string START = "Start ";
        public const string GAME_INIT = "Game_Init";
        public const string TURN = "Turn ";
        public const string MY = "My";
        public const string OTHER = "Other";
    }
}
