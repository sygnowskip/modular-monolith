IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'orders' AND  TABLE_NAME = 'OrderItem')
BEGIN
	CREATE TABLE [orders].[OrderItem](
		[Id] bigint IDENTITY(1,1) NOT NULL,
		[OrderId] bigint NOT NULL,
        [ExternalId] UNIQUEIDENTIFIER NOT NULL,
        [Name] NVARCHAR(MAX) NOT NULL,
        [ProductType] NVARCHAR(MAX) NOT NULL,
        [Quantity] INT NOT NULL,
        [NetAmount] DECIMAL(7,3) NOT NULL,
        [NetCurrency] NVARCHAR(3) NOT NULL,
        [TaxAmount] DECIMAL(7,3) NOT NULL,
        [TaxCurrency] NVARCHAR(3) NOT NULL,

        CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY ([OrderId]) REFERENCES [orders].[Order]([Id])
	) ON [PRIMARY]
END