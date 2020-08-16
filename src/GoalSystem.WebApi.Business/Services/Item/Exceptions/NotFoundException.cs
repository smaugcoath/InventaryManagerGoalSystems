namespace GoalSystems.WebApi.Business.Services.ItemService.Exceptions
{
    using System;

    [Serializable]
    public sealed class NotFoundException : Exception
    {
        private const string ExceptionMessage = "The element does not exist.";
        private static string CustomMessage(string message) => $"{ExceptionMessage}{Environment.NewLine}{message}";

        internal NotFoundException() : base(ExceptionMessage) { }
        internal NotFoundException(string message) : base(CustomMessage(message)) { }
        internal NotFoundException(string message, Exception inner) : base(CustomMessage(message), inner) { }

        private NotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
