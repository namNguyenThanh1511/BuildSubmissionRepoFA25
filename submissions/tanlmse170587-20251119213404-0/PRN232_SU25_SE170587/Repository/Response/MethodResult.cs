using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Response
{
    public abstract record MethodResult<T>
    {
        public int StatusCode { get; }

        private MethodResult(int statusCode)
        {
            StatusCode = statusCode;
        }

        public abstract TOut Match<TOut>(
            Func<ErrorResponse, int, TOut> whenFailure,
            Func<T, int, TOut> whenSuccess);

        public abstract Task<MethodResult<TOut>> Bind<TOut>(
            Func<T, Task<MethodResult<TOut>>> f);
       
        public record Success(T Data, int Code) : MethodResult<T>(Code)
        {
            public override TOut Match<TOut>(
                Func<ErrorResponse, int, TOut> whenFailure,
                Func<T, int, TOut> whenSuccess)
                => whenSuccess(Data, StatusCode);

            public override async Task<MethodResult<TOut>> Bind<TOut>(
                Func<T, Task<MethodResult<TOut>>> f)
                => await f(Data);
        }

     
        public record Failure(ErrorResponse Error, int Code) : MethodResult<T>(Code)
        {
            public override TOut Match<TOut>(
                Func<ErrorResponse, int, TOut> whenFailure,
                Func<T, int, TOut> whenSuccess)
                => whenFailure(Error, StatusCode);

            public override Task<MethodResult<TOut>> Bind<TOut>(
                Func<T, Task<MethodResult<TOut>>> f)
                => Task.FromResult<MethodResult<TOut>>(new MethodResult<TOut>.Failure(Error, StatusCode));
        }
    }

}
