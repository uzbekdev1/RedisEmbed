using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RE.Client.Core;
using RE.Client.Entities;
using RE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RE.Client;

namespace RE.Web.Jobs
{
    public class EmployeeStoreService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RedisProxy<Employee> _proxy;

        public EmployeeStoreService(RedisProxy<Employee> proxy,
            IServiceScopeFactory serviceScopeFactory,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EmployeeStoreService>();
            _serviceScopeFactory = serviceScopeFactory;
            _proxy = proxy;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cache Hosted Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                        var items = await context.Employees.Where(w => w.IsDeleted != true).ToArrayAsync(cancellationToken: cancellationToken);

                        _proxy.StoreAll(items);
                    }

                    await Task.Delay(24 * 3600 * 1000, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            _logger.LogInformation("Cache Hosted Service is stopping.");
        }
    }
}
