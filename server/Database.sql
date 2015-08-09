CREATE TABLE `users` (
  `key` char(32) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `enabled` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`key`),
  KEY `name_index` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `sessions` (
  `id` char(32) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `user_key` char(32) NOT NULL,
  `start_time` bigint(20) DEFAULT NULL,
  `end_time` bigint(20) DEFAULT NULL,
  `session_time` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`user_key`,`id`),
  KEY `start_time_index` (`start_time`) USING BTREE,
  KEY `end_time_index` (`end_time`) USING BTREE,
  KEY `id_index` (`id`) USING BTREE,
  CONSTRAINT `sessions_ibfk_1` FOREIGN KEY (`user_key`) REFERENCES `users` (`key`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `data_types` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `name_index` (`name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

CREATE TABLE `data` (
  `id` varchar(255) NOT NULL,
  `session_id` char(32) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `type_id` int(11) NOT NULL,
  `array_index` int(11) NOT NULL,
  `value` text NOT NULL,
  `timestamp` bigint(20) NOT NULL,
  PRIMARY KEY (`session_id`,`id`,`timestamp`,`array_index`),
  KEY `id_index` (`id`) USING BTREE,
  KEY `timestamp_index` (`timestamp`) USING BTREE,
  KEY `data_ibfk_2` (`type_id`),
  CONSTRAINT `data_ibfk_1` FOREIGN KEY (`session_id`) REFERENCES `sessions` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `data_ibfk_2` FOREIGN KEY (`type_id`) REFERENCES `data_types` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;