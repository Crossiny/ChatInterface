using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Threading;

namespace ChatInterface
{
    class Client
    {
        DispatcherTimer dt;
        TcpClient tcpClient;
        StreamRW streamRW;

        /// <summary>
        /// Gibt zurück ob der Client mit dem Server verbunden ist.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return tcpClient.Connected;
            }
        }

        /// <summary>
        /// Verbindet mit dem Server und meldet sich mit den Nutzerdaten an.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns>Gibt an ob die Verbindung und der Login erfolgreich waren.</returns>
        public bool Connect(string Username, string Password)
        {
            tcpClient = new TcpClient("localhost", 1337);
            streamRW = new StreamRW(tcpClient.GetStream());

            streamRW.WriteLine(String.Format("{0}:{1}", Username, Password));

            return (streamRW.ReadLine() == "Login successfull") ? true : false;
        }

        public void SendMessage(string Message)
        {
            streamRW.WriteLine("Message:" + Message);
        }

        /// <summary>
        /// Beendet die Verbindung mit dem Server.
        /// </summary>
        public void Disconnect()
        {
            streamRW.WriteLine("Disconnect:Logout");
            tcpClient.Close();
        }        
    }
}