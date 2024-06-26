﻿
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
                "./_content/BlazorStargazerModal/js/index1.bundle.js").AsTask());
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
        }
        public async Task<ModalResult<string[]>> GetAddress(WalletType wallet)
        {
            var module = await _moduleTask.Value;
            await EnsureWalletAvailability();
            var provider = wallet == WalletType.DAG ? "dag_accounts" : "eth_accounts";
            var stringResult = await module.InvokeAsync<string>("getAddress", provider);
            var result = JsonSerializer.Deserialize<ModalResult<string[]>>(stringResult);
            return result;
        }
        public async Task<ModalResult<SignatureResult>> SignMessage(WalletType wallet, string message, dynamic metadata)
        {
            var module = await _moduleTask.Value;
            await EnsureWalletAvailability();
            var provider = wallet == WalletType.DAG ? "dag_accounts" : "eth_accounts";
            var stringResult = await module.InvokeAsync<string>("signConstellation", new object[]{message, metadata});
            var result = JsonSerializer.Deserialize<ModalResult<SignatureResult>>(stringResult);
            return result;
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
