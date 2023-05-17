-- 1------------------------------------------------------------------------------------------

--CREATE PROCEDURE sp_getnotificationsettingbyuserid
--@user_id int
--AS
--BEGIN
	
--	SELECT 
--	[user_id],
--	[recommend_mission],
--	[recommend_story],
--	[volunteering_hour],
--	[volunteering_goal],
--	[my_story],
--	[new_mission],
--	[new_message],
--	[mission_application],
--	[news],
--	[mail]
--	FROM [CI].[dbo].[notification_setting]
--	WHERE [user_id] = @user_id

--END


-- 2------------------------------------------------------------------------------------------

CREATE PROCEDURE sp_SendNotificationToAllUsers
@user_id int,
@notification_text text,
@notification_type int,
@user_avtar TEXT,
@to_users TEXT,
@created_at DATETIME,
@setting_type_name text
AS 
BEGIN

		INSERT INTO notification (notification_text,notification_type,user_avtar) 
		VALUES (@notification_text,@notification_type,@user_avtar);

		
		DECLARE @notification_id BIGINT;
		DECLARE @is_read BIT;
		SET @notification_id = SCOPE_IDENTITY();
		SET @is_read = 0;


		IF(@notification_text = 4)
		BEGIN
			INSERT INTO user_notification (user_id, notification_id,is_read,created_at)
			SELECT t.value, @notification_id,@is_read,@created_at
			FROM STRING_SPLIT(@to_users, ',') AS t;
		END
		ELSE
		BEGIN
			INSERT INTO user_notification (user_id, notification_id, is_read, created_at)
			SELECT ns.user_id, @notification_id, @is_read, @created_at
			FROM notification_setting AS ns
			WHERE QUOTENAME(@setting_type_name) = 1;
		END

		SELECT u.*
		FROM notification_setting ns 
		JOIN [dbo].[user] as u
		ON ns.user_id = u.user_id
		WHERE QUOTENAME(@setting_type_name) = 1 AND ns.mail = 1;

END