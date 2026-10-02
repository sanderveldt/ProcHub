using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MainShippingTermId",
                table: "Suppliers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MainLeadTimeDays",
                table: "Suppliers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SamplePaymentTermId",
                table: "Suppliers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    OrderDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    OrderType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    OrderStatus = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: false),
                    SupplierCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    SupplierName = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    PaymentTermId = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentTermDescription = table.Column<string>(type: "TEXT", nullable: false),
                    PaymentTiming = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DueDateReference = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    BalanceDueDays = table.Column<int>(type: "INTEGER", nullable: false),
                    BalanceDueDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    DepositPercentage = table.Column<decimal>(type: "TEXT", precision: 4, scale: 3, nullable: false, defaultValue: 0m),
                    DepositPaidDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    DepositPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    OrderAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    PaymentStatus = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    BalancePaidDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    OrderPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShippingTermId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShippingTermName = table.Column<string>(type: "TEXT", nullable: false),
                    ConfirmationReference = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    InvoiceReference = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    OrderNotes = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ShipmentSize = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    ShipmentReference = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    OrderConfirmationDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    RequestedProductionDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ProductionDoneDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    InvoiceDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ShippingDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    BillOfLadingDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ArrivalDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ExpectedDeliveryDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    DeliveryDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ProductionTimeDays = table.Column<int>(type: "INTEGER", nullable: true),
                    ShippingTimeDays = table.Column<int>(type: "INTEGER", nullable: true),
                    LeadTimeDays = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderConfirmationSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    InvoiceSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    OrderInspectionDone = table.Column<bool>(type: "INTEGER", nullable: false),
                    CertificateOfOriginSaved = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PaymentTerms_PaymentTermId",
                        column: x => x.PaymentTermId,
                        principalTable: "PaymentTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_ShippingTerms_ShippingTermId",
                        column: x => x.ShippingTermId,
                        principalTable: "ShippingTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SamplePaymentTermId",
                table: "Suppliers",
                column: "SamplePaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Code",
                table: "PurchaseOrders",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PaymentTermId",
                table: "PurchaseOrders",
                column: "PaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ShippingTermId",
                table: "PurchaseOrders",
                column: "ShippingTermId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_PaymentTerms_SamplePaymentTermId",
                table: "Suppliers",
                column: "SamplePaymentTermId",
                principalTable: "PaymentTerms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_PaymentTerms_SamplePaymentTermId",
                table: "Suppliers");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_SamplePaymentTermId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "SamplePaymentTermId",
                table: "Suppliers");

            migrationBuilder.AlterColumn<int>(
                name: "MainShippingTermId",
                table: "Suppliers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "MainLeadTimeDays",
                table: "Suppliers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
