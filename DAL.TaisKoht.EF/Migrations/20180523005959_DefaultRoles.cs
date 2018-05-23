using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DAL.TaisKoht.EF.Migrations
{
    public partial class DefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Role",
                new[] { "RoleId", "Name", "AccessLevel", "Description", "UpdateTime", "AddTime", "Active" },
                new object[] { 1, "admin", "9001", "System administrator with god-like abilities", DateTime.UtcNow, DateTime.UtcNow, true });

            migrationBuilder.InsertData("Role",
                new[] { "RoleId", "Name", "AccessLevel", "Description", "UpdateTime", "AddTime", "Active" },
                new object[] { 2, "premium", "1001", "Payed usr with advanced privileges", DateTime.UtcNow, DateTime.UtcNow, true });

            migrationBuilder.InsertData("Role",
                new[] { "RoleId", "Name", "AccessLevel", "Description", "UpdateTime", "AddTime", "Active" },
                new object[] { 3, "default", "1", "Normal user", DateTime.UtcNow, DateTime.UtcNow, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("Role", "Name", "admin");
            migrationBuilder.DeleteData("Role", "Name", "premium");
            migrationBuilder.DeleteData("Role", "Name", "default");
        }
    }
}
