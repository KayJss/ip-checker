using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

class Program
{ 
    static void Main()
    {
        Console.Write("Domain veya IP girin: ");
        string target = Console.ReadLine();

        // Eğer bir domain girilirse IP'ye çözümlenir
        string ip = target;
        if (IsDomain(target))
        {
            ip = ResolveDomainToIp(target);
        }

        if (ip != null)
        {
            Console.WriteLine($"Hedef: {target}, IP: {ip}");

            Console.WriteLine("Açık portlar:");

            for (int port = 1; port <= 1024; port++)
            {
                if (IsPortOpen(ip, port))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Port {port} açık");
                }
            }

            Console.ForegroundColor = ConsoleColor.Gray; // Renkleri sıfırla

            Console.WriteLine("ICMP Ping sonucu:");

            if (IsPingSuccessful(target))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("ICMP Ping başarılı");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ICMP Ping başarısız");
            }
        }
        else
        {
            Console.WriteLine($"{target} çözümlenemedi.");
        }
    }

    static bool IsDomain(string input)
    {
        // Basit bir kontrol, "." içeriyorsa bir domain kabul ediyoruz.
        return input.Contains(".");
    }

    static string ResolveDomainToIp(string domain)
    {
        try
        {
            IPAddress[] addresses = Dns.GetHostAddresses(domain);
            return addresses[0].ToString();
        }
        catch (Exception)
        {
            return null;
        }
    }

    static bool IsPortOpen(string host, int port)
    {
        try
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                tcpClient.Connect(host, port);
                return true;
            }
        }
        catch (Exception)
        {
            // Port kapalı durumunda
            return false;
        }
    }

    static bool IsPingSuccessful(string target)
    {
        try
        { 
            using (Ping ping = new Ping())
            {
                PingReply reply = ping.Send(target);
                return reply.Status == IPStatus.Success;
            }
        }
        catch (Exception)
        {
            // Ping başarısız durumunda
            return false;
        }
    }
}
