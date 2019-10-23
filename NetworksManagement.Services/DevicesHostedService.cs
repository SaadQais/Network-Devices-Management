using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetworksManagement.Core;
using NetworksManagement.Data.Enums;
using NetworksManagement.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworksManagement.Services
{
    public class DevicesHostedService : IHostedService
    {
        private Timer _timer;
        public IServiceProvider ServiceProvider { get; }

        private const string baseUri = "https://localhost:44302/";

        public DevicesHostedService(IServiceProvider services)
        {
            ServiceProvider = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IDevicesRepository>();

                var devices = service.GetAll().Where(d => d.Type == DeviceType.Mikrotik).ToList();

                foreach (var device in devices)
                {
                    CallApiAsync(device).Wait();
                }
            }
        }

        private async Task<string> CallApiAsync(Device device)
        {
            StringBuilder result = new StringBuilder();

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(10);

                    ServicePointManager.ServerCertificateValidationCallback =
                        delegate (object s, X509Certificate certificate,
                                 X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        { return true; };

                    client.BaseAddress = new Uri(baseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Post Method 
                    var json = JsonConvert.SerializeObject(new { device.Id, device.Name });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await
                        client.PostAsync("Tools/DeviceVersion", content);

                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        result.Append("Task Completed " + Environment.NewLine);
                    }
                    else
                    {
                        result.Append("Cannot connect to the device");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Append(ex.Message + ",");
            }

            return result.ToString();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
