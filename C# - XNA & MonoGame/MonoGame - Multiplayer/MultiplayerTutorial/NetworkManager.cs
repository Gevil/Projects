using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;
namespace MultiplayerTutorial
{
    class NetworkManager
    {
        NetPeerConfiguration Config;
        NetPeer Peer;

        public bool IsHost { get { return Peer != null ? Peer is NetServer : false; } }

        public NetworkManager()
        {
            // The first thing we should is create the config data for both client and server
            // if these do not match. They will not connect to each other.
            Config = new NetPeerConfiguration("Game");

        }

        public void Host()
        {
            // Create server and tell it to start listening for connections.
            Config.Port = 25000;
            NetServer server = new NetServer(Config);
            server.Start();
            // Assign it to NetPeer so it can be used interchangibly with client.
            Peer = server;

            Console.WriteLine("Server has started.");
        }

        public void Connect(string ip)
        {
            // Create client with config and tell it to start then connect to the given IP.
            NetClient client = new NetClient(Config);
            client.Start();
            client.Connect(ip, 25000);
      
            Peer = client;

            Console.WriteLine("Client has started.");
        }

        public void Update()
        {
            if (Peer == null)
                return;

            // Read through messages sent from clients/server and decide what to do with them.

            NetIncomingMessage packet;
            while ((packet = Peer.ReadMessage()) != null)
            {
                switch (packet.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)packet.ReadByte();
                        string reason = packet.ReadString();

                        switch (status)
                        {
                            case NetConnectionStatus.Connected:
                                if (IsHost)
                                    Console.WriteLine("Client joined.");
                                else
                                    Console.WriteLine("Connected!");
                                

                                break;

                            case NetConnectionStatus.Disconnected:
                                if (IsHost)
                                    Console.WriteLine("Client disconnected: " + reason);
                                else
                                    Console.WriteLine("Lost connection " + reason);

                                break;

                        }
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(packet.ReadString());
                        break;

                    case NetIncomingMessageType.Data:
                        // Our game data!                        
                                
                        
                        break;

                    default:
                        Console.WriteLine("Unhandled type: " + packet.MessageType);
                        break;
                }
                Peer.Recycle(packet);
            }
        }
    }
}
