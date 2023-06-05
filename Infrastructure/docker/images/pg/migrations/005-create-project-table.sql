CREATE TABLE project (
projectID          uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
name               VARCHAR(100) NOT NULL,
projectLeaderID    uuid,
clientID           uuid,
priceHour          FLOAT,
FOREIGN KEY (projectLeaderID) REFERENCES freelancer (userID),
FOREIGN KEY (clientID) REFERENCES client (userID)
);
