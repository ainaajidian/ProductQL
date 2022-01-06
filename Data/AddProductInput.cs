using System;

namespace ProductQL.Data
{
    public record AddProductInput(
        int? Id,
        string Name,
        int Stock,
        double Price,
        DateTime Created
     );
}
