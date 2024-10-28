using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppointmentConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorUserId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorUserId",
                table: "Appointments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "556fac79-0f57-4e9e-8d28-664f29b342f7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "578c49f0-df51-4bbe-b121-dc9777fe0fcd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb3cf168-bb63-4a3a-9e63-fb09a385a49e");

            migrationBuilder.DropColumn(
                name: "DoctorUserId",
                table: "Appointments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "02f0fafe-5ffe-404b-998c-9895e87067de", null, "Admin", "ADMIN" },
                    { "78f0b83c-4bd1-49a8-8b5f-d56648be0a2c", null, "Patient", "PATIENT" },
                    { "be1524a1-2561-453d-8e01-47ad77955c6f", null, "Doctor", "DOCTOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "02f0fafe-5ffe-404b-998c-9895e87067de");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78f0b83c-4bd1-49a8-8b5f-d56648be0a2c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be1524a1-2561-453d-8e01-47ad77955c6f");

            migrationBuilder.AddColumn<string>(
                name: "DoctorUserId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "556fac79-0f57-4e9e-8d28-664f29b342f7", null, "Patient", "PATIENT" },
                    { "578c49f0-df51-4bbe-b121-dc9777fe0fcd", null, "Doctor", "DOCTOR" },
                    { "cb3cf168-bb63-4a3a-9e63-fb09a385a49e", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorUserId",
                table: "Appointments",
                column: "DoctorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorUserId",
                table: "Appointments",
                column: "DoctorUserId",
                principalTable: "Doctors",
                principalColumn: "UserId");
        }
    }
}
