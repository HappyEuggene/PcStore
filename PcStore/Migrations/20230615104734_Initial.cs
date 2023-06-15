using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PcStore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemSpec",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Frequency = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    MemCapacity = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    MemType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Cores = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Power = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSpec_1", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ItemType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Manufacture = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Delivery = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    UserId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    ItemSpecId = table.Column<int>(type: "int", nullable: true),
                    ItemTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.id);
                    table.ForeignKey(
                        name: "FK_Item_ItemSpec_ItemSpecId",
                        column: x => x.ItemSpecId,
                        principalTable: "ItemSpec",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Item_ItemType_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    OrdersId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => new { x.ItemId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_OrderItem_Item",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_OrderItem_Orders",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemSpec",
                table: "Item",
                column: "ItemSpecId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemType",
                table: "Item",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_Orders",
                table: "OrderItem",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Users",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ItemSpec");

            migrationBuilder.DropTable(
                name: "ItemType");
        }
    }
}
