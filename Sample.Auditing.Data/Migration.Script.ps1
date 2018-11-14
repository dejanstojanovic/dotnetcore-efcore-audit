Add-Migration InitialCreate -Project Sample.Auditing.Data  -Context CatalogDbContext
Update-Database -Project Sample.Auditing.Data -Context CatalogDbContext