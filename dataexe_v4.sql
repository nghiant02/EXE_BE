-- --------------------------------------------------------
-- Host:                         localhost
-- Server version:               Microsoft SQL Server 2022 (RTM-CU9) (KB5030731) - 16.0.4085.2
-- Server OS:                    Linux (Ubuntu 22.04.3 LTS) <X64>
-- HeidiSQL Version:             12.7.0.6850
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES  */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for EXE201
CREATE DATABASE IF NOT EXISTS "EXE201";
USE "EXE201";

-- Dumping structure for table EXE201.Address
CREATE TABLE IF NOT EXISTS "Address" (
	"AddressID" INT NOT NULL,
	"UserID" INT NULL DEFAULT NULL,
	"Street" VARCHAR(255) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"City" VARCHAR(100) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"State" VARCHAR(100) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"PostalCode" VARCHAR(20) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"Country" VARCHAR(100) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	PRIMARY KEY ("AddressID"),
	FOREIGN KEY INDEX "FK__Address__UserID__5629CD9C" ("UserID"),
	CONSTRAINT "FK__Address__UserID__5629CD9C" FOREIGN KEY ("UserID") REFERENCES "User" ("UserID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.Address: 2 rows
/*!40000 ALTER TABLE "Address" DISABLE KEYS */;
INSERT INTO "Address" ("AddressID", "UserID", "Street", "City", "State", "PostalCode", "Country") VALUES
	(1, 3, '123 Main St', 'Metropolis', 'State', '12345', 'Country'),
	(2, 2, '456 Elm St', 'Smalltown', 'State', '67890', 'Country');
/*!40000 ALTER TABLE "Address" ENABLE KEYS */;

-- Dumping structure for table EXE201.Cart
CREATE TABLE IF NOT EXISTS "Cart" (
	"CartID" INT NOT NULL,
	"UserID" INT NULL DEFAULT NULL,
	"ProductID" INT NULL DEFAULT NULL,
	"Quantity" INT NULL DEFAULT NULL,
	PRIMARY KEY ("CartID"),
	FOREIGN KEY INDEX "FK__Cart__ProductID__571DF1D5" ("ProductID"),
	FOREIGN KEY INDEX "FK__Cart__UserID__5812160E" ("UserID"),
	CONSTRAINT "FK__Cart__UserID__5812160E" FOREIGN KEY ("UserID") REFERENCES "User" ("UserID") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK__Cart__ProductID__571DF1D5" FOREIGN KEY ("ProductID") REFERENCES "Product" ("ProductID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.Cart: 2 rows
/*!40000 ALTER TABLE "Cart" DISABLE KEYS */;
INSERT INTO "Cart" ("CartID", "UserID", "ProductID", "Quantity") VALUES
	(1, 3, 4, 1),
	(2, 3, 6, 2);
/*!40000 ALTER TABLE "Cart" ENABLE KEYS */;

-- Dumping structure for table EXE201.Category
CREATE TABLE IF NOT EXISTS "Category" (
	"CategoryID" INT NOT NULL,
	"CategoryName" VARCHAR(100) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"CategoryDescription" TEXT NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	PRIMARY KEY ("CategoryID")
);

-- Dumping data for table EXE201.Category: 3 rows
/*!40000 ALTER TABLE "Category" DISABLE KEYS */;
INSERT INTO "Category" ("CategoryID", "CategoryName", "CategoryDescription") VALUES
	(1, 'Dresses', 'Various styles of dresses'),
	(2, 'Suits', 'Formal and casual suits'),
	(3, 'Accessories', 'Fashion accessories');
/*!40000 ALTER TABLE "Category" ENABLE KEYS */;

-- Dumping structure for table EXE201.Deposit
CREATE TABLE IF NOT EXISTS "Deposit" (
	"DepositID" INT NOT NULL,
	"OrderID" INT NULL DEFAULT NULL,
	"UserID" INT NULL DEFAULT NULL,
	"DepositAmount" DECIMAL(10,2) NULL DEFAULT NULL,
	"DateDeposited" DATETIME NULL DEFAULT NULL,
	"DepositStatus" VARCHAR(15) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	PRIMARY KEY ("DepositID"),
	FOREIGN KEY INDEX "FK__Deposit__OrderID__59063A47" ("OrderID"),
	FOREIGN KEY INDEX "FK__Deposit__UserID__59FA5E80" ("UserID"),
	CONSTRAINT "FK__Deposit__OrderID__59063A47" FOREIGN KEY ("OrderID") REFERENCES "RentalOrder" ("OrderID") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK__Deposit__UserID__59FA5E80" FOREIGN KEY ("UserID") REFERENCES "User" ("UserID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.Deposit: 2 rows
/*!40000 ALTER TABLE "Deposit" DISABLE KEYS */;
INSERT INTO "Deposit" ("DepositID", "OrderID", "UserID", "DepositAmount", "DateDeposited", "DepositStatus") VALUES
	(1, 1, 3, 100, '2024-05-01 10:00:00.000', 'Completed'),
	(2, 2, 3, 150, '2024-05-02 11:00:00.000', 'Pending');
/*!40000 ALTER TABLE "Deposit" ENABLE KEYS */;

-- Dumping structure for table EXE201.Feedback
CREATE TABLE IF NOT EXISTS "Feedback" (
	"FeedbackID" INT NOT NULL,
	"UserID" INT NULL DEFAULT NULL,
	"ProductID" INT NULL DEFAULT NULL,
	"FeedbackComment" TEXT NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"FeedbackImage" VARCHAR(255) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"FeedbackStatus" VARCHAR(10) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"DateGiven" DATETIME NULL DEFAULT NULL,
	PRIMARY KEY ("FeedbackID"),
	FOREIGN KEY INDEX "FK__Feedback__Produc__5AEE82B9" ("ProductID"),
	FOREIGN KEY INDEX "FK__Feedback__UserID__5BE2A6F2" ("UserID"),
	CONSTRAINT "FK__Feedback__UserID__5BE2A6F2" FOREIGN KEY ("UserID") REFERENCES "User" ("UserID") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK__Feedback__Produc__5AEE82B9" FOREIGN KEY ("ProductID") REFERENCES "Product" ("ProductID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.Feedback: 2 rows
/*!40000 ALTER TABLE "Feedback" DISABLE KEYS */;
INSERT INTO "Feedback" ("FeedbackID", "UserID", "ProductID", "FeedbackComment", "FeedbackImage", "FeedbackStatus", "DateGiven") VALUES
	(1, 3, 1, 'The evening gown was perfect!', NULL, 'Public', '2024-05-08 12:00:00.000'),
	(2, 3, 3, 'The business suit was very comfortable.', NULL, 'Public', '2024-04-08 12:00:00.000');
/*!40000 ALTER TABLE "Feedback" ENABLE KEYS */;

-- Dumping structure for table EXE201.Inventory
CREATE TABLE IF NOT EXISTS "Inventory" (
	"InventoryID" INT NOT NULL,
	"ProductID" INT NULL DEFAULT NULL,
	"QuantityAvailable" INT NULL DEFAULT NULL,
	PRIMARY KEY ("InventoryID"),
	FOREIGN KEY INDEX "FK__Inventory__Produ__5CD6CB2B" ("ProductID"),
	CONSTRAINT "FK__Inventory__Produ__5CD6CB2B" FOREIGN KEY ("ProductID") REFERENCES "Product" ("ProductID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.Inventory: 6 rows
/*!40000 ALTER TABLE "Inventory" DISABLE KEYS */;
INSERT INTO "Inventory" ("InventoryID", "ProductID", "QuantityAvailable") VALUES
	(1, 1, 10),
	(2, 2, 5),
	(3, 3, 15),
	(4, 4, 20),
	(5, 5, 50),
	(6, 6, 30);
/*!40000 ALTER TABLE "Inventory" ENABLE KEYS */;

-- Dumping structure for table EXE201.Membership
CREATE TABLE IF NOT EXISTS "Membership" (
	"MembershipID" INT NOT NULL,
	"UserID" INT NULL DEFAULT NULL,
	"MembershipTypeID" INT NULL DEFAULT NULL,
	"StartDate" DATETIME NULL DEFAULT NULL,
	"EndDate" DATETIME NULL DEFAULT NULL,
	"MembershipStatus" VARCHAR(10) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	PRIMARY KEY ("MembershipID"),
	FOREIGN KEY INDEX "FK__Membershi__Membe__5DCAEF64" ("MembershipTypeID"),
	FOREIGN KEY INDEX "FK__Membershi__UserI__5EBF139D" ("UserID"),
	CONSTRAINT "FK__Membershi__UserI__5EBF139D" FOREIGN KEY ("UserID") REFERENCES "User" ("UserID") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK__Membershi__Membe__5DCAEF64" FOREIGN KEY ("MembershipTypeID") REFERENCES "MembershipType" ("MembershipTypeID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.Membership: 2 rows
/*!40000 ALTER TABLE "Membership" DISABLE KEYS */;
INSERT INTO "Membership" ("MembershipID", "UserID", "MembershipTypeID", "StartDate", "EndDate", "MembershipStatus") VALUES
	(1, 3, 1, '2024-01-01 00:00:00.000', '2024-12-31 00:00:00.000', 'Active'),
	(2, 2, 2, '2024-01-01 00:00:00.000', '2024-12-31 00:00:00.000', 'Active');
/*!40000 ALTER TABLE "Membership" ENABLE KEYS */;

-- Dumping structure for table EXE201.MembershipType
CREATE TABLE IF NOT EXISTS "MembershipType" (
	"MembershipTypeID" INT NOT NULL,
	"MembershipTypeName" VARCHAR(50) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"MembershipDescription" TEXT NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"MembershipBenefits" TEXT NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	PRIMARY KEY ("MembershipTypeID")
);

-- Dumping data for table EXE201.MembershipType: 2 rows
/*!40000 ALTER TABLE "MembershipType" DISABLE KEYS */;
INSERT INTO "MembershipType" ("MembershipTypeID", "MembershipTypeName", "MembershipDescription", "MembershipBenefits") VALUES
	(1, 'Gold', 'Gold membership with premium benefits', 'Free shipping, Discounts'),
	(2, 'Silver', 'Silver membership with standard benefits', 'Discounts, Early access');
/*!40000 ALTER TABLE "MembershipType" ENABLE KEYS */;

-- Dumping structure for table EXE201.Notification
CREATE TABLE IF NOT EXISTS "Notification" (
	"NotificationID" INT NOT NULL,
	"UserID" INT NULL DEFAULT NULL,
	"NotificationMessage" TEXT NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"DateSent" DATETIME NULL DEFAULT NULL,
	PRIMARY KEY ("NotificationID"),
	FOREIGN KEY INDEX "FK__Notificat__UserI__5FB337D6" ("UserID"),
	CONSTRAINT "FK__Notificat__UserI__5FB337D6" FOREIGN KEY ("UserID") REFERENCES "User" ("UserID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.Notification: 2 rows
/*!40000 ALTER TABLE "Notification" DISABLE KEYS */;
INSERT INTO "Notification" ("NotificationID", "UserID", "NotificationMessage", "DateSent") VALUES
	(1, 3, 'Your rental order has been placed successfully.', '2024-05-01 10:05:00.000'),
	(2, 3, 'Your rental order has been completed.', '2024-04-06 09:05:00.000');
/*!40000 ALTER TABLE "Notification" ENABLE KEYS */;

-- Dumping structure for table EXE201.Payment
CREATE TABLE IF NOT EXISTS "Payment" (
	"PaymentID" INT NOT NULL,
	"OrderID" INT NULL DEFAULT NULL,
	"UserID" INT NULL DEFAULT NULL,
	"PaymentAmount" DECIMAL(10,2) NULL DEFAULT NULL,
	"PaymentMethod" VARCHAR(50) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"PaymentStatus" VARCHAR(15) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	PRIMARY KEY ("PaymentID"),
	FOREIGN KEY INDEX "FK__Payment__OrderID__60A75C0F" ("OrderID"),
	FOREIGN KEY INDEX "FK__Payment__UserID__619B8048" ("UserID"),
	CONSTRAINT "FK__Payment__OrderID__60A75C0F" FOREIGN KEY ("OrderID") REFERENCES "RentalOrder" ("OrderID") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK__Payment__UserID__619B8048" FOREIGN KEY ("UserID") REFERENCES "User" ("UserID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.Payment: 2 rows
/*!40000 ALTER TABLE "Payment" DISABLE KEYS */;
INSERT INTO "Payment" ("PaymentID", "OrderID", "UserID", "PaymentAmount", "PaymentMethod", "PaymentStatus") VALUES
	(1, 1, 3, 200, 'Credit Card', 'Pending'),
	(2, 2, 3, 150, 'Credit Card', 'Completed');
/*!40000 ALTER TABLE "Payment" ENABLE KEYS */;

-- Dumping structure for table EXE201.Product
CREATE TABLE IF NOT EXISTS "Product" (
	"ProductID" INT NOT NULL,
	"ProductName" VARCHAR(100) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"ProductDescription" TEXT NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"ProductImage" VARCHAR(255) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"ProductStatus" VARCHAR(15) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"ProductPrice" FLOAT NULL DEFAULT NULL,
	"CategoryID" INT NULL DEFAULT NULL,
	PRIMARY KEY ("ProductID"),
	FOREIGN KEY INDEX "FK__Product__Categor__628FA481" ("CategoryID"),
	CONSTRAINT "FK__Product__Categor__628FA481" FOREIGN KEY ("CategoryID") REFERENCES "Category" ("CategoryID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.Product: 6 rows
/*!40000 ALTER TABLE "Product" DISABLE KEYS */;
INSERT INTO "Product" ("ProductID", "ProductName", "ProductDescription", "ProductImage", "ProductStatus", "ProductPrice", "CategoryID") VALUES
	(1, 'Evening Gown', 'Elegant evening gown perfect for formal events', NULL, 'Available', 100, 1),
	(2, 'Wedding Dress', 'Beautiful wedding dress with lace details', NULL, 'Available', 150, 1),
	(3, 'Business Suit', 'Classic business suit for formal occasions', NULL, 'Available', 120, 2),
	(4, 'Casual Suit', 'Stylish casual suit for everyday wear', NULL, 'Available', 80, 2),
	(5, 'Necklace', 'Elegant necklace to complement any outfit', NULL, 'Available', 20, 3),
	(6, 'Handbag', 'Chic handbag for all occasions', NULL, 'Available', 50, 3);
/*!40000 ALTER TABLE "Product" ENABLE KEYS */;

-- Dumping structure for table EXE201.Rating
CREATE TABLE IF NOT EXISTS "Rating" (
	"RatingID" INT NOT NULL,
	"UserID" INT NULL DEFAULT NULL,
	"ProductID" INT NULL DEFAULT NULL,
	"RatingValue" INT NULL DEFAULT NULL,
	"DateGiven" DATETIME NULL DEFAULT NULL,
	PRIMARY KEY ("RatingID"),
	FOREIGN KEY INDEX "FK__Rating__ProductI__6383C8BA" ("ProductID"),
	FOREIGN KEY INDEX "FK__Rating__UserID__6477ECF3" ("UserID"),
	CONSTRAINT "FK__Rating__UserID__6477ECF3" FOREIGN KEY ("UserID") REFERENCES "User" ("UserID") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK__Rating__ProductI__6383C8BA" FOREIGN KEY ("ProductID") REFERENCES "Product" ("ProductID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.Rating: 2 rows
/*!40000 ALTER TABLE "Rating" DISABLE KEYS */;
INSERT INTO "Rating" ("RatingID", "UserID", "ProductID", "RatingValue", "DateGiven") VALUES
	(1, 3, 1, 5, '2024-05-08 12:00:00.000'),
	(2, 3, 3, 4, '2024-04-08 12:00:00.000');
/*!40000 ALTER TABLE "Rating" ENABLE KEYS */;

-- Dumping structure for table EXE201.RentalOrder
CREATE TABLE IF NOT EXISTS "RentalOrder" (
	"OrderID" INT NOT NULL,
	"UserID" INT NULL DEFAULT NULL,
	"OrderStatus" VARCHAR(15) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"DatePlaced" DATETIME NULL DEFAULT NULL,
	"DueDate" DATETIME NULL DEFAULT NULL,
	"ReturnDate" DATETIME NULL DEFAULT NULL,
	"OrderTotal" DECIMAL(10,2) NULL DEFAULT NULL,
	PRIMARY KEY ("OrderID"),
	FOREIGN KEY INDEX "FK__RentalOrd__UserI__656C112C" ("UserID"),
	CONSTRAINT "FK__RentalOrd__UserI__656C112C" FOREIGN KEY ("UserID") REFERENCES "User" ("UserID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.RentalOrder: 2 rows
/*!40000 ALTER TABLE "RentalOrder" DISABLE KEYS */;
INSERT INTO "RentalOrder" ("OrderID", "UserID", "OrderStatus", "DatePlaced", "DueDate", "ReturnDate", "OrderTotal") VALUES
	(1, 3, 'Placed', '2024-05-01 10:00:00.000', '2024-05-07 10:00:00.000', NULL, 200),
	(2, 3, 'Completed', '2024-04-01 11:00:00.000', '2024-04-07 11:00:00.000', '2024-04-06 09:00:00.000', 150);
/*!40000 ALTER TABLE "RentalOrder" ENABLE KEYS */;

-- Dumping structure for table EXE201.RentalOrderDetails
CREATE TABLE IF NOT EXISTS "RentalOrderDetails" (
	"OrderDetailsID" INT NOT NULL,
	"OrderID" INT NULL DEFAULT NULL,
	"ProductID" INT NULL DEFAULT NULL,
	"Quantity" INT NULL DEFAULT NULL,
	"RentalStart" DATETIME NULL DEFAULT NULL,
	"RentalEnd" DATETIME NULL DEFAULT NULL,
	PRIMARY KEY ("OrderDetailsID"),
	FOREIGN KEY INDEX "FK__RentalOrd__Order__66603565" ("OrderID"),
	FOREIGN KEY INDEX "FK__RentalOrd__Produ__6754599E" ("ProductID"),
	CONSTRAINT "FK__RentalOrd__Order__66603565" FOREIGN KEY ("OrderID") REFERENCES "RentalOrder" ("OrderID") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK__RentalOrd__Produ__6754599E" FOREIGN KEY ("ProductID") REFERENCES "Product" ("ProductID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.RentalOrderDetails: 4 rows
/*!40000 ALTER TABLE "RentalOrderDetails" DISABLE KEYS */;
INSERT INTO "RentalOrderDetails" ("OrderDetailsID", "OrderID", "ProductID", "Quantity", "RentalStart", "RentalEnd") VALUES
	(1, 1, 1, 1, '2024-05-01 10:00:00.000', '2024-05-07 10:00:00.000'),
	(2, 1, 2, 1, '2024-05-01 10:00:00.000', '2024-05-07 10:00:00.000'),
	(3, 2, 3, 1, '2024-04-01 11:00:00.000', '2024-04-07 11:00:00.000'),
	(4, 2, 5, 1, '2024-04-01 11:00:00.000', '2024-04-07 11:00:00.000');
/*!40000 ALTER TABLE "RentalOrderDetails" ENABLE KEYS */;

-- Dumping structure for table EXE201.Role
CREATE TABLE IF NOT EXISTS "Role" (
	"RoleID" INT NOT NULL,
	"RoleName" VARCHAR(50) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"RoleDescription" VARCHAR(255) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	PRIMARY KEY ("RoleID")
);

-- Dumping data for table EXE201.Role: 3 rows
/*!40000 ALTER TABLE "Role" DISABLE KEYS */;
INSERT INTO "Role" ("RoleID", "RoleName", "RoleDescription") VALUES
	(1, 'Admin', 'Administrator with full access'),
	(2, 'Staff', 'Staff member with limited access'),
	(3, 'Customer', 'Regular customer with basic access');
/*!40000 ALTER TABLE "Role" ENABLE KEYS */;

-- Dumping structure for table EXE201.User
CREATE TABLE IF NOT EXISTS "User" (
	"UserID" INT NOT NULL,
	"UserName" VARCHAR(50) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"FullName" VARCHAR(100) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"Password" VARCHAR(255) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"Phone" VARCHAR(15) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"Gender" INT NULL DEFAULT NULL,
	"DateOfBirth" DATE NULL DEFAULT NULL,
	"Email" VARCHAR(100) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"ProfileImage" VARCHAR(255) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"AccountStatus" VARCHAR(10) NULL DEFAULT NULL COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	PRIMARY KEY ("UserID")
);

-- Dumping data for table EXE201.User: 4 rows
/*!40000 ALTER TABLE "User" DISABLE KEYS */;
INSERT INTO "User" ("UserID", "UserName", "FullName", "Password", "Phone", "Gender", "DateOfBirth", "Email", "ProfileImage", "AccountStatus") VALUES
	(1, 'admin01', 'Admin User', 'adminpass', '1234567890', 1, '1980-01-01', 'admin@example.com', NULL, 'Active'),
	(2, 'staff01', 'Staff User', 'staffpass', '0987654321', 1, '1990-01-01', 'staff@example.com', NULL, 'Active'),
	(3, 'customer01', 'Customer User', 'customerpass', '1122334455', 2, '2000-01-01', 'customer@example.com', NULL, 'Active'),
	(4, 'minhduc', 'Nguy?n Ð?c Minh', 'minhnd', 'string', 0, '2002-08-01', 'lammjnhphong4560@gmail.com', 'string', NULL);
/*!40000 ALTER TABLE "User" ENABLE KEYS */;

-- Dumping structure for table EXE201.UserRole
CREATE TABLE IF NOT EXISTS "UserRole" (
	"UserID" INT NOT NULL,
	"RoleID" INT NOT NULL,
	PRIMARY KEY ("RoleID", "UserID"),
	FOREIGN KEY INDEX "FK__UserRole__RoleID__68487DD7" ("RoleID"),
	FOREIGN KEY INDEX "FK__UserRole__UserID__693CA210" ("UserID"),
	CONSTRAINT "FK__UserRole__UserID__693CA210" FOREIGN KEY ("UserID") REFERENCES "User" ("UserID") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK__UserRole__RoleID__68487DD7" FOREIGN KEY ("RoleID") REFERENCES "Role" ("RoleID") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Dumping data for table EXE201.UserRole: 3 rows
/*!40000 ALTER TABLE "UserRole" DISABLE KEYS */;
INSERT INTO "UserRole" ("UserID", "RoleID") VALUES
	(1, 1),
	(2, 2),
	(3, 3);
/*!40000 ALTER TABLE "UserRole" ENABLE KEYS */;

-- Dumping structure for table EXE201.VerifyCode
CREATE TABLE IF NOT EXISTS "VerifyCode" (
	"Id" VARCHAR(50) NULL DEFAULT 'NULL' COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"Email" VARCHAR(50) NULL DEFAULT 'NULL' COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"Code" VARCHAR(50) NULL DEFAULT 'NULL' COLLATE 'SQL_Latin1_General_CP1_CI_AS',
	"CreateAt" DATETIME NULL DEFAULT 'NULL'
);

-- Dumping data for table EXE201.VerifyCode: 0 rows
/*!40000 ALTER TABLE "VerifyCode" DISABLE KEYS */;
/*!40000 ALTER TABLE "VerifyCode" ENABLE KEYS */;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
