using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceProtocol.Packets.Polling
{
    public class A2S_INFO
    {
        public byte protocolVersion;
        public string serverName;
        public string serverMap;
        public string gameFolder;
        public string gameName;
        public short gameId;

        public int players;
        public int maxPlayers;
        public int bots;

        public A2S_INFO_ServerType serverType;
        public A2S_INFO_Environment serverEnvironment;

        public bool serverIsPrivate;
        public bool serverIsVacSecured;

        public static A2S_INFO Read(byte[] buf)
        {
            //First, read in the buffer
            MemoryStream ms = new MemoryStream(buf);
            //First, skip the first four bytes of padding and response type
            ms.Position += 5;
            byte protocolVersion = ServerConnector.Ms_ReadByte(ref ms);
            string name = ServerConnector.Ms_ReadString(ref ms);
            string map = ServerConnector.Ms_ReadString(ref ms);
            string folderName = ServerConnector.Ms_ReadString(ref ms);
            string gameName = ServerConnector.Ms_ReadString(ref ms);
            short steamId = ServerConnector.Ms_ReadInt16(ref ms);
            int players = (int)ServerConnector.Ms_ReadByte(ref ms);
            int maxPlayers = (int)ServerConnector.Ms_ReadByte(ref ms);
            int bots = (int)ServerConnector.Ms_ReadByte(ref ms);
            char serverTypeChar = ServerConnector.Ms_ReadChar(ref ms);
            char serverEnviornmentChar = ServerConnector.Ms_ReadChar(ref ms);

            bool privateServer = ServerConnector.Ms_ReadBool(ref ms);
            bool vac = ServerConnector.Ms_ReadBool(ref ms);


            //Convert enums
            A2S_INFO_ServerType serverType = A2S_INFO_ServerType.Unknown;
            switch(serverTypeChar)
            {
                case 'd':
                    serverType = A2S_INFO_ServerType.Dedicated;
                    break;
                case 'l':
                    serverType = A2S_INFO_ServerType.Non_Dedicated;
                    break;
                case 'p':
                    serverType = A2S_INFO_ServerType.SourceTV;
                    break;
            }

            A2S_INFO_Environment serverEnvi = A2S_INFO_Environment.Unknown;
            switch(serverEnviornmentChar)
            {
                case 'm':
                    serverEnvi = A2S_INFO_Environment.MacOS;
                    break;
                case 'o':
                    serverEnvi = A2S_INFO_Environment.MacOS;
                    break;
                case 'w':
                    serverEnvi = A2S_INFO_Environment.Windows;
                    break;
                case 'l':
                    serverEnvi = A2S_INFO_Environment.Linux;
                    break;
            }



            //Convert this 
            A2S_INFO packet = new A2S_INFO();
            packet.protocolVersion = protocolVersion;
            packet.serverName = name;
            packet.serverMap = map;
            packet.gameFolder = folderName;
            packet.gameName = gameName;
            packet.gameId = steamId;
            packet.players = players;
            packet.maxPlayers = maxPlayers;
            packet.bots = bots;
            packet.serverType = serverType;
            packet.serverEnvironment = serverEnvi;
            packet.serverIsVacSecured = vac;
            packet.serverIsPrivate = privateServer;

            return packet;
        }
    }

    public enum A2S_INFO_ServerType
    {
        Dedicated,
        Non_Dedicated,
        SourceTV,
        Unknown
    }

    public enum A2S_INFO_Environment
    {
        Linux,
        Windows,
        MacOS,
        Unknown
    }
}
