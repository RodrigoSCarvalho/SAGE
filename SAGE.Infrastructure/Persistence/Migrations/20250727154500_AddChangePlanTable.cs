using FluentMigrator;

namespace SAGE.Infrastructure.Persistence.Migrations;

[Migration(20250727154500)]
public class CreateChangePlan : Migration
{
    public override void Up()
    {
        Create.Table("ChangePlans")
          .WithColumn("Id").AsGuid().PrimaryKey()
          .WithColumn("Title").AsString(255).NotNullable()
          .WithColumn("Description").AsString(5000).NotNullable()
          .WithColumn("CreatedAt").AsDateTime().NotNullable()
          .WithColumn("CreatedBy").AsGuid().NotNullable();
    }
    public override void Down()
    {
        Delete.Table("ChangePlans");
    }
}