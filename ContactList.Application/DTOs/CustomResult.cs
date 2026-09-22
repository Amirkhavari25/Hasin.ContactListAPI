namespace ContactList.Application.DTOs
{
    public class CustomResult
    {
        public bool IsSuccess { get; protected set; }
        public string? ErrorMessage { get; protected set; }
        public List<string> ValidationErrors { get; protected set; } = [];
        public int? ErrorCode { get; protected set; }
        protected CustomResult()
        {
        }
        protected CustomResult(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
        }

        protected CustomResult(List<string> validationErrors)
        {
            IsSuccess = false;
            ValidationErrors = validationErrors;
            ErrorMessage = string.Join(", ", validationErrors);
        }

        public static CustomResult SuccessResult()
        {
            return new CustomResult
            {
                IsSuccess = true
            };
        }

        public static CustomResult FailureResult(string errorMessage)
        {
            return new CustomResult(errorMessage);
        }

        public static CustomResult FailureResult(List<string> validationErrors)
        {
            return new CustomResult(validationErrors);
        }

        public static CustomResult FailureResult(string errorMessage, int errorCode)
        {
            return new CustomResult(errorMessage)
            {
                ErrorCode = errorCode
            };
        }
    }
    public class CustomResult<T> : CustomResult
    where T : class
    {
        public T? Data { get; private set; }

        private CustomResult(T data)
        {
            IsSuccess = true;
            Data = data;
        }

        private CustomResult(string errorMessage)
            : base(errorMessage)
        {
        }

        private CustomResult(List<string> validationErrors)
            : base(validationErrors)
        {
        }

        public static CustomResult<T> SuccessResult(T data)
        {
            return new CustomResult<T>(data);
        }

        public static CustomResult<T> FailureResult(
            string errorMessage)
        {
            return new CustomResult<T>(errorMessage);
        }

        public static CustomResult<T> FailureResult(
            List<string> validationErrors)
        {
            return new CustomResult<T>(validationErrors);
        }

        public static CustomResult<T> FailureResult(
            string errorMessage,
            int errorCode)
        {
            return new CustomResult<T>(errorMessage)
            {
                ErrorCode = errorCode
            };
        }
    }
}
