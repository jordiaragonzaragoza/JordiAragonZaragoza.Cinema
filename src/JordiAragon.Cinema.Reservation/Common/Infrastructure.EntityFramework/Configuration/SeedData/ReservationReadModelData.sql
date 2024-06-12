IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;


BEGIN TRANSACTION;


CREATE TABLE [AvailableSeats] (
    [Id] uniqueidentifier NOT NULL,
    [SeatId] uniqueidentifier NOT NULL,
    [Row] smallint NOT NULL,
    [SeatNumber] smallint NOT NULL,
    [ShowtimeId] uniqueidentifier NOT NULL,
    [AuditoriumId] uniqueidentifier NOT NULL,
    [AuditoriumName] nvarchar(max) NULL,
    CONSTRAINT [PK_AvailableSeats] PRIMARY KEY ([Id])
);


CREATE TABLE [Checkpoints] (
    [Id] uniqueidentifier NOT NULL,
    [Position] decimal(20,0) NOT NULL,
    [CheckpointedAtOnUtc] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Checkpoints] PRIMARY KEY ([Id])
);


CREATE TABLE [Showtimes] (
    [Id] uniqueidentifier NOT NULL,
    [SessionDateOnUtc] datetimeoffset NOT NULL,
    [MovieId] uniqueidentifier NOT NULL,
    [MovieTitle] nvarchar(max) NULL,
    [MovieRuntime] time NOT NULL,
    [AuditoriumId] uniqueidentifier NOT NULL,
    [AuditoriumName] nvarchar(max) NULL,
    CONSTRAINT [PK_Showtimes] PRIMARY KEY ([Id])
);


CREATE TABLE [Tickets] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [ShowtimeId] uniqueidentifier NOT NULL,
    [SessionDateOnUtc] datetimeoffset NOT NULL,
    [AuditoriumName] nvarchar(max) NULL,
    [MovieTitle] nvarchar(max) NULL,
    [IsPurchased] bit NOT NULL,
    [CreatedTimeOnUtc] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Tickets] PRIMARY KEY ([Id])
);


CREATE TABLE [TicketsSeats] (
    [Id] uniqueidentifier NOT NULL,
    [TicketId] uniqueidentifier NOT NULL,
    [Row] smallint NOT NULL,
    [SeatNumber] smallint NOT NULL,
    CONSTRAINT [PK_TicketsSeats] PRIMARY KEY ([Id], [TicketId]),
    CONSTRAINT [FK_TicketsSeats_Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [Tickets] ([Id]) ON DELETE CASCADE
);


CREATE INDEX [IX_TicketsSeats_TicketId] ON [TicketsSeats] ([TicketId]);


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240520165357_InitialReadStoreMigration', N'7.0.14');


COMMIT;


