using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DT6;

public class SocketClient
{
    private static String response = String.Empty;

    private static ManualResetEvent connectDone = new(false);
    private static ManualResetEvent sendDone = new(false);
    private static ManualResetEvent receiveDone = new(false);

    public static int Main(String[] args)
    {
        Received += (arg) => Console.Write("Received:" + arg);
        Task.Run(() => StartClient());
        Thread.Sleep(1000);
        Console.ReadKey();
        return 0;
    }

    public static event Action<string> Received;

    public static void StartClient()
    {
        try
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8000);

            Socket client = new Socket(ipAddress.AddressFamily,  SocketType.Stream, ProtocolType.Tcp);

            client.BeginConnect(remoteEP, ConnectCallback, client);
            connectDone.WaitOne();

            Send(client, "This is a test<EOF>");
            sendDone.WaitOne();
            Receive(client);
            receiveDone.WaitOne();

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            var client = (Socket)ar.AsyncState!;
            client.EndConnect(ar);
            Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint);
            connectDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void Send(Socket client, String data)
    {
        var byteData = Encoding.ASCII.GetBytes(data);

        client.BeginSend(byteData, 0, byteData.Length, 0,  SendCallback, client);
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            var client = (Socket)ar.AsyncState!;

            var bytesSent = client.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to server.", bytesSent);
            sendDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void Receive(Socket client)
    {
        try
        {
            var state = new StateObject();
            state.workSocket = client;
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            var state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

            // Read data from the remote device.
            var bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                var recvString = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                state.sb.Append(recvString);
                Received(recvString);

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                }
                receiveDone.Set();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}

// State object for receiving data from remote device.
public class StateObject
{
    public Socket workSocket = null;
    public const int BufferSize = 256;
    public byte[] buffer = new byte[BufferSize];
    public StringBuilder sb = new StringBuilder();
}
