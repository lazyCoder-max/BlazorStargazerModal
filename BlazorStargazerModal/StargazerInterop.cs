
using BlazorStargazerModal.Interfaces;
using BlazorStargazerModal.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorStargazerModal
{
    public class StargazerInterop : IAsyncDisposable, IStargazerInterop
    {
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
        private readonly DotNetObjectReference<StargazerInterop> _jsRef;
        private bool _isWalletAvailable;
        public StargazerInterop(IJSRuntime jsRuntime)
        {
            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import",
                "./_content/BlazorStargazerModal/js/index.bundle.js").AsTask());
            _jsRef = DotNetObjectReference.Create(this);
        }

        public async Task EnsureWalletAvailability()
        {
            var module = await _moduleTask.Value;
            var stringResult = await module.InvokeAsync<string>("checkWalletAvailability");
            var result = JsonSerializer.Deserialize<ModalResult<bool>>(stringResult);
            if(result!=null && result.Success)
            {
                _isWalletAvailable = result.Result;
            }
        }

        public async Task ConnectWallet()
        {
            var module = await _moduleTask.Value;
            await EnsureWalletAvailability();
            var stringResult = await module.InvokeAsync<string>("activateStargazerProviders");
            var result = JsonSerializer.Deserialize<ModalResult<string>>(stringResult);
            await GetAddress(WalletType.DAG);
            await SignMessage(WalletType.DAG, "Welcome to Obius", null);
        }
        public async Task GetAddress(WalletType wallet)
        {
            var module = await _moduleTask.Value;
            await EnsureWalletAvailability();
            var provider = wallet == WalletType.DAG ? "dag_accounts" : "eth_accounts";
            var stringResult = await module.InvokeAsync<string>("getAddress", provider);
            var result = JsonSerializer.Deserialize<ModalResult<string[]>>(stringResult);
        }
        public async Task SignMessage(WalletType wallet, string message, object[] metadata)
        {
            var module = await _moduleTask.Value;
            await EnsureWalletAvailability();
            var provider = wallet == WalletType.DAG ? "dag_accounts" : "eth_accounts";
            var _metadata = new
            {
                field1 = metadata[0],
                field2 = metadata[1],
                field3 = metadata[2]
            };
            var stringResult = await module.InvokeAsync<string>("signConstellation", message, _metadata);
            var result = JsonSerializer.Deserialize<ModalResult<SignatureResult>>(stringResult);
        }
        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
