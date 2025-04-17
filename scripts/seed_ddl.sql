CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE SEQUENCE "MySequence" AS integer START WITH 1000 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;

CREATE TABLE "Categories" (
    "Id" uuid NOT NULL,
    "Name" varchar(150) NOT NULL,
    "Code" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
);

CREATE TABLE "Payments" (
    "Id" uuid NOT NULL,
    "OrderId" uuid NOT NULL,
    "Status" varchar(100) NOT NULL,
    "Amount" numeric(18,2) NOT NULL,
    "CardName" varchar(250) NOT NULL,
    "CardNumber" varchar(16) NOT NULL,
    "CardExpiration" varchar(10) NOT NULL,
    "CardCvv" varchar(4) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Payments" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Username" varchar(100) NOT NULL,
    "Email" varchar(100) NOT NULL,
    "Phone" varchar(100) NOT NULL,
    "Password" varchar(100) NOT NULL,
    "Role" character varying(20) NOT NULL,
    "Status" character varying(20) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Vouchers" (
    "Id" uuid NOT NULL,
    "Code" varchar(100) NOT NULL,
    "Percentage" numeric,
    "DiscountValue" numeric,
    "Quantity" integer NOT NULL,
    "DiscountVoucherType" integer NOT NULL,
    "UsageDate" timestamp with time zone,
    "ExpirationDate" timestamp with time zone NOT NULL,
    "Active" boolean NOT NULL,
    "Used" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Vouchers" PRIMARY KEY ("Id")
);

CREATE TABLE "Products" (
    "Id" uuid NOT NULL,
    "CategoryId" uuid NOT NULL,
    "Title" varchar(150) NOT NULL,
    "Description" varchar(250) NOT NULL,
    "Price" numeric(18,2) NOT NULL,
    "Image" varchar(250),
    "QuantityStock" integer NOT NULL,
    "Rate" double precision,
    "Rating_Count" smallint,
    "Active" boolean NOT NULL,
    "Height" double precision,
    "Width" double precision,
    "Depth" double precision,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Products" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id")
);

CREATE TABLE "Transactions" (
    "Id" uuid NOT NULL,
    "OrderId" uuid NOT NULL,
    "PaymentId" uuid NOT NULL,
    "Total" numeric(18,2) NOT NULL,
    "TransactionStatus" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Transactions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Transactions_Payments_PaymentId" FOREIGN KEY ("PaymentId") REFERENCES "Payments" ("Id")
);

CREATE TABLE "Orders" (
    "Id" uuid NOT NULL,
    "Code" integer NOT NULL DEFAULT (nextval('"MySequence"')),
    "CustomerId" uuid NOT NULL,
    "VoucherId" uuid,
    "IsVoucherUsed" boolean NOT NULL,
    "Discount" numeric(18,2) NOT NULL,
    "TotalValue" numeric(18,2) NOT NULL,
    "Status" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Orders" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Orders_Vouchers_VoucherId" FOREIGN KEY ("VoucherId") REFERENCES "Vouchers" ("Id")
);

CREATE TABLE "OrderItems" (
    "Id" uuid NOT NULL,
    "OrderId" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "ProductName" varchar(250) NOT NULL,
    "Quantity" integer NOT NULL,
    "UnitPrice" numeric(18,2) NOT NULL,
    "Discount" numeric(18,2) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_OrderItems" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id")
);

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

CREATE INDEX "IX_Orders_VoucherId" ON "Orders" ("VoucherId");

CREATE INDEX "IX_Products_CategoryId" ON "Products" ("CategoryId");

CREATE UNIQUE INDEX "IX_Transactions_PaymentId" ON "Transactions" ("PaymentId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250414144548_InitialMigration', '8.0.10');

COMMIT;

