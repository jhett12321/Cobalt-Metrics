CREATE TABLE `users` (
  `key` char(32) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `enabled` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`key`),
  KEY `name_index` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `sessions` (
  `id` char(32) NOT NULL,
  `user_key` char(32) NOT NULL,
  `version` varchar(255) DEFAULT NULL,
  `start_time` bigint(20) DEFAULT NULL,
  `end_time` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`user_key`,`id`),
  KEY `start_time_index` (`start_time`) USING BTREE,
  KEY `end_time_index` (`end_time`) USING BTREE,
  KEY `id_index` (`id`) USING BTREE,
  KEY `version_index` (`version`) USING BTREE,
  CONSTRAINT `sessions_ibfk_1` FOREIGN KEY (`user_key`) REFERENCES `users` (`key`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `data_types` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `name_index` (`name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `data_id_hashes` (
  `user_key` char(32) NOT NULL,
  `hash` char(32) NOT NULL,
  `id` text NOT NULL,
  PRIMARY KEY (`hash`),
  KEY `data_id_hashes_ibfk_1` (`user_key`),
  CONSTRAINT `data_id_hashes_ibfk_1` FOREIGN KEY (`user_key`) REFERENCES `users` (`key`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `data` (
  `hash` char(32) NOT NULL,
  `user_key` char(32) NOT NULL,
  `session_id` char(32) NOT NULL,
  `type_id` int(11) NOT NULL,
  `array_index` int(11) NOT NULL,
  `value` text NOT NULL,
  `timestamp` bigint(20) NOT NULL,
  PRIMARY KEY (`user_key`,`session_id`,`hash`,`timestamp`,`array_index`),
  KEY `session_id_index` (`session_id`) USING BTREE,
  KEY `id_index` (`hash`) USING BTREE,
  KEY `timestamp_index` (`timestamp`) USING BTREE,
  KEY `type_id_index` (`type_id`) USING BTREE,
  CONSTRAINT `data_ibfk_1` FOREIGN KEY (`user_key`) REFERENCES `users` (`key`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `data_ibfk_2` FOREIGN KEY (`session_id`) REFERENCES `sessions` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `data_ibfk_3` FOREIGN KEY (`type_id`) REFERENCES `data_types` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `data_ibfk_4` FOREIGN KEY (`hash`) REFERENCES `data_id_hashes` (`hash`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 

CREATE TABLE `data_frequency` (
  `hash` char(32) NOT NULL,
  `user_key` char(32) NOT NULL,
  `session_id` char(32) NOT NULL,
  `type_id` int(11) NOT NULL,
  `frequency` int(11) NOT NULL,
  PRIMARY KEY (`user_key`,`session_id`,`hash`),
  KEY `session_id_index` (`session_id`) USING BTREE,
  KEY `id_index` (`hash`) USING BTREE,
  KEY `type_id_index` (`type_id`) USING BTREE,
  CONSTRAINT `data_frequency_ibfk_1` FOREIGN KEY (`user_key`) REFERENCES `users` (`key`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `data_frequency_ibfk_2` FOREIGN KEY (`session_id`) REFERENCES `sessions` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `data_frequency_ibfk_3` FOREIGN KEY (`type_id`) REFERENCES `data_types` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `data_frequency_ibfk_4` FOREIGN KEY (`hash`) REFERENCES `data_id_hashes` (`hash`) ON DELETE CASCADE ON UPDATE CASCADE,
) ENGINE=InnoDB DEFAULT CHARSET=utf8;