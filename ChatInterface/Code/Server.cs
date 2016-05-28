using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace ChatInterface
{
    class Server
    {
        static Dictionary<string, TcpClient> connectedUser;

        /// <summary>
        /// Startet den Listener der neue Verbindungen von Clients annimmt.
        /// </summary>
        public static void startServer()
        {
            connectedUser = new Dictionary<string, TcpClient>();
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 1337); tcpListener.Start();
            Console.WriteLine("Listener started");

            // Wartet permanent auf Anfragen von Clients.
            while (true)
            {
                try
                {
                    AcceptClient(tcpListener.AcceptTcpClient());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                }
            }
        }

        /// <summary>
        /// Überprüft die Anmeldedaten, weißt dem Client eine Connection zu und startet diese in einem neuen Thread.
        /// </summary>
        /// <param name="tcpClient">Der Client, der die Verbindung angefragt hat.</param>
        static void AcceptClient(TcpClient tcpClient)
        {
            StreamRW streamRW = new StreamRW(tcpClient.GetStream());
            string message = streamRW.ReadLine();
            string username = message.Split(':')[0];
            string password = message.Split(':')[1];

            if (username == password)
            {
                streamRW.WriteLine("Login successfull");
                connectedUser[username] = tcpClient;
                Console.WriteLine("{0} logged in", username);

                // Erstellt ein Objekt von Connection, das sich in einem neuen Thread 
                // um sämtliche weiteren Nachrichten vom Client kümmert.
                Connection c = new Connection(username, tcpClient);
                Thread t = new Thread(new ThreadStart(c.Start));
                t.Start();
            }
            else
            {
                Console.WriteLine("{0} failed to login", username);
                tcpClient.Close();
            }
        }
    }

    class Connection
    {
        TcpClient tcpClient;
        StreamRW streamRW;
        string username;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="Username">Username mit dem sich der Client angemeldet hat.</param>
        /// <param name="TcpClient">Der Client der die Verbindung darstellt.</param>
        public Connection(string Username, TcpClient TcpClient)
        {
            tcpClient = TcpClient;
            streamRW = new StreamRW(tcpClient.GetStream());
            username = Username;
            Console.WriteLine("Established connection for {0}", username);
        }

        /// <summary>
        /// Beginnt mit der Überwachung des Streams und läuft solange der TcpClient connected ist.
        /// </summary>
        public void Start()
        {            
            while (tcpClient.Connected)
            {
                string Message = streamRW.ReadLine();
                if (Message != null)
                    ProcessMessage(Message);
            }
        }

        public static void Stop()
        {

        }

        /// <summary>
        /// Verarbeitet Nachrichten, die an den Server von der Verbindung gesendet werden.
        /// </summary>
        /// <param name="Message">Aufbau: Befehle:Parameter</param>
        /// <returns></returns>
        bool ProcessMessage(string Message)
        {
            string command = Message.Split(':')[0];
            string param = Message.Remove(0, command.Length +1);

            switch (command)
            {
                case "Message":
                    Console.WriteLine("{0} said: {1}", username, param);
                    break;
                case "Disconnect":
                    Console.WriteLine("{0} disconnected: {1}", username, param);
                    break;
            }
            return true;
        }
    }
}
