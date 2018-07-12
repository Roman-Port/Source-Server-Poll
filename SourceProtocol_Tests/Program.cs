using SourceProtocol;
using SourceProtocol.Packets.Polling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceProtocol_Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var client = ServerConnector.EstablishConnection("23.95.51.146", 29015);
            Console.WriteLine("Established connection");
            var response = ServerConnector.GetData(ref client, System.IO.File.ReadAllBytes(@"C:\Users\Roman\Desktop\send4.bin"));
            System.IO.File.WriteAllBytes(@"C:\Users\Roman\Desktop\got.bin", response);
            //Read in
            
            A2S_INFO p = A2S_INFO.Read(response);
            Console.WriteLine(p.serverName);
            Console.WriteLine(p.serverMap);
            Console.WriteLine(p.gameFolder);
            Console.WriteLine(p.gameName);
            Console.WriteLine(p.gameId);
            Console.WriteLine(p.players);
            Console.WriteLine(p.maxPlayers);
            Console.WriteLine(p.bots);
            Console.WriteLine(p.type.ToString());
            Console.WriteLine(p.environment.ToString());
            Console.WriteLine("ok");

            Console.ReadLine();*/



            var client = new PollingRequest("192.223.24.122", 27016);
            //var client = new PollingRequest("23.95.51.146", 29015);

            var p = client.InfoPoll();

            Console.WriteLine("=== SERVER STATS ===");
            Console.WriteLine("Server Name   : "+p.serverName);
            Console.WriteLine("Server Map    : " + p.serverMap);
            Console.WriteLine("Game Folder   : " + p.gameFolder);
            Console.WriteLine("Game Name     : " + p.gameName);
            Console.WriteLine("Game ID       : " + p.gameId);
            Console.WriteLine("Players       : " + p.players);
            Console.WriteLine("Max Players   : " + p.maxPlayers);
            Console.WriteLine("Bots          : " + p.bots);
            Console.WriteLine("Type          : " + p.serverType.ToString());
            Console.WriteLine("Environment   : " + p.serverEnvironment.ToString());
            Console.WriteLine("Is VAC        : " + p.serverIsVacSecured);
            Console.WriteLine("Private       : " + p.serverIsPrivate);

            Console.WriteLine("\r\n=== PLAYER LIST ===");
            var players = client.PlayerListPoll().players;
            foreach(var pl in players)
            {
                Console.WriteLine("Player Name   : " + pl.name);
                //Console.WriteLine("Player Index  : " + pl.index);
                Console.WriteLine("Player Score  : " + pl.score);
                Console.WriteLine("Logged In For : " + pl.duration+" seconds");
                Console.WriteLine("     =====");
            }

            Console.ReadLine();
        }
    }
}
