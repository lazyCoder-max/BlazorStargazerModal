using System.Text.Json.Serialization;

namespace BlazorStargazerModal.Models
{
    /// <summary>
    /// RPC Urls
    /// </summary>
    /// <param name="Http">The http connection for the rpc</param>
    /// <param name="WebSocket">The websocket connection for the rpc</param>
    public record RpcUrl(
            [property: JsonPropertyName("http")] IReadOnlyList<string> Http,
            [property: JsonPropertyName("webSocket")] IReadOnlyList<string>? WebSocket
        );
    public record SignatureResult(
            [property: JsonPropertyName("signatureRequestEncoded")] string EncodedSignatureRequest,
            [property: JsonPropertyName("publicKey")] string PublicKey,
            [property: JsonPropertyName("signature")] string Signature
        );
    public record SignatureRequest(
            [property: JsonPropertyName("signatureRequestEncoded")] string EncodedSignatureRequest,
            [property: JsonPropertyName("publicKey")] string PublicKey,
            [property: JsonPropertyName("signature")] string Signature
        );
    public enum WalletType
    {
        DAG = 0,
        ETH = 1
    }
    
}
