echo "Purging old DB"
rm ActiveBear.db
rm -r Migrations
mkdir Migrations

echo "Adding migration"
dotnet ef migrations add InitialCreate --context ActiveBearContext

echo "Migrating..."
dotnet ef database update --context ActiveBearContext

echo
echo
echo "Schema replacement finished. Run solution to seed."
