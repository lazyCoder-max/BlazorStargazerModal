## BlazorStargazerModal
Blazor Stargazer Modal is a simple interop for connecting with Stargazer wallet for  ASP.NET or Blazor Web Applications.
the Existing Interop have the following functionalities at the moment:

 1. Wallet Connection
 2. Getting Address for both Dag and EVM compatiable chains
 3. Message Signing for signing Messages
 ### Usage:
 **Step 1:**  After the installation completed, initialize the modal into your service
 ``
 builder.Services.AddStargazerModal();
 ``
 **Step 2**: Usage Example (the following example assumes that you are using MudBlazor)

         @page "/"
        @using BlazorStargazerModal.Interfaces
        @rendermode InteractiveServer
        <PageTitle>Home</PageTitle>
        
        <h1>Hello, world!</h1>
        
        Welcome to your new app.
        <MudButton Variant="Variant.Filled" Color="Color.Primary" OnClick="ConnectWalletAsync">
            Connect Wallet
        </MudButton>
        <MudButton Variant="Variant.Filled" Color="Color.Primary" OnClick="Sign">
            Sign Message
        </MudButton>
        <br/>
        <MudText Typo="Typo.body1">
            @address
        </MudText>
        @code{
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
