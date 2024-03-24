using BlazorStargazerModal.Interfaces;

namespace BlazorStargazerModal
{
    public static class StargazerModalServiceCollectionExtensions
    {
        public static IServiceCollection AddStargazerModal(this IServiceCollection services)
        {
            services.AddScoped<IStargazerInterop, StargazerInterop>();
            return services;
        }
    }
}
