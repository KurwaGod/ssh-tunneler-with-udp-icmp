using System;
using System.Net.Sockets;
using System.Net;
using Renci.SshNet;
class Program{
static void Main(string[] args){
using var ssh=new SshClient("host","user","pass");
ssh.Connect();
var cmd=ssh.CreateCommand("ping -c 1 8.8.8.8");
Console.WriteLine(cmd.Execute());
using var udpClient=new UdpClient(0);
var ep=new IPEndPoint(IPAddress.Loopback,9999);
var fwd=new ForwardedPortLocal(ep.Address.ToString(),(uint)ep.Port,"8.8.8.8",53);
ssh.AddForwardedPort(fwd);
fwd.Start();
udpClient.Connect(ep);
udpClient.Send(new byte[]{0x00},1);
var resp=udpClient.Receive(ref ep);
Console.WriteLine(BitConverter.ToString(resp));
fwd.Stop();
ssh.Disconnect();}}
