CREATE TABLE users (
     userID             uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
     displayName VARCHAR(255),
     username    VARCHAR(255),
     password    VARCHAR(255)
);