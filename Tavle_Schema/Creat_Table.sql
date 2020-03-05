CREATE TABLE Sample.dbo.Foo (
  FooId uniqueidentifier NOT NULL,
  Name nvarchar(50) NOT NULL,
  Description nvarchar(100) NOT NULL,
  Enable bit NOT NULL DEFAULT (0),
  CreateTime datetime NOT NULL DEFAULT (getdate()),
  UpdateTime datetime NOT NULL DEFAULT (getdate()),
  CONSTRAINT PK_Foo_FooId PRIMARY KEY CLUSTERED (FooId)
)
ON [PRIMARY]
