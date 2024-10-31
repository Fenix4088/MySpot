using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Should;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Framework;

public class ServiceCollectionTests
{

    private interface IMessanger
    {
        void Send();
    }

    private class Messanger : IMessanger
    {
        private readonly Guid _id = Guid.NewGuid();
        public void Send() => Console.WriteLine(_id);
    }

    [Fact]
    public void test()
    {
        var serviceCollection = new ServiceCollection();
        //? Each time when we ask Framework to use some object implementation, each time a NEW object instace will be created
        // serviceCollection.AddTransient<>();
        // serviceCollection.AddTransient<IMessanger, Messanger>();
        
        //? Each time when we ask Framework to use some object implementation, each time a SAME object instace will be used
        //? we can use Singleton if object DO NOT change itself state
        // serviceCollection.AddSingleton();

        //? Hybrid between AddTransient and AddSingleton
        //? Scoped lifetime services are created once per request. 
        //serviceCollection.AddScoped();

        serviceCollection.AddScoped<IMessanger, Messanger>();


        var serviceProvider = serviceCollection.BuildServiceProvider();

        //? creating a scope to show how AddScope works
        using (var scope = serviceProvider.CreateScope())
        {
            var messenger = scope.ServiceProvider.GetRequiredService<IMessanger>();
            messenger.Send();
            
            var messenger2 = scope.ServiceProvider.GetRequiredService<IMessanger>();
            messenger2.Send();
            
            messenger.ShouldNotBeNull();
            messenger2.ShouldNotBeNull();
            messenger.ShouldBe(messenger2);
        }

    }
}