﻿using System;
using System.Threading.Tasks;
using EasyNetQ.Management.Client.Model;
using Xunit;

namespace EasyNetQ.Management.Client.IntegrationTests;

[Collection("RabbitMQ")]
public class ScenarioTest
{
    private readonly RabbitMqFixture fixture;

    public ScenarioTest(RabbitMqFixture fixture) => this.fixture = fixture;

    /// <summary>
    ///     Demonstrate how to create a virtual host, add some users, set permissions
    ///     and create exchanges, queues and bindings.
    /// </summary>
    [Fact]
    public async Task Should_be_able_to_provision_a_virtual_host()
    {
        // first create a new virtual host
        var vhost = await fixture.ManagementClient.CreateVhostAsync("my_virtual_host");

        // next create a user for that virtual host
        var user = await fixture.ManagementClient.CreateUserAsync(new UserInfo("mike", "topSecret").AddTag("administrator"))
            ;

        // give the new user all permissions on the virtual host
        await fixture.ManagementClient.CreatePermissionAsync(new PermissionInfo(user, vhost));

        // now log in again as the new user
        var management = new ManagementClient(fixture.Host, user.Name, "topSecret");

        // test that everything's OK
        await management.IsAliveAsync(vhost);

        // create an exchange
        var exchange = await management.CreateExchangeAsync(new ExchangeInfo("my_exchagne", "direct"), vhost)
            ;

        // create a queue
        var queue = await management.CreateQueueAsync(new QueueInfo("my_queue"), vhost);

        // bind the exchange to the queue
        await management.CreateBindingAsync(exchange, queue, new BindingInfo("my_routing_key"))
            ;

        // publish a test message
        await management.PublishAsync(exchange, new PublishInfo("my_routing_key", "Hello World!"))
            ;

        // get any messages on the queue
        var messages = await management
            .GetMessagesFromQueueAsync(queue, new GetMessagesCriteria(1, Ackmodes.ack_requeue_false))
            ;

        foreach (var message in messages) Console.Out.WriteLine("message.payload = {0}", message.Payload);
    }
}
