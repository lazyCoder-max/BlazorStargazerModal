using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BlazorStargazerModal.Models
{
    public record ModalResult([property: JsonPropertyName("error"), JsonProperty("error")] string? Error)
    {
        public bool IsErrored => Error is not null;
    }
    public record ModalResult<T>(string? Error,
        [property: JsonPropertyName("result"), JsonProperty("result")] T? Result,
        [property: JsonPropertyName("success"), JsonProperty("success")] bool Success) : ModalResult(Error);
}
