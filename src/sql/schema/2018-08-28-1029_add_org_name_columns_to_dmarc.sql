ALTER TABLE `dns_record_dmarc`
ADD COLUMN `org_domain` VARCHAR(255) NULL DEFAULT NULL AFTER `domain_id`,
ADD COLUMN `is_tld` TINYINT NULL DEFAULT NULL AFTER `org_domain`,
ADD COLUMN `is_inherited` TINYINT NULL DEFAULT NULL AFTER `is_tld`;


