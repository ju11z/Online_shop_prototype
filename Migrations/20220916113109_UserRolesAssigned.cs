using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop_1.Migrations
{
    public partial class UserRolesAssigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"declare @userId nvarchar(50),@roleId int;
select @userId=Id from AspNetUsers where Email='aaa@gmail.com';
select @roleId=Id from AspNetRoles where Name='Admin';
insert into AspNetUserRoles(UserId,RoleId) values (@userId,@roleId);

select @userId=Id from AspNetUsers where Email='mmm@gmail.com';
select @roleId=Id from AspNetRoles where Name='Manager';
insert into AspNetUserRoles(UserId,RoleId) values (@userId,@roleId);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from AspNetUserRoles");
        }
    }
}
