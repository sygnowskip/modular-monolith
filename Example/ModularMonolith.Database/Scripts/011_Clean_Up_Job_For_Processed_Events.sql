IF NOT EXISTS(
	SELECT * FROM msdb.dbo.sysjobs 
    WHERE name = 'ProcessedEventCleanUp')
BEGIN
    EXEC msdb.dbo.sp_add_job  
       @job_name = N'ProcessedEventCleanUp',   
       @enabled = 1,   
       @description = N'Removing older than 30 days processed events' ;
    
    EXEC msdb.dbo.sp_add_jobstep  
        @job_name = N'ProcessedEventCleanUp',   
        @step_name = N'Run clean up',   
        @subsystem = N'TSQL',   
        @command = 'DELETE FROM [Hexure].[events].[ProcessedEvent] WHERE [ProcessedOn] < DATEADD(day, -30, GETDATE())';
    
    EXEC msdb.dbo.sp_add_schedule  
        @schedule_name = N'Everyday schedule',   
        @freq_type = 4,  -- daily start
        @freq_interval = 1,
        @active_start_time = '010000' ;   -- start time 01:00:00
    
    EXEC msdb.dbo.sp_attach_schedule  
       @job_name = N'ProcessedEventCleanUp',  
       @schedule_name = N'Everyday schedule';
    
    EXEC msdb.dbo.sp_add_jobserver  
       @job_name = N'ProcessedEventCleanUp',  
       @server_name = @@servername;
END