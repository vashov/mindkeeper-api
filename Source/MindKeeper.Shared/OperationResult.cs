namespace MindKeeper.Shared
{
    public class OperationResult<T>
    {
        public T Data { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool IsOk { get; private set; }

        private OperationResult()
        {
        }

        public static OperationResult<T> Ok(T data)
        {
            return new OperationResult<T>
            {
                Data = data,
                IsOk = true,
                ErrorMessage = string.Empty
            };
        }

        public static OperationResult<T> Error(string errorMessage)
        {
            return new OperationResult<T>
            {
                Data = default,
                IsOk = false,
                ErrorMessage = errorMessage
            };
        }

        public static explicit operator OperationResult(OperationResult<T> result)
        {
            return result.IsOk
                ? OperationResult.Ok()
                : OperationResult.Error(result.ErrorMessage);
        }
    }

    public class OperationResult
    {
        public string ErrorMessage { get; private set; }
        public bool IsOk { get; private set; }

        private OperationResult()
        {
        }

        public static OperationResult Ok()
        {
            return new OperationResult
            {
                IsOk = true,
                ErrorMessage = string.Empty
            };
        }

        public static OperationResult Error(string errorMessage)
        {
            return new OperationResult
            {
                IsOk = false,
                ErrorMessage = errorMessage
            };
        }

        public OperationResult<T> AsError<T>()
        {
            return OperationResult<T>.Error(this.ErrorMessage ?? "Unknown error.");
        }
    }
}
