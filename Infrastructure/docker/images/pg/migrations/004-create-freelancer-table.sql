CREATE TABLE freelancer (
userID        uuid PRIMARY KEY,
dailyAvgHours FLOAT,
FOREIGN KEY (userID) REFERENCES users (userID)
);
