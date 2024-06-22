
CREATE TABLE outbox (
   id         BIGINT       PRIMARY KEY GENERATED BY DEFAULT AS IDENTITY,
   message_type VARCHAR(250) NOT NULL,
   data       JSONB        NOT NULL
);
