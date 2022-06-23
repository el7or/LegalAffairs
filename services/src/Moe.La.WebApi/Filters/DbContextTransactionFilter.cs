using Microsoft.AspNetCore.Mvc.Filters;
using Moe.La.Infrastructure.DbContexts;
using System;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Filters
{
    public class DbContextTransactionFilter : IAsyncActionFilter
    {
        private readonly LaDbContext _dbContext;

        public DbContextTransactionFilter(LaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                await _dbContext.BeginTransactionAsync();

                var actionExecuted = await next();

                if (actionExecuted.Exception != null && !actionExecuted.ExceptionHandled)
                {
                    _dbContext.RollbackTransaction();
                }
                else
                {
                    await _dbContext.CommitTransactionAsync();
                }
            }
            catch (Exception)
            {
                _dbContext.RollbackTransaction();
                //throw;
            }
        }
    }
}
