CREATE TABLE client (
userID        uuid PRIMARY KEY,
FOREIGN KEY (userID) REFERENCES users (userID)
);
