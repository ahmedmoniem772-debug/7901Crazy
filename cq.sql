/*
 Navicat Premium Dump SQL

 Source Server         : localhost_3306
 Source Server Type    : MySQL
 Source Server Version : 50715 (5.7.15-log)
 Source Host           : localhost:3306
 Source Schema         : cq

 Target Server Type    : MySQL
 Target Server Version : 50715 (5.7.15-log)
 File Encoding         : 65001

 Date: 12/05/2026 20:55:11
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for a_news
-- ----------------------------
DROP TABLE IF EXISTS `a_news`;
CREATE TABLE `a_news`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `news` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `title` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `date` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `writer` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 16 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of a_news
-- ----------------------------
INSERT INTO `a_news` VALUES (15, '<div align=\"center\"><font size=\"4\"><b><font color=\"#993333\">New We</font><b><font color=\"#993333\">b Site Giants </font><br><br><font color=\"#CC6633\"># </font><br><br><font color=\"#CC3300\">#</font><br></b></b></font></div>', 'TopConquer', 'Jun 28, 2014', 'AbdoOo');

-- ----------------------------
-- Table structure for accounts
-- ----------------------------
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `Username` char(25) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `Password` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `Name` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `HDSerial` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `EarthID` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Email` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `IP` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `TokenChangePass` varchar(220) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `LastCheck` bigint(20) UNSIGNED NULL DEFAULT 0,
  `State` tinyint(3) UNSIGNED NULL DEFAULT 0,
  `Question` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Code` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Country` char(110) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `City` char(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `secretquestion` char(45) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `realname` char(25) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `machine` char(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `lastvote` char(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `mobilenumber` bigint(20) NULL DEFAULT 0,
  `securitycode` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `date` varchar(0) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `joined` varchar(220) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Online` tinyint(3) UNSIGNED NULL DEFAULT 0,
  `ViewCps` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ViewProfile` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ViewMoney` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `SecretCode` smallint(6) NULL DEFAULT 0,
  `answer` char(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Olduid` bigint(20) NULL DEFAULT NULL,
  PRIMARY KEY (`Username`) USING BTREE,
  UNIQUE INDEX `f43f43`(`ID`) USING BTREE,
  INDEX `ggg`(`Username`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 28481296 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of accounts
-- ----------------------------
INSERT INTO `accounts` VALUES (28481295, '110', '110', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);
INSERT INTO `accounts` VALUES (28481294, '100', '100', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);
INSERT INTO `accounts` VALUES (28481293, '90', '90', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);
INSERT INTO `accounts` VALUES (28481292, '80', '80', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);
INSERT INTO `accounts` VALUES (28481291, '70', '70', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);
INSERT INTO `accounts` VALUES (28481290, '60', '60', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);
INSERT INTO `accounts` VALUES (28481289, '30', '30', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);
INSERT INTO `accounts` VALUES (28481288, '20', '20', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);
INSERT INTO `accounts` VALUES (28481287, '10', '10', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);
INSERT INTO `accounts` VALUES (28481286, '40', '40', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);
INSERT INTO `accounts` VALUES (28481285, 'VirusX', 'Hossny@015', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);
INSERT INTO `accounts` VALUES (28481284, '50', '50', '', NULL, NULL, '', NULL, NULL, 0, 0, '', '', '', '', '', '', '', '', 0, '', '', NULL, 0, '', '', '', 0, '', NULL);

-- ----------------------------
-- Table structure for admin
-- ----------------------------
DROP TABLE IF EXISTS `admin`;
CREATE TABLE `admin`  (
  `user_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `user_name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `pass` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `status` enum('admin') CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`user_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of admin
-- ----------------------------

-- ----------------------------
-- Table structure for background
-- ----------------------------
DROP TABLE IF EXISTS `background`;
CREATE TABLE `background`  (
  `background_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `image` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`background_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 7 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of background
-- ----------------------------

-- ----------------------------
-- Table structure for bandmacid
-- ----------------------------
DROP TABLE IF EXISTS `bandmacid`;
CREATE TABLE `bandmacid`  (
  `MacID` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0000000000000000'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of bandmacid
-- ----------------------------

-- ----------------------------
-- Table structure for bankcps
-- ----------------------------
DROP TABLE IF EXISTS `bankcps`;
CREATE TABLE `bankcps`  (
  `UID` int(11) NOT NULL DEFAULT 0,
  `Cps` bigint(20) NULL DEFAULT 0,
  PRIMARY KEY (`UID`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = FIXED;

-- ----------------------------
-- Records of bankcps
-- ----------------------------

-- ----------------------------
-- Table structure for banned
-- ----------------------------
DROP TABLE IF EXISTS `banned`;
CREATE TABLE `banned`  (
  `username` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `Hours` bigint(20) UNSIGNED NOT NULL DEFAULT 0,
  `StartBan` bigint(20) UNSIGNED NOT NULL DEFAULT 0,
  `Reason` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`username`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of banned
-- ----------------------------

-- ----------------------------
-- Table structure for banner
-- ----------------------------
DROP TABLE IF EXISTS `banner`;
CREATE TABLE `banner`  (
  `banner_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `image` varchar(110) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `title` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `s_title` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`banner_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of banner
-- ----------------------------
INSERT INTO `banner` VALUES (2, 'banner_gift', 'https://4.top4top.net/p_1445f59la1.gif', 'ClashConquer');

-- ----------------------------
-- Table structure for charge
-- ----------------------------
DROP TABLE IF EXISTS `charge`;
CREATE TABLE `charge`  (
  `Username` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Numberofcard` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Value` varchar(15) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UID` bigint(20) NULL DEFAULT NULL,
  `case_card` set('','ClaimNow','Done','Failed','Waiting') CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT 'Waiting',
  `FailReason` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT 'None'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of charge
-- ----------------------------
INSERT INTO `charge` VALUES ('DarkHer', '19383560568355', '50', 10278804, 'Done', 'None');
INSERT INTO `charge` VALUES ('kariemkhaled', '2037580107688581', '150', 10279828, 'Done', 'None');

-- ----------------------------
-- Table structure for comments
-- ----------------------------
DROP TABLE IF EXISTS `comments`;
CREATE TABLE `comments`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `postid` int(11) NOT NULL,
  `comment` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `user` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `date` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `writerid` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of comments
-- ----------------------------
INSERT INTO `comments` VALUES (1, 112, 'GGGGGG', '25', 'DF', 1);

-- ----------------------------
-- Table structure for configuration
-- ----------------------------
DROP TABLE IF EXISTS `configuration`;
CREATE TABLE `configuration`  (
  `EntityID` bigint(20) UNSIGNED NOT NULL DEFAULT 1000000,
  `Server` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `PlayersOnline` bigint(20) NULL DEFAULT 0,
  PRIMARY KEY (`Server`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of configuration
-- ----------------------------
INSERT INTO `configuration` VALUES (88888070, 'VirusX', 0);

-- ----------------------------
-- Table structure for cpanel
-- ----------------------------
DROP TABLE IF EXISTS `cpanel`;
CREATE TABLE `cpanel`  (
  `Website_Name` varchar(18) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Website_url` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Domain` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `date` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Time` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Email` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `password` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Host` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Port` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `SMTPSecure` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `mate` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `sitemap` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `GM` bigint(20) NULL DEFAULT NULL,
  `stylee` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Transfer` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `version` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `max_execution` bigint(20) NULL DEFAULT NULL,
  `King` bigint(20) NULL DEFAULT NULL,
  `prince` bigint(20) NULL DEFAULT NULL,
  `Doke` bigint(20) NULL DEFAULT NULL,
  `limits` bigint(20) NULL DEFAULT NULL,
  `boy` bigint(20) NULL DEFAULT NULL,
  `girl` bigint(20) NULL DEFAULT NULL,
  `language` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `link_paypal` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `email_paypal` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `logo` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of cpanel
-- ----------------------------
INSERT INTO `cpanel` VALUES ('Test', 'http://www.RotaConquer.ml', NULL, 'Africa/Cairo', 'Y/m/d h:i:s', '0', '0', 'smtp.gmail.com', '465', 'ssl', 'null', 'true', 4, '\r\n', 1, 'True', 'TITANIUM', 60, 3, 16, 34, 10, 3, 2, 'Einglish', '0', '0', '0');

-- ----------------------------
-- Table structure for cps_rank
-- ----------------------------
DROP TABLE IF EXISTS `cps_rank`;
CREATE TABLE `cps_rank`  (
  `UID` bigint(20) UNSIGNED NULL DEFAULT NULL,
  `Name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `cps` bigint(20) NULL DEFAULT NULL,
  `Class` bigint(20) NULL DEFAULT NULL,
  `Level` bigint(20) NULL DEFAULT NULL
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of cps_rank
-- ----------------------------

-- ----------------------------
-- Table structure for crossonline
-- ----------------------------
DROP TABLE IF EXISTS `crossonline`;
CREATE TABLE `crossonline`  (
  `Name` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `OnlineCount` int(11) NULL DEFAULT 0,
  PRIMARY KEY (`Name`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of crossonline
-- ----------------------------

-- ----------------------------
-- Table structure for ctfranks
-- ----------------------------
DROP TABLE IF EXISTS `ctfranks`;
CREATE TABLE `ctfranks`  (
  `UID` bigint(20) NULL DEFAULT NULL,
  `GuildName` varchar(25) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `GuildLeader` varchar(25) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CTFPoints` bigint(20) NULL DEFAULT NULL
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of ctfranks
-- ----------------------------

-- ----------------------------
-- Table structure for donation_paypal
-- ----------------------------
DROP TABLE IF EXISTS `donation_paypal`;
CREATE TABLE `donation_paypal`  (
  `id_pay` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `time` datetime NOT NULL,
  `account_name` varchar(60) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `pachet_id` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `sv_id` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `successed` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`id_pay`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 2139106 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of donation_paypal
-- ----------------------------

-- ----------------------------
-- Table structure for donationmoney
-- ----------------------------
DROP TABLE IF EXISTS `donationmoney`;
CREATE TABLE `donationmoney`  (
  `UID` bigint(20) NOT NULL,
  `Name` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Cart` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`UID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of donationmoney
-- ----------------------------

-- ----------------------------
-- Table structure for download
-- ----------------------------
DROP TABLE IF EXISTS `download`;
CREATE TABLE `download`  (
  `download_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `from` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `url` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `type` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`download_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 5 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of download
-- ----------------------------

-- ----------------------------
-- Table structure for event
-- ----------------------------
DROP TABLE IF EXISTS `event`;
CREATE TABLE `event`  (
  `event_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `description` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`event_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of event
-- ----------------------------
INSERT INTO `event` VALUES (1, '<table align=\"center\" border=\"1\" cellpadding=\"1\" cellspacing=\"1\" style=\"height:100px; width:900px\">\r\n	<thead>\r\n		<tr>\r\n			<th scope=\"col\"><span style=\"font-size:14px\"><strong>Monday</strong></span></th>\r\n			<th scope=\"col\"><span style=\"font-size:14px\"><strong>Tuesday</strong></span></th>\r\n			<th scope=\"col\"><span style=\"font-size:14px\"><strong>Wednesday</strong></span></th>\r\n			<th scope=\"col\"><span style=\"font-size:14px\"><strong>Thursday</strong></span></th>\r\n			<th scope=\"col\"><span style=\"font-size:14px\"><strong>Friday</strong></span></th>\r\n			<th scope=\"col\"><span style=\"font-size:14px\"><strong>Saturday</strong></span></th>\r\n			<th scope=\"col\"><span style=\"font-size:14px\"><strong>Sunday</strong></span></th>\r\n		</tr>\r\n	</thead>\r\n	<tbody>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Horse Racing</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Horse Racing</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Horse Racing</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Horse Racing</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Horse Racing</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Horse Racing</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Horse Racing</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Extreme PK</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Extreme PK</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Extreme PK</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Extreme PK</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Extreme PK</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Extreme PK</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Extreme PK</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Clan War</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Clan War</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Clan War</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Clan War</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Clan War</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Clan War</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Clan War</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DBShower</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DBShower</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DBShower</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DBShower</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DBShower</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DBShower</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DBShower</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DragonWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DragonWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DragonWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DragonWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DragonWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DragonWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>DragonWar</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FootBall</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FootBall</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FootBall</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FootBall</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FootBall</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FootBall</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FootBall</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>LastManStand</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>LastManStand</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>LastManStand</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>LastManStand</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>LastManStand</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>LastManStand</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>LastManStand</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>KillerOfElite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>KillerOfElite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>KillerOfElite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>KillerOfElite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>KillerOfElite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>KillerOfElite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>KillerOfElite</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FreezeWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FreezeWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FreezeWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FreezeWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FreezeWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FreezeWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>FreezeWar</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>squidwardoctops</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>squidwardoctops</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>squidwardoctops</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>squidwardoctops</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>squidwardoctops</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>squidwardoctops</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>squidwardoctops</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>skillTournament</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>skillTournament</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>skillTournament</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>skillTournament</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>skillTournament</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>skillTournament</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>skillTournament</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Battlefield</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Battlefield</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Battlefield</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Battlefield</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Battlefield</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Battlefield</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Battlefield</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ChaosGuard</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ChaosGuard</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ChaosGuard</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ChaosGuard</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ChaosGuard</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ChaosGuard</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ChaosGuard</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Treasure Thief</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Treasure Thief</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Treasure Thief</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Treasure Thief</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Treasure Thief</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Treasure Thief</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Treasure Thief</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Dis City</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Dis City</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Dis City</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Dis City</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Dis City</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Dis City</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Dis City</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Nemesis</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Nemesis</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Nemesis</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Nemesis</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Nemesis</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Nemesis</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Nemesis</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>SnowBanshee</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Banshee</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Banshee</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Banshee</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Banshee</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Banshee</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Banshee</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Terato Dragon</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Terato Dragon</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Terato Dragon</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Terato Dragon</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Terato Dragon</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Terato Dragon</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Terato Dragon</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Killer Elite</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>titan</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>titan</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>titan</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>titan</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>titan</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>titan</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>titan</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ganoderma</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ganoderma</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ganoderma</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ganoderma</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ganoderma</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ganoderma</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>ganoderma</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Team PK</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Elite PK</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Weekly PK</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Class PK</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Capture The Flag</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>Guild War</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>SuperGuildWar</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>SkillTeamPk</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n		</tr>\r\n		<tr>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>PowerArena</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n			<td style=\"text-align:center\"><span style=\"font-size:14px\"><strong>********</strong></span></td>\r\n		</tr>\r\n	</tbody>\r\n</table>\r\n\r\n<p>&nbsp;</p>\r\n');

-- ----------------------------
-- Table structure for goldprize
-- ----------------------------
DROP TABLE IF EXISTS `goldprize`;
CREATE TABLE `goldprize`  (
  `UID` bigint(20) UNSIGNED NOT NULL DEFAULT 0,
  `Name` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `Goldprize` bigint(20) UNSIGNED NOT NULL DEFAULT 1,
  `Number` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Number`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 151 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of goldprize
-- ----------------------------

-- ----------------------------
-- Table structure for koboardranking
-- ----------------------------
DROP TABLE IF EXISTS `koboardranking`;
CREATE TABLE `koboardranking`  (
  `UID` int(11) NOT NULL,
  `PlayerName` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Rank` int(11) NULL DEFAULT NULL,
  `Points` int(11) NULL DEFAULT NULL,
  `PerviousRank` int(11) NULL DEFAULT NULL,
  `PerviousPoints` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`UID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of koboardranking
-- ----------------------------

-- ----------------------------
-- Table structure for likes
-- ----------------------------
DROP TABLE IF EXISTS `likes`;
CREATE TABLE `likes`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `postid` int(11) NOT NULL,
  `user` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of likes
-- ----------------------------

-- ----------------------------
-- Table structure for log_payments
-- ----------------------------
DROP TABLE IF EXISTS `log_payments`;
CREATE TABLE `log_payments`  (
  `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT,
  `user` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `alert` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `IP` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of log_payments
-- ----------------------------

-- ----------------------------
-- Table structure for logo
-- ----------------------------
DROP TABLE IF EXISTS `logo`;
CREATE TABLE `logo`  (
  `logo_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `image` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`logo_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of logo
-- ----------------------------

-- ----------------------------
-- Table structure for ls_banpcs
-- ----------------------------
DROP TABLE IF EXISTS `ls_banpcs`;
CREATE TABLE `ls_banpcs`  (
  `HDSerial_int` int(11) NOT NULL AUTO_INCREMENT,
  `HDSerial` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PCName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Reason` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `StartBan` bigint(20) NOT NULL,
  `Hours` int(11) NOT NULL,
  `Permenant` int(11) NOT NULL,
  PRIMARY KEY (`HDSerial_int`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of ls_banpcs
-- ----------------------------

-- ----------------------------
-- Table structure for ls_banplayers
-- ----------------------------
DROP TABLE IF EXISTS `ls_banplayers`;
CREATE TABLE `ls_banplayers`  (
  `UID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Reason` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `StartBan` bigint(20) NOT NULL,
  `Hours` int(11) NOT NULL,
  `Permenant` int(11) NOT NULL,
  PRIMARY KEY (`UID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of ls_banplayers
-- ----------------------------

-- ----------------------------
-- Table structure for ls_gw
-- ----------------------------
DROP TABLE IF EXISTS `ls_gw`;
CREATE TABLE `ls_gw`  (
  `ID` int(11) NOT NULL,
  `WinnerGuild` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT '0',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of ls_gw
-- ----------------------------

-- ----------------------------
-- Table structure for macs
-- ----------------------------
DROP TABLE IF EXISTS `macs`;
CREATE TABLE `macs`  (
  `id` char(15) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `Online` tinyint(4) NULL DEFAULT 0,
  `mac` varchar(220) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `accounts` tinyint(3) UNSIGNED NULL DEFAULT 0,
  `EntityID` bigint(20) UNSIGNED NULL DEFAULT 0,
  `macs` varchar(220) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of macs
-- ----------------------------

-- ----------------------------
-- Table structure for macs1
-- ----------------------------
DROP TABLE IF EXISTS `macs1`;
CREATE TABLE `macs1`  (
  `id` char(15) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `Online` tinyint(4) NULL DEFAULT 0,
  `mac` varchar(220) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `accounts` tinyint(3) UNSIGNED NULL DEFAULT 0,
  `EntityID` bigint(20) UNSIGNED NULL DEFAULT 0,
  `macs` varchar(220) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of macs1
-- ----------------------------

-- ----------------------------
-- Table structure for myreferralsip
-- ----------------------------
DROP TABLE IF EXISTS `myreferralsip`;
CREATE TABLE `myreferralsip`  (
  `ip_int` int(11) NOT NULL,
  `ip` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`ip_int`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of myreferralsip
-- ----------------------------

-- ----------------------------
-- Table structure for myreferralsmac
-- ----------------------------
DROP TABLE IF EXISTS `myreferralsmac`;
CREATE TABLE `myreferralsmac`  (
  `mac_int` int(11) NOT NULL,
  `mac` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`mac_int`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of myreferralsmac
-- ----------------------------
INSERT INTO `myreferralsmac` VALUES (-2074713440, '902B347CC19A');
INSERT INTO `myreferralsmac` VALUES (-2052521562, '001635A8A8E2');
INSERT INTO `myreferralsmac` VALUES (-1954757221, 'A0D3C12E2D4B');
INSERT INTO `myreferralsmac` VALUES (-1914691149, '002481221353');
INSERT INTO `myreferralsmac` VALUES (-1873710002, '0A0027000008');
INSERT INTO `myreferralsmac` VALUES (-1838883020, '00137276E3A5');
INSERT INTO `myreferralsmac` VALUES (-1801322549, 'C83DD47A0999');
INSERT INTO `myreferralsmac` VALUES (-1727507592, '904CE579E780');
INSERT INTO `myreferralsmac` VALUES (-1704464032, '047D7B27EC05');
INSERT INTO `myreferralsmac` VALUES (-1685144419, '5065F327C22D');
INSERT INTO `myreferralsmac` VALUES (-1651161384, 'D8D38581690B');
INSERT INTO `myreferralsmac` VALUES (-1512050187, '6451063FF082');
INSERT INTO `myreferralsmac` VALUES (-1463031445, 'B42E99083144');
INSERT INTO `myreferralsmac` VALUES (-1408986908, '1078D27D66A4');
INSERT INTO `myreferralsmac` VALUES (-1272085322, '00E0202F7ADE');
INSERT INTO `myreferralsmac` VALUES (-1204912466, 'C8FF2841D969');
INSERT INTO `myreferralsmac` VALUES (-1083343097, '001AA0506B1F');
INSERT INTO `myreferralsmac` VALUES (-1069765279, 'BCA8A6904052');
INSERT INTO `myreferralsmac` VALUES (-1002900025, '5404A6DA062D');
INSERT INTO `myreferralsmac` VALUES (-925073466, '342387F4683B');
INSERT INTO `myreferralsmac` VALUES (-908451973, '54E1ADEBAFB5');
INSERT INTO `myreferralsmac` VALUES (-797095638, 'A44E31306A1C');
INSERT INTO `myreferralsmac` VALUES (-746822994, '183DA2A7C6EC');
INSERT INTO `myreferralsmac` VALUES (-740549234, '0030670ACA67');
INSERT INTO `myreferralsmac` VALUES (-712423501, '002268714F18');
INSERT INTO `myreferralsmac` VALUES (-578306753, '2C44FD303C5F');
INSERT INTO `myreferralsmac` VALUES (-560908807, 'A81E846CEDB2');
INSERT INTO `myreferralsmac` VALUES (-533532104, 'FCAA145E6D5E');
INSERT INTO `myreferralsmac` VALUES (-518226322, 'ECB1D7601654');
INSERT INTO `myreferralsmac` VALUES (-498265738, '0C84DCF9EB15');
INSERT INTO `myreferralsmac` VALUES (-471587791, '80C16EFFACAC');
INSERT INTO `myreferralsmac` VALUES (-467888535, '0024E81EBBC5');
INSERT INTO `myreferralsmac` VALUES (-224613227, '00232405A4B0');
INSERT INTO `myreferralsmac` VALUES (-110927796, '001FD03812FA');
INSERT INTO `myreferralsmac` VALUES (67589733, 'FCF8AE5BB6D9');
INSERT INTO `myreferralsmac` VALUES (371857150, '');
INSERT INTO `myreferralsmac` VALUES (569991304, 'C89CDC71F0E9');
INSERT INTO `myreferralsmac` VALUES (657958668, '001999B3036B');
INSERT INTO `myreferralsmac` VALUES (672894014, '50465DB4E209');
INSERT INTO `myreferralsmac` VALUES (768507929, '001FD0080F15');
INSERT INTO `myreferralsmac` VALUES (972658019, '6451064A1E7A');
INSERT INTO `myreferralsmac` VALUES (1029342749, '180373CF2730');
INSERT INTO `myreferralsmac` VALUES (1162756148, 'F04DA225F73F');
INSERT INTO `myreferralsmac` VALUES (1276520411, '0027105A1DB8');
INSERT INTO `myreferralsmac` VALUES (1320259980, 'C4346B5DBD3F');
INSERT INTO `myreferralsmac` VALUES (1330725445, '78ACC0C47EF8');
INSERT INTO `myreferralsmac` VALUES (1359612708, '0024818C4363');
INSERT INTO `myreferralsmac` VALUES (1437406492, '3C91807C7E97');
INSERT INTO `myreferralsmac` VALUES (1495842023, '0024217F636D');
INSERT INTO `myreferralsmac` VALUES (1629357018, '3CA0678E9D15');
INSERT INTO `myreferralsmac` VALUES (1721503782, '001966EB9934');
INSERT INTO `myreferralsmac` VALUES (2090636309, '001F293591D7');
INSERT INTO `myreferralsmac` VALUES (2115821137, '7446A0B8C94E');
INSERT INTO `myreferralsmac` VALUES (2127525432, '1234567890AB');

-- ----------------------------
-- Table structure for news
-- ----------------------------
DROP TABLE IF EXISTS `news`;
CREATE TABLE `news`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `article` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `writeruid` int(11) NOT NULL,
  `time` int(11) NOT NULL,
  `Year` int(11) NOT NULL,
  `month` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `day` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `likes` int(11) NULL DEFAULT 0,
  `iconstyle` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT 'fa fa-envelope bg-blue',
  `Image` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of news
-- ----------------------------

-- ----------------------------
-- Table structure for nobility
-- ----------------------------
DROP TABLE IF EXISTS `nobility`;
CREATE TABLE `nobility`  (
  `EntityUID` bigint(20) UNSIGNED NOT NULL DEFAULT 0,
  `EntityName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `Donation` bigint(20) UNSIGNED NULL DEFAULT 0
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of nobility
-- ----------------------------

-- ----------------------------
-- Table structure for online
-- ----------------------------
DROP TABLE IF EXISTS `online`;
CREATE TABLE `online`  (
  `Name` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `OnlineCount` int(11) NULL DEFAULT 0,
  PRIMARY KEY (`Name`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of online
-- ----------------------------

-- ----------------------------
-- Table structure for partners
-- ----------------------------
DROP TABLE IF EXISTS `partners`;
CREATE TABLE `partners`  (
  `partners_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `image` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`partners_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 14 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of partners
-- ----------------------------

-- ----------------------------
-- Table structure for payment_logs
-- ----------------------------
DROP TABLE IF EXISTS `payment_logs`;
CREATE TABLE `payment_logs`  (
  `txn_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `gateway` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `amount` decimal(4, 2) NOT NULL,
  `item` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `email` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `account` varchar(34) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `coins` smallint(6) NOT NULL,
  `paid` tinyint(1) NOT NULL,
  `timestamp` datetime NOT NULL,
  PRIMARY KEY (`txn_id`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of payment_logs
-- ----------------------------

-- ----------------------------
-- Table structure for payments
-- ----------------------------
DROP TABLE IF EXISTS `payments`;
CREATE TABLE `payments`  (
  `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT,
  `product_id` int(11) NOT NULL,
  `txn_id` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `IDPayment` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `payment_gross` double(8, 2) NOT NULL,
  `currency_code` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `payer_id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `payer_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `payer_email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `payer_country` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `payment_status` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `user` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `token` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of payments
-- ----------------------------

-- ----------------------------
-- Table structure for playersonline
-- ----------------------------
DROP TABLE IF EXISTS `playersonline`;
CREATE TABLE `playersonline`  (
  `UID` int(10) UNSIGNED NULL DEFAULT 0,
  `Online` int(10) UNSIGNED NULL DEFAULT 0
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = FIXED;

-- ----------------------------
-- Records of playersonline
-- ----------------------------

-- ----------------------------
-- Table structure for points
-- ----------------------------
DROP TABLE IF EXISTS `points`;
CREATE TABLE `points`  (
  `user` varchar(16) CHARACTER SET cp1256 COLLATE cp1256_general_ci NOT NULL,
  `bonus` int(11) NULL DEFAULT 0,
  PRIMARY KEY (`user`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = cp1256 COLLATE = cp1256_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of points
-- ----------------------------

-- ----------------------------
-- Table structure for post
-- ----------------------------
DROP TABLE IF EXISTS `post`;
CREATE TABLE `post`  (
  `ID` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Image` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Title` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of post
-- ----------------------------

-- ----------------------------
-- Table structure for posts
-- ----------------------------
DROP TABLE IF EXISTS `posts`;
CREATE TABLE `posts`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `title` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `post` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `user` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `date` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Image` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'default.png',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of posts
-- ----------------------------
INSERT INTO `posts` VALUES (1, 'Last GuildWar,,', '<h3>What`s on your mind?<iframe frameborder=\"0\" src=\"//#\" width=\"640\" height=\"360\" class=\"note-video-clip\"></iframe></h3>', '[GM]Morad[PM]', '09-Dec-2019 03:00 pm', 'default.png');
INSERT INTO `posts` VALUES (2, 'Welcome To Server ZeusOnline[E', '<ol><li style=\"text-align: center; \">Server 4 king ,</li><li style=\"text-align: center; \">Drop 100-200 Random ,</li><li style=\"text-align: center; \">ElitePK-SkillTeamPK-TeamPK-CTF-ClanWar Full100% ,</li><li style=\"text-align: center; \">GuildWar On Friday ,</li><li style=\"text-align: center; \">Poker Full 100% ,</li><li style=\"text-align: center; \">Arena, Arena Team ,</li><li style=\"text-align: center; \">Events - Top`s ,</li><li style=\"text-align: center; \">Stuff And Chi And Jiang Free ,</li><li style=\"text-align: center; \">We wish you a nice time at the server ♥♥,&nbsp;<br></li></ol>', '[TQ]Morad[GM]', '01-Sep-2018 07:16 pm', 'default.png');
INSERT INTO `posts` VALUES (3, 'Information about [GM]', '<ol><li style=\"text-align: center;\">Number[GM] ,</li><li style=\"text-align: center;\">01023454323&nbsp; ,</li><li style=\"text-align: center;\">There are no other numbers for the [GM] ,</li><li style=\"text-align: center;\">The only GM only it\'s a [Morad] ,</li><li style=\"text-align: center;\">Good luck , ♥</li></ol2', '[TQ]Morad[GM]', '01-Sep-2018 07:17 pm', 'default.png');

-- ----------------------------
-- Table structure for puplishers
-- ----------------------------
DROP TABLE IF EXISTS `puplishers`;
CREATE TABLE `puplishers`  (
  `UID` bigint(20) NOT NULL DEFAULT 0,
  `Username` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '0',
  `referralcode` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '0',
  `referralpoints` bigint(20) NULL DEFAULT 0,
  `referrals` int(11) NULL DEFAULT 0,
  PRIMARY KEY (`UID`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of puplishers
-- ----------------------------

-- ----------------------------
-- Table structure for quests
-- ----------------------------
DROP TABLE IF EXISTS `quests`;
CREATE TABLE `quests`  (
  `UID` int(11) NOT NULL,
  `MAC` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `SkyPass` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `VipQuest` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `BrightFortune` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `EverythingHasAPrice` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ReleasetheSouls` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `MagnoliaEnvoy` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `SpiritBead` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `HeavenTreasury` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `BallonLand` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`UID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of quests
-- ----------------------------

-- ----------------------------
-- Table structure for rankings
-- ----------------------------
DROP TABLE IF EXISTS `rankings`;
CREATE TABLE `rankings`  (
  `UID` bigint(20) UNSIGNED NULL DEFAULT NULL,
  `Name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Face` int(10) UNSIGNED NULL DEFAULT NULL,
  `cps` bigint(20) UNSIGNED NULL DEFAULT 0,
  `Class` bigint(20) NULL DEFAULT NULL,
  `Level` bigint(20) NULL DEFAULT NULL,
  `VIPLevel` tinyint(3) UNSIGNED NOT NULL,
  `Gold` bigint(20) UNSIGNED NULL DEFAULT NULL,
  `OnlinePoints` bigint(20) NULL DEFAULT NULL,
  `GoldSave` bigint(20) UNSIGNED NULL DEFAULT NULL,
  `CpsSave` bigint(20) UNSIGNED NULL DEFAULT NULL,
  `ActivityRanking` bigint(20) UNSIGNED NULL DEFAULT NULL,
  `ActivityPoints` bigint(20) UNSIGNED NULL DEFAULT NULL,
  `Donation` bigint(20) UNSIGNED NULL DEFAULT NULL,
  `TotalWin` int(10) UNSIGNED NULL DEFAULT 0,
  `TotalLose` int(10) UNSIGNED NULL DEFAULT 0,
  `Online` tinyint(3) UNSIGNED NULL DEFAULT 0
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of rankings
-- ----------------------------

-- ----------------------------
-- Table structure for redeem
-- ----------------------------
DROP TABLE IF EXISTS `redeem`;
CREATE TABLE `redeem`  (
  `UID` bigint(20) NOT NULL,
  `ServerID` bigint(20) UNSIGNED NULL DEFAULT 1,
  `done` varchar(22) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`UID`) USING BTREE,
  UNIQUE INDEX `qwewas`(`UID`) USING BTREE,
  INDEX `eeee`(`ServerID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of redeem
-- ----------------------------

-- ----------------------------
-- Table structure for referrals
-- ----------------------------
DROP TABLE IF EXISTS `referrals`;
CREATE TABLE `referrals`  (
  `UID` bigint(20) NOT NULL DEFAULT 0,
  `Username` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '0',
  `referralcode` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '0',
  `Puplisheruid` bigint(20) NULL DEFAULT 0,
  `OnlineMinutes` bigint(20) NULL DEFAULT 0,
  PRIMARY KEY (`UID`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of referrals
-- ----------------------------

-- ----------------------------
-- Table structure for servers
-- ----------------------------
DROP TABLE IF EXISTS `servers`;
CREATE TABLE `servers`  (
  `Name` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `IP` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Port` int(10) UNSIGNED NULL DEFAULT NULL,
  `TransferKey` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_cs NOT NULL,
  `TransferSalt` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_cs NOT NULL,
  PRIMARY KEY (`Name`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of servers
-- ----------------------------
INSERT INTO `servers` VALUES ('Test1', '26.138.155.160', 5819, 'EypKhLvYJ3zdLCTyz9Ak8RAgM78tY5F32b7CUXDuLDJDFBH8H67BWy9QThmaN5VS', 'MyqVgBf3ytALHWLXbJxSUX4uFEu3Xmz2UAY9sTTm8AScB7Kk2uwqDSnuNJske4BJ');
INSERT INTO `servers` VALUES ('VirusX', '192.168.1.116', 5818, 'EypKhLvYJ3zdLCTyz9Ak8RAgM78tY5F32b7CUXDuLDJDFBH8H67BWy9QThmaN5VS', 'MyqVgBf3ytALHWLXbJxSUX4uFEu3Xmz2UAY9sTTm8AScB7Kk2uwqDSnuNJske4BJ');

-- ----------------------------
-- Table structure for shop
-- ----------------------------
DROP TABLE IF EXISTS `shop`;
CREATE TABLE `shop`  (
  `name_item` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `value_item` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `desc_item` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `img_item` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`name_item`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of shop
-- ----------------------------

-- ----------------------------
-- Table structure for slider
-- ----------------------------
DROP TABLE IF EXISTS `slider`;
CREATE TABLE `slider`  (
  `slider_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `title` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `description` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `image` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`slider_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 28 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of slider
-- ----------------------------

-- ----------------------------
-- Table structure for social
-- ----------------------------
DROP TABLE IF EXISTS `social`;
CREATE TABLE `social`  (
  `social_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `face` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `twitter` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`social_id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of social
-- ----------------------------

-- ----------------------------
-- Table structure for spellseditor
-- ----------------------------
DROP TABLE IF EXISTS `spellseditor`;
CREATE TABLE `spellseditor`  (
  `ID` int(4) UNSIGNED ZEROFILL NOT NULL DEFAULT 0000,
  `IncreaseDmg` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0',
  `DecreaseDmg` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of spellseditor
-- ----------------------------
INSERT INTO `spellseditor` VALUES (1002, '0', '3');
INSERT INTO `spellseditor` VALUES (1001, '0', '3');
INSERT INTO `spellseditor` VALUES (1000, '0', '3');
INSERT INTO `spellseditor` VALUES (1180, '0', '3');
INSERT INTO `spellseditor` VALUES (1165, '0', '3');
INSERT INTO `spellseditor` VALUES (1160, '0', '3');
INSERT INTO `spellseditor` VALUES (1150, '0', '3');
INSERT INTO `spellseditor` VALUES (1115, '0', '0');
INSERT INTO `spellseditor` VALUES (10415, '1.2', '0');
INSERT INTO `spellseditor` VALUES (1120, '0', '3');
INSERT INTO `spellseditor` VALUES (10315, '0', '3');
INSERT INTO `spellseditor` VALUES (12240, '1.15', '0');
INSERT INTO `spellseditor` VALUES (12090, '0', '3');
INSERT INTO `spellseditor` VALUES (11070, '0', '0');
INSERT INTO `spellseditor` VALUES (12550, '2.7', '0');

-- ----------------------------
-- Table structure for status
-- ----------------------------
DROP TABLE IF EXISTS `status`;
CREATE TABLE `status`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `EntityID` int(11) NOT NULL DEFAULT 0,
  `status` bigint(20) UNSIGNED NOT NULL DEFAULT 0,
  `time` bigint(20) NULL DEFAULT 0,
  `flagtype` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 89662 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = FIXED;

-- ----------------------------
-- Records of status
-- ----------------------------
INSERT INTO `status` VALUES (78121, 10278258, 76, 20210211095118, NULL);
INSERT INTO `status` VALUES (82011, 10278388, 90, 20210227125801, NULL);
INSERT INTO `status` VALUES (78597, 10278186, 187, 20210213141038, NULL);
INSERT INTO `status` VALUES (79303, 10277976, 188, 20210216081700, NULL);
INSERT INTO `status` VALUES (76593, 10277852, 69, 20210202162440, NULL);
INSERT INTO `status` VALUES (78164, 10278270, 73, 20210211150006, NULL);
INSERT INTO `status` VALUES (81909, 10278480, 153, 20210228013052, NULL);
INSERT INTO `status` VALUES (77395, 10278142, 153, 20210208013023, NULL);
INSERT INTO `status` VALUES (78595, 10278186, 73, 20210213140130, NULL);
INSERT INTO `status` VALUES (81123, 10278500, 188, 20210223211700, NULL);
INSERT INTO `status` VALUES (78128, 10278258, 78, 20210211103926, NULL);
INSERT INTO `status` VALUES (78119, 10278258, 78, 20210211093912, NULL);
INSERT INTO `status` VALUES (81883, 10278340, 90, 20210226225800, NULL);
INSERT INTO `status` VALUES (76475, 10277861, 73, 20210202000009, NULL);
INSERT INTO `status` VALUES (78405, 10278323, 69, 20210212182400, NULL);
INSERT INTO `status` VALUES (81682, 10278518, 78, 20210226044146, NULL);
INSERT INTO `status` VALUES (81448, 10278515, 190, 20210225064601, NULL);
INSERT INTO `status` VALUES (81963, 10278534, 69, 20210227082400, NULL);
INSERT INTO `status` VALUES (77222, 10277820, 151, 20210207013012, NULL);
INSERT INTO `status` VALUES (79494, 10278401, 76, 20210217015102, NULL);
INSERT INTO `status` VALUES (76371, 10277813, 66, 20210201070501, NULL);
INSERT INTO `status` VALUES (76372, 10277814, 81, 20210201071402, NULL);
INSERT INTO `status` VALUES (76373, 10277814, 69, 20210201072401, NULL);
INSERT INTO `status` VALUES (76374, 10277814, 78, 20210201073900, NULL);
INSERT INTO `status` VALUES (76375, 10277814, 76, 20210201075134, NULL);
INSERT INTO `status` VALUES (76376, 10277814, 90, 20210201075801, NULL);
INSERT INTO `status` VALUES (76377, 10277814, 66, 20210201080500, NULL);
INSERT INTO `status` VALUES (76378, 10277814, 81, 20210201081402, NULL);
INSERT INTO `status` VALUES (76379, 10277814, 69, 20210201082401, NULL);
INSERT INTO `status` VALUES (76380, 10277814, 76, 20210201085101, NULL);
INSERT INTO `status` VALUES (76381, 10277814, 90, 20210201085801, NULL);
INSERT INTO `status` VALUES (76382, 10277814, 66, 20210201090501, NULL);
INSERT INTO `status` VALUES (76383, 10277814, 81, 20210201091400, NULL);
INSERT INTO `status` VALUES (76384, 10277813, 78, 20210201093901, NULL);
INSERT INTO `status` VALUES (76385, 10277813, 89, 20210201094301, NULL);
INSERT INTO `status` VALUES (80496, 10278253, 81, 20210221061401, NULL);
INSERT INTO `status` VALUES (81121, 10277938, 187, 20210223211005, NULL);
INSERT INTO `status` VALUES (80102, 10278445, 90, 20210219155800, NULL);
INSERT INTO `status` VALUES (78869, 10278359, 89, 20210214144443, NULL);
INSERT INTO `status` VALUES (80494, 10278253, 66, 20210221060501, NULL);
INSERT INTO `status` VALUES (79492, 10278401, 69, 20210217012400, NULL);
INSERT INTO `status` VALUES (78122, 10278258, 90, 20210211095800, NULL);
INSERT INTO `status` VALUES (79491, 10278401, 188, 20210217011700, NULL);
INSERT INTO `status` VALUES (81128, 10277938, 89, 20210223214328, NULL);
INSERT INTO `status` VALUES (76471, 10277861, 78, 20210201233953, NULL);
INSERT INTO `status` VALUES (77507, 10278043, 89, 20210207154301, NULL);
INSERT INTO `status` VALUES (80495, 10278253, 187, 20210221061000, NULL);
INSERT INTO `status` VALUES (77034, 10278097, 76, 20210205015358, NULL);
INSERT INTO `status` VALUES (81055, 10278168, 73, 20210223160008, NULL);
INSERT INTO `status` VALUES (78692, 10278185, 189, 20210213213500, NULL);
INSERT INTO `status` VALUES (81129, 10277938, 190, 20210223214627, NULL);
INSERT INTO `status` VALUES (78772, 10278255, 82, 20210214053106, NULL);
INSERT INTO `status` VALUES (77949, 10278214, 69, 20210210122400, NULL);
INSERT INTO `status` VALUES (81444, 10278517, 69, 20210225062456, NULL);
INSERT INTO `status` VALUES (79520, 10277979, 66, 20210217080503, NULL);
INSERT INTO `status` VALUES (82140, 10278600, 66, 20210227230502, NULL);
INSERT INTO `status` VALUES (76799, 10277945, 89, 20210203204344, NULL);
INSERT INTO `status` VALUES (76766, 10277832, 81, 20210203161421, NULL);
INSERT INTO `status` VALUES (76797, 10277945, 69, 20210203202527, NULL);
INSERT INTO `status` VALUES (78693, 10278185, 78, 20210213214017, NULL);
INSERT INTO `status` VALUES (82135, 10278269, 78, 20210227223901, NULL);
INSERT INTO `status` VALUES (76603, 10277929, 90, 20210202175801, NULL);
INSERT INTO `status` VALUES (82138, 10277864, 90, 20210227225808, NULL);
INSERT INTO `status` VALUES (76673, 10277973, 89, 20210203034324, NULL);
INSERT INTO `status` VALUES (77997, 10278223, 75, 20210211173619, NULL);
INSERT INTO `status` VALUES (79224, 10278397, 190, 20210215234601, NULL);
INSERT INTO `status` VALUES (80419, 10278277, 187, 20210220231048, NULL);
INSERT INTO `status` VALUES (76796, 10277945, 81, 20210203201428, NULL);
INSERT INTO `status` VALUES (82129, 10277923, 187, 20210227221042, NULL);
INSERT INTO `status` VALUES (82124, 10277923, 190, 20210227214612, NULL);
INSERT INTO `status` VALUES (79222, 10278397, 82, 20210215233108, NULL);
INSERT INTO `status` VALUES (79951, 10278350, 78, 20210218234028, NULL);
INSERT INTO `status` VALUES (77923, 10278207, 82, 20210210043101, NULL);
INSERT INTO `status` VALUES (78425, 10278234, 78, 20210212194030, NULL);
INSERT INTO `status` VALUES (77467, 10278077, 76, 20210207105102, NULL);
INSERT INTO `status` VALUES (77766, 10278011, 81, 20210209041945, NULL);
INSERT INTO `status` VALUES (81120, 10277938, 66, 20210223210540, NULL);
INSERT INTO `status` VALUES (82001, 10278378, 90, 20210227115801, NULL);
INSERT INTO `status` VALUES (78424, 10278234, 189, 20210212193520, NULL);
INSERT INTO `status` VALUES (78694, 10278185, 89, 20210213214345, NULL);
INSERT INTO `status` VALUES (79142, 10278368, 89, 20210215164315, NULL);
INSERT INTO `status` VALUES (78691, 10278185, 82, 20210213213126, NULL);
INSERT INTO `status` VALUES (77930, 10277873, 89, 20210210094301, NULL);
INSERT INTO `status` VALUES (79488, 10278401, 76, 20210217005102, NULL);
INSERT INTO `status` VALUES (81154, 10277778, 76, 20210223235117, NULL);
INSERT INTO `status` VALUES (78599, 10278186, 188, 20210213141747, NULL);
INSERT INTO `status` VALUES (82137, 10278269, 76, 20210227225432, NULL);
INSERT INTO `status` VALUES (78802, 10278375, 189, 20210214093500, NULL);
INSERT INTO `status` VALUES (77927, 10277873, 89, 20210210084300, NULL);
INSERT INTO `status` VALUES (78596, 10278186, 66, 20210213140655, NULL);
INSERT INTO `status` VALUES (78571, 10278353, 90, 20210213115802, NULL);
INSERT INTO `status` VALUES (81403, 10278508, 66, 20210225000502, NULL);
INSERT INTO `status` VALUES (78909, 10277972, 76, 20210214175227, NULL);
INSERT INTO `status` VALUES (78769, 10278255, 66, 20210214050501, NULL);
INSERT INTO `status` VALUES (79493, 10278401, 82, 20210217013102, NULL);
INSERT INTO `status` VALUES (78529, 10278310, 190, 20210213034623, NULL);
INSERT INTO `status` VALUES (79487, 10278401, 190, 20210217004602, NULL);
INSERT INTO `status` VALUES (78511, 10278274, 151, 20210214013104, NULL);
INSERT INTO `status` VALUES (82111, 10278266, 190, 20210227204600, NULL);
INSERT INTO `status` VALUES (82131, 10277923, 188, 20210227221722, NULL);
INSERT INTO `status` VALUES (78690, 10278185, 69, 20210213212550, NULL);
INSERT INTO `status` VALUES (79952, 10278350, 89, 20210218234335, NULL);
INSERT INTO `status` VALUES (81145, 10277908, 187, 20210223231022, NULL);
INSERT INTO `status` VALUES (82064, 10278437, 188, 20210227171747, NULL);
INSERT INTO `status` VALUES (82110, 10278266, 89, 20210227204308, NULL);
INSERT INTO `status` VALUES (82122, 10277923, 78, 20210227214126, NULL);
INSERT INTO `status` VALUES (82144, 10278191, 69, 20210227232402, NULL);
INSERT INTO `status` VALUES (81447, 10278515, 89, 20210225064300, NULL);
INSERT INTO `status` VALUES (78768, 10278255, 73, 20210214050028, NULL);
INSERT INTO `status` VALUES (80411, 10278277, 69, 20210220222415, NULL);
INSERT INTO `status` VALUES (80093, 10278445, 90, 20210219145801, NULL);
INSERT INTO `status` VALUES (78598, 10278186, 81, 20210213141437, NULL);
INSERT INTO `status` VALUES (81960, 10278534, 187, 20210227081002, NULL);
INSERT INTO `status` VALUES (81880, 10278340, 89, 20210226224335, NULL);
INSERT INTO `status` VALUES (81840, 10278183, 188, 20210226191959, NULL);
INSERT INTO `status` VALUES (79302, 10277976, 81, 20210216081404, NULL);
INSERT INTO `status` VALUES (82132, 10277923, 69, 20210227222402, NULL);
INSERT INTO `status` VALUES (82109, 10278266, 78, 20210227203901, NULL);
INSERT INTO `status` VALUES (81877, 10278340, 82, 20210226223213, NULL);
INSERT INTO `status` VALUES (81445, 10278517, 82, 20210225063102, NULL);
INSERT INTO `status` VALUES (81446, 10278515, 189, 20210225063501, NULL);
INSERT INTO `status` VALUES (81146, 10277908, 81, 20210223231404, NULL);
INSERT INTO `status` VALUES (81683, 10278518, 89, 20210226044301, NULL);
INSERT INTO `status` VALUES (82108, 10278266, 189, 20210227203517, NULL);
INSERT INTO `status` VALUES (81105, 10277938, 90, 20210223195811, NULL);
INSERT INTO `status` VALUES (81147, 10277908, 188, 20210223231704, NULL);
INSERT INTO `status` VALUES (82016, 10278388, 69, 20210227132413, NULL);
INSERT INTO `status` VALUES (82121, 10277923, 189, 20210227213512, NULL);
INSERT INTO `status` VALUES (80811, 10278205, 69, 20210222142423, NULL);
INSERT INTO `status` VALUES (82143, 10278278, 188, 20210227231701, NULL);
INSERT INTO `status` VALUES (81862, 10278244, 187, 20210226211106, NULL);
INSERT INTO `status` VALUES (82126, 10277913, 90, 20210227215827, NULL);
INSERT INTO `status` VALUES (80410, 10278277, 188, 20210220221712, NULL);
INSERT INTO `status` VALUES (81881, 10278340, 190, 20210226224600, NULL);
INSERT INTO `status` VALUES (81103, 10277938, 190, 20210223194626, NULL);
INSERT INTO `status` VALUES (82012, 10278137, 73, 20210227130038, NULL);
INSERT INTO `status` VALUES (79213, 10278106, 69, 20210215222401, NULL);
INSERT INTO `status` VALUES (79485, 10278401, 82, 20210217003101, NULL);
INSERT INTO `status` VALUES (79304, 10277976, 69, 20210216082402, NULL);
INSERT INTO `status` VALUES (81562, 10278117, 63, 20210226173026, NULL);
INSERT INTO `status` VALUES (82119, 10277923, 69, 20210227212414, NULL);
INSERT INTO `status` VALUES (79486, 10278401, 78, 20210217003902, NULL);
INSERT INTO `status` VALUES (82125, 10277923, 76, 20210227215129, NULL);
INSERT INTO `status` VALUES (81844, 10278183, 189, 20210226194027, NULL);
INSERT INTO `status` VALUES (81127, 10277938, 78, 20210223214007, NULL);
INSERT INTO `status` VALUES (81067, 10278168, 73, 20210223170002, NULL);
INSERT INTO `status` VALUES (81888, 10278340, 69, 20210226232401, NULL);
INSERT INTO `status` VALUES (81114, 10277938, 78, 20210223203932, NULL);
INSERT INTO `status` VALUES (82134, 10278269, 189, 20210227223508, NULL);
INSERT INTO `status` VALUES (82117, 10278506, 81, 20210227211427, NULL);
INSERT INTO `status` VALUES (81887, 10278340, 188, 20210226231711, NULL);
INSERT INTO `status` VALUES (81873, 10278340, 187, 20210226221000, NULL);
INSERT INTO `status` VALUES (81886, 10278340, 187, 20210226231002, NULL);
INSERT INTO `status` VALUES (82142, 10278278, 81, 20210227231406, NULL);
INSERT INTO `status` VALUES (81875, 10278340, 188, 20210226221826, NULL);
INSERT INTO `status` VALUES (82127, 10277913, 73, 20210227220030, NULL);
INSERT INTO `status` VALUES (81878, 10278340, 189, 20210226223551, NULL);
INSERT INTO `status` VALUES (81882, 10278340, 76, 20210226225130, NULL);
INSERT INTO `status` VALUES (82141, 10278278, 187, 20210227231001, NULL);
INSERT INTO `status` VALUES (82081, 10278529, 75, 20210228173019, NULL);
INSERT INTO `status` VALUES (82079, 10277884, 74, 20210228173005, NULL);
INSERT INTO `status` VALUES (82120, 10278506, 82, 20210227213108, NULL);
INSERT INTO `status` VALUES (82133, 10278269, 82, 20210227223101, NULL);
INSERT INTO `status` VALUES (81876, 10278340, 69, 20210226222515, NULL);
INSERT INTO `status` VALUES (82105, 10278506, 188, 20210227201758, NULL);
INSERT INTO `status` VALUES (81866, 10278244, 82, 20210226213241, NULL);
INSERT INTO `status` VALUES (82128, 10277913, 66, 20210227220555, NULL);
INSERT INTO `status` VALUES (82073, 10277909, 66, 20210227180613, NULL);
INSERT INTO `status` VALUES (81907, 10277883, 151, 20210228013005, NULL);
INSERT INTO `status` VALUES (82116, 10277923, 187, 20210227211058, NULL);
INSERT INTO `status` VALUES (81867, 10278244, 189, 20210226213617, NULL);
INSERT INTO `status` VALUES (82078, 10277884, 152, 20210228173005, NULL);
INSERT INTO `status` VALUES (82130, 10277923, 81, 20210227221432, NULL);
INSERT INTO `status` VALUES (82139, 10277864, 73, 20210227230001, NULL);
INSERT INTO `status` VALUES (82118, 10277913, 188, 20210227211708, NULL);
INSERT INTO `status` VALUES (82115, 10277913, 66, 20210227210602, NULL);
INSERT INTO `status` VALUES (82080, 10278559, 63, 20210228173007, NULL);
INSERT INTO `status` VALUES (81861, 10278244, 66, 20210226210526, NULL);
INSERT INTO `status` VALUES (82114, 10278313, 73, 20210227210040, NULL);
INSERT INTO `status` VALUES (82476, 10278705, 73, 20210831040011, NULL);
INSERT INTO `status` VALUES (89561, 10279717, 76, 20210930045102, NULL);
INSERT INTO `status` VALUES (83483, 10279122, 66, 20210904090500, NULL);
INSERT INTO `status` VALUES (82692, 10278852, 89, 20210831234341, NULL);
INSERT INTO `status` VALUES (82673, 10278802, 81, 20210831221402, NULL);
INSERT INTO `status` VALUES (85403, 10279330, 90, 20210911215827, NULL);
INSERT INTO `status` VALUES (83671, 10279084, 69, 20210905022417, NULL);
INSERT INTO `status` VALUES (89604, 10279214, 81, 20210930141416, NULL);
INSERT INTO `status` VALUES (82388, 10278769, 188, 20210830201902, NULL);
INSERT INTO `status` VALUES (82635, 10278875, 76, 20210831175148, NULL);
INSERT INTO `status` VALUES (89636, 10278839, 188, 20210930171717, NULL);
INSERT INTO `status` VALUES (89580, 10279718, 73, 20210930065913, NULL);
INSERT INTO `status` VALUES (83128, 10279023, 189, 20210902223500, NULL);
INSERT INTO `status` VALUES (89567, 10279717, 188, 20210930051701, NULL);
INSERT INTO `status` VALUES (82484, 10278705, 78, 20210831043959, NULL);
INSERT INTO `status` VALUES (82627, 10278863, 81, 20210831171440, NULL);
INSERT INTO `status` VALUES (89639, 10279517, 189, 20210930173520, NULL);
INSERT INTO `status` VALUES (82486, 10278705, 190, 20210831044600, NULL);
INSERT INTO `status` VALUES (84939, 10279255, 82, 20210910003101, NULL);
INSERT INTO `status` VALUES (89453, 10279799, 66, 20210929170556, NULL);
INSERT INTO `status` VALUES (82729, 10278670, 81, 20210901031401, NULL);
INSERT INTO `status` VALUES (82485, 10278705, 89, 20210831044320, NULL);
INSERT INTO `status` VALUES (82909, 10278938, 89, 20210902014311, NULL);
INSERT INTO `status` VALUES (84719, 10279189, 66, 20210909040500, NULL);
INSERT INTO `status` VALUES (89302, 10278984, 187, 20210929001002, NULL);
INSERT INTO `status` VALUES (85486, 10279371, 82, 20210912083100, NULL);
INSERT INTO `status` VALUES (89577, 10279552, 189, 20210930063502, NULL);
INSERT INTO `status` VALUES (83468, 10279117, 89, 20210904074303, NULL);
INSERT INTO `status` VALUES (82626, 10278863, 187, 20210831171002, NULL);
INSERT INTO `status` VALUES (82831, 10278934, 188, 20210901171708, NULL);
INSERT INTO `status` VALUES (84013, 10279042, 76, 20210906065101, NULL);
INSERT INTO `status` VALUES (82596, 10278690, 34, 20220327213936, NULL);
INSERT INTO `status` VALUES (82400, 10278699, 81, 20210830211403, NULL);
INSERT INTO `status` VALUES (82434, 10278783, 187, 20210831001417, NULL);
INSERT INTO `status` VALUES (82982, 10278971, 81, 20210902101412, NULL);
INSERT INTO `status` VALUES (82616, 10278840, 81, 20210831161401, NULL);
INSERT INTO `status` VALUES (82461, 10278795, 152, 20210901013024, NULL);
INSERT INTO `status` VALUES (82723, 10278717, 153, 20210902013044, NULL);
INSERT INTO `status` VALUES (84747, 10279189, 189, 20210909063501, NULL);
INSERT INTO `status` VALUES (84044, 10279197, 66, 20210906110501, NULL);
INSERT INTO `status` VALUES (89638, 10279517, 82, 20210930173100, NULL);
INSERT INTO `status` VALUES (83676, 10278908, 189, 20210905023500, NULL);
INSERT INTO `status` VALUES (88201, 10279731, 73, 20210924110033, NULL);
INSERT INTO `status` VALUES (82410, 10278767, 188, 20210830221702, NULL);
INSERT INTO `status` VALUES (84907, 10279177, 189, 20210909213633, NULL);
INSERT INTO `status` VALUES (82990, 10278971, 73, 20210902110007, NULL);
INSERT INTO `status` VALUES (87322, 10279594, 69, 20210920182404, NULL);
INSERT INTO `status` VALUES (82463, 10278706, 151, 20210901013039, NULL);
INSERT INTO `status` VALUES (89467, 10279799, 69, 20210929182411, NULL);
INSERT INTO `status` VALUES (83601, 10279156, 188, 20210904191701, NULL);
INSERT INTO `status` VALUES (85482, 10279426, 189, 20210912063501, NULL);
INSERT INTO `status` VALUES (82441, 10278783, 89, 20210831004530, NULL);
INSERT INTO `status` VALUES (84730, 10279365, 187, 20210909051001, NULL);
INSERT INTO `status` VALUES (82343, 10278719, 90, 20210830165800, NULL);
INSERT INTO `status` VALUES (89438, 10279805, 190, 20210929154602, NULL);
INSERT INTO `status` VALUES (86824, 10279525, 69, 20210918132457, NULL);
INSERT INTO `status` VALUES (82915, 10278938, 187, 20210902021024, NULL);
INSERT INTO `status` VALUES (82785, 10278907, 89, 20210901124302, NULL);
INSERT INTO `status` VALUES (82440, 10278783, 78, 20210831003952, NULL);
INSERT INTO `status` VALUES (83599, 10279157, 187, 20210904191001, NULL);
INSERT INTO `status` VALUES (89207, 10279908, 189, 20210928153502, NULL);
INSERT INTO `status` VALUES (88191, 10279431, 189, 20210924093523, NULL);
INSERT INTO `status` VALUES (88804, 10279646, 81, 20210926211401, NULL);
INSERT INTO `status` VALUES (89525, 10279465, 73, 20210930010014, NULL);
INSERT INTO `status` VALUES (82436, 10278783, 188, 20210831001824, NULL);
INSERT INTO `status` VALUES (89574, 10279552, 188, 20210930061700, NULL);
INSERT INTO `status` VALUES (89642, 10279517, 190, 20210930174601, NULL);
INSERT INTO `status` VALUES (82783, 10278907, 189, 20210901123500, NULL);
INSERT INTO `status` VALUES (84017, 10279042, 188, 20210906071700, NULL);
INSERT INTO `status` VALUES (82678, 10278802, 78, 20210831223923, NULL);
INSERT INTO `status` VALUES (82646, 10278706, 75, 20210901173034, NULL);
INSERT INTO `status` VALUES (89640, 10279517, 78, 20210930173936, NULL);
INSERT INTO `status` VALUES (82782, 10278907, 82, 20210901123103, NULL);
INSERT INTO `status` VALUES (84016, 10279042, 66, 20210906070501, NULL);
INSERT INTO `status` VALUES (83286, 10278759, 69, 20210903142402, NULL);
INSERT INTO `status` VALUES (82984, 10278971, 189, 20210902103500, NULL);
INSERT INTO `status` VALUES (85473, 10279426, 76, 20210912055100, NULL);
INSERT INTO `status` VALUES (82911, 10278938, 76, 20210902015214, NULL);
INSERT INTO `status` VALUES (89623, 10279712, 66, 20210930160506, NULL);
INSERT INTO `status` VALUES (85474, 10279426, 90, 20210912055801, NULL);
INSERT INTO `status` VALUES (82615, 10278840, 187, 20210831161053, NULL);
INSERT INTO `status` VALUES (84637, 10278940, 189, 20210908203501, NULL);
INSERT INTO `status` VALUES (82540, 10278742, 81, 20210831101402, NULL);
INSERT INTO `status` VALUES (82674, 10278802, 188, 20210831221702, NULL);
INSERT INTO `status` VALUES (85753, 10278753, 187, 20210913171001, NULL);
INSERT INTO `status` VALUES (82994, 10278971, 188, 20210902111707, NULL);
INSERT INTO `status` VALUES (82910, 10278938, 190, 20210902014608, NULL);
INSERT INTO `status` VALUES (83522, 10279127, 82, 20210904123107, NULL);
INSERT INTO `status` VALUES (82830, 10278934, 81, 20210901171416, NULL);
INSERT INTO `status` VALUES (82565, 10278830, 82, 20210831123100, NULL);
INSERT INTO `status` VALUES (82712, 10278921, 190, 20210901014616, NULL);
INSERT INTO `status` VALUES (82555, 10278830, 189, 20210831113501, NULL);
INSERT INTO `status` VALUES (82556, 10278830, 78, 20210831113900, NULL);
INSERT INTO `status` VALUES (85267, 10279476, 66, 20210911080518, NULL);
INSERT INTO `status` VALUES (88487, 10279649, 190, 20210925134619, NULL);
INSERT INTO `status` VALUES (82559, 10278830, 76, 20210831115100, NULL);
INSERT INTO `status` VALUES (82711, 10278921, 89, 20210901014401, NULL);
INSERT INTO `status` VALUES (82561, 10278830, 66, 20210831120501, NULL);
INSERT INTO `status` VALUES (84604, 10279208, 66, 20210908180500, NULL);
INSERT INTO `status` VALUES (82872, 10278910, 76, 20210901215100, NULL);
INSERT INTO `status` VALUES (82693, 10278852, 190, 20210831234601, NULL);
INSERT INTO `status` VALUES (89632, 10279518, 76, 20210930165144, NULL);
INSERT INTO `status` VALUES (85404, 10279330, 73, 20210911220026, NULL);
INSERT INTO `status` VALUES (83291, 10278759, 190, 20210903144616, NULL);
INSERT INTO `status` VALUES (84928, 10279255, 189, 20210909233523, NULL);
INSERT INTO `status` VALUES (89524, 10279465, 76, 20210930005102, NULL);
INSERT INTO `status` VALUES (84746, 10279189, 82, 20210909063101, NULL);
INSERT INTO `status` VALUES (84904, 10279177, 188, 20210909211710, NULL);
INSERT INTO `status` VALUES (85763, 10278753, 73, 20210913180006, NULL);
INSERT INTO `status` VALUES (89500, 10279947, 189, 20210929213513, NULL);
INSERT INTO `status` VALUES (89450, 10279859, 89, 20210929164329, NULL);
INSERT INTO `status` VALUES (89497, 10279292, 188, 20210929211718, NULL);
INSERT INTO `status` VALUES (82917, 10278938, 188, 20210902021700, NULL);
INSERT INTO `status` VALUES (89379, 10279828, 190, 20210929084756, NULL);
INSERT INTO `status` VALUES (84600, 10279208, 89, 20210908174418, NULL);
INSERT INTO `status` VALUES (83330, 10278966, 81, 20210903181400, NULL);
INSERT INTO `status` VALUES (83356, 10279080, 78, 20210903183900, NULL);
INSERT INTO `status` VALUES (85481, 10279426, 82, 20210912063101, NULL);
INSERT INTO `status` VALUES (82993, 10278971, 81, 20210902111413, NULL);
INSERT INTO `status` VALUES (85752, 10278753, 73, 20210913170001, NULL);
INSERT INTO `status` VALUES (83600, 10279156, 81, 20210904191401, NULL);
INSERT INTO `status` VALUES (82916, 10278938, 81, 20210902021609, NULL);
INSERT INTO `status` VALUES (87825, 10278903, 189, 20210922213500, NULL);
INSERT INTO `status` VALUES (83612, 10279158, 189, 20210904203502, NULL);
INSERT INTO `status` VALUES (89613, 10279955, 187, 20210930151010, NULL);
INSERT INTO `status` VALUES (83611, 10279158, 82, 20210904203339, NULL);
INSERT INTO `status` VALUES (88344, 10279719, 187, 20210925001052, NULL);
INSERT INTO `status` VALUES (83372, 10278666, 187, 20210903221121, NULL);
INSERT INTO `status` VALUES (89618, 10279712, 78, 20210930153908, NULL);
INSERT INTO `status` VALUES (85767, 10278753, 82, 20210913183106, NULL);
INSERT INTO `status` VALUES (83675, 10278908, 82, 20210905023101, NULL);
INSERT INTO `status` VALUES (89637, 10279750, 69, 20210930172426, NULL);
INSERT INTO `status` VALUES (83523, 10279126, 189, 20210904123532, NULL);
INSERT INTO `status` VALUES (87200, 10278815, 151, 20210921013042, NULL);
INSERT INTO `status` VALUES (84951, 10279405, 82, 20210910013304, NULL);
INSERT INTO `status` VALUES (87315, 10279648, 89, 20210920174301, NULL);
INSERT INTO `status` VALUES (89621, 10279712, 76, 20210930155104, NULL);
INSERT INTO `status` VALUES (84140, 10278667, 187, 20210906201049, NULL);
INSERT INTO `status` VALUES (84726, 10279189, 190, 20210909044601, NULL);
INSERT INTO `status` VALUES (89092, 10279679, 66, 20210928040502, NULL);
INSERT INTO `status` VALUES (84731, 10279365, 81, 20210909051401, NULL);
INSERT INTO `status` VALUES (83123, 10279023, 90, 20210902215801, NULL);
INSERT INTO `status` VALUES (84054, 10279225, 189, 20210906123500, NULL);
INSERT INTO `status` VALUES (84740, 10279189, 73, 20210909060106, NULL);
INSERT INTO `status` VALUES (82995, 10278971, 69, 20210902112403, NULL);
INSERT INTO `status` VALUES (83834, 10278773, 187, 20210905161014, NULL);
INSERT INTO `status` VALUES (87080, 10279577, 66, 20210919150615, NULL);
INSERT INTO `status` VALUES (83471, 10278744, 90, 20210904075800, NULL);
INSERT INTO `status` VALUES (85422, 10279318, 189, 20210911233714, NULL);
INSERT INTO `status` VALUES (84001, 10278965, 66, 20210906050716, NULL);
INSERT INTO `status` VALUES (85766, 10278753, 75, 20210914173005, NULL);
INSERT INTO `status` VALUES (85757, 10278753, 82, 20210913173118, NULL);
INSERT INTO `status` VALUES (84051, 10279225, 66, 20210906120500, NULL);
INSERT INTO `status` VALUES (89654, 10279593, 190, 20210930184601, NULL);
INSERT INTO `status` VALUES (89088, 10279679, 89, 20210928034346, NULL);
INSERT INTO `status` VALUES (83290, 10278759, 89, 20210903144316, NULL);
INSERT INTO `status` VALUES (85994, 10279524, 189, 20210914163500, NULL);
INSERT INTO `status` VALUES (84829, 10278991, 66, 20210909150501, NULL);
INSERT INTO `status` VALUES (84161, 10278667, 90, 20210906215835, NULL);
INSERT INTO `status` VALUES (89287, 10278981, 78, 20210928223935, NULL);
INSERT INTO `status` VALUES (83605, 10279158, 89, 20210904194301, NULL);
INSERT INTO `status` VALUES (84053, 10279225, 69, 20210906122401, NULL);
INSERT INTO `status` VALUES (84940, 10279255, 189, 20210910003501, NULL);
INSERT INTO `status` VALUES (83357, 10279080, 89, 20210903184305, NULL);
INSERT INTO `status` VALUES (83602, 10279156, 69, 20210904192400, NULL);
INSERT INTO `status` VALUES (85574, 10278785, 81, 20210912191402, NULL);
INSERT INTO `status` VALUES (89660, 10279593, 188, 20210930191701, NULL);
INSERT INTO `status` VALUES (85756, 10278753, 69, 20210913172400, NULL);
INSERT INTO `status` VALUES (83320, 10278966, 82, 20210903173101, NULL);
INSERT INTO `status` VALUES (88204, 10279731, 188, 20210924111701, NULL);
INSERT INTO `status` VALUES (83592, 10279055, 75, 20210905173142, NULL);
INSERT INTO `status` VALUES (84012, 10279042, 190, 20210906064801, NULL);
INSERT INTO `status` VALUES (87813, 10278903, 78, 20210922203901, NULL);
INSERT INTO `status` VALUES (85478, 10279426, 81, 20210912061401, NULL);
INSERT INTO `status` VALUES (89658, 10279593, 66, 20210930190501, NULL);
INSERT INTO `status` VALUES (83371, 10278666, 66, 20210903220950, NULL);
INSERT INTO `status` VALUES (85761, 10278753, 76, 20210913175102, NULL);
INSERT INTO `status` VALUES (87316, 10279647, 190, 20210920174613, NULL);
INSERT INTO `status` VALUES (83609, 10279090, 188, 20210904201701, NULL);
INSERT INTO `status` VALUES (83333, 10278966, 78, 20210903183900, NULL);
INSERT INTO `status` VALUES (85562, 10279384, 187, 20210912181001, NULL);
INSERT INTO `status` VALUES (83446, 10278817, 188, 20210904041700, NULL);
INSERT INTO `status` VALUES (84739, 10279189, 76, 20210909055100, NULL);
INSERT INTO `status` VALUES (87820, 10278903, 187, 20210922211006, NULL);
INSERT INTO `status` VALUES (84905, 10279177, 69, 20210909212400, NULL);
INSERT INTO `status` VALUES (84449, 10279306, 78, 20210908033902, NULL);
INSERT INTO `status` VALUES (84882, 10279395, 69, 20210909192402, NULL);
INSERT INTO `status` VALUES (83450, 10279111, 66, 20210904050501, NULL);
INSERT INTO `status` VALUES (85472, 10279426, 190, 20210912054600, NULL);
INSERT INTO `status` VALUES (83598, 10279156, 66, 20210904190519, NULL);
INSERT INTO `status` VALUES (85419, 10279318, 188, 20210911231907, NULL);
INSERT INTO `status` VALUES (89376, 10279828, 189, 20210929083530, NULL);
INSERT INTO `status` VALUES (83445, 10279106, 73, 20210904040033, NULL);
INSERT INTO `status` VALUES (89504, 10279947, 73, 20210929215949, NULL);
INSERT INTO `status` VALUES (83951, 10279215, 188, 20210906011738, NULL);
INSERT INTO `status` VALUES (83790, 10279055, 78, 20210905123914, NULL);
INSERT INTO `status` VALUES (89617, 10279712, 82, 20210930153100, NULL);
INSERT INTO `status` VALUES (89573, 10279552, 81, 20210930061402, NULL);
INSERT INTO `status` VALUES (85771, 10279510, 69, 20210913192402, NULL);
INSERT INTO `status` VALUES (89630, 10279592, 89, 20210930164352, NULL);
INSERT INTO `status` VALUES (85523, 10279472, 81, 20210912141401, NULL);
INSERT INTO `status` VALUES (89656, 10279595, 90, 20210930185812, NULL);
INSERT INTO `status` VALUES (89502, 10279292, 76, 20210929215102, NULL);
INSERT INTO `status` VALUES (85262, 10279476, 78, 20210911073928, NULL);
INSERT INTO `status` VALUES (89520, 10279598, 73, 20210929235910, NULL);
INSERT INTO `status` VALUES (84160, 10278667, 76, 20210906215208, NULL);
INSERT INTO `status` VALUES (83717, 10278701, 69, 20210905062417, NULL);
INSERT INTO `status` VALUES (83837, 10278773, 69, 20210905162401, NULL);
INSERT INTO `status` VALUES (83793, 10279055, 90, 20210905125801, NULL);
INSERT INTO `status` VALUES (84738, 10279189, 190, 20210909054601, NULL);
INSERT INTO `status` VALUES (84144, 10278667, 189, 20210906203718, NULL);
INSERT INTO `status` VALUES (84774, 10279370, 190, 20210909094648, NULL);
INSERT INTO `status` VALUES (84779, 10279370, 81, 20210909101402, NULL);
INSERT INTO `status` VALUES (85755, 10278753, 188, 20210913171701, NULL);
INSERT INTO `status` VALUES (84143, 10278667, 82, 20210906203239, NULL);
INSERT INTO `status` VALUES (86829, 10279525, 190, 20210918134636, NULL);
INSERT INTO `status` VALUES (86051, 10279358, 187, 20210914221003, NULL);
INSERT INTO `status` VALUES (84052, 10279225, 188, 20210906121700, NULL);
INSERT INTO `status` VALUES (86021, 10278630, 187, 20210914191001, NULL);
INSERT INTO `status` VALUES (85754, 10278877, 81, 20210913171400, NULL);
INSERT INTO `status` VALUES (84778, 10279370, 187, 20210909101244, NULL);
INSERT INTO `status` VALUES (89622, 10279517, 73, 20210930160005, NULL);
INSERT INTO `status` VALUES (89365, 10279937, 78, 20210929073900, NULL);
INSERT INTO `status` VALUES (83940, 10279215, 82, 20210906003100, NULL);
INSERT INTO `status` VALUES (84507, 10279325, 90, 20210908085801, NULL);
INSERT INTO `status` VALUES (85377, 10279058, 89, 20210911194311, NULL);
INSERT INTO `status` VALUES (89653, 10279593, 89, 20210930184339, NULL);
INSERT INTO `status` VALUES (89501, 10279947, 89, 20210929214314, NULL);
INSERT INTO `status` VALUES (85480, 10279426, 69, 20210912062401, NULL);
INSERT INTO `status` VALUES (89620, 10279712, 190, 20210930154602, NULL);
INSERT INTO `status` VALUES (83941, 10279215, 189, 20210906003502, NULL);
INSERT INTO `status` VALUES (89156, 10279887, 89, 20210928104302, NULL);
INSERT INTO `status` VALUES (84671, 10279216, 73, 20210909000058, NULL);
INSERT INTO `status` VALUES (87079, 10279499, 73, 20210919150001, NULL);
INSERT INTO `status` VALUES (85765, 10278753, 69, 20210913182424, NULL);
INSERT INTO `status` VALUES (89616, 10279712, 69, 20210930152400, NULL);
INSERT INTO `status` VALUES (89566, 10279717, 81, 20210930051400, NULL);
INSERT INTO `status` VALUES (85479, 10279426, 188, 20210912061701, NULL);
INSERT INTO `status` VALUES (84050, 10279225, 90, 20210906115801, NULL);
INSERT INTO `status` VALUES (89301, 10278981, 73, 20210928235903, NULL);
INSERT INTO `status` VALUES (84716, 10279189, 76, 20210909035101, NULL);
INSERT INTO `status` VALUES (89299, 10278981, 76, 20210928235123, NULL);
INSERT INTO `status` VALUES (85381, 10279058, 73, 20210911200008, NULL);
INSERT INTO `status` VALUES (85758, 10278753, 78, 20210913173922, NULL);
INSERT INTO `status` VALUES (84045, 10279197, 187, 20210906111001, NULL);
INSERT INTO `status` VALUES (84046, 10279197, 81, 20210906111401, NULL);
INSERT INTO `status` VALUES (89029, 10279568, 189, 20210927223508, NULL);
INSERT INTO `status` VALUES (85743, 10279260, 81, 20210913151409, NULL);
INSERT INTO `status` VALUES (84049, 10279197, 78, 20210906113902, NULL);
INSERT INTO `status` VALUES (86908, 10279221, 66, 20210918220500, NULL);
INSERT INTO `status` VALUES (85759, 10278753, 89, 20210913174313, NULL);
INSERT INTO `status` VALUES (84903, 10279177, 81, 20210909211414, NULL);
INSERT INTO `status` VALUES (84490, 10279323, 66, 20210908070631, NULL);
INSERT INTO `status` VALUES (84493, 10279323, 188, 20210908071736, NULL);
INSERT INTO `status` VALUES (84518, 10279191, 190, 20210908094601, NULL);
INSERT INTO `status` VALUES (84732, 10279365, 188, 20210909051724, NULL);
INSERT INTO `status` VALUES (84615, 10279208, 78, 20210908184009, NULL);
INSERT INTO `status` VALUES (89612, 10279214, 73, 20210930145906, NULL);
INSERT INTO `status` VALUES (84499, 10279323, 90, 20210908075842, NULL);
INSERT INTO `status` VALUES (85477, 10279426, 187, 20210912061000, NULL);
INSERT INTO `status` VALUES (85026, 10278682, 73, 20210910090007, NULL);
INSERT INTO `status` VALUES (88356, 10279809, 81, 20210925011400, NULL);
INSERT INTO `status` VALUES (85772, 10279510, 82, 20210913193115, NULL);
INSERT INTO `status` VALUES (86729, 10279168, 188, 20210918031700, NULL);
INSERT INTO `status` VALUES (89518, 10279598, 76, 20210929235100, NULL);
INSERT INTO `status` VALUES (85987, 10279527, 78, 20210914153901, NULL);
INSERT INTO `status` VALUES (88546, 10279769, 75, 20210926173908, NULL);
INSERT INTO `status` VALUES (86961, 10279511, 153, 20210920013018, NULL);
INSERT INTO `status` VALUES (87717, 10279735, 81, 20210922121432, NULL);
INSERT INTO `status` VALUES (88536, 10278912, 76, 20210925175157, NULL);
INSERT INTO `status` VALUES (89242, 10278640, 75, 20210929173019, NULL);
INSERT INTO `status` VALUES (89507, 10279947, 81, 20210929221400, NULL);
INSERT INTO `status` VALUES (85522, 10279472, 187, 20210912141002, NULL);
INSERT INTO `status` VALUES (85476, 10279426, 66, 20210912060500, NULL);
INSERT INTO `status` VALUES (89523, 10279598, 188, 20210930001709, NULL);
INSERT INTO `status` VALUES (86197, 10279486, 90, 20210915165800, NULL);
INSERT INTO `status` VALUES (88274, 10278741, 75, 20210925173005, NULL);
INSERT INTO `status` VALUES (85991, 10279524, 188, 20210914161901, NULL);
INSERT INTO `status` VALUES (89607, 10279214, 189, 20210930143501, NULL);
INSERT INTO `status` VALUES (89413, 10279762, 190, 20210929134845, NULL);
INSERT INTO `status` VALUES (89628, 10279592, 82, 20210930163102, NULL);
INSERT INTO `status` VALUES (89633, 10279517, 90, 20210930165800, NULL);
INSERT INTO `status` VALUES (86149, 10279447, 78, 20210915123913, NULL);
INSERT INTO `status` VALUES (89083, 10279679, 81, 20210928031411, NULL);
INSERT INTO `status` VALUES (89652, 10279593, 189, 20210930183502, NULL);
INSERT INTO `status` VALUES (89578, 10279552, 190, 20210930064602, NULL);
INSERT INTO `status` VALUES (87309, 10279648, 187, 20210920171001, NULL);
INSERT INTO `status` VALUES (89557, 10279717, 189, 20210930043501, NULL);
INSERT INTO `status` VALUES (89605, 10279214, 69, 20210930142402, NULL);
INSERT INTO `status` VALUES (89563, 10279717, 73, 20210930045902, NULL);
INSERT INTO `status` VALUES (86701, 10279583, 187, 20210918011000, NULL);
INSERT INTO `status` VALUES (87333, 10279614, 66, 20210920190501, NULL);
INSERT INTO `status` VALUES (88802, 10279740, 90, 20210926205801, NULL);
INSERT INTO `status` VALUES (89650, 10278838, 69, 20210930182402, NULL);
INSERT INTO `status` VALUES (86151, 10279447, 190, 20210915124609, NULL);
INSERT INTO `status` VALUES (89091, 10279441, 90, 20210928035823, NULL);
INSERT INTO `status` VALUES (89649, 10278839, 188, 20210930181719, NULL);
INSERT INTO `status` VALUES (86827, 10279525, 78, 20210918133930, NULL);
INSERT INTO `status` VALUES (89516, 10279598, 78, 20210929233900, NULL);
INSERT INTO `status` VALUES (87308, 10279648, 66, 20210920170501, NULL);
INSERT INTO `status` VALUES (87826, 10278903, 78, 20210922213910, NULL);
INSERT INTO `status` VALUES (88343, 10279685, 66, 20210925000511, NULL);
INSERT INTO `status` VALUES (89241, 10279514, 63, 20210929173018, NULL);
INSERT INTO `status` VALUES (89648, 10279750, 81, 20210930181401, NULL);
INSERT INTO `status` VALUES (85982, 10279454, 187, 20210914151020, NULL);
INSERT INTO `status` VALUES (87061, 10279597, 69, 20210919132400, NULL);
INSERT INTO `status` VALUES (89162, 10279887, 188, 20210928111701, NULL);
INSERT INTO `status` VALUES (87329, 10279648, 190, 20210920184629, NULL);
INSERT INTO `status` VALUES (89480, 10279815, 76, 20210929195101, NULL);
INSERT INTO `status` VALUES (87314, 10279648, 78, 20210920173900, NULL);
INSERT INTO `status` VALUES (86013, 10279447, 75, 20210915173005, NULL);
INSERT INTO `status` VALUES (86014, 10279471, 63, 20210915173031, NULL);
INSERT INTO `status` VALUES (89647, 10279517, 187, 20210930181002, NULL);
INSERT INTO `status` VALUES (89645, 10279517, 73, 20210930180007, NULL);
INSERT INTO `status` VALUES (89643, 10279517, 76, 20210930175100, NULL);
INSERT INTO `status` VALUES (89158, 10279887, 76, 20210928105108, NULL);
INSERT INTO `status` VALUES (89627, 10279712, 69, 20210930162414, NULL);
INSERT INTO `status` VALUES (89503, 10279947, 90, 20210929215801, NULL);
INSERT INTO `status` VALUES (89655, 10278838, 76, 20210930185101, NULL);
INSERT INTO `status` VALUES (88261, 10279787, 82, 20210924173110, NULL);
INSERT INTO `status` VALUES (89585, 10279585, 78, 20210930123901, NULL);
INSERT INTO `status` VALUES (89300, 10278981, 90, 20210928235802, NULL);
INSERT INTO `status` VALUES (88205, 10279154, 73, 20210924120236, NULL);
INSERT INTO `status` VALUES (87328, 10279648, 78, 20210920184323, NULL);
INSERT INTO `status` VALUES (88824, 10279875, 66, 20210926230501, NULL);
INSERT INTO `status` VALUES (89372, 10279828, 81, 20210929081402, NULL);
INSERT INTO `status` VALUES (89408, 10279762, 69, 20210929132414, NULL);
INSERT INTO `status` VALUES (89482, 10279746, 73, 20210929200026, NULL);
INSERT INTO `status` VALUES (89606, 10279214, 82, 20210930143153, NULL);
INSERT INTO `status` VALUES (89479, 10279558, 190, 20210929194603, NULL);
INSERT INTO `status` VALUES (89087, 10279679, 78, 20210928033911, NULL);
INSERT INTO `status` VALUES (88203, 10279731, 81, 20210924111400, NULL);
INSERT INTO `status` VALUES (89625, 10279712, 81, 20210930161409, NULL);
INSERT INTO `status` VALUES (89198, 10279908, 89, 20210928144302, NULL);
INSERT INTO `status` VALUES (89641, 10279517, 89, 20210930174301, NULL);
INSERT INTO `status` VALUES (89659, 10279593, 187, 20210930191000, NULL);
INSERT INTO `status` VALUES (88595, 10279764, 73, 20210925225914, NULL);
INSERT INTO `status` VALUES (89090, 10279441, 76, 20210928035133, NULL);
INSERT INTO `status` VALUES (89298, 10278981, 190, 20210928234601, NULL);
INSERT INTO `status` VALUES (89330, 10279640, 76, 20210929035145, NULL);
INSERT INTO `status` VALUES (89454, 10279799, 187, 20210929171001, NULL);
INSERT INTO `status` VALUES (87824, 10278903, 82, 20210922213127, NULL);
INSERT INTO `status` VALUES (88202, 10279731, 187, 20210924111000, NULL);
INSERT INTO `status` VALUES (89412, 10279762, 89, 20210929134408, NULL);
INSERT INTO `status` VALUES (89086, 10279679, 189, 20210928033500, NULL);
INSERT INTO `status` VALUES (89239, 10279762, 152, 20210929173007, NULL);
INSERT INTO `status` VALUES (88034, 10279710, 63, 20210924173027, NULL);
INSERT INTO `status` VALUES (87827, 10278903, 89, 20210922214300, NULL);
INSERT INTO `status` VALUES (89473, 10278943, 81, 20210929191409, NULL);
INSERT INTO `status` VALUES (89185, 10279868, 189, 20210928133516, NULL);
INSERT INTO `status` VALUES (89635, 10279592, 66, 20210930170512, NULL);
INSERT INTO `status` VALUES (87324, 10279559, 63, 20210921173010, NULL);
INSERT INTO `status` VALUES (89041, 10279568, 189, 20210927233500, NULL);
INSERT INTO `status` VALUES (89657, 10278838, 73, 20210930190031, NULL);
INSERT INTO `status` VALUES (89515, 10279598, 69, 20210929232400, NULL);
INSERT INTO `status` VALUES (89160, 10279887, 66, 20210928110502, NULL);
INSERT INTO `status` VALUES (89384, 10279828, 81, 20210929091402, NULL);
INSERT INTO `status` VALUES (87815, 10278903, 190, 20210922204610, NULL);
INSERT INTO `status` VALUES (89199, 10279908, 190, 20210928144602, NULL);
INSERT INTO `status` VALUES (89581, 10279552, 66, 20210930070500, NULL);
INSERT INTO `status` VALUES (89406, 10279762, 81, 20210929131430, NULL);
INSERT INTO `status` VALUES (89448, 10279805, 189, 20210929163644, NULL);
INSERT INTO `status` VALUES (89576, 10279552, 82, 20210930063101, NULL);
INSERT INTO `status` VALUES (88539, 10279769, 81, 20210925181408, NULL);
INSERT INTO `status` VALUES (89560, 10279717, 190, 20210930044601, NULL);
INSERT INTO `status` VALUES (89046, 10279568, 76, 20210927235222, NULL);
INSERT INTO `status` VALUES (89626, 10279712, 188, 20210930161714, NULL);
INSERT INTO `status` VALUES (88197, 10279431, 73, 20210924100017, NULL);
INSERT INTO `status` VALUES (89541, 10279936, 66, 20210930030508, NULL);
INSERT INTO `status` VALUES (89240, 10279762, 74, 20210929173007, NULL);
INSERT INTO `status` VALUES (89579, 10279718, 90, 20210930065802, NULL);
INSERT INTO `status` VALUES (89651, 10278839, 82, 20210930183102, NULL);
INSERT INTO `status` VALUES (87711, 10279259, 89, 20210922114300, NULL);
INSERT INTO `status` VALUES (88482, 10279649, 69, 20210925132421, NULL);
INSERT INTO `status` VALUES (89634, 10279517, 73, 20210930170013, NULL);
INSERT INTO `status` VALUES (88598, 10279764, 81, 20210925231443, NULL);
INSERT INTO `status` VALUES (89329, 10279640, 190, 20210929034602, NULL);
INSERT INTO `status` VALUES (89624, 10279712, 187, 20210930161008, NULL);
INSERT INTO `status` VALUES (89481, 10279746, 90, 20210929195800, NULL);
INSERT INTO `status` VALUES (89646, 10278838, 66, 20210930180502, NULL);
INSERT INTO `status` VALUES (89197, 10279908, 78, 20210928143902, NULL);

-- ----------------------------
-- Table structure for store
-- ----------------------------
DROP TABLE IF EXISTS `store`;
CREATE TABLE `store`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `Name` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `price` float(10, 2) NOT NULL,
  `image` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'default.png',
  `description` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`, `Name`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 24 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of store
-- ----------------------------
INSERT INTO `store` VALUES (1, 'ConquerPoints [14M]', 10.00, 'ConquerPoints [10M]-1565409058', 'CPS ,');
INSERT INTO `store` VALUES (2, 'King [30Day]', 100.00, 'King [30Day]-1565409301', 'King Out Donation 30 Days');
INSERT INTO `store` VALUES (3, 'Token GoldPrize', 50.00, 'Token GoldPrize-1565409403', 'GoldPrize ,');
INSERT INTO `store` VALUES (17, 'Run Max +9 [Yellow]', 10.00, 'Yellowrune-1561657123', 'You can choose Rune of your choice level 9');
INSERT INTO `store` VALUES (18, 'Run Max +27 [Blue]', 20.00, 'YellowBlue-159867123', 'You can choose Rune of your choice level 27');
INSERT INTO `store` VALUES (19, 'Relic[Epic]', 30.00, 'Rilec-1592243539', 'Relic Epic');
INSERT INTO `store` VALUES (20, 'P15Anima', 60.00, 'P15Anima-1591243403', 'P15Anima');
INSERT INTO `store` VALUES (21, 'P16Anima', 100.00, 'P16Anima-1591243437', 'P16Anima');
INSERT INTO `store` VALUES (22, 'P17Anima', 150.00, 'P17Anima-1598945643', 'P17Anima');
INSERT INTO `store` VALUES (23, 'P18Anima', 200.00, 'P18Anima-15912345618', 'P18Anima');

-- ----------------------------
-- Table structure for stores
-- ----------------------------
DROP TABLE IF EXISTS `stores`;
CREATE TABLE `stores`  (
  `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT,
  `item` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `img` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `price` double(8, 2) NOT NULL,
  `Quantity` double(8, 2) NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of stores
-- ----------------------------

-- ----------------------------
-- Table structure for tickets
-- ----------------------------
DROP TABLE IF EXISTS `tickets`;
CREATE TABLE `tickets`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `title` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `category` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `problem` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `User` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `time` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Status` int(11) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tickets
-- ----------------------------

-- ----------------------------
-- Table structure for tickets_replys
-- ----------------------------
DROP TABLE IF EXISTS `tickets_replys`;
CREATE TABLE `tickets_replys`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `ticketid` int(11) NOT NULL,
  `reply` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `user` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `date` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tickets_replys
-- ----------------------------

-- ----------------------------
-- Table structure for vodacard
-- ----------------------------
DROP TABLE IF EXISTS `vodacard`;
CREATE TABLE `vodacard`  (
  `Email` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `fbID` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `cardNum` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `hmCash` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `phNum` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of vodacard
-- ----------------------------

-- ----------------------------
-- Table structure for vodacash
-- ----------------------------
DROP TABLE IF EXISTS `vodacash`;
CREATE TABLE `vodacash`  (
  `Email` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `fbID` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `opNum` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `hmCash` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `phNum` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of vodacash
-- ----------------------------

-- ----------------------------
-- Table structure for vote
-- ----------------------------
DROP TABLE IF EXISTS `vote`;
CREATE TABLE `vote`  (
  `UID` bigint(20) NULL DEFAULT NULL,
  `user` varchar(16) CHARACTER SET cp1256 COLLATE cp1256_general_ci NULL DEFAULT NULL,
  `ip` varchar(100) CHARACTER SET cp1256 COLLATE cp1256_general_ci NOT NULL,
  `State` varchar(50) CHARACTER SET cp1256 COLLATE cp1256_general_ci NULL DEFAULT NULL,
  `Time` bigint(20) NULL DEFAULT 0
) ENGINE = MyISAM CHARACTER SET = cp1256 COLLATE = cp1256_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of vote
-- ----------------------------

-- ----------------------------
-- Table structure for votes
-- ----------------------------
DROP TABLE IF EXISTS `votes`;
CREATE TABLE `votes`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `user` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ip_address` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `time` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of votes
-- ----------------------------

-- ----------------------------
-- Table structure for vtm_news
-- ----------------------------
DROP TABLE IF EXISTS `vtm_news`;
CREATE TABLE `vtm_news`  (
  `id_news` int(11) NOT NULL AUTO_INCREMENT,
  `title_news` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `subject_news` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
  `img_news` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `time_news` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_news`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of vtm_news
-- ----------------------------
INSERT INTO `vtm_news` VALUES (1, 'wwww', 'wssss', NULL, NULL);

SET FOREIGN_KEY_CHECKS = 1;
