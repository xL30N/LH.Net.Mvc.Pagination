# LH.Core.Mvc.Pagination
MVC package for pagination

```csharp
int totalItems = 100;
int pageSize = 10;
int currentPage = 1;

PaginatedList list = new PaginatedList(totalItems, currentPage, pageSize);
```

## Taghelper

```csharp
@addTagHelper *, LH.Core.Mvc.Pagination
```

Once the tag helper has been imported, it can be added to the required view and only requires the current page and total pages to generate the required HTML.

```csharp
<pagination page="@Model.CurrentPage" total-pages="@Model.TotalPages">
```

Additional route data can also be provided to the tag helper if required. The following example illustrates an additional object being passed to the tag helper to append filter to query string for the pagination links.

```csharp
<pagination page="@Model.CurrentPage" total-pages="@Model.TotalPages" route-data="@new { filter = ViewBag.Filter }"></pagination>
```