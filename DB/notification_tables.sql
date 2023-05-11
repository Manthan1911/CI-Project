CREATE TABLE notification_setting
(
	user_id BIGINT primary key,
	recommend_mission BIT DEFAULT 0,
	recommend_story BIT DEFAULT 0,
	volunteering_hour BIT DEFAULT 0,
	volunteering_goal BIT DEFAULT 0,
	my_story BIT DEFAULT 0,
	new_mission BIT DEFAULT 1,
	new_message BIT DEFAULT 1,
	mission_application BIT DEFAULT 1,
	news BIT DEFAULT 0,
	mail BIT DEFAULT 0,
	FOREIGN KEY (user_id) REFERENCES [dbo].[user](user_id),
);

CREATE TABLE notification
(
	notification_id BIGINT PRIMARY KEY IDENTITY(1,1),
	notification_text TEXT,
	notification_type INT,
	user_avtar TEXT,
);

CREATE TABLE user_notification
(
	user_notification_id BIGINT PRIMARY KEY IDENTITY(1,1),
	user_id BIGINT,
	notification_id BIGINT,
	is_read BIT,
	created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_at DATETIME,
	FOREIGN KEY (user_id) REFERENCES [dbo].[user](user_id),
	FOREIGN KEY (notification_id) REFERENCES notification(notification_id)
);

CREATE TABLE last_check
(
	user_id BIGINT PRIMARY KEY,
	created_at datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
); 