let walletAvailable = false;
let dagProvider;
let ethProvider;

function getErrorResponse(e) {
    let response = {
        result: null,
        error: e.reason ?? e.message ?? e,
        success: false
    }
    return JSON.stringify(response);
}
function getSuccessResponse(result) {
    let response = {
        result: result,
        error: null,
        success: true
    };
    return JSON.stringify(response);
}
function getSuccessResponseWithReplacer(result, replacer) {
    let response = {
        result: result,
        error: null,
        success: true
    };
    return JSON.stringify(response, replacer);

}

export async function checkWalletAvailability() {
    if (window.stargazer) {
        try {
            walletAvailable = true;
            return getSuccessResponse(walletAvailable);
        } catch (e) {
            getErrorResponse(e);
        }
    }
    walletAvailable = false;
    return getSuccessResponse(walletAvailable);
}
export async function activateStargazerProviders() {
    try {
        if (walletAvailable) {
            const walletProvider = window.stargazer;
            dagProvider = walletProvider.getProvider('constellation');
            await dagProvider.activate();

            ethProvider = walletProvider.getProvider('ethereum');
            await ethProvider.activate();
            return getSuccessResponse(provider);
        }
        else {
            return getErrorResponse("Wallet is not Available");
        }
    } catch (e) {
        return getErrorResponse(e);
    }
}
export async function getAddress(providerName) {
    try {
        if (walletAvailable) {
            let provider;
            if (providerName == "eth_accounts") {
                provider = ethProvider;
            }
            else if (providerName == "dag_accounts") {
                provider = dagProvider;
            }
            const account = await provider.request({ method: providerName, params: [] });

            return getSuccessResponse(account);
        }
        else {
            return getErrorResponse("Wallet is not Available");
        }
    } catch (e) {
        return getErrorResponse(e);
    }
}
export async function signConstellation(message, _metadata) {
    try {
        if (walletAvailable) {
            let provider = dagProvider;

            const signatureRequest = {
                content: message,
                metadata: {_metadata}
            };
            const utf8Encode = unescape(encodeURIComponent(JSON.stringify(signatureRequest)));
            const signatureRequestEncoded = window.btoa(utf8Encode);
            const account = await provider.request({ method: 'dag_accounts', params: [] });
            const userAddress = account[0];
            const signature = await provider.request({
                method: 'dag_signMessage',
                params: [userAddress, signatureRequestEncoded]
            });
            const publicKey = await provider.request({ method: 'dag_getPublicKey', params: [userAddress] });

            // Send your signature trio for further verification
            const payload = { signatureRequestEncoded, signature, publicKey };
            return getSuccessResponse({ signatureRequestEncoded, signature, publicKey });
        } else {
            return getErrorResponse("Wallet is not Available");
        }
    } catch (e) {
        return getErrorResponse(e);
    }
}
