namespace GoalSystems.WebApi.Business.Services.ItemService.Exceptions
{
    using System;

    [Serializable]
    public sealed class ItemDuplicatedNameException : Exception
    {
        private const string ExceptionMessage = "The name alredy exists.";
        private static string CustomMessage(string message) => $"{ExceptionMessage}{Environment.NewLine}{message}";

        internal ItemDuplicatedNameException() : base(ExceptionMessage) { }
        internal ItemDuplicatedNameException(string message) : base(CustomMessage(message)) { }
        internal ItemDuplicatedNameException(string message, Exception inner) : base(CustomMessage(message), inner) { }

        private ItemDuplicatedNameException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
