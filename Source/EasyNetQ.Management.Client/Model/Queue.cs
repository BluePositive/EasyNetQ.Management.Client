namespace EasyNetQ.Management.Client.Model;

#nullable disable

public class Queue
{
    public long Memory { get; set; }
    public string IdleSince { get; set; }
    public string Policy { get; set; }
    public string ExclusiveConsumerTag { get; set; }
    public int MessagesReady { get; set; }
    public int MessagesUnacknowledged { get; set; }
    public int Messages { get; set; }
    public int Consumers { get; set; }
    public int ActiveConsumers { get; set; }
    public BackingQueueStatus BackingQueueStatus { get; set; }
    public List<ConsumerDetail> ConsumerDetails { get; set; }
    public long? HeadMessageTimestamp { get; set; }
    public string Name { get; set; }
    public string Vhost { get; set; }
    public bool Durable { get; set; }
    public bool Exclusive { get; set; }
    public bool AutoDelete { get; set; }
    public Dictionary<string, string> Arguments { get; set; }
    public string Node { get; set; }
    public List<string> SlaveNodes { get; set; }
    public List<string> SynchronisedSlaveNodes { get; set; }
    public LengthsDetails MessagesDetails { get; set; }
    public LengthsDetails MessagesReadyDetails { get; set; }
    public LengthsDetails MessagesUnacknowledgedDetails { get; set; }
    public MessageStats MessageStats { get; set; }
}
