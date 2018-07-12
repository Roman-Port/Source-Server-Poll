using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SourceProtocol
{
    public class ServerConnector
    {
        public static UdpClient EstablishConnection(IPEndPoint endpoint)
        {
            UdpClient client = new UdpClient(endpoint.Port);
            client.Connect(endpoint);
            return client;
        }

        public static UdpClient EstablishConnection(string ip, int port)
        {
            IPAddress addr = IPAddress.Parse(ip);
            IPEndPoint endpoint = new IPEndPoint(addr, port);
            return EstablishConnection(endpoint);
        }

        public static byte[] GetData(ref UdpClient client, byte[] data)
        {
            client.Send(data, data.Length);
            IPEndPoint endpoint = null;
            return client.Receive(ref endpoint);
        }

        public static void TestConn()
        {
            var client = ServerConnector.EstablishConnection("23.95.51.146", 29015);
            Console.WriteLine("Established connection");
            var response = ServerConnector.GetData(ref client, System.IO.File.ReadAllBytes(@"E:\send.bin"));
            Console.WriteLine("Got response of length " + response.Length);
            Console.WriteLine(Encoding.ASCII.GetString(response));

            Console.ReadLine();
        }

        public const bool SERVER_LITTLE_ENDIAN = false;

        public static byte Ms_ReadByte(ref MemoryStream ms)
        {
            byte[] buf = new byte[1];
            ms.Read(buf, 0, 1);
            return buf[0];
        }

        public static byte[] Ms_ReadBytes(ref MemoryStream ms, int count)
        {
            byte[] buf = new byte[count];
            ms.Read(buf, 0, count);
            return buf;
        }

        public static int Ms_ReadInt32(ref MemoryStream ms)
        {
            byte[] buf = new byte[4];
            ms.Read(buf, 0, 4);
            if (BitConverter.IsLittleEndian == SERVER_LITTLE_ENDIAN)
                Array.Reverse(buf);
            return BitConverter.ToInt32(buf, 0);
        }

        public static long Ms_ReadInt64(ref MemoryStream ms)
        {
            byte[] buf = new byte[8];
            ms.Read(buf, 0, 8);
            if (BitConverter.IsLittleEndian == SERVER_LITTLE_ENDIAN)
                Array.Reverse(buf);
            return BitConverter.ToInt64(buf, 0);
        }

        public static float Ms_ReadFloat32(ref MemoryStream ms)
        {
            byte[] buf = new byte[4];
            ms.Read(buf, 0, 4);
            if (BitConverter.IsLittleEndian == SERVER_LITTLE_ENDIAN)
                Array.Reverse(buf);
            return BitConverter.ToSingle(buf, 0);
        }

        public static short Ms_ReadInt16(ref MemoryStream ms)
        {
            byte[] buf = new byte[2];
            ms.Read(buf, 0, 2);
            if (BitConverter.IsLittleEndian == SERVER_LITTLE_ENDIAN)
                Array.Reverse(buf);
            return BitConverter.ToInt16(buf, 0);
        }

        public static bool Ms_ReadBool(ref MemoryStream ms)
        {
            //First, get byte
            byte b = Ms_ReadByte(ref ms);
            return b != 0x00;
        }

        public static string Ms_ReadString(ref MemoryStream ms, int bufferSize = 500)
        {
            byte[] buffer = new byte[bufferSize];
            int index = 0;
            //Keep reading bytes until it is 0x00.
            while(true)
            {
                buffer[index] = Ms_ReadByte(ref ms);
                if(buffer[index] == 0x00)
                {
                    //break
                    break;
                }
                index++;
            }
            //Convert
            return Encoding.ASCII.GetString(buffer,0,index);
        }

        public static char Ms_ReadChar(ref MemoryStream ms)
        {
            byte[] b = new byte[1];
            b[0] = Ms_ReadByte(ref ms);
            return Encoding.ASCII.GetString(b)[0];
        }
    }
}
