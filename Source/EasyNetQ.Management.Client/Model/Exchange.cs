namespace EasyNetQ.Management.Client.Model;

#nullable disable

public class Exchange
{
    public string Name { get; set; }
    public string Vhost { get; set; }
    public string Type { get; set; }
    public bool Durable { get; set; }
    public bool AutoDelete { get; set; }
    public bool Internal { get; set; }
    public Dictionary<string, string> Arguments { get; set; }
}
