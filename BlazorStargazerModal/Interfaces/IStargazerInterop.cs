using BlazorStargazerModal.Models;

namespace BlazorStargazerModal.Interfaces
{
    public interface IStargazerInterop
    {
        ValueTask DisposeAsync();
        Task EnsureWalletAvailability();
        Task ConnectWallet();
        Task<ModalResult<SignatureResult>> SignMessage(WalletType wallet, string message, object[] metadata);
        Task<ModalResult<string[]>> GetAddress(WalletType wallet);
    }
}