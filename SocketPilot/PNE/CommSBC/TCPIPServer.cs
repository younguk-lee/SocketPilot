///
/// TCPIPServer: This file implements server of TCP/IP for communication between PNE CTSMonitorPro and SBC.
/// @author Younguk Lee
/// @version 1.0
/// @see CTSMonitorPro의 Pack용으로 SBC와의 통신을 위한 구현입니다.
///
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;


namespace PNE.SocketPilot.CommSBC
{
    /// <summary>
    /// TCPIPServer Class는 SBC와의 통신에서 Server의 역할로 Client의 통신 요청을 기다린다.
    /// @class TCPIPServer
    /// @author Younguk Lee
    /// @date 2020.11.13
    /// </summary>
    public class TCPIPServer
    {
        public TCPIPServer()
        {
            tcpListener = null;
            clientSocket = null;
        }

        private TcpListener tcpListener;
        private Socket clientSocket;
        public TcpListener TcpListener { set; get; }
        public Socket ClientSocket { set; get; }

        public void InitComm()
        {
            try
            {
                IPAddress ipAddress = new IPAddress(new byte[] { 127, 0, 0, 1 });
                tcpListener = new TcpListener(ipAddress, 5001);
                tcpListener.Start();
                while (true)
                {
                    clientSocket = tcpListener.AcceptSocket();
                    ClientHandler clientHandler = new ClientHandler(clientSocket);
                    Task.Run(() => clientHandler.Chat());
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                clientSocket.Close();
            }
        }
    }

    /// <summary>
    /// ClientHandler Class는 Client의 요청을 처리한다.
    /// @class ClientHandler
    /// @author Younguk Lee
    /// @date 2020.11.13
    /// </summary>
    public class ClientHandler
    {
        public ClientHandler(Socket socket)
        {
            networkStream = null;
            streamReader = null;
            streamWriter = null;
            this.socket = socket;
        }

        private Socket socket;
        private NetworkStream networkStream;
        private StreamReader streamReader;
        private StreamWriter streamWriter;

        public void Chat()
        {
            networkStream = new NetworkStream(socket);
            Encoding encode = Encoding.GetEncoding("utf-8");

            streamReader = new StreamReader(networkStream, encode);
            streamWriter = new StreamWriter(networkStream, encode) { AutoFlush = true };

            while (true)
            {
                string readLine = streamReader.ReadLine();
                Console.WriteLine(readLine);
                streamWriter.WriteLine(readLine);
            }

        }
    }
}