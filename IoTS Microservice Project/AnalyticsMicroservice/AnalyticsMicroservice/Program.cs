using System;
using AnalyticsMicroservice.Contracts;
using AnalyticsMicroservice.Services;
using System.Threading.Tasks;
using System.Threading;

namespace AnalyticsMicroservice
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Thread.Sleep(TimeSpan.FromSeconds(60));
            IAnalyticsService analyticsService = new AnalyticsService();
            IUnitOfWork unitOfWork = new UnitOfWork();
            IMessageService messageService = new MessageService(unitOfWork);
            ICassandraService cassandraService = new CassandraService(unitOfWork);
            IConsumeService consumeService = new ConsumeService(unitOfWork, analyticsService, cassandraService, messageService);

            consumeService.Consume();

            Console.WriteLine("Press [Exit] to exit.");
            string sequence = Console.ReadLine();
            while(sequence != "Exit")
                sequence = Console.ReadLine();
        }
    }
}
