IF NOT EXISTS (SELECT 1 FROM sys.tables T 
				JOIN sys.Columns C ON C.object_id = T.object_id
				WHERE T.name = 'Share' AND C.Name = 'ShareType')
BEGIN
	ALTER TABLE cred2.Share ADD ShareType VARCHAR(30) NOT NULL DEFAULT 'email'
END
GO