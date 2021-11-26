USE `hitlady`;

CREATE TABLE IF NOT EXISTS `users` (
  `id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `discord_id` VARCHAR(255) NOT NULL,
  `lastfm` VARCHAR(255) NOT NULL,
  `created_at` DATETIME NOT NULL DEFAULT current_timestamp(),
  `updated_at` DATETIME NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

ALTER TABLE `users`
  CHANGE `discord_id` `discord_id` BIGINT UNSIGNED NOT NULL;

CREATE TABLE IF NOT EXISTS `plays` (
  `id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `discord_id` BIGINT UNSIGNED NOT NULL,
  `artist_name` VARCHAR(255) NOT NULL,
  `play_count` INT(11) NOT NULL,
  `created_at` DATETIME NOT NULL DEFAULT current_timestamp(),
  `updated_at` DATETIME NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `guild_assignments` (
  `id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `discord_id` BIGINT UNSIGNED NOT NULL,
  `guild_id` BIGINT UNSIGNED NOT NULL,
  `created_at` DATETIME NOT NULL DEFAULT current_timestamp(),
  `updated_at` DATETIME NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
