using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceProtocol.Packets.Polling
{
    public class A2S_PLAYER
    {
        public A2S_PLAYER_obj[] players;

        public static A2S_PLAYER Read(byte[] data, bool useLongScore = false)
        {
            A2S_PLAYER obj = new A2S_PLAYER();
            MemoryStream ms = new MemoryStream(data);
            //Skip padding
            ms.Position += 5;
            //Read number of players
            int playerCount = (int)ServerConnector.Ms_ReadByte(ref ms);
            obj.players = new A2S_PLAYER_obj[playerCount];
            int playerIndex = 0;
            //Loop through all
            while(playerIndex<playerCount)
            {
                A2S_PLAYER_obj player = new A2S_PLAYER_obj();
                player.index = (int)ServerConnector.Ms_ReadByte(ref ms);
                player.name = ServerConnector.Ms_ReadString(ref ms);
                if(useLongScore)
                {
                    player.score = ServerConnector.Ms_ReadInt64(ref ms);
                } else
                {
                    player.score = ServerConnector.Ms_ReadInt32(ref ms);
                }
                player.duration = ServerConnector.Ms_ReadFloat32(ref ms);

                obj.players[playerIndex] = player;
                playerIndex++;
            }
            return obj;
        }
    }

    public class A2S_PLAYER_obj
    {
        public int index;
        public string name;
        public long score;
        public float duration;
    }
}
