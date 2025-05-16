using Microsoft.EntityFrameworkCore;

namespace Core.Utilities.Paging;

public static class IQueryablePaginateExtensions
{
    public static async Task<IPaginate<T>> ToPaginateAsync<T>(this IQueryable<T> source, int index, int size,
                                                              int from = 0,
                                                              CancellationToken cancellationToken = default)
    {

        int count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
        List<T> items;
        if (index == -1 && size == -1)
            items = await source.ToListAsync(cancellationToken).ConfigureAwait(false);
        else
        {
            if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must from <= Index");
            items = await source.Skip((index - from) * size).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);
        }
        Paginate<T> list = new()
        {
            Index = index,
            Size = size,
            From = from,
            Count = count,
            Items = items,
            Pages = (int)Math.Ceiling(count / (double)size)
        };
        return list;
    }


    public static IPaginate<T> ToPaginate<T>(this IQueryable<T> source, int index, int size, int from = 0)
    {

        int count = source.Count();
        List<T> items;
        if (index == -1 && size == -1)
            items = source.ToList();
        else
        {
            if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must from <= Index");
            items = source.Skip((index - from) * size).Take(size).ToList();
        }
        Paginate<T> list = new()
        {
            Index = index,
            Size = size,
            From = from,
            Count = count,
            Items = items,
            Pages = index == -1 && size == -1 ? 0 : (int)Math.Ceiling(count / (double)size)
        };
        return list;
    }
}