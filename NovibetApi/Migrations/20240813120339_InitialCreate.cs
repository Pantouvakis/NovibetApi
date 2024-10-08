﻿//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace NovibetApi.Migrations
//{
//    /// <inheritdoc />
//    public partial class InitialCreate : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "Countries",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
//                    TwoLetterCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
//                    ThreeLetterCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
//                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Countries", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "IPAddresses",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    CountryId = table.Column<int>(type: "int", nullable: false),
//                    IP = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
//                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
//                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_IPAddresses", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_IPAddresses_Countries_CountryId",
//                        column: x => x.CountryId,
//                        principalTable: "Countries",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_IPAddresses_CountryId",
//                table: "IPAddresses",
//                column: "CountryId");

//            migrationBuilder.CreateIndex(
//                name: "IX_IPAddresses_IP",
//                table: "IPAddresses",
//                column: "IP",
//                unique: true);
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "IPAddresses");

//            migrationBuilder.DropTable(
//                name: "Countries");
//        }
//    }
//}
