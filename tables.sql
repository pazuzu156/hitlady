CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `discord_id` varchar(255) NOT NULL,
  `lastfm` varchar(255) NOT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  `updated_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
