using AutoMapper;
using Core.Utilities.IoC;
using Core.Utilities.Paging;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Core.Extensions
{
    public static class GeneralExtensions
    {
        public static T ClearCircularReference<T>(this T model)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true, // JSON çıktısını düzenli yazmak için
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };

            // Serialize edip deserialize ederek circular referansı kaldırma
            string serializeModel = JsonSerializer.Serialize(model, options);
            return JsonSerializer.Deserialize<T>(serializeModel, options);
        }
        public static List<T> ToClearCircularList<T>(this IQueryable<T> items)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true, // JSON çıktısını düzenli yazmak için
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };

            // Serialize edip deserialize ederek circular referansı kaldırma
            string serializeModel = JsonSerializer.Serialize(items, options);
            return JsonSerializer.Deserialize<List<T>>(serializeModel, options);
        }
        public static async Task<List<T>> ToClearCircularListAsync<T>(this IQueryable<T> items)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true, // JSON çıktısını düzenli yazmak için
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };

            // Serialize edip deserialize ederek circular referansı kaldırma
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Serialize işlemini asenkron gerçekleştir
                await JsonSerializer.SerializeAsync(memoryStream, items, options);
                memoryStream.Seek(0, SeekOrigin.Begin); // Bellek akışını başa al

                // Deserialize işlemini asenkron gerçekleştir
                return await JsonSerializer.DeserializeAsync<List<T>>(memoryStream, options);
            }
        }
        public static Paginate<TTarget> ToMappedPaginate<TSource, TTarget>(this IPaginate<TSource> paginateModel)
        {
            IMapper mapper = ServiceTool.ServiceProvider.GetRequiredService<IMapper>();
            return new Paginate<TTarget>
            {
                From = paginateModel.From,
                Index = paginateModel.Index,
                Size = paginateModel.Size,
                Count = paginateModel.Count,
                Pages = paginateModel.Pages,
                Items = mapper.Map<List<TTarget>>(paginateModel.Items)
            };
        }
    }
}
