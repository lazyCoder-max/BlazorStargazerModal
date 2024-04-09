using BlazorStargazerModal.Interfaces;
using BlazorStargazerModal.Models;
using Microsoft.AspNetCore.Components;

namespace Demo.Components.Pages
{
    public partial class Home
    {
        [Inject] IStargazerInterop Stargazer { get; set; }
        private string address;
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        public async Task ConnectWalletAsync()
        {
            await Stargazer.ConnectWallet();
            var result = await Stargazer.GetAddress(WalletType.DAG);
            if (result.Success)
            {
                address = result.Result.GetValue(0).ToString();
            }
            StateHasChanged();
        }
        public void Sign()
        {
            var metaData = new { updateType = "MintMailBox", name = "MailBox2" };
            Stargazer.SignMessage(BlazorStargazerModal.Models.WalletType.DAG, @$"", metaData);
        }
    }
}
