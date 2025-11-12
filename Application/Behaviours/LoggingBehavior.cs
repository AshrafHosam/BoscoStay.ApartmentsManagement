using Application.Contracts.Helpers;
using Domain.LogEntities;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IDbLogger _dbLogger;
        public LoggingBehavior(IDbLogger dbLogger)
        {
            _dbLogger = dbLogger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ApiResponseLog logModel = new();
            logModel.RequestBody = JsonConvert.SerializeObject(request);
            logModel.RequestName = typeof(TRequest).Name;

            try
            {
                var response = await next();

                logModel.ResponseBody = JsonConvert.SerializeObject(response);

                var jsonObject = JObject.Parse(logModel.ResponseBody);
                if (jsonObject != null && jsonObject.ContainsKey("StatusCode"))
                    logModel.StatusCode = jsonObject["StatusCode"].Value<int>();

                return response;
            }
            catch (ValidationException ex)
            {
                logModel.StatusCode = 400;
                logModel.ResponseBody = JsonConvert.SerializeObject(ex.Errors);
                throw;
            }
            catch (Exception ex)
            {
                logModel.StatusCode = 500;
                logModel.ResponseBody = JsonConvert.SerializeObject(ex);
                throw;
            }
            finally
            {
                await _dbLogger.SaveLog(logModel);
            }
        }
    }
}
