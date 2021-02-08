///
/// TCPIPServer: This file implements client of TCP/IP for communication between PNE CTSMonitorPro and SBC for test.
/// @author Younguk Lee
/// @version 1.0
/// @see CTSMonitorPro의 Pack용으로 SBC와의 통신을 위한 구현입니다.
///
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace PNE.SocketPilot.CommSBC
{
    /// <summary>
    /// TCPIPClient Class는 SBC와의 통신에서 Client의 역할로 Server에 통신을 요청하며 데이터를 주고 받는다.
    /// @class TCPIPClient
    /// @author Younguk Lee
    /// @date 2020.11.13
    /// </summary>
    class TCPIPClient
    {
        private TcpClient client;
        public TcpClient Client { set; get; }

        private TCPIPClient()
        {
            client = null;
        }

        public void InitTCPIPClient()
        {
            try
            {
                client = new TcpClient();
                client.Connect("localhost", 5001);
                NetworkStream networkStream = client.GetStream();

                Encoding encode = Encoding.GetEncoding("utf-8");
                StreamWriter streamWriter = new StreamWriter(networkStream) { AutoFlush = true };
                StreamReader streamReader = new StreamReader(networkStream, encode);
                string dataToSend = "SendData!!!Yhe~";

                while(true)
                {
                    streamWriter.WriteLine(dataToSend);
                    if(dataToSend.IndexOf("<EOF>") > -1) break;
                    {
                        Console.Write(streamReader.ReadLine());
                        dataToSend = "SendData!!!Yhe!";
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                client.Close();
            }
        }
    }
}
