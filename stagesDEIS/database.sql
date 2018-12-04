[40m[32minfo[39m[22m[49m: Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core 2.1.4-rtm-31024 initialized 'ApplicationDbContext' using provider 'Microsoft.EntityFrameworkCore.Sqlite' with options: None
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

CREATE TABLE "AspNetRoles" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_AspNetRoles" PRIMARY KEY,
    "Name" TEXT NULL,
    "NormalizedName" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL
);

CREATE TABLE "AspNetUsers" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_AspNetUsers" PRIMARY KEY,
    "UserName" TEXT NULL,
    "NormalizedUserName" TEXT NULL,
    "Email" TEXT NULL,
    "NormalizedEmail" TEXT NULL,
    "EmailConfirmed" INTEGER NOT NULL,
    "PasswordHash" TEXT NULL,
    "SecurityStamp" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "PhoneNumberConfirmed" INTEGER NOT NULL,
    "TwoFactorEnabled" INTEGER NOT NULL,
    "LockoutEnd" TEXT NULL,
    "LockoutEnabled" INTEGER NOT NULL,
    "AccessFailedCount" INTEGER NOT NULL
);

CREATE TABLE "Company" (
    "CompanyId" INTEGER NOT NULL CONSTRAINT "PK_Company" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Address" TEXT NOT NULL,
    "Contact" TEXT NOT NULL
);

CREATE TABLE "Professor" (
    "ProfessorId" INTEGER NOT NULL CONSTRAINT "PK_Professor" PRIMARY KEY AUTOINCREMENT
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY AUTOINCREMENT,
    "RoleId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY AUTOINCREMENT,
    "UserId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT NULL,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" TEXT NOT NULL,
    "RoleId" TEXT NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" TEXT NOT NULL,
    "LoginProvider" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Value" TEXT NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Proposals" (
    "ProposalId" INTEGER NOT NULL CONSTRAINT "PK_Proposals" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "Date" TEXT NOT NULL,
    "State" INTEGER NOT NULL,
    "AccessConditions" TEXT NOT NULL,
    "Branch" INTEGER NOT NULL,
    "Objectives" TEXT NOT NULL,
    "ProfessorId" INTEGER NOT NULL,
    "CompanyId" INTEGER NOT NULL,
    "PlacedStudentId" INTEGER NULL,
    CONSTRAINT "FK_Proposals_Company_CompanyId" FOREIGN KEY ("CompanyId") REFERENCES "Company" ("CompanyId") ON DELETE CASCADE,
    CONSTRAINT "FK_Proposals_Professor_ProfessorId" FOREIGN KEY ("ProfessorId") REFERENCES "Professor" ("ProfessorId") ON DELETE CASCADE,
    CONSTRAINT "FK_Proposals_Student_PlacedStudentId" FOREIGN KEY ("PlacedStudentId") REFERENCES "Student" ("StudentId") ON DELETE RESTRICT
);

CREATE TABLE "Student" (
    "StudentId" INTEGER NOT NULL CONSTRAINT "PK_Student" PRIMARY KEY AUTOINCREMENT,
    "Branch" INTEGER NOT NULL,
    "ProposalId" INTEGER NULL,
    CONSTRAINT "FK_Student_Proposals_ProposalId" FOREIGN KEY ("ProposalId") REFERENCES "Proposals" ("ProposalId") ON DELETE RESTRICT
);

CREATE TABLE "Grade" (
    "GradeId" INTEGER NOT NULL CONSTRAINT "PK_Grade" PRIMARY KEY AUTOINCREMENT,
    "Subject" TEXT NOT NULL,
    "Pontuation" INTEGER NOT NULL,
    "StudentId" INTEGER NULL,
    CONSTRAINT "FK_Grade_Student_StudentId" FOREIGN KEY ("StudentId") REFERENCES "Student" ("StudentId") ON DELETE RESTRICT
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

CREATE INDEX "IX_Grade_StudentId" ON "Grade" ("StudentId");

CREATE INDEX "IX_Proposals_CompanyId" ON "Proposals" ("CompanyId");

CREATE INDEX "IX_Proposals_PlacedStudentId" ON "Proposals" ("PlacedStudentId");

CREATE INDEX "IX_Proposals_ProfessorId" ON "Proposals" ("ProfessorId");

CREATE INDEX "IX_Student_ProposalId" ON "Student" ("ProposalId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20181203165548_CreateDatabase', '2.1.4-rtm-31024');


