Add-Migration InitialCreate -Project Sample.History.Data  -Context CatalogDbContext
Update-Database -Project Sample.History.Data -Context CatalogDbContext