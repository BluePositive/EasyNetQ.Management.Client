﻿using EasyNetQ.Management.Client.Model;
using EasyNetQ.Management.Client.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace EasyNetQ.Management.Client.Tests;

public class ManagementClientInternalsTests
{
    /// <summary>
    /// Checks for regression from using Int32 instead of Int64 for RecvOct and so on
    /// </summary>
    [Fact]
    public void GetConnections_CheckDeserializeLargeNumbers()
    {
        //TODO: redesign the ManagementClient by factoring out some of it's responsibilities and use dependency injection
        //for this test we'd seperate out the deserialization.

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(true, true)
            }
        };

        settings.Converters.Add(new PropertyConverter());

        String responseBody = GetExampleGetConnectionsJsonResponseBody();

        var connections = JsonConvert.DeserializeObject<IEnumerable<Connection>>(responseBody, settings);
    }

    [Fact]
    public void GetChannels()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(true, true)
            }
        };

        settings.Converters.Add(new PropertyConverter());

        String responseBody = GetExampleGetChannelsJsonResponseBody();

        var channels = JsonConvert.DeserializeObject<IEnumerable<Channel>>(responseBody, settings).ToList();

        Assert.Equal(2, channels.Count);

        Assert.Equal(48538, channels[0].MessageStats.DeliverGet);
        Assert.Equal(48538, channels[0].MessageStats.DeliverNoAck);
        Assert.Equal(0, channels[0].MessageStats.Publish);

        Assert.Equal(0, channels[1].MessageStats.DeliverGet);
        Assert.Equal(0, channels[1].MessageStats.DeliverNoAck);
        Assert.Equal(48538, channels[1].MessageStats.Publish);

    }

    private string GetExampleGetChannelsJsonResponseBody()
    {
        return @"
[{
	""connection_details"": {
		""name"": ""10.1.1.55:62305"",
		""peer_address"": ""10.1.1.55"",
		""peer_port"": 62305
	},
	""message_stats"":
	{
		""deliver_get"": 48538,
		""deliver_get_details"":
		{
			""rate"": 1300.1826094676971,
			""interval"": 5000836,
			""last_event"": 1357767545361
		},
		""deliver_no_ack"": 48538,
		""deliver_no_ack_details"":
		{
			""rate"": 1300.1826094676971,
			""interval"": 5000836,
			""last_event"": 1357767545361
		}
	},
	""transactional"": false,
	""confirm"": false,
	""consumer_count"": 1,
	""messages_unacknowledged"": 0,
	""messages_unconfirmed"": 0,
	""messages_uncommitted"": 0,
	""acks_uncommitted"": 0,
	""prefetch_count"": 0,
	""client_flow_blocked"": false,
	""name"": ""10.1.1.55:62305:1"",
	""node"": ""rabbit@centosRabbit"",
	""number"": 1,
	""user"": ""liquid_dialler_user"",
	""vhost"": ""DANDESKTOP""
},
{
	""connection_details"": {
		""name"": ""10.1.1.55:62305"",
		""peer_address"": ""10.1.1.55"",
		""peer_port"": 62305
	},
	""message_stats"": {
		""publish"": 48538,
		""publish_details"": {
			""rate"": 1300.125933453228,
			""interval"": 5001054,
			""last_event"": 1357767545361
		}
	},
	""transactional"": false,
	""confirm"": false,
	""consumer_count"": 0,
	""messages_unacknowledged"": 0,
	""messages_unconfirmed"": 0,
	""messages_uncommitted"": 0,
	""acks_uncommitted"": 0,
	""prefetch_count"": 0,
	""client_flow_blocked"": false,
	""name"": ""10.1.1.55:62305:2"",
	""node"": ""rabbit@centosRabbit"",
	""number"": 2,
	""user"": ""liquid_dialler_user"",
	""vhost"": ""DANDESKTOP""
}]
";
    }

    private string GetExampleGetConnectionsJsonResponseBody()
    {
        return @"
[{
	""recv_oct"": 2899479294,
	""recv_cnt"": 7765146,
	""send_oct"": 2956033026,
	""send_cnt"": 11310800,
	""send_pend"": 0,
	""state"": ""running"",
	""channels"": 2,
	""recv_oct_details"": {
		""rate"": 657003.9141596435,
		""interval"": 5000818,
		""last_event"": 1357575675448
	},
	""send_oct_details"": {
		""rate"": 669916.001742115,
		""interval"": 5000818,
		""last_event"": 1357575675448
	},
	""name"": ""10.1.1.55:50703"",
	""type"": ""network"",
	""node"": ""rabbit@centosRabbit"",
	""address"": ""10.1.1.67"",
	""port"": 5672,
	""peer_address"": ""10.1.1.55"",
	""peer_port"": 50703,
	""ssl"": false,
	""peer_cert_subject"": """",
	""peer_cert_issuer"": """",
	""peer_cert_validity"": """",
	""auth_mechanism"": ""PLAIN"",
	""ssl_protocol"": """",
	""ssl_key_exchange"": """",
	""ssl_cipher"": """",
	""ssl_hash"": """",
	""protocol"": ""AMQP 0-9-1"",
	""user"": ""liquid_dialler_user"",
	""vhost"": ""DANDESKTOP"",
	""timeout"": 0,
	""frame_max"": 131072,
	""client_properties"": {
		""x-Liquid-MachineName"": ""DANDESKTOP"",
		""platform"": "".NET"",
		""copyright"": ""Copyright (C) 2007-2012 VMware, Inc."",
		""version"": ""3.0.0.0"",
		""information"": ""Licensed under the MPL.  See http://www.rabbitmq.com/"",
		""capabilities"": {
			""publisher_confirms"": true,
			""exchange_exchange_bindings"": true,
			""consumer_cancel_notify"": true,
			""basic.nack"": true
		},
		""product"": ""RabbitMQ"",
		""x-Liquid-Process"": ""Some useful description""
	}
}]";
    }
}
