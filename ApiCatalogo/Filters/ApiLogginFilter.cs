using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiCatalogo.Filters
{
    public class ApiLogginFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public ApiLogginFilter(ILogger<ApiLogginFilter> logger)
        { _logger = logger; }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            _logger.LogInformation("Executando -> OnActionExecuting"); 
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Executando -> OnActionExecuted"); 
        }
    }
}
