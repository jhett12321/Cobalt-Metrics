CREATE TABLE `users` (
  `id` char(32) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `enabled` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `name_index` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `sessions` (
  `id` binary(16) NOT NULL,
  `user_id` char(32) NOT NULL,
  `start_time` bigint(20) NOT NULL,
  `end_time` bigint(20) NOT NULL,
  `session_time` bigint(20) NOT NULL,
  PRIMARY KEY (`user_id`,`id`),
  KEY `start_time_index` (`start_time`) USING BTREE,
  KEY `end_time_index` (`end_time`) USING BTREE,
  KEY `id_index` (`id`) USING BTREE,
  CONSTRAINT `sessions_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `data_single` (
  `id` varchar(255) NOT NULL,
  `session_id` binary(16) NOT NULL,
  `value` text NOT NULL,
  `timestamp` bigint(20) NOT NULL,
  PRIMARY KEY (`session_id`,`id`,`timestamp`),
  KEY `id_index` (`id`) USING BTREE,
  KEY `timestamp_index` (`timestamp`) USING BTREE,
  CONSTRAINT `data_single_ibfk_1` FOREIGN KEY (`session_id`) REFERENCES `sessions` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `data_array` (
  `id` varchar(255) NOT NULL,
  `session_id` binary(16) NOT NULL,
  `array_index` int(11) NOT NULL,
  `value` text NOT NULL,
  `timestamp` bigint(20) NOT NULL,
  PRIMARY KEY (`session_id`,`id`,`timestamp`,`array_index`),
  KEY `id_index` (`id`) USING BTREE,
  KEY `timestamp_index` (`timestamp`) USING BTREE,
  CONSTRAINT `data_array_ibfk_1` FOREIGN KEY (`session_id`) REFERENCES `sessions` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;