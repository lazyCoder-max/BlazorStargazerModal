// src/js/index.js
var walletAvailable = false;
var dagProvider;
var ethProvider;
function getErrorResponse(e) {
  let response = {
    result: null,
    error: e.reason ?? e.message ?? e,
    success: false
  };
  return JSON.stringify(response);
}
function getSuccessResponse(result) {
  let response = {
    result,
    error: null,
    success: true
  };
  return JSON.stringify(response);
}
async function checkWalletAvailability() {
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
async function activateStargazerProviders() {
  try {
    if (walletAvailable) {
      const walletProvider = window.stargazer;
      dagProvider = walletProvider.getProvider("constellation");
      await dagProvider.activate();
      ethProvider = walletProvider.getProvider("ethereum");
      await ethProvider.activate();
      return getSuccessResponse(provider);
    } else {
      return getErrorResponse("Wallet is not Available");
    }
  } catch (e) {
    return getErrorResponse(e);
  }
}
async function getAddress(providerName) {
  try {
    if (walletAvailable) {
      let provider2;
      if (providerName == "eth_accounts") {
        provider2 = ethProvider;
      } else if (providerName == "dag_accounts") {
        provider2 = dagProvider;
      }
      const account = await provider2.request({ method: providerName, params: [] });
      return getSuccessResponse(account);
    } else {
      return getErrorResponse("Wallet is not Available");
    }
  } catch (e) {
    return getErrorResponse(e);
  }
}
async function signConstellation(message, _metadata) {
  try {
    if (walletAvailable) {
      let provider2 = dagProvider;
      const signatureRequest = {
        content: message,
        metadata: _metadata
      };
      const utf8Encode = unescape(encodeURIComponent(JSON.stringify(signatureRequest)));
      const signatureRequestEncoded = window.btoa(utf8Encode);
      const account = await provider2.request({ method: "dag_accounts", params: [] });
      const userAddress = account[0];
      const signature = await provider2.request({
        method: "dag_signMessage",
        params: [userAddress, signatureRequestEncoded]
      });
      const publicKey = await provider2.request({ method: "dag_getPublicKey", params: [userAddress] });
      const payload = { signatureRequestEncoded, signature, publicKey };
      return getSuccessResponse({ signatureRequestEncoded, signature, publicKey });
    } else {
      return getErrorResponse("Wallet is not Available");
    }
  } catch (e) {
    return getErrorResponse(e);
  }
}
export {
  activateStargazerProviders,
  checkWalletAvailability,
  getAddress,
  signConstellation
};
//# sourceMappingURL=index1.bundle.js.map
