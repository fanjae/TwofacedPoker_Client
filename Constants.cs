﻿using System;
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
        public const string COMPLETE_CREATE_ROOM = "/Complete_Create_Room";
        public const string EXIST_ROOM = "/Exist_Room";
        public const string NOT_EXIST_ROOM = "/Not_Exist_Room";
        public const string NO_ROOM = "/No_Room";
        public const string EXIT_ROOM = "/Exit_Room";
        public const string GET_CHATTING_ROOM = "/Get_Chatting_Room";
        public const string CREATE_CHATTING_ROOM = "/Create_Chatting_Room ";
        public const string JOIN_CHATTING_ROOM = "/Join_Chatting_Room ";
        public const string LOGIN = "/Login";

        
        public const string GAME_CLIENT_EVENT = "/Game_Client_Event ";
        public const string GAME_READY = "Game_Ready ";
        public const string READY_DONE = "Ready_Done";
        public const string READY_NOT_DONE = "Ready_Not_Done";
        public const string LOAD_PLAYER = "Load_Player ";
        public const string GAME_PRE_CALL = "Game_Pre_Call";
        public const string GAME_PRE_LOAD_PLAYER = "Game_Pre_Load_Player";
        public const string GAME_PRE_LOAD_ID = "Game_Pre_Load ID ";
        public const string GAME_PRE_LOAD_READY = "Game_Pre_Load Ready";
        public const string GAME_PRE_LOAD_DONE = "Game_Pre_Load Done";
        public const string GAME_PRE_LOAD_ID_DONE = "Pre_Load_ID_Done ";
        public const string GAME_PRE_LOAD_READY_DONE = "Pre_Load_Ready_Done";
        public const string GAME_PRE_LOAD_DONE_DONE = "Pre_Load_Done_Done";
    }
}
