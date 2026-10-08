using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProcHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DepositPercentage = table.Column<decimal>(type: "numeric(4,3)", precision: 4, scale: 3, nullable: false, defaultValue: 0m),
                    PaymentTiming = table.Column<int>(type: "integer", nullable: false),
                    DueDateReference = table.Column<int>(type: "integer", nullable: false),
                    BalanceDueDays = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTerms", x => x.Id);
                    table.CheckConstraint("CK_PaymentTerms_BalanceDueDays", "\"BalanceDueDays\" >= 0");
                    table.CheckConstraint("CK_PaymentTerms_DepositPercentage", "\"DepositPercentage\" >= 0 AND \"DepositPercentage\" <= 1");
                });

            migrationBuilder.CreateTable(
                name: "ShippingTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    FullName = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: true),
                    MainShippingTermId = table.Column<int>(type: "integer", nullable: false),
                    SecondaryShippingTermId = table.Column<int>(type: "integer", nullable: true),
                    SampleShippingTermId = table.Column<int>(type: "integer", nullable: true),
                    ShippingTimeDays = table.Column<int>(type: "integer", nullable: true),
                    ProductionTimeDays = table.Column<int>(type: "integer", nullable: true),
                    DefaultPaymentTermId = table.Column<int>(type: "integer", nullable: false),
                    SecondaryPaymentTermId = table.Column<int>(type: "integer", nullable: true),
                    SamplePaymentTermId = table.Column<int>(type: "integer", nullable: true),
                    MainLeadTimeDays = table.Column<int>(type: "integer", nullable: false),
                    SecondaryLeadTimeDays = table.Column<int>(type: "integer", nullable: true),
                    SampleLeadTimeDays = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_PaymentTerms_DefaultPaymentTermId",
                        column: x => x.DefaultPaymentTermId,
                        principalTable: "PaymentTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_PaymentTerms_SamplePaymentTermId",
                        column: x => x.SamplePaymentTermId,
                        principalTable: "PaymentTerms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Suppliers_PaymentTerms_SecondaryPaymentTermId",
                        column: x => x.SecondaryPaymentTermId,
                        principalTable: "PaymentTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_ShippingTerms_MainShippingTermId",
                        column: x => x.MainShippingTermId,
                        principalTable: "ShippingTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_ShippingTerms_SampleShippingTermId",
                        column: x => x.SampleShippingTermId,
                        principalTable: "ShippingTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_ShippingTerms_SecondaryShippingTermId",
                        column: x => x.SecondaryShippingTermId,
                        principalTable: "ShippingTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OrderDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OrderType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OrderStatus = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: false),
                    SupplierCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SupplierName = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    PaymentTermId = table.Column<int>(type: "integer", nullable: false),
                    PaymentTermDescription = table.Column<string>(type: "text", nullable: false),
                    PaymentTiming = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DueDateReference = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BalanceDueDays = table.Column<int>(type: "integer", nullable: false),
                    BalanceDueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DepositPercentage = table.Column<decimal>(type: "numeric(4,3)", precision: 4, scale: 3, nullable: false, defaultValue: 0m),
                    DepositPaidDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DepositPaid = table.Column<bool>(type: "boolean", nullable: false),
                    OrderAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DepositAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentStatus = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    BalancePaidDate = table.Column<DateOnly>(type: "date", nullable: true),
                    OrderPaid = table.Column<bool>(type: "boolean", nullable: false),
                    ShippingTermId = table.Column<int>(type: "integer", nullable: false),
                    ShippingTermName = table.Column<string>(type: "text", nullable: false),
                    ConfirmationReference = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    InvoiceReference = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    OrderNotes = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ShipmentSize = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    ShipmentReference = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    OrderConfirmationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    RequestedProductionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ProductionDoneDate = table.Column<DateOnly>(type: "date", nullable: true),
                    InvoiceDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ShippingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    BillOfLadingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ArrivalDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpectedDeliveryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DeliveryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ProductionTimeDays = table.Column<int>(type: "integer", nullable: true),
                    ShippingTimeDays = table.Column<int>(type: "integer", nullable: true),
                    LeadTimeDays = table.Column<int>(type: "integer", nullable: false),
                    OrderConfirmationSent = table.Column<bool>(type: "boolean", nullable: false),
                    InvoiceSent = table.Column<bool>(type: "boolean", nullable: false),
                    OrderInspectionDone = table.Column<bool>(type: "boolean", nullable: false),
                    CertificateOfOriginSaved = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_ShippingTerms_Name",
                table: "ShippingTerms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Code",
                table: "Suppliers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_DefaultPaymentTermId",
                table: "Suppliers",
                column: "DefaultPaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_MainShippingTermId",
                table: "Suppliers",
                column: "MainShippingTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Name",
                table: "Suppliers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SamplePaymentTermId",
                table: "Suppliers",
                column: "SamplePaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SampleShippingTermId",
                table: "Suppliers",
                column: "SampleShippingTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SecondaryPaymentTermId",
                table: "Suppliers",
                column: "SecondaryPaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SecondaryShippingTermId",
                table: "Suppliers",
                column: "SecondaryShippingTermId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "PaymentTerms");

            migrationBuilder.DropTable(
                name: "ShippingTerms");
        }
    }
}
