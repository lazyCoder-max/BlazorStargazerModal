namespace BlazorStargazerModal.Interfaces
{
    public interface IStargazerInterop
    {
        ValueTask DisposeAsync();
        Task EnsureWalletAvailability();
        Task ConnectWallet();
    }
}