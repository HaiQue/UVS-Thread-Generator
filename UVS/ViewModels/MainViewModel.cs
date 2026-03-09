using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using UVS.Models;
using UVS.Data;
using UVS.Services;

namespace UVS.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IDataGeneratorService _generator = new DataGeneratorService();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StartCommand))]
        private int _threadCount = 2;

        [ObservableProperty]
        private bool _isRunning;

        // 20 most recent items
        public ObservableCollection<GeneratedData> RecentLogs { get; } = new();

        private CancellationTokenSource? _cts;

        [RelayCommand(CanExecute = nameof(CanStart))]
        private async Task Start()
        {
            // Validate thread count
            if (ThreadCount < 2 || ThreadCount > 15) return;

            IsRunning = true;
            _cts = new CancellationTokenSource();
            RecentLogs.Clear();

            // Ensure database is ready
            using (var db = new AppDbContext())
            {
                await db.Database.EnsureCreatedAsync();
            }

            var tasks = new List<Task>();
            for (int i = 1; i <= ThreadCount; i++)
            {
                int currentId = i;
                // Independent threads
                tasks.Add(Task.Run(() => WorkerLoop(currentId, _cts.Token)));
            }

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                // Handled gracefully on Stop
            }
            finally
            {
                IsRunning = false;
            }
        }

        private async Task WorkerLoop(int threadId, CancellationToken token)
        {
            var rnd = new Random();
            while (!token.IsCancellationRequested)
            {
                // Random string length 5-10
                int length = rnd.Next(5, 11);

                string randomText = _generator.GenerateRandomString(length);

                var entry = new GeneratedData
                {
                    ThreadId = threadId,
                    Time = DateTime.Now,
                    Data = randomText
                };

                // Save to MS SQL
                using (var db = new AppDbContext())
                {
                    db.Logs.Add(entry);
                    await db.SaveChangesAsync(token);
                }

                // Update UI safely via Dispatcher
                App.Current.Dispatcher.Invoke(() =>
                {
                    RecentLogs.Insert(0, entry);
                    if (RecentLogs.Count > 20) RecentLogs.RemoveAt(20);
                });

                // Random interval 0.5 - 2 seconds
                await Task.Delay(rnd.Next(500, 2001), token);
            }
        }

        // Stop command
        [RelayCommand]
        private void Stop() => _cts?.Cancel();

        private bool CanStart() => !IsRunning && ThreadCount >= 2 && ThreadCount <= 15;
    }
}