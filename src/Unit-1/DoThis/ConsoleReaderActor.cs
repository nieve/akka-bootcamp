using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Shutdown"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        public const string StartCommand = "start";
        private readonly IActorRef _validationActor;

        public ConsoleReaderActor(IActorRef validationActor)
        {
            _validationActor = validationActor;
        }

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
                PrintInstructions();

            ValidateMessage();
        }

        private void ValidateMessage()
        {
            var message = Console.ReadLine();
            if (string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                UntypedActor.Context.System.Shutdown();
                return;
            }

            _validationActor.Tell(message);
        }

        private static void PrintInstructions()
        {
            Console.WriteLine("Please provide the URI of a log file on disk.\n");
        }

    }
}