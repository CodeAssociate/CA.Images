using CA.Blocks.Images.Resize;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Blocks.Images.DependencyInjection
{
    public static  class ImagesDependencyInjection
    {
        public static IServiceCollection AddCaBlocksImages(this IServiceCollection services)
        {
            services.AddTransient<IImageResizerLib, ImageResizerLib>();

            return services;
        }
    }
}
