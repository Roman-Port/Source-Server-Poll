using SourceProtocol.Packets.Polling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SourceProtocol
{
    public class PollingRequest
    {
        //This is a request to poll the server

        public string ip;
        public int port;
        private IPEndPoint endpoint;

        private UdpClient connection;
        public bool connectionAlive = false;

        public PollingRequest(string _ip, int _port)
        {
            ip = _ip;
            port = _port;
            IPAddress addr = IPAddress.Parse(ip);
            endpoint = new IPEndPoint(addr,port);
        }

        private UdpClient GetConn()
        {
            if (connectionAlive)
            {
                //return existing connection
                return connection;
            }
            else
            {
                var client = ServerConnector.EstablishConnection(endpoint);
                connection = client;
                connectionAlive = true;
                return client;
            }
        }

        public A2S_INFO InfoPoll()
        {
            var client = GetConn();
            byte[] req = new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45, 0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00};
            var response = ServerConnector.GetData(ref client, req);
            return A2S_INFO.Read(response);
        }

        public A2S_PLAYER PlayerListPoll()
        {
            //First, request the challenge.
            var client = GetConn();
            byte[] req = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x55, 0xFF, 0xFF, 0xFF, 0xFF };
            //Request challenge
            byte[] chal = ServerConnector.GetData(ref client, req);
            //Swap challenge results and request again
            req[5] = chal[5];
            req[6] = chal[6];
            req[7] = chal[7];
            req[8] = chal[8];
            byte[] results = ServerConnector.GetData(ref client, req);
            //Results contains the list
            return A2S_PLAYER.Read(results);
        }

        private byte[] DoChallenge(byte reqType, byte endReqType, ref UdpClient client)
        {
            //First, request the challenge.
            byte[] req = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, reqType, 0xFF, 0xFF, 0xFF, 0xFF };
            //Request challenge
            byte[] chal = ServerConnector.GetData(ref client, req);
            //Swap challenge results and request again
            req[5] = chal[5];
            req[6] = chal[6];
            req[7] = chal[7];
            req[8] = chal[8];

            

            req[4] = endReqType;
            byte[] results = ServerConnector.GetData(ref client, req);
            return results;
        }

        public Dictionary<string, string> RulesPoll()
        {
            var client = GetConn();
            byte[] data = DoChallenge(0x56, 0x56, ref client);
            //Parse dictonary
            MemoryStream ms = new MemoryStream(data);
            //Skip padding and header
            ms.Position += 5;
            short rulesLength = ServerConnector.Ms_ReadInt16(ref ms);
            int i = 0;
            Console.WriteLine(rulesLength);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            while (i<rulesLength)
            {
                try
                {
                    dict.Add(ServerConnector.Ms_ReadString(ref ms), ServerConnector.Ms_ReadString(ref ms));
                } catch (Exception ex)
                {

                }
                i += 1;
            }
            return dict;
        }
    }
}
