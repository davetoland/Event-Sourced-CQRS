using System;

namespace CQRSE.Messages
{
    public interface ICommand
    {
        Guid Id { get; }
    }

    public class Command : ICommand
    {
        public Guid Id { get; set; }
    }

}